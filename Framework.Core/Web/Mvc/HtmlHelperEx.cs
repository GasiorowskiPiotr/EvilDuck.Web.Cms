using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace EvilDuck.Framework.Core.Web.Mvc
{
    public static class HtmlHelperEx
    {
        public static EvilDuckHelper EvilDuck(this HtmlHelper html)
        {
            return new EvilDuckHelper(html);
        }
    }

    public class EvilDuckHelper
    {
        private readonly HtmlHelper _html;

        public EvilDuckHelper(HtmlHelper html)
        {
            _html = html;
        }

        public string ActiveMenuItem(params string[] possibleControllers)
        {
            if (possibleControllers == null)
                return String.Empty;

            if (possibleControllers.Contains(_html.ViewContext.RouteData.Values["controller"]))
            {
                return "active";
            }
            return String.Empty;
        }

        public MvcHtmlString Pager(QueryModel pagingDto, int allCount, string containerName)
        {
            String toFirst;
            String toLast;

            var currentPage = pagingDto.Skip / pagingDto.Take;
            var lastPage = allCount / pagingDto.Take;

            if (currentPage > 0)
            {
                var toFirstLink = new AjaxHelper(_html.ViewContext, _html.ViewDataContainer)
                    .ActionLink("<<", (string)_html.ViewContext.RouteData.Values["action"], (string)_html.ViewContext.RouteData.Values["controller"], pagingDto.Alter(skip: 0), new AjaxOptions
                    {
                        HttpMethod = "GET",
                        UpdateTargetId = containerName,
                        InsertionMode = InsertionMode.Replace,
                        OnSuccess = "lopor.reloadGrid()"
                    });

                var toPrevious = new AjaxHelper(_html.ViewContext, _html.ViewDataContainer)
                    .ActionLink("<", (string)_html.ViewContext.RouteData.Values["action"],
                        (string)_html.ViewContext.RouteData.Values["controller"],
                        pagingDto.Alter(skip: pagingDto.Skip - pagingDto.Take), new AjaxOptions
                        {
                            HttpMethod = "GET",
                            UpdateTargetId = containerName,
                            InsertionMode = InsertionMode.Replace,
                            OnSuccess = "lopor.reloadGrid()"
                        });

                toFirst = String.Format("<li>{0}</li><li>{1}</li>", toFirstLink.ToHtmlString(), toPrevious.ToHtmlString());
            }
            else
            {
                toFirst = String.Empty;
            }

            if (currentPage < lastPage)
            {
                var toLastLink = new AjaxHelper(_html.ViewContext, _html.ViewDataContainer).ActionLink(">>", (string)_html.ViewContext.RouteData.Values["action"],
                    (string)_html.ViewContext.RouteData.Values["controller"], pagingDto.Alter(skip: (allCount / pagingDto.Take) * pagingDto.Take), new AjaxOptions
                    {
                        HttpMethod = "GET",
                        UpdateTargetId = containerName,
                        InsertionMode = InsertionMode.Replace,
                        OnSuccess = "lopor.reloadGrid()"
                    });

                var toNext = new AjaxHelper(_html.ViewContext, _html.ViewDataContainer).ActionLink(">",
                    (string)_html.ViewContext.RouteData.Values["action"],
                    (string)_html.ViewContext.RouteData.Values["controller"],
                    pagingDto.Alter(skip: (pagingDto.Skip + pagingDto.Take)), new AjaxOptions()
                    {
                        HttpMethod = "GET",
                        UpdateTargetId = containerName,
                        InsertionMode = InsertionMode.Replace,
                        OnSuccess = "lopor.reloadGrid()"
                    });

                toLast = String.Format("<li>{0}</li><li>{1}</li>", toNext.ToHtmlString(), toLastLink.ToHtmlString());
            }
            else
            {
                toLast = String.Empty;
            }

            var sb = new StringBuilder();
            const string format = "<li class=\"{0}\">{1}</li>";
            for (int i = currentPage - 5; i < currentPage + 5; i++)
            {
                if (i < 0)
                    continue;

                if (i > lastPage)
                    break;

                var @class = currentPage == i ? "active" : String.Empty;


                var pageLink = new AjaxHelper(_html.ViewContext, _html.ViewDataContainer).ActionLink(String.Format("{0}", i + 1),
                    (string)_html.ViewContext.RouteData.Values["action"], (string)_html.ViewContext.RouteData.Values["controller"], pagingDto.Alter(skip: i * pagingDto.Take), new AjaxOptions
                    {
                        HttpMethod = "GET",
                        InsertionMode = InsertionMode.Replace,
                        UpdateTargetId = containerName,
                        OnSuccess = "lopor.reloadGrid()"
                    });

                sb.AppendFormat(format, @class, pageLink.ToHtmlString());
            }

            return new MvcHtmlString(PagerMain(toFirst, sb.ToString(), toLast));

        }

        public IEnumerable<SelectListItem> EnumToSelectList(Type enumType, string propertyName, int value = 0)
        {
            var selectListItems = new List<SelectListItem>();

            const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static;

            var fields = enumType.GetFields(bindingFlags);

            foreach (FieldInfo field in fields)
            {
                object fieldValue = field.GetRawConstantValue();

                selectListItems.Add(new SelectListItem { Text = GetDisplayName(field), Value = fieldValue.ToString(), Selected = ((int)fieldValue == value) });
            }

            return selectListItems;
        }

        private static string GetDisplayName(MemberInfo fieldInfo)
        {
            var display = fieldInfo.GetCustomAttribute<DisplayAttribute>(false);
            if (display == null)
                return fieldInfo.Name;

            var name = display.GetName();

            return !String.IsNullOrEmpty(name) ? name : fieldInfo.Name;
        }

        private static string PagerMain(string toStart, string numbers, string toEnd)
        {
            return String.Format(PagerMainFormat, toStart, numbers, toEnd);
        }

        private const string PagerMainFormat = "<ul class=\"pagination\">\r\n{0}\r\n{1}\r\n{2}\r\n</ul>";
    }
}

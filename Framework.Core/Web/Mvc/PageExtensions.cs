using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EvilDuck.Framework.Core.Web.Mvc
{
    public static class PageExtensions
    {
        public static void InitializeScreen<TModel>(this WebViewPage<TModel> page, string title = "", int width = 12, string layout = "../Shared/_Screen.cshtml")
        {
            page.ViewBag.Title = title;
            page.ViewBag.Width = width;
            page.Layout = layout;
        }
    }
}

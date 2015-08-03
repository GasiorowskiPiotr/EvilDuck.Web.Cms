using System.Web.Optimization;

namespace EvilDuck.Platform.Cms
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/admin/app.js"));

            bundles.Add(new StyleBundle("~/css/admin").Include(
                "~/Content/css/font-awesome.css",
                "~/Content/bootstrap.min.css",
                "~/Content/admin/AdminLTE.css",
                "~/Content/admin/skin-blue.min.css",
                "~/Content/Site.css"));
        }
    }
}

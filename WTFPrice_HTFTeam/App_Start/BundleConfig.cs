using System.Web;
using System.Web.Optimization;

namespace WTFPrice_HTFTeam
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/htfjs").Include(
                      "~/Scripts/jquery.min.js", "~/Scripts/bootstrap.min.js",
                      "~/Scripts/jquery.mobilemenu.js", "~/Scripts/jquery.flexslider-min.js",
                      "~/Scripts/jquery.backtotop.js", "~/Scripts/NewsBin/NewsBin.js"));

            bundles.Add(new ScriptBundle("~/bundles/htfslider").Include(
                      "~/Scripts/jssor.js", "~/Scripts/jssor.slider.js",
                      "~/Scripts/vertical-thumbnail.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminjs").Include(
                      "~/Scripts/jquery.min.js", "~/Scripts/bootstrap.min.js", "~/Scripts/metisMenu.min.js",
                      "~/Scripts/sb-admin-2.js", "~/CKEditor/ckeditor.js"));

            bundles.Add(new StyleBundle("~/Content/htfcss").Include(
                      "~/Content/bootstrap.min.css", "~/Content/fontawesome-4.3.0.min.css", "~/Content/custom.flexslider.css", "~/Content/framework.css",
                      "~/Content/layout.css", "~/Content/htfstyle.css", "~/Content/NewsBin/NewsBin.css"));

            bundles.Add(new StyleBundle("~/Content/admincss").Include(
                      "~/Content/bootstrap.min.css", "~/Content/metisMenu.min.css", "~/Content/sb-admin-2.css",
                      "~/Content/fontawesome-4.3.0.min.css", "~/Content/NewsBin/NewsBin.css"));
        }
    }
}

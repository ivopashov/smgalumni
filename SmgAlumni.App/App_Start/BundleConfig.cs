using System.Web;
using System.Web.Optimization;

namespace SmgAlumni.App
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(
                 new ScriptBundle("~/bundles/js/lib").Include(
                     "~/Scripts/jquery-{version}.js",
                     "~/Scripts/angular.js",
                     "~/Scripts/angular-ui-router.js",
                     "~/Scripts/angular-file-upload-shim.min.js",
                     "~/Scripts/angular-file-upload.min.js",
                     "~/Scripts/loading-bar.js",
                     "~/Scripts/toastr.min.js",
                     "~/Scripts/ngDialog.min.js",
                     "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                     "~/Scripts/angular-sanitize.min.js"));

            bundles.Add(
                new ScriptBundle("~/bundles/js/spa")
                    .Include("~/App/app.js")
                    .IncludeDirectory("~/App/interceptors", "*.js", true)
                    .IncludeDirectory("~/App/directives", "*.js", true)
                    .IncludeDirectory("~/App/services", "*.js", true)
                    .IncludeDirectory("~/App/factories", "*.js", true)
                    .IncludeDirectory("~/App/controllers", "*.js",true)
                    .IncludeDirectory("~/App/controllers/account", "*.js", true)
                    .IncludeDirectory("~/App/controllers/authentication", "*.js", true)
                    .IncludeDirectory("~/App/filters", "*.js", true));


            bundles.Add(
                new StyleBundle("~/bundles/css/site").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/Site.css",
                    "~/Content/toastr.css",
                    "~/Content/less/ng-table.css",
                    "~/Content/loading-bar.css",
                    "~/Content/ngDialog.css",
                    "~/Content/ngDialog-theme-default.css"));
        }
    }
}

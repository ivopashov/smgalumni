using SmgAlumni.App.App_Start;
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
                     "~/Scripts/jquery-1.10.2.js",
                     "~/Scripts/angular.js",
                     "~/Scripts/angular-ui-router.js",
                     "~/Scripts/angular-file-upload-shim.min.js",
                     "~/Scripts/angular-file-upload.min.js",
                     "~/Scripts/loading-bar.js",
                     "~/Scripts/toastr.min.js",
                     "~/Scripts/ngDialog.min.js",
                     "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                     "~/Scripts/angular-sanitize.min.js",
                     "~/Scripts/ng-ckeditor.js",
                     "~/Scripts/ng-table.js",
                     "~/Scripts/spin.js",
                     "~/Scripts/angular-loading.js",
                     "~/Scripts/ng-ckeditor.js",
                     "~/Scripts/ng-tags-input.js",
                     "~/Scripts/lodash.js"));


            bundles.Add(
                new ScriptBundle("~/bundles/js/spa")
                    .Include("~/App/app.js")
                     .IncludeDirectory("~/App/interceptors", "*.js", true)
                     .IncludeDirectory("~/App/directives", "*.js", true)
                     .IncludeDirectory("~/App/services", "*.js", true)
                     .IncludeDirectory("~/App/factories", "*.js", true)
                     .IncludeDirectory("~/App/controllers", "*.js", true));


            bundles.Add(
                new StyleBundle("~/bundles/css/site").Include(
                    "~/Content/bootstrap/bootstrap.css",
                    "~/Content/main.css",
                    "~/Content/toastr.css",
                    "~/Content/loading-bar.css",
                    "~/Content/ngDialog.css",
                    "~/Content/angularLoading.css",
                    "~/Content/ngDialog-theme-default.css",
                    "~/Content/ng-ckeditor.css",
                    "~/Content/ng-table.css",
                    "~/Content/ng-tags-input.css"
                    ));


            BundleTable.EnableOptimizations = false;
        }

    }
}

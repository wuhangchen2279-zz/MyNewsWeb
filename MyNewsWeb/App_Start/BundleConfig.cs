using System.Web;
using System.Web.Optimization;

namespace MyNewsWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.12.4.js",
                        "~/Scripts/jquery-ui-1.12.1.js",
                        "~/Scripts/myScripts/JQueryUI.js",
                        "~/Scripts/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/pdfgenerator").Include(
                        "~/Scripts/canvg.js",
                        "~/Scripts/jspdf.min.js",
                        "~/Scripts/html2canvas.min.js",
                        "~/Scripts/myScripts/SavePDF.js"));

            bundles.Add(new ScriptBundle("~/bundles/reCAPTCHA").Include(
                        "~/Scripts/recaptcha_api.js",
                        "~/Scripts/myScripts/reCaptchaVerify.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/angular.js",
                      "~/Scripts/ng-text-truncate.js",
                      "~/Scripts/Chart.min.js",
                      "~/Scripts/angular-chart.min.js",
                      "~/Scripts/myScripts/AngularForAdmin.js",
                      "~/Scripts/smart-table.js",
                      "~/Scripts/angularjs-dropdown-multiselect.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery.dataTables.min.css"));
        }
    }
}

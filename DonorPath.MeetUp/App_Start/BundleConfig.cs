using System.Web;
using System.Web.Optimization;

namespace DonorPath.MeetUp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*", 
                        "~/Scripts/mvcextensions.js"));

            bundles.Add(new ScriptBundle("~/bundles/donorpathmeetup").Include(
                        "~/Scripts/donorpath.meetup.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/moment.js",
                        "~/Scripts/moment-timezone-with-data.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerydatetimepicker").Include(
                        "~/Scripts/jquery.datetimepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusiveajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/jquerydatetimepicker").Include(
                      "~/Content/jquery.datetimepicker.css"));
        }
    }
}

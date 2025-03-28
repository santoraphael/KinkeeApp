using System.Web.Optimization;

namespace Plataforma
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/modules/css/banner").Include(
                      "~/modules/css/banner.css"));

            bundles.Add(new StyleBundle("~/index/css").Include(
                      "~/modules/css/index.css"));

            bundles.Add(new StyleBundle("~/chat/css").Include(
                      "~/modules/css/chat.css"));

            bundles.Add(new StyleBundle("~/editarperfil/css").Include(
                      "~/modules/css/EditarPerfil.css"));

            bundles.Add(new StyleBundle("~/Fonts").Include(
                      "~/modules/css/fonts.css"));

            bundles.Add(new StyleBundle("~/pulsar/Kinkee/assets/css/sl.css").Include(
                      "~/pulsar/Kinkee/assets/css/sl/bundle6514.css",
                      "~/pulsar/Kinkee/assets/css/sl/bootstrap-sl-common6514.css"));

            bundles.Add(new StyleBundle("~/pulsar/Kinkee/assets/css/sl/app/home.css").Include(
                      "~/pulsar/Kinkee/assets/css/sl/app/home/home.css"));


            bundles.Add(new StyleBundle("~/modules/css/Wizard").Include(
                      "~/modules/css/Wizard.css"));

            bundles.Add(new StyleBundle("~/modules/css/certification-modal-intro").Include("~/modules/css/certification-modal-intro.css"));




            bundles.Add(new ScriptBundle("~/modules/js/wizard").Include(
                      "~/modules/js/wizard.js"));

            bundles.Add(new ScriptBundle("~/Home/js").Include(
                      "~/modules/js/home.js"));

            bundles.Add(new ScriptBundle("~/Sobre/js").Include(
                      "~/pulsar/accounts/js/allsites/allsites.min6514.js?v=1476702930",
                      "~/pulsar/Kinkee/js/common/script.min6514.js?v=1476702930"));

            bundles.Add(new ScriptBundle("~/Events/events").Include(
                      "~/Events/events.js"));

            bundles.Add(new ScriptBundle("~/modules/js/home").Include(
                      "~/modules/js/home.js"));

            bundles.Add(new ScriptBundle("~/modules/js/layout").Include(
                      "~/modules/js/layout.js"));
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


            BundleTable.EnableOptimizations = true;
        }
    }
}

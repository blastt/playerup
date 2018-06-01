using System.Web;
using System.Web.Optimization;

namespace Market.Web
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // готово к выпуску, используйте средство сборки по адресу https://modernizr.com, чтобы выбрать только необходимые тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/accordion").Include(
                      "~/Content/Accordion.css"));

            bundles.Add(new StyleBundle("~/bundles/accordion").Include(
                      "~/Scripts/Accordion.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                      "~/Scripts/jquery-ui.js"));


            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
                      "~/Content/jquery-ui.css"));





            bundles.Add(new ScriptBundle("~/bundles/Custom").Include(
                      "~/Scripts/Custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr-messages").Include(
                      "~/Scripts/signalr-messages.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-create-offer").Include(
                      "~/Scripts/jquery-create-offer.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-dialogs").Include(
                      "~/Scripts/jquery-dialogs.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-feedback-details").Include(
                      "~/Scripts/jquery-feedback-details.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-messages").Include(
                      "~/Scripts/jquery-messages.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-modal-feedback").Include(
                      "~/Scripts/jquery-modal-feedback.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-modal-send").Include(
                      "~/Scripts/jquery-modal-send.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-modal").Include(
                      "~/Scripts/jquery-modal.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-user-info").Include(
                      "~/Scripts/jquery-user-info.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-modal-send").Include(
                      "~/Scripts/jquery-modal-send.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-modal-send").Include(
                      "~/Scripts/jquery-modal-send.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-modal-send").Include(
                      "~/Scripts/jquery-modal-send.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-modal-send").Include(
                      "~/Scripts/jquery-modal-send.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-modal-send").Include(
                      "~/Scripts/jquery-modal-send.js"));


        }
    }
}

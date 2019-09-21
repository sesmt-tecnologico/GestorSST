using System.Web.Optimization;

namespace GISWeb.App_Start
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jQuery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jQuery/jquery.validate*",
                        "~/Scripts/jQuery/jquery.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/jQuery/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/ace_js_master").Include(
                        "~/Scripts/Ace/bootstrap.js",
                        "~/Scripts/Ace/bootbox.js",
                        "~/Scripts/Ace/ace.js",
                        "~/Scripts/Ace/ace.basics.js",
                        "~/Scripts/Ace/ace.ajax-content.js",
                        "~/Scripts/Ace/ace.sidebar.js",
                        "~/Scripts/Ace/ace.sidebar-scroll-1.js",
                        "~/Scripts/Ace/elements.scroller.js",
                        "~/Scripts/Ace/elements.aside.js",
                        "~/Scripts/Custom/jquery.ui.datepicker-pt-BR.js",
                        "~/Scripts/Ace/jQ/jquery-ui.js",
                        "~/Scripts/Ace/jQ/jquery-ui.custom.js",
                        "~/Scripts/Ace/jQ/jquery.ui.touch-punch.js",
                        "~/Scripts/Ace/jQ/jquery.gritter.js",
                        "~/Scripts/Ace/chosen.jquery.js",
                        "~/Scripts/Pace/pace.js"));

            bundles.Add(new ScriptBundle("~/bundles/ace_js_extra").Include(
                        "~/Scripts/Ace/ace-extra.js"));

            bundles.Add(new StyleBundle("~/bundles/ace_css_master").Include(
                        "~/Content/Ace/Chosen/chosen.min.css",
                        "~/Content/Ace/css/bootstrap.css",
                        "~/Content/Ace/FontAwesome/css/font-awesome.css",
                        "~/Content/Ace/css/jquery-ui.custom.css",
                        "~/Content/Ace/css/jquery-ui.css",
                        "~/Content/Ace/css/jquery.gritter.css",
                        "~/Content/Ace/css/ace-fonts.css",
                        "~/Content/Ace/css/ace.css"));

            bundles.Add(new ScriptBundle("~/bundles/jQ_dataTable").Include(
                        "~/Scripts/Ace/dataTables/jquery.dataTables.js",
                        "~/Scripts/Ace/dataTables/jquery.dataTables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jQ_tableTools").Include(
                        "~/Scripts/Ace/dataTables/dataTables.tableTools.js",
                        "~/Scripts/Ace/dataTables/dataTables.colVis.js"));

            bundles.Add(new ScriptBundle("~/bundles/ace_js_fileUpload").Include(
                        "~/Scripts/Ace/dropzone.js"));

            bundles.Add(new StyleBundle("~/bundles/ace_css_fileUpload").Include(
                        "~/Content/Ace/css/dropzone.css"));

            bundles.Add(new ScriptBundle("~/bundles/croppie_js").Include(
                        "~/Scripts/Croppie/croppie.js",
                        "~/Scripts/Ace/src/elements.fileinput.js"));

            bundles.Add(new StyleBundle("~/bundles/croppie_css").Include(
                        "~/Content/Croppie/croppie.css"));

            bundles.Add(new StyleBundle("~/bundles/chosen_css").Include(
                        "~/Components/chosen/chosen.css"));


            //####################################################################



        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;
using System.IO;

namespace NGChat
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            var appBundle = new ScriptBundle("~/bundles/scripts/app");
            appBundle.Orderer = new AsIsBundleOrderer();
            appBundle.Include(
                // bootstrap
                "~/Scripts/app/components/ui-bootstrap/ui-bootstrap-tpls-0.3.0.js",

                // app
                "~/Scripts/app/app.js",
                "~/Scripts/app/directives/scrollOnRefresh.js",
                "~/Scripts/app/directives/notifyChatConnection.js",
                "~/Scripts/app/directives/chatNotifications.js",
                "~/Scripts/app/directives/submitTextarea.js",
                "~/Scripts/app/directives/errors.js",
                "~/Scripts/app/directives/serverValidation.js",
                "~/Scripts/app/services/enumFactory.js",
                "~/Scripts/app/services/userFactory.js",
                "~/Scripts/app/services/chatFactory.js",
                "~/Scripts/app/controllers/HomeController.js",
                "~/Scripts/app/controllers/LogInController.js",
                "~/Scripts/app/controllers/NavController.js",
                "~/Scripts/app/modules/chatModule.js"

                );

            // normal "Bundle" used in order to load and bundle ".min.js" files instead of minifying them
            // libs files were previously minified, so they don't need re-minifying
            var libsBundle = new Bundle("~/bundles/scripts/libs");
            libsBundle.Orderer = new AsIsBundleOrderer();
            libsBundle.Include(
                "~/Scripts/libs/jquery/jquery-2.0.0.js",
                "~/Scripts/libs/signalr/jquery.signalR-1.1.2.js",
                //"~/Scripts/libs/bootstrap/bootstrap.js",
                "~/Scripts/libs/misc/semicolonFix.js",
                "~/Scripts/libs/angular/angular.js",
                "~/Scripts/libs/angular/i18n/angular-locale_pl-pl.js",
                "~/Scripts/libs/angular/angular-resource.js",
                "~/Scripts/libs/angular/angular-sanitize.js",
                "~/Scripts/libs/desktop-notify/desktop-notify.js"
                );

            var stylesBundle = new StyleImagePathBundle("~/bundles/styles");
            stylesBundle.Orderer = new AsIsBundleOrderer();
            stylesBundle.Include(
                // jquery
                //"~/Content/styles/jquery/jquery.ui.core.css",
                //"~/Content/styles/jquery/jquery.ui.resizable.css",
                //"~/Content/styles/jquery/jquery.ui.selectable.css",
                //"~/Content/styles/jquery/jquery.ui.accordion.css",
                //"~/Content/styles/jquery/jquery.ui.autocomplete.css",
                //"~/Content/styles/jquery/jquery.ui.button.css",
                //"~/Content/styles/jquery/jquery.ui.dialog.css",
                //"~/Content/styles/jquery/jquery.ui.slider.css",
                //"~/Content/styles/jquery/jquery.ui.tabs.css",
                //"~/Content/styles/jquery/jquery.ui.datepicker.css",
                //"~/Content/styles/jquery/jquery.ui.progressbar.css",
                //"~/Content/styles/jquery/jquery.ui.theme.css",

                // bootstrap
                "~/Content/styles/bootstrap/bootstrap.css",
                "~/Content/styles/bootstrap/bootstrap-responsive.css",

                // reset styles - not needed (bootstrap is already using it)
                //"~/Content/styles/normalize.css",

                // main app
                "~/Content/styles/site.css");

            bundles.Add(libsBundle);
            bundles.Add(appBundle);
            bundles.Add(stylesBundle);
        }
    }

    public class AsIsBundleOrderer : IBundleOrderer
    {
        public virtual IEnumerable<FileInfo> OrderFiles(BundleContext context, IEnumerable<FileInfo> files)
        {
            return files;
        }
    }

    public class StyleImagePathBundle : Bundle
    {
        public StyleImagePathBundle(string virtualPath) : base(virtualPath, new IBundleTransform[1]
      {
        (IBundleTransform) new CssMinify()
      })
        {
        }

        public StyleImagePathBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath, new IBundleTransform[1]
      {
        (IBundleTransform) new CssMinify()
      })
        {
        }

        public new Bundle Include(params string[] virtualPaths)
        {
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                // Debugging. Bundling will not occur so act normal and no one gets hurt.
                base.Include(virtualPaths.ToArray());
                return this;
            }

            // In production mode so CSS will be bundled. Correct image paths.
            var bundlePaths = new List<string>();
            var svr = HttpContext.Current.Server;
            foreach (var path in virtualPaths)
            {
                var pattern = new Regex(@"url\s*\(\s*([""']?)([^:)]+)\1\s*\)", RegexOptions.IgnoreCase);
                var contents = System.IO.File.ReadAllText(svr.MapPath(path));
                if (!pattern.IsMatch(contents))
                {
                    bundlePaths.Add(path);
                    continue;
                }


                var bundlePath = (System.IO.Path.GetDirectoryName(path) ?? string.Empty).Replace(@"\", "/") + "/";
                var bundleUrlPath = VirtualPathUtility.ToAbsolute(bundlePath);
                var bundleFilePath = String.Format("{0}{1}.bundle{2}",
                                                   bundlePath,
                                                   System.IO.Path.GetFileNameWithoutExtension(path),
                                                   System.IO.Path.GetExtension(path));
                contents = pattern.Replace(contents, "url($1" + bundleUrlPath + "$2$1)");
                System.IO.File.WriteAllText(svr.MapPath(bundleFilePath), contents);
                bundlePaths.Add(bundleFilePath);
            }
            base.Include(bundlePaths.ToArray());
            return this;
        }

    }

}
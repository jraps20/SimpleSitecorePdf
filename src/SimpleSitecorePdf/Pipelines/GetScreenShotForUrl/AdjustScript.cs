using Sitecore.ContentTesting.Pipelines.GetScreenShotForURL;

namespace SimpleSitecorePdf.Pipelines.GetScreenShotForUrl
{
    public class AdjustScript : GenerateScreenShotProcessor
    {
        public string Timeout { get; set; }

        public bool DirectPdf { get; set; }

        private const string StandardExit = "phantom.exit();\r\n});";

        private string TimeoutExit => "window.setTimeout(function () {\r\n    page.paperSize = {format: 'A4'};\r\n    page.settings.dpi = '96';\r\n    page.render('%outfile%', {format: '" + (DirectPdf ? "pdf" : "png") + "'});\r\n    phantom.exit();\r\n  }, " + Timeout + ");\r\n});";

        public override void Process(GetScreenShotForURLArgs args)
        {
            args.Script = args.Script.Replace(StandardExit, TimeoutExit);

            var outputFilePath = Sitecore.ContentTesting.Helpers.PathHelper.MapPath(args.OutputFilename).Replace("\\", "\\\\");

            // script cleanup
            args.Script = args.Script.Replace($"page.render('{outputFilePath}');", "");
            args.Script = args.Script.Replace("%outfile%", outputFilePath);

            if (!DirectPdf)
                return;

            args.Script = args.Script.Replace(".png", ".pdf");
            // adjust output file name for later consumption
            args.OutputFilename = args.OutputFilename.Replace(".png", ".pdf");
        }
    }
}
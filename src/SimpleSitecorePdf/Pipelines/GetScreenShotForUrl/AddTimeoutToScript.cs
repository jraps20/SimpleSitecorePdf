using Sitecore.ContentTesting.Pipelines.GetScreenShotForURL;

namespace SimpleSitecorePdf.Pipelines.GetScreenShotForUrl
{
    public class AddTimeoutToScript : GenerateScreenShotProcessor
    {
        public string Timeout { get; set; }

        private const string StandardExit = "phantom.exit();\r\n});";

        private string TimeoutExit => "window.setTimeout(function () {\r\n    page.render('%outfile%');\r\n    phantom.exit();\r\n  }, " + Timeout + ");\r\n});";

        public override void Process(GetScreenShotForURLArgs args)
        {
            args.Script = args.Script.Replace(StandardExit, TimeoutExit);

            var outputFilePath = Sitecore.ContentTesting.Helpers.PathHelper.MapPath(args.OutputFilename).Replace("\\", "\\\\");

            // script cleanup
            args.Script = args.Script.Replace($"page.render('{outputFilePath}');", "");
            args.Script = args.Script.Replace("%outfile%", outputFilePath);
        }
    }
}
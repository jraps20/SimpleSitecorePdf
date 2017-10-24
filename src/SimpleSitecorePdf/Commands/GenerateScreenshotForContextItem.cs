using System.Linq;
using Sitecore.ContentTesting.Pipelines.GetScreenShotForURL;
using Sitecore.Pipelines;
using Sitecore.Shell.Framework.Commands;

namespace SimpleSitecorePdf.Commands
{
    /// <summary>
    /// Attach this command to a button in the Core database (ribbon) for display in Content Editor
    /// 
    /// Clicking the button will generate a PDF of the selected item in ~\temp\screenshots\{obfuscated item name}.pdf
    /// </summary>
    public class GenerateScreenshotForContextItem : Command
    {
        public override void Execute(CommandContext context)
        {
            var item = context.Items.FirstOrDefault();

            if (item == null)
                return;

            var args = new GetScreenShotForURLArgs(item.ID);

            CorePipeline.Run("getScreenShotForURL", args);
        }
    }
}
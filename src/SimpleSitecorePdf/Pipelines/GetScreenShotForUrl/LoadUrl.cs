using System;
using System.Linq;
using Sitecore.ContentTesting.Pipelines.GetScreenShotForURL;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Web;

namespace SimpleSitecorePdf.Pipelines.GetScreenShotForUrl
{
    public class LoadUrl : GenerateScreenShotProcessor
    {
        public string DatabaseName { get; set; }

        public Database ContextDatabase => Sitecore.Configuration.Factory.GetDatabase(DatabaseName);

        public override void Process(GetScreenShotForURLArgs args)
        {
            Assert.IsNotNull(args, nameof(args));
            
            var item = ContextDatabase.GetItem(args.ItemId);

            if(item == null)
                Log.Warn($"Unable to find item {args.ItemId} in database: {ContextDatabase}", this);

            using (new DatabaseSwitcher(ContextDatabase))
            {
                var site = ResolveContextSite(item);

                if (site == null)
                    Log.Warn($"Unable to find site for item {args.ItemId}", this);

                using (new SiteContextSwitcher(new SiteContext(site)))

                    args.Url = WebUtil.GetServerUrl() + LinkManager.GetItemUrl(item);
            }
        }

        private static SiteInfo ResolveContextSite(Item item)
        {
            var itemPath = item.Paths.FullPath;
            return SiteContextFactory.Sites
                .Where(s => s.RootPath != "" & itemPath.StartsWith(s.RootPath, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(s => s.RootPath.Length)
                .FirstOrDefault();
        }
    }
}
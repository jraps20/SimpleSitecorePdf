using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Sitecore.ContentTesting.Pipelines.GetScreenShotForURL;

namespace SimpleSitecorePdf.Pipelines.GetScreenShotForUrl
{
    public class GeneratePdf : GenerateScreenShotProcessor
    {
        public override void Process(GetScreenShotForURLArgs args)
        {
            var imagePath = Sitecore.ContentTesting.Helpers.PathHelper.MapPath(args.OutputFilename);
            var pdfPath = imagePath.Replace(".png", ".pdf");

            using (var doc = new PdfDocument())
            {
                var img = XImage.FromFile(imagePath);

                var page = new PdfPage
                {
                    Height = img.PointHeight,
                    Width = img.PointWidth
                };
                doc.Pages.Add(page);
                var xgr = XGraphics.FromPdfPage(doc.Pages[0]);

                xgr.DrawImage(img, 0, 0);
                doc.Save(pdfPath);
            }
        }
    }
}
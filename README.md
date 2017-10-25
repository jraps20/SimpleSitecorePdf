# SimpleSitecorePdf
To modify source solution, add **Sitecore.ContentTesting.dll** and **Sitecore.Kernel.dll** to the **~\src\lib** directory for your given Sitecore version.

# Quick Start Guides
## Direct To PDF
This option is the most ideal. It relies on zero external dependencies and exclusively uses out-of-the-box Sitecore functionality. This method uses PhantomJS to generate the PDF. PhantomJS uses the *"print"* media when rendering directly to PDF. If you desire the *"screen"* media to be representative on a PDF, please use the *Image To PDF* method. If your CSS does not specify a difference between *"print"* and *"screen"*, then you can most likely use this method as well.

**_This method will generate a paged PDF by default, but can be easily updated via the source if desired._**

1. Copy contents of **~\quickstart\DirectToPdf** directly into a Sitecore web root
2. Install package **~\src\SitecorePackages\Create PDF Command.zip**
    * This will install a command item to **[core]/sitecore/content/Applications/Content Editor/Ribbons/Chunks/Operations/Create PDF**
	* You can then switch to the **[master]** database and view the command button at **Home > Operations > Create PDF**
3. From Content Editor, select an item with a layout and click **Create PDF**
4. A PDF of the item will be generated to **[webroot]\temp\screenshots**
    * The name of the item is obfuscated as part of the screenshot generation process from Sitecore
	
## Image To PDF
Use this option if you have specific "print" styles that you do not wish to render to the PDF. This method first uses PhantomJS to generate a PNG of the Sitecore item (which uses the "screen" media type by default). It then uses the PDFSharp library to map the image to a PDF. 

**_This method will generate a single PDF, non-paged. This is not easily updated via the source._**

1. Copy contents of **~\quickstart\DirectToPdf** directly into a Sitecore web root
2. Install package **~\src\SitecorePackages\Create PDF Command.zip**
    * This will install a command item to **[core]/sitecore/content/Applications/Content Editor/Ribbons/Chunks/Operations/Create PDF**
	* You can then switch to the **[master]** database and view the command button at **Home > Operations > Create PDF**
3. From Content Editor, select an item with a layout and click **Create PDF**
4. A PDF of the item will be generated to **[webroot]\temp\screenshots**
    * The name of the item is obfuscated as part of the screenshot generation process from Sitecore
	
# Extending

It's very simple to use this tool in other ways than the example command, simply call the correct pipeline with appropriate arguments

```cs
 var args = new GetScreenShotForURLArgs(item.ID);
 CorePipeline.Run("getScreenShotForURL", args);
```

Notice all it takes as a parameter is a **Sitecore Item ID**. 

`args.OutputFilename` contains the path to the generated image or PDF and is accessible after the pipeline has finished executing. From this, you can open the PDF from disk and send to the user.

# Summary

The resulting PDf can be modified by adjusting settings in the native **Sitecore.ContentTesting.config** file. Aspect ratio, JavaScript and many other settings can be adjusted.

By extending the `getScreenShotForURL` pipeline with extra processors, additional information can be passed to **PhantomJS** prior to generation.  For example, if your page is behind a login, you can pass in a cookie to allow **PhantomJS** to consume the page properly. You could also dynamically adjust the view port, or generate multiple rendered sizes of a page.

Also worth noting, by using this code, the original screenshot testing functionality will be broken. If your use-case relies on this code, you will need to modify the configs and adjust the `PopulateUrlParameters` and `AdjustScript` processors respectively.
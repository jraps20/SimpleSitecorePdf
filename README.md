# SimpleSitecorePdf
To modify source solution, add **Sitecore.ContentTesting.dll** and **Sitecore.Kernel.dll** to the **~\src\lib** directory for your given Sitecore version.

# Quick Start Guides
Use one of the two quick start guides below to quickly get up and running with this functionality.
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

Also worth noting, by using this code, the original screenshot testing functionality will be broken. If your application relies on this code, you will need to modify the configs and adjust the `PopulateUrlParameters` and `AdjustScript` processors respectively.

# Troubleshooting

If you're having difficulty rendering a PDF, here are a few tips to help troubleshoot.

### No File Generated

During the generation of the PDF, Sitecore creates a JavaScript file in the **[webroot]\temp\screenshots** directory. The file has an obfuscated name, but if you are in this directory during generation you'll know it's for your current request. I recommend opening this file quickly in an editor because once generation is complete, the file is deleted.

With the file contents, you can then execute PhantomJS manually. In my testing, this is the easiest way to find issues. Because PhantomJS is an executable, there is not great place (that I've found) to see errors during generation without manually running it.

*Run the following command from the working directory* **_[webroot]\temp\screenshots_**

`CMD> C:\absolute\path\to\phantomjs.exe --ssl-protocl=any ObfuscatedJSFile.js`

If it throws errors, google for them :)

### Incorrect URL Being Generated

See the "No File Generated" section for viewing the JS file Sitecore creates.  You will see the requested URL in there. If it isn't what you were hoping, you can modify the `LoadUrl` processor and adapt it to create your URL properly- for example, adding new query string parameters.

### Page Requires Authentication

Since pipelines run outside the context of a request, you can pass in cookies before the pipeline is ran.
*Non-tested pseudo code below, but you get the gist of it*
```cs
 var args = new GetScreenShotForURLArgs(item.ID);
 args.Cookies = new List<KeyValuePair<string, string>>(){new KeyValuePair<string, string>("cookiename", "cookievalue"};
 CorePipeline.Run("getScreenShotForURL", args);
```

### List of properties that can be set prior to generation

*Note: Because by default this Sitecore framework is intended for testing, properties like Revision, Version, etc. are included. You can use them as part of this library, but will need to account for them with your own custom processors.*

**Sitecore.ContentTesting.Pipelines.GetScreenShotForURL.GetScreenShotForURLArgs**

```cs
 public ID ItemId { get; protected set; }

 public string Revision { get; set; }

 public string Language { get; set; }

 public int? Version { get; set; }

 public ID DeviceId { get; set; }

 public int? CompareVersion { get; set; }

 public string Combination { get; set; }

 public IEnumerable<ShortID> MvVariants { get; set; }

 public IEnumerable<ID> Rules { get; set; }

 public IEnumerable<ID> Variants { get; set; }

 public IEnumerable<KeyValuePair<string, string>> Cookies { get; set; }

 [Obsolete("No longer supported. Use locking to ensure single generation per file.")]
 public bool IgnoreAlreadyGenerating { get; set; }

 public string Url { get; set; }

 public string Script { get; set; }

 public string ImageBase64 { get; set; }

 public string OutputFilename { get; set; }

 [Obsolete("Use locking to avoid multiple generations for the same screenshot at once.")]
 public bool AlreadyGenerating { get; set; }

 public Size ViewPortSize { get; set; }

 public NameValueCollection UrlParameters { get; set; }
```

### Default Pipeline Processors
```xml
 <getScreenShotForURL>
  <processor type="Sitecore.ContentTesting.Pipelines.GetScreenShotForURL.GenerateFilename, Sitecore.ContentTesting" />
  <processor type="Sitecore.ContentTesting.Pipelines.GetScreenShotForURL.CheckCachedImage, Sitecore.ContentTesting" />
  <processor type="Sitecore.ContentTesting.Pipelines.GetScreenShotForURL.CheckDisabler, Sitecore.ContentTesting" />
  <processor type="Sitecore.ContentTesting.Pipelines.GetScreenShotForURL.PopulateUrlParameters, Sitecore.ContentTesting" />
  <processor type="Sitecore.ContentTesting.Pipelines.GetScreenShotForURL.LoadUrl, Sitecore.ContentTesting" />
  <processor type="Sitecore.ContentTesting.Pipelines.GetScreenShotForURL.RenderScripts, Sitecore.ContentTesting" />
  <processor type="Sitecore.ContentTesting.Pipelines.GetScreenShotForURL.WriteScriptToDisk, Sitecore.ContentTesting" />
  <processor type="Sitecore.ContentTesting.Pipelines.GetScreenShotForURL.GenerateScreenShot, Sitecore.ContentTesting" />
  <processor type="Sitecore.ContentTesting.Pipelines.GetScreenShotForURL.DeleteScript, Sitecore.ContentTesting" />
 </getScreenShotForURL>
```
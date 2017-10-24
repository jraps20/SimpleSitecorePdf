# SimpleSitecorePdf
To modify solution, add Sitecore.ContentTesting.dll and Sitecore.Kernel.dll to the ~\src\lib\ directory for your given Sitecore version.

# Quick Start

1 Copy contents of **~\quickstart** directly into a web root
2 Install package **~\src\SitecorePackages\Create PDF Command.zip**
    * This will install a command item to **[core]/sitecore/content/Applications/Content Editor/Ribbons/Chunks/Operations/Create PDF**
	* You can then switch to the **[master]** database and view the command button at **Home > Operations > Create PDF**
3 From Content Editor, select an item with a layout and click **Create PDF**
4 A PDF of the item will be generated to **<webroot>\temp\screenshots**
    * The name of the item is obfuscated as part of the screenshot generation process from Sitecore
	
# Extending

It's very simple to use this tool in other ways, simply call the correct pipeline with appropriate arguments

```
 var args = new GetScreenShotForURLArgs(item.ID);
 CorePipeline.Run("getScreenShotForURL", args);
```

All it takes as a parameter is an item ID.  

# Summary

This library uses the out-of-the-box Sitecore screenshot functionality to first create an image of the page using PhantomJS, then map it to a PDF using PDFSharp.

The resulting screenshot can be modified by adjusting settings in the native Sitecore.ContentTesting.config file. Aspect ratio, JavaScript and many other settings can be adjusted.

By extending the pipeline further via additional processors, additional information can be passed to PhantomJS prior to generation.  For example, if your page is behind a login, you can pass in a cookie to allow PhantomJS to consume the page properly.

Also worth noting, by using this code, the original screenshot testing functionality will be broken. If your use-case relies on this code, you will need to modify the SimpleSitecorePdf.config and remove the **<patch:delete />** of the Check Disabler and adjust the Populate Url Parameters processor accordingly.
# HtmlToXslFo
HTML to XSL-FO (C#) convert utility. This lite can then be used in FO.NET to convert HTML files into PDF documents 

**Note:** To convert you generated XSL-FO file to PDF you can use a free utility like [FO.NET](https://fonet.codeplex.com/)

Sample Usage:
<pre>
var fo = HtmlToXslFo.Convert(ReadFile(@"simple.htm"));
//File.WriteAllText(@"C:\temp\simple.fo.xml", fo);

var bytes = RenderFo2Pdf(fo);
Assert.GreaterOrEqual(bytes.Length, 2057);
//File.WriteAllBytes(@"C:\temp\tests\demo.pdf", bytes);

private static byte[] RenderFo2Pdf(string xmlFo)
{
    var foReader = new XmlTextReader(new StringReader(xmlFo));

    var options = new PdfRendererOptions();
    FonetDriver driver = FonetDriver.Make();
    driver.OnError += HandleError;
    using (var stream = new MemoryStream())
    {
        driver.Options = options;
        driver.Render(foReader, stream);
        return stream.ToArray();
    }
}

</pre>

namespace HtmlToXmlFo.Tests
{
    using System;
    using System.IO;
    using System.Xml;
    using Fonet;
    using Fonet.Render.Pdf;
    using NUnit.Framework;
    using WhichMan.Utilities.HtmlToXslFo;

    [TestFixture]
    public class HtmlDocumentTests: BaseTest
    {
        [Test]
        public void Can_convert_html_document()
        {
            //var fo = HtmlToXslFo.Convert("<html><head><title>La Cosa Nostra</title></head><body><h1>C<font color=blue>h</font>apter 1</h1><h2>Welcome to the Game</h2></body></html>");

            var fo = HtmlToXslFo.Convert(ReadFile(@"simple.htm"));
            File.WriteAllText(@"C:\temp\tests\demo.xml", fo);

            var bytes = RenderFo2Pdf(fo);
            File.WriteAllBytes(@"C:\temp\tests\demo.pdf", bytes);

            //var str = File.ReadAllText(@"data\footnote.xml");
            //File.WriteAllBytes(@"C:\temp\tests\footnote.pdf", RenderFo2Pdf(str));

            Assert.AreEqual("<fo:inline color=\"red\" font-family=\"Arial\">Bereje</fo:inline>", fo);
        }

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

        private static void HandleError(object driver, FonetEventArgs fonetEventArgs)
        {
            var message = fonetEventArgs.GetMessage();
            Console.WriteLine("[ERROR] {0}", message);
        }

        [Test]
        public void Can_convert_layout_master()
        {
            var fo = HtmlToXslFo.Convert("<layout-master></layout-master>");
            Assert.AreEqual("<fo:layout-master-set/>", fo);
        }

        [Test]
        public void Can_convert_simple_page()
        {
            var fo = HtmlToXslFo.Convert("<simple-page></simple-page>");
            Assert.AreEqual("<fo:simple-page-master master-name=\"simple\" page-height=\"11in\" page-width=\"8.5in\" margin-top=\".1in\" margin-bottom=\".5in\" margin-left=\".5in\" margin-right=\".5in\"/>", fo);
        }

        [Test]
        public void Can_convert_region_body()
        {
            var fo = HtmlToXslFo.Convert("<region-body></region-body>");
            Assert.AreEqual("<fo:region-body region-name=\"region-body\" margin-bottom=\".5in\" margin-top=\".5in\"/>", fo);
        }

        [Test]
        public void Can_convert_region_before()
        {
            var fo = HtmlToXslFo.Convert("<region-before></region-before>");
            Assert.AreEqual("<fo:region-before region-name=\"region-before\" extent=\".5in\"/>", fo);
        }

        [Test]
        public void Can_convert_region_after()
        {
            var fo = HtmlToXslFo.Convert("<region-after></region-after>");
            Assert.AreEqual("<fo:region-after region-name=\"region-after\" extent=\".5in\"/>", fo);
        }

        [Test]
        public void Can_convert_page_sequence()
        {
            var fo = HtmlToXslFo.Convert("<page-sequence></page-sequence>");
            Assert.AreEqual("<fo:page-sequence master-reference=\"simple\"/>", fo);
        }
    }
}
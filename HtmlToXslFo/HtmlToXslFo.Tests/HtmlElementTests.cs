using NUnit.Framework;

namespace HtmlToXmlFo.Tests
{
    using WhichMan.Utilities.HtmlToXslFo;

    [TestFixture]
    public class HtmlElementTests
    {
        [Test]
        public void Can_convert_font_element()
        {
            var fo = HtmlToXslFo.Convert("<font color=\"red\" face=\"Arial\">Bereje</font>");
            Assert.AreEqual("<fo:inline color=\"red\" font-family=\"Arial\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_span_element()
        {
            var fo = HtmlToXslFo.Convert("<span style=\"color:red\">Bereje</span>");
            Assert.AreEqual("<fo:inline color=\"red\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_bold_element()
        {
            var fo = HtmlToXslFo.Convert("<b style=\"color:red\">Bereje</b>");
            Assert.AreEqual("<fo:inline font-weight=\"bold\" color=\"red\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_strong_element()
        {
            var fo = HtmlToXslFo.Convert("<strong style=\"color:red\">Bereje</strong>");
            Assert.AreEqual("<fo:inline font-weight=\"bold\" color=\"red\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_italic_element()
        {
            var fo = HtmlToXslFo.Convert("<i style=\"color:red\">Bereje</i>");
            Assert.AreEqual("<fo:inline font-style=\"italic\" color=\"red\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_em_element()
        {
            var fo = HtmlToXslFo.Convert("<em style=\"color:red\">Bereje</em>");
            
            Assert.AreEqual("<fo:inline font-style=\"italic\" color=\"red\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_underline_element()
        {
            var fo = HtmlToXslFo.Convert("<u style=\"color:red\">Bereje</u>");
            Assert.AreEqual("<fo:inline text-decoration=\"underline\" color=\"red\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_subscript_element()
        {
            var fo = HtmlToXslFo.Convert("<sub style=\"color:red\">Bereje</sub>");
            
            Assert.AreEqual("<fo:inline vertical-align=\"sub\" font-size=\"75%\" color=\"red\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_superscript_element()
        {
            var fo = HtmlToXslFo.Convert("<sup style=\"color:red\">Bereje</sup>");
            Assert.AreEqual("<fo:inline vertical-align=\"super\" font-size=\"75%\" color=\"red\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_strike_through_element()
        {
            var fo = HtmlToXslFo.Convert("<s style=\"color:red\">Bereje</s>");
            
            Assert.AreEqual("<fo:inline text-decoration=\"line-through\" color=\"red\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_strike_element()
        {
            var fo = HtmlToXslFo.Convert("<strike style=\"color:red\">Bereje</strike>");
            
            Assert.AreEqual("<fo:inline text-decoration=\"line-through\" color=\"red\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_small_element()
        {
            var fo = HtmlToXslFo.Convert("<small style=\"color:red\">Bereje</small>");
            
            Assert.AreEqual("<fo:inline font-size=\"80%\" color=\"red\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_code_element()
        {
            var fo = HtmlToXslFo.Convert("<code style=\"font-weight:normal\">Bereje</code>");
            
            Assert.AreEqual("<fo:inline font-family=\"monospace\" color=\"navy\" font-weight=\"normal\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_var_element()
        {
            var fo = HtmlToXslFo.Convert("<var style=\"color:green;\">Bereje</var>");
            
            Assert.AreEqual("<fo:inline font-style=\"italic\" font-family=\"monospace\" color=\"green\">Bereje</fo:inline>", fo);
        }

        [Test]
        public void Can_convert_pre_element()
        {
            var fo = HtmlToXslFo.Convert("<pre style=\"font-weight:normal\">Bereje</pre>");
            
            Assert.AreEqual("<fo:block white-space-collapse=\"false\" wrap-option=\"no-wrap\" font-family=\"monospace\" font-weight=\"normal\">Bereje</fo:block>", fo);
        }

        [Test]
        public void Can_convert_image_element()
        {
            var fo = HtmlToXslFo.Convert("<img src=\"http://whichman.com/logo.png\" width=100 height=5em/>");
            
            Assert.AreEqual("<fo:external-graphic src=\"http://whichman.com/logo.png\" width=\"100px\" height=\"5em\"/>", fo);
        }

        [Test]
        public void Can_convert_link_element()
        {
            var fo = HtmlToXslFo.Convert("<a href=\"http://whichman.com\" target=\"_blank\">WhichMan</a>");
            
            Assert.AreEqual("<fo:basic-link text-decoration=\"underline\" color=\"blue\" role=\"external link\" external-destination=\"http://whichman.com\">WhichMan</fo:basic-link>", fo);
        }

        [Test]
        public void Can_convert_pagebreak_before_style()
        {
            var fo = HtmlToXslFo.Convert("<div style=\"page-break-before:always\">Bereje</div>");
            
            Assert.AreEqual("<fo:block break-before=\"page\">Bereje</fo:block>", fo);
        }
        [Test]
        public void Can_convert_pagebreak_after_style()
        {
            var fo = HtmlToXslFo.Convert("<p style=\"page-break-after:always\">Bereje</p>");
            
            Assert.AreEqual("<fo:block space-before=\"3pt\" space-after=\"3pt\" break-after=\"page\">Bereje</fo:block>", fo);
        }
        [Test]
        public void Can_convert_align_left_attribute()
        {
            var fo = HtmlToXslFo.Convert("<div align=\"left\">Bereje</div>");
            
            Assert.AreEqual("<fo:block text-align=\"start\">Bereje</fo:block>", fo);
        }
        [Test]
        public void Can_convert_align_right_attribute()
        {
            var fo = HtmlToXslFo.Convert("<p align=\"right\">Bereje</p>");
            
            Assert.AreEqual("<fo:block space-before=\"3pt\" space-after=\"3pt\" text-align=\"end\">Bereje</fo:block>", fo);
        }
        [Test]
        public void Can_convert_align_center_attribute()
        {
            var fo = HtmlToXslFo.Convert("<div align=\"center\">Bereje</div>");
            
            Assert.AreEqual("<fo:block text-align=\"center\">Bereje</fo:block>", fo);
        }
        [Test]
        public void Can_convert_center_element()
        {
            var fo = HtmlToXslFo.Convert("<center style=\"color:green;\">Bereje</center>");
            
            Assert.AreEqual("<fo:block text-align=\"center\" color=\"green\">Bereje</fo:block>", fo);
        }
        [Test]
        public void Can_convert_h1_element()
        {
            var fo = HtmlToXslFo.Convert("<h1 style=\"color:green;\">Bereje</h1>");
            
            Assert.AreEqual("<fo:block font-size=\"160%\" font-weight=\"bold\" space-before=\"6pt\" space-after=\"6pt\" color=\"green\">Bereje</fo:block>", fo);
        }
        [Test]
        public void Can_convert_h2_element()
        {
            var fo = HtmlToXslFo.Convert("<h2 style=\"color:green;\">Bereje</h2>");
            
            Assert.AreEqual("<fo:block font-size=\"140%\" font-weight=\"bold\" space-before=\"6pt\" space-after=\"6pt\" color=\"green\">Bereje</fo:block>", fo);
        }
        [Test]
        public void Can_convert_h3_element()
        {
            var fo = HtmlToXslFo.Convert("<h3 style=\"color:green;\">Bereje</h3>");
            
            Assert.AreEqual("<fo:block font-size=\"120%\" font-weight=\"bold\" space-before=\"6pt\" space-after=\"6pt\" color=\"green\">Bereje</fo:block>", fo);
        }
        [Test]
        public void Can_convert_h4_element()
        {
            var fo = HtmlToXslFo.Convert("<h4 style=\"color:green;\">Bereje</h4>");
            
            Assert.AreEqual("<fo:block font-size=\"100%\" font-weight=\"bold\" space-before=\"6pt\" space-after=\"6pt\" color=\"green\">Bereje</fo:block>", fo);
        }
        [Test]
        public void Can_convert_h5_element()
        {
            var fo = HtmlToXslFo.Convert("<h5 style=\"color:green;\">Bereje</h5>");
            
            Assert.AreEqual("<fo:block font-size=\"80%\" font-weight=\"bold\" space-before=\"6pt\" space-after=\"6pt\" color=\"green\">Bereje</fo:block>", fo);
        }

        [Test]
        public void Can_convert_br_element()
        {
            var fo = HtmlToXslFo.Convert("<br/>");
            
            Assert.AreEqual("<fo:block>&#xA;</fo:block>", fo);
        }
        [Test]
        public void Can_convert_hr_element()
        {
            var fo = HtmlToXslFo.Convert("<hr/>");
            
            Assert.AreEqual("<fo:block><fo:leader leader-pattern=\"rule\" leader-length.optimum=\"100%\" rule-style=\"double\" rule-thickness=\"0.1mm\"/></fo:block>", fo);
        }

        [Test]
        public void Can_convert_body_element()
        {
            var fo = HtmlToXslFo.Convert("<body>Hello</body>");
            
            Assert.AreEqual("<fo:flow flow-name=\"region-body\"><fo:block>Hello</fo:block></fo:flow>", fo);
        }

        [Test]
        public void Can_convert_html_element()
        {
            var fo = HtmlToXslFo.Convert("<html/>");
            Assert.AreEqual("<fo:root xmlns:fo=\"http://www.w3.org/1999/XSL/Format\"/>", fo);
        }

        [Test]
        public void Can_convert_td_element()
        {
            var fo = HtmlToXslFo.Convert("<td>Hello</td>");
            Assert.AreEqual("<fo:table-cell><fo:block>Hello</fo:block></fo:table-cell>", fo);
        }

        [Test]
        public void Can_convert_th_element()
        {
            var fo = HtmlToXslFo.Convert("<th>Hello</th>");
            Assert.AreEqual("<fo:table-cell><fo:block font-weight=\"bold\" text-align=\"center\">Hello</fo:block></fo:table-cell>", fo);
        }

        [Test]
        public void Can_convert_tr_element()
        {
            var fo = HtmlToXslFo.Convert("<tr></tr>");
            Assert.AreEqual("<fo:table-row/>", fo);
        }

        [Test]
        public void Can_convert_tbody_element()
        {
            var fo = HtmlToXslFo.Convert("<tbody></tbody>");
            Assert.AreEqual("<fo:table-body/>", fo);
        }

        [Test]
        public void Can_convert_thead_element()
        {
            var fo = HtmlToXslFo.Convert("<thead></thead>");
            Assert.AreEqual("<fo:table-header/>", fo);
        }

        [Test]
        public void Can_convert_table_element()
        {
            var fo = HtmlToXslFo.Convert("<table></table>");
            Assert.AreEqual("<fo:table><fo:table-body/></fo:table>", fo);

            var fo2 = HtmlToXslFo.Convert("<table><tr><td width=\"25mm\">Hello</td></tr></table>");
            Assert.AreEqual("<fo:table><fo:table-column column-width=\"25mm\"/><fo:table-body><fo:table-row><fo:table-cell><fo:block>Hello</fo:block></fo:table-cell></fo:table-row></fo:table-body></fo:table>", fo2);

        }

        [Test]
        public void Can_convert_table_column_element()
        {
            var fo = HtmlToXslFo.Convert("<table-column width=\"25mm\"/>");
            Assert.AreEqual("<fo:table-column column-width=\"25mm\"/>", fo);
        }

        [Test]
        public void Can_convert_ul_element()
        {
            var fo = HtmlToXslFo.Convert("<ul/>");
            Assert.AreEqual("<fo:list-block provisional-distance-between-starts=\"1cm\" provisional-label-separation=\"0.5cm\" space-after=\"12pt\" start-indent=\"1cm\"/>", fo);
        }

        [Test]
        public void Can_convert_ol_element()
        {
            var fo = HtmlToXslFo.Convert("<ol/>");
            Assert.AreEqual("<fo:list-block provisional-distance-between-starts=\"1cm\" provisional-label-separation=\"0.5cm\" space-after=\"12pt\" start-indent=\"1cm\"/>", fo);
        }

        [Test]
        public void Can_convert_ul_li_element()
        {
            const string expected = @"
<fo:list-item>
<fo:list-item-label end-indent=""label-end()"">
<fo:block>&#x02022;</fo:block>
</fo:list-item-label>
<fo:list-item-body start-indent=""body-start()"">
<fo:block>The Game</fo:block>
</fo:list-item-body>
</fo:list-item>
";
            var fo = HtmlToXslFo.Convert("<li>The Game</li>");
            Assert.AreEqual(expected.Replace("\r\n", ""), fo);
        }

        [Test]
        public void Can_convert_ol_li_element()
        {
            const string expected = @"
<fo:list-item>
<fo:list-item-label end-indent=""label-end()"">
<fo:block>&#x02022;</fo:block>
</fo:list-item-label>
<fo:list-item-body start-indent=""body-start()"">
<fo:block>The Game</fo:block>
</fo:list-item-body>
</fo:list-item>
";
            var fo = HtmlToXslFo.Convert("<ol><li>The Game</li></ol>");
            Assert.AreEqual(expected.Replace("\r\n", ""), fo);
        }
    }
}

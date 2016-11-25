namespace WhichMan.Utilities.HtmlToXslFo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using HtmlParsing;

    public static class HtmlToXslFo
    {
        #region - Private helper fields -
        private const string _layout = @"
<layout-master>
	<simple-page>
		<region-body/>
		<region-before/>
		<region-after/>
	</simple-page>
</layout-master>
";

        private const string _sequence = @"
<page-sequence>
	<static-content flow-name=""region-before""/>
	<static-content flow-name=""region-after""/>
</page-sequence>
";
        #endregion

        public static string Convert(string html)
        {
            var el = Parse(html);

            if (el?.Name == "html")
                AddPageLayout(el);

            AddAdditionalElements(el);

            var result = el.ToXmlFo();
            return result;
        }

        private static void AddAdditionalElements(HtmlElement el)
        {
            var cells = el.FindDescendants("td").ToList();
            if (el.Name == "td")
                cells.Add(el);
            foreach (var cell in cells)
            {
                var block = Parse("<div/>");
                block.ChildNodes.AddRange(cell.ChildNodes);
                cell.ChildNodes.Clear();
                cell.ChildNodes.Add(block);
            }

            cells = el.FindDescendants("th").ToList();
            if (el.Name == "th")
                cells.Add(el);
            foreach (var cell in cells)
            {
                var block = Parse("<div style=\"font-weight:bold;text-align:center;\">");
                block.ChildNodes.AddRange(cell.ChildNodes);
                cell.ChildNodes.Clear();
                cell.ChildNodes.Add(block);
            }

            var tables = el.FindDescendants("table").ToList();
            if (el.Name == "table")
                tables.Add(el);
            foreach (var table in tables)
            {
                var cols = table.FindDescendants("table-column").ToList();
                if (cols.Count > 0)
                    continue;
                
                var row = table.FindDescendants("thead").FirstOrDefault();
                if (row != null)
                    row = row.FindDescendants("tr").FirstOrDefault();
                else
                {
                    row = table.FindDescendants("tbody").FirstOrDefault();
                    if (row != null)
                        row = row.FindDescendants("tr").FirstOrDefault();
                    else
                    {
                        row = table.FindDescendants("tr").FirstOrDefault();

                        //add tbody tag
                        var tbody = Parse("<tbody/>");
                        tbody.ChildNodes.AddRange(table.ChildNodes);
                        table.ChildNodes.Clear();
                        table.ChildNodes.Add(tbody);
                    }
                }

                if (row == null)
                    continue;

                var sb = new StringBuilder();
                foreach (var item in row.ChildNodes)
                {
                    sb.Append("<table-column");
                    var col = item as HtmlElement;
                    foreach (var attr in col?.Attributes)
                    {
                        if (attr.Name == "width")
                            sb.Append(" width=\"" + attr.Value + "\"");
                    }
                    sb.Append("/>");
                }
                var elems = HtmlParser.Parse(sb.ToString()).ChildNodes;
                for (var i = 0; i < elems.Count; i++)
                    table.ChildNodes.Insert(i, elems[i]);
            }

            AddAdditionalElementsToLists(el, "ol");
            AddAdditionalElementsToLists(el, "ul");
        }

        private static void AddAdditionalElementsToLists(HtmlElement el, string name)
        {
            var lists = el.FindDescendants(name).ToList();
            if (el.Name == name)
                lists.Add(el);
            foreach (var list in lists)
            {
                var type = list.GetAttributeValue("type");
                var start = list.GetAttributeValue("start").ToInt();
                
                foreach (var item in list.ChildNodes)
                {
                    var li = item as HtmlElement;
                    if(li == null)
                        continue;

                    var label = GetListItemLabel(li, name, type, start);
                    var div = Parse("<div/>");
                    div.ChildNodes.AddRange(li.ChildNodes);
                    foreach (var attribute in li.Attributes)
                    {
                        div.Add(attribute.Name, attribute.Value);
                    }

                    var doc = HtmlParser.Parse("<list-item-label><div>"+label+ "</div></list-item-label><list-item-body></list-item-body>");
                    var body = doc.ChildNodes[1] as HtmlElement;
                    body?.ChildNodes.Add(div);

                    li.ChildNodes.Clear();
                    li.ChildNodes.AddRange(doc.ChildNodes);
                }
            }
        }

        private static string GetListItemLabel(HtmlElement el, string parentName, string type, int? start)
        {
            if (el == null)
                return null;

            if(parentName == "ol")
            {
                var position = el.SiblingsBefore("li").Count() + 1;
                if (start != null)
                    start = start + position - 1;
                else
                    start = position;

                var number = start + ". ";
                if (type == "i" || type == "I")
                {
                    try
                    {
                        number = start.Value.ToRoman() + ". ";
                        if (type == "i") number = number.ToLower();
                    }
                    catch { }
                }
                else if (type == "a" || type == "A")
                {
                    number = start.Value.GetExcelColumnName() + ". ";
                    if (type == "a") number = number.ToLower();
                }
                return number;
            }
            else
            {
                return "&#x02022;";
            }
        }

        private static HtmlElement Parse(string html)
        {
            var doc = HtmlParser.Parse(html, true);
            HtmlSanitizer.Sanitize(doc);
            var el = doc.ChildNodes.FirstOrDefault(c => (c is HtmlElement));
            return el as HtmlElement;
        }

        private static void AddPageLayout(HtmlElement el)
        {
            if (el.FindDescendants("layout-master").Count() > 0)
                return;

            for (var i = 0; i < el.ChildNodes.Count; i++)
            {
                var node = el.ChildNodes[i] as HtmlElement;
                if (node?.Name == "body")
                {
                    var layout = Parse(_layout);
                    var sequnce = Parse(_sequence);

                    var body = el.ChildNodes[i];
                    sequnce.ChildNodes.Add(body);
                    el.ChildNodes[i] = sequnce;

                    el.ChildNodes.Insert(0, layout);
                    break;
                }
            }
        }
        private static string ToXmlFo(this HtmlNode node)
        {
            if (node == null)
                return string.Empty;
            if (node is HtmlText)
            {
                if (node.Parent is HtmlElement && (node.Parent as HtmlElement).Name == "body")
                    return "<fo:block>"+ node.ToString() + "</fo:block>";
                return node.ToString();
            }
            if (!(node is HtmlElement))
                return string.Empty;

            var el = (HtmlElement) node;
            switch (el.Name)
            {
                case "h1":
                    return Header(el, 160);
                case "h2":
                    return Header(el, 140);
                case "h3":
                    return Header(el, 120);
                case "h4":
                    return Header(el, 100);
                case "h5":
                    return Header(el, 80);
                case "br":
                    return "<fo:block>&#xA;</fo:block>";
                case "hr":
                    return "<fo:block><fo:leader leader-pattern=\"rule\" leader-length.optimum=\"100%\" rule-style=\"double\" rule-thickness=\"0.1mm\"/></fo:block>";
                case "font":
                case "span":
                    return Inline(el);
                case "a":
                    return El(el, "basic-link", Nvp("text-decoration", "underline"), Nvp("color", "blue"), Nvp("role", "external link"));
                case "b":
                case "strong":
                    return Inline(el, Nvp("font-weight","bold"));
                case "i":
                case "em":
                    return Inline(el, Nvp("font-style", "italic"));
                case "u":
                    return Inline(el, Nvp("text-decoration", "underline"));
                case "s":
                case "strike":
                    return Inline(el, Nvp("text-decoration", "line-through"));
                case "sup":
                    return Inline(el, Nvp("vertical-align", "super"), Nvp("font-size", "75%"));
                case "sub":
                    return Inline(el, Nvp("vertical-align", "sub"), Nvp("font-size", "75%"));
                case "small":
                    return Inline(el, Nvp("font-size", "80%"));
                case "var":
                    return Inline(el, Nvp("font-style", "italic"), Nvp("font-family", "monospace"));
                case "img":
                    return El(el, "external-graphic");
                case "code":
                    return Inline(el, Nvp("font-family", "monospace"), Nvp("color", "navy"));
                case "pre":
                    return Block(el, Nvp("white-space-collapse", "false"), Nvp("wrap-option", "no-wrap"), Nvp("font-family", "monospace"));
                case "center":
                    return Block(el, Nvp("text-align", "center"));
                case "body":
                    return El(el, "flow", Nvp("flow-name", "region-body"));
                case "html":
                    return El(el, "root", Nvp("xmlns:fo", "http://www.w3.org/1999/XSL/Format"));
                case "head":
                    return null;
                case "layout-master":
                    return El(el, "layout-master-set");
                case "simple-page":
                    return El(el, "simple-page-master", Nvp("master-name", "simple"), Nvp("page-height", "11in"), Nvp("page-width", "8.5in"), 
                        Nvp("margin-top", ".1in"), Nvp("margin-bottom", ".5in"), Nvp("margin-left", ".5in"), Nvp("margin-right", ".5in"));
                case "region-body":
                    return El(el, "region-body", Nvp("region-name", "region-body"), Nvp("margin-bottom", ".5in"), Nvp("margin-top", ".5in"));
                case "region-before":
                    return El(el, "region-before", Nvp("region-name", "region-before"), Nvp("extent", ".5in"));
                case "region-after":
                    return El(el, "region-after", Nvp("region-name", "region-after"), Nvp("extent", ".5in"));
                case "page-sequence":
                    return El(el, "page-sequence", Nvp("master-reference", "simple"));
                case "static-content":
                    return El(el, "static-content", Nvp("flow-name", el.Attributes[0].Value));
                case "table":
                case "table-column":
                    return El(el, el.Name);
                case "tr":
                    return El(el, "table-row");
                case "td":
                case "th":
                    return TableCell(el);
                case "thead":
                    return El(el, "table-header");
                case "tbody":
                    return El(el, "table-body");
                case "div":
                    return Block(el);
                case "li":
                    return ListItem(el);
                case "list-item-label":
                    return El(el, el.Name, Nvp("end-indent", "label-end()"));
                case "list-item-body":
                    return El(el, el.Name, Nvp("start-indent", "body-start()"));
                case "ol":
                case "ul":
                    return ListBlock(el);
                case "p":
                    return Block(el, Nvp("space-before", "3pt"), Nvp("space-after", "3pt"));
                case "blockquote":
                    return Block(el, Nvp("space-before", "6pt"), Nvp("space-after", "6pt"), Nvp("start-indent", "12pt"), Nvp("stop-indent", "12pt"));
                default:
                    return Block(el);                    
            }
        }

        private class NameValue
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private static NameValue Nvp(string name, string value)
        {
            return new NameValue { Name = name, Value = value };
        }

        private static string Inline(HtmlElement el, params NameValue[] attributes)
        {
            return El(el, "inline", attributes);
        }

        private static string Block(HtmlElement el, params NameValue[] attributes)
        {
            return El(el, "block", attributes);
        }

        private static string Header(HtmlElement el, int size)
        {
            return El(el, "block", Nvp("font-size", size+"%"), Nvp("font-weight", "bold"), Nvp("space-before", "6pt"), Nvp("space-after", "6pt"));
        }

        private static string ListBlock(HtmlElement el)
        {
            var spaceAfter = el.HasAncestor("ul") || el.HasAncestor("ol") ? 0 : 12;
            var ancestors = el.FindAncestors("ul", "ol").ToArray();
            var startIndent = ancestors.Length > 0 ? (1 + ancestors.Length * 1.25) : 1;
            return El(el, "list-block", Nvp("provisional-distance-between-starts", "1cm"), Nvp("provisional-label-separation", "0.5cm"), 
                Nvp("space-after", spaceAfter+"pt"), Nvp("start-indent", startIndent+"cm"));
        }

        private static string ListItem(HtmlElement el)
        {
            var buf = new StringBuilder("<fo:list-item>");
            foreach (var node in el.ChildNodes)
            {
                buf.Append(node.ToXmlFo());
            }
            buf.Append("</fo:list-item>");

            return buf.ToString();
        }

        private static string TableCell(HtmlElement el)
        {
            var list = new List<NameValue>();
            var border = el.GetAncestorAttrValue("table", "border");
            if (border != null)
                list.Add(Nvp("border", "solid 0." + border + "mm black"));

            var cellpadding = el.GetAncestorAttrValue("table", "cellpadding");
            if (cellpadding != null)
            {
                list.Add(Nvp("padding-left", cellpadding + "pt"));
                list.Add(Nvp("padding-top", cellpadding + "pt"));
                list.Add(Nvp("padding-right", cellpadding + "pt"));
                list.Add(Nvp("padding-bottom", cellpadding + "pt"));
            }
            return El(el, "table-cell", list.ToArray());
        }

        private static string El(HtmlElement el, string tagName, params NameValue[] attributes)
        {
            var buf = new StringBuilder("<fo:" + tagName);

            var myAttributes = GetAttibutes(el, attributes);

            foreach (var attr in myAttributes)
            {
                buf.AppendFormat(" {0}=\"{1}\"", attr.Name, attr.Value);                    
            }

            if (el.ChildNodes.Count == 0)
            {
                buf.Append("/>");
                return buf.ToString();
            }
            buf.Append(">");

            foreach (var node in el.ChildNodes)
            {
                buf.Append(node.ToXmlFo());
            }

            buf.AppendFormat("</fo:{0}>", tagName);
            return buf.ToString();
        }

        private static NameValue[] GetAttibutes(HtmlElement el, NameValue[] attributes)
        {
            var dict = new Dictionary<string, string>();
            if (attributes != null)
            {
                foreach (var attr in attributes)
                {
                    dict[attr.Name] = attr.Value;
                }
            }

            foreach (var node in el.Attributes)
            {
                if (node.Name == "style")
                {
                    var styles = node.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(c => Nvp(c.SubstringBefore(":").Trim(), c.SubstringAfter(":").Trim())).ToArray();
                    foreach (var attr in styles)
                    {
                        if (attr.Name == "page-break-before")
                            dict["break-before"] = "page";
                        else if (attr.Name == "page-break-after")
                            dict["break-after"] = "page";
                        else
                            dict[attr.Name] = attr.Value;
                    }
                }
                else
                {
                    var attr = CreateAttribute(el.Name, node);
                    if(attr!= null)
                        dict[attr.Name] = attr.Value;
                }
            }            
            return dict.Select(c=> Nvp(c.Key, c.Value)).ToArray();
        }        

        private static NameValue CreateAttribute(string parentName, HtmlAttribute el)
        {
            switch (el.Name)
            {
                case "nowrap":
                    return Nvp("white-space", "nowrap");
                case "face":
                    return Nvp("font-family", el.Value);
                case "href":
                    return Nvp("external-destination", el.Value);
                case "align":
                    var align = el.Value == "right" ? "end" : (el.Value == "left" ? "start" : "center");
                    return Nvp("text-align", align);
                case "src":
                case "color":
                case "role":
                    return Nvp(el.Name, el.Value);
                case "height":
                    return Nvp(el.Name, el.Value.IsNumeric() ? el.Value + "px" : el.Value);
                case "width":
                    if (parentName == "td")
                        return null;
                    var name = parentName == "table-column" ? "column-width" : "width";
                    return Nvp(name, el.Value.IsNumeric() ? el.Value + "px" : el.Value);
                default:
                    return null;
            }
        }

        
    }
}

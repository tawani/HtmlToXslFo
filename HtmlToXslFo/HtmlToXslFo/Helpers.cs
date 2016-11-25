namespace WhichMan.Utilities.HtmlToXslFo
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using HtmlParsing;

    internal static class Helpers
    {        
        public static string SubstringBefore(this string str, string separator)
        {
            if (string.IsNullOrEmpty(str) || separator == null)
                return str;
            if (separator == string.Empty)
                return string.Empty;

            CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            int index = compareInfo.IndexOf(str, separator, CompareOptions.Ordinal);

            if (index < 0)
            {
                //No such substring
                return str;
            }

            return str.Substring(0, index);
        }

        public static string SubstringAfter(this string str, string separator)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            if (separator == null)
                return string.Empty;

            CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            int index = compareInfo.IndexOf(str, separator, CompareOptions.Ordinal);
            if (index < 0)
            {
                //No such substring
                return string.Empty;
            }

            return str.Substring(index + separator.Length);
        }

        public static bool IsNumeric(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            decimal tmp;
            return decimal.TryParse(str, out tmp);
        }

        public static int? ToInt(this object data)
        {
            var s = string.Format("{0}", data);
            int n;
            return int.TryParse(s, out n) ? n : (int?)null;
        }

        public static bool HasAncestor(this HtmlElement el, string name)
        {
            return el.FindAncestor(name) != null;
        }

        public static string GetAncestorAttrValue(this HtmlElement el, string name, string attrName)
        {
            return el.FindAncestor(name)?.GetAttributeValue(attrName) ;
        }

        public static IEnumerable<HtmlElement> FindAncestors(this HtmlElement el, params string[] names)
        {
            if (el.Parent != null && el.Parent is HtmlElement)
            {
                var parent = el.Parent as HtmlElement;
                if (names.Contains(parent.Name))
                    yield return parent;
                var list = parent.FindAncestors(names);
                foreach (var item in list)
                    yield return item;
            }
        }

        public static IEnumerable<HtmlElement> SiblingsBefore(this HtmlElement el, string name)
        {
            if (el.PrevNode != null && el.PrevNode is HtmlElement)
            {
                var prevNode = el.PrevNode as HtmlElement;
                if (name == prevNode.Name)
                    yield return prevNode;
                var list = prevNode.SiblingsBefore(name);
                foreach (var item in list)
                    yield return item;
            }
        }

        public static string ToRoman(this int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        public static string GetExcelColumnName(this int number)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var value = "";
            var index = number - 1;
            if (index >= letters.Length)
                value += letters[index / letters.Length - 1];

            value += letters[index % letters.Length];

            return value;
        }
    }
}
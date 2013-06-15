using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NGChat.Infrastructure.Utils
{
    public class ExpandUrlsParser
    {
        public string Target = "";

        /// <summary>
        /// Expands links into HTML hyperlinks inside of text or HTML.
        /// </summary>
        /// <param name="Text">The text to expand</param>
        /// <param name="Target">Target frame where output is displayed</param>
        /// <returns></returns>
        public string ExpandUrls(string Text)
        {

            string pattern = @"[""'=]?(http://|ftp://|https://|www\.|ftp\.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])";

            // *** Expand embedded hyperlinks
            System.Text.RegularExpressions.RegexOptions options =
                RegexOptions.IgnorePatternWhitespace |
                RegexOptions.Multiline |
                RegexOptions.IgnoreCase;

            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(pattern, options);

            MatchEvaluator MatchEval = new MatchEvaluator(this.ExpandUrlsRegExEvaluator);
            return Regex.Replace(Text, pattern, MatchEval);
        }

        /// <summary>
        /// Internal RegExEvaluator callback. Expands the URL
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        private string ExpandUrlsRegExEvaluator(System.Text.RegularExpressions.Match M)
        {
            string Href = M.Value; // M.Groups[0].Value;

            // *** if string starts within an HREF don't expand it
            if (Href.StartsWith("=") ||
                Href.StartsWith("'") ||
                Href.StartsWith("\""))
                return Href;

            string Text = Href;

            if (Href.IndexOf("://") < 0)
            {
                if (Href.StartsWith("www."))
                    Href = "http://" + Href;
                else if (Href.StartsWith("ftp"))
                    Href = "ftp://" + Href;
                else if (Href.IndexOf("@") > -1)
                    Href = "mailto:" + Href;
            }

            string Targ = !string.IsNullOrEmpty(this.Target) ? " target='" + this.Target + "'" : "";

            return "<a href='" + Href + "'" + Targ +
                    ">" + Text + "</a>";
        }

    }
}
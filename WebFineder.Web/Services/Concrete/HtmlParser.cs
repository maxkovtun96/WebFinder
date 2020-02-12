using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebFineder.Web.Services.Abstract;

namespace WebFineder.Web.Services.Concrete
{
    public class HtmlParser : IHtmlParser
    {
        const string UrlPattern = @"http:\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
        public IEnumerable<string> GetUrls(string text)
        {
            foreach (Match match in Regex.Matches(text, UrlPattern))
                yield return match.Value;

        }

        public int GetWorldCountInText(string text, string world) 
            => Regex.Matches(text, $"{world}", RegexOptions.IgnoreCase).Count();
    }
}

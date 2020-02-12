using System;
using System.Collections.Generic;

namespace WebFineder.Web.Services.Abstract
{
    public interface IHtmlParser
    {
        IEnumerable<string> GetUrls(string text);
        int GetWorldCountInText(string text, string world);
    }
}

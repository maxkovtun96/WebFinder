using System;
using System.Collections.Generic;

namespace WebFineder.Web.Services.Model
{
    public class FindWordServiceModel
    {
        public Uri WebAddress { get; set; }
        public int WordsCount { get; set; }
        public bool IsCalculated { get; private set; } = false;
        public string SiteUrl => this.WebAddress.AbsoluteUri;
        public List<FindWordServiceModel> SubNodes { get; set; }

        public FindWordServiceModel()
        {
            this.SubNodes = new List<FindWordServiceModel>();
        }

        public FindWordServiceModel(Uri uri)
        {
            this.WebAddress = uri;
            this.SubNodes = new List<FindWordServiceModel>();
        }

        public static FindWordServiceModel Create(string webUrl)
        {
            return new FindWordServiceModel(new Uri(webUrl));
        }

        public void MarkAsCalculated() => this.IsCalculated = true;
    }
}

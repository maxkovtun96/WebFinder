using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebFineder.Web.Services.Abstract;
using WebFineder.Web.Services.Model.Configuration;

namespace WebFineder.Web.Services.Concrete
{
    public class HtmlLoader : IHtmlLoader
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HtmlLoader> _logger;
        private readonly WebFinderSettings _webFinderSettings;

        public HtmlLoader(ILogger<HtmlLoader> logger,
            IOptions<WebFinderSettings> webFinderSettings)
        {
            _webFinderSettings = webFinderSettings.Value;
            _httpClient = new HttpClient();
            _logger = logger;
            _httpClient.Timeout = new TimeSpan(0,0, _webFinderSettings.RequestTimeoutSeconds);
        }

        public async Task<string> Load(Uri uri, CancellationToken cancelationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _httpClient.GetAsync(uri.AbsoluteUri, cancelationToken);
                using HttpContent content = response.Content;
                return await content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message, e);
                return string.Empty;
            }
        }
    }
}

using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFineder.Web.Services.Abstract;
using WebFineder.Web.Services.Model;
using WebFineder.Web.Services.Model.Configuration;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace WebFineder.Web.Services.Concrete
{
    public class HierarchicalWebFinder : IHierarchicalWebFinder
    {
        private readonly WebFinderSettings _webFinderSettings;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HierarchicalWebFinder> _logger;

        public HierarchicalWebFinder(
            IOptions<WebFinderSettings> webFinderSettings,
            IServiceProvider serviceProvider,
            ILogger<HierarchicalWebFinder> logger)
        {
            _webFinderSettings = webFinderSettings.Value;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<FindWordServiceModel> Find(FindRequestModel findRequestModel, CancellationToken cancellationToken = default)
        {
            var initNode = FindWordServiceModel.Create(findRequestModel.TargetSite);
            await BFS(initNode, findRequestModel.Word, cancellationToken);
            return initNode;
        }

        private async Task BFS(FindWordServiceModel node, string word, CancellationToken cancellationToken = default)
        {
            using var semaphore = new SemaphoreSlim(_webFinderSettings.MaxThreads);
            var nodeQueue = new ConcurrentQueue<FindWordServiceModel>();
            var visitedUrls = new ConcurrentDictionary<string, string>();
            nodeQueue.Enqueue(node);
            await EnrichNode(node, word);
            visitedUrls.TryAdd(node.SiteUrl, node.SiteUrl);
            while (nodeQueue.Count() > 0)
            {
                await semaphore.WaitAsync(cancellationToken);
                nodeQueue.TryDequeue(out FindWordServiceModel curr);
                
                var tasks = curr.SubNodes.Select(chiled => Task.Run(async () =>
                {
                    try
                    {
                        if (visitedUrls.ContainsKey(chiled.SiteUrl) || visitedUrls.Count() > _webFinderSettings.MaxSites)
                        {
                            chiled = null;
                            return;
                        }
                        nodeQueue.Enqueue(chiled);
                        await EnrichNode(chiled, word);
                        visitedUrls.AddOrUpdate(chiled.SiteUrl, chiled.SiteUrl, (_, old) => old);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
                await Task.WhenAll(tasks);
            }
        }

        private async Task EnrichNode(FindWordServiceModel node, string word, CancellationToken cancellationToken = default)
        {
            var htmlLoader = (IHtmlLoader)_serviceProvider.GetService(typeof(IHtmlLoader));
            var htmlParser = (IHtmlParser)_serviceProvider.GetService(typeof(IHtmlParser));

            if (node.IsCalculated) return;

            var htmlStr = await htmlLoader.Load(node.WebAddress, cancellationToken);
            var count = htmlParser.GetWorldCountInText(htmlStr, word);
            var subUrls = htmlParser.GetUrls(htmlStr);
            node.WordsCount = count;
            node.SubNodes
                .AddRange(subUrls.Select(x => new FindWordServiceModel { WebAddress = new Uri(x) }));

            node.MarkAsCalculated();
        }
    }
}

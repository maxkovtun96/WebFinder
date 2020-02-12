using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebFineder.Web.Models;
using WebFineder.Web.Services.Abstract;
using WebFineder.Web.Services.Model;

namespace WebFineder.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHierarchicalWebFinder _hierarchicalWebFinder;
        

        public HomeController(
            ILogger<HomeController> logger, 
            IHierarchicalWebFinder hierarchicalWebFinder)
        {
            _logger = logger;
            _hierarchicalWebFinder = hierarchicalWebFinder;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Find(string url, string word, CancellationToken cancellationToken = default)
        {
            var serviceRequest = new FindRequestModel 
            {
                TargetSite = url,
                Word = word
            };
            var model = await _hierarchicalWebFinder.Find(serviceRequest, cancellationToken);
            return Ok(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

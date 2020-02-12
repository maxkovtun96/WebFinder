using System.Threading;
using System.Threading.Tasks;
using WebFineder.Web.Services.Model;

namespace WebFineder.Web.Services.Abstract
{
    public interface IHierarchicalWebFinder
    {
        Task<FindWordServiceModel> Find(FindRequestModel findRequestModel, CancellationToken cancellationToken = default);

    }
}

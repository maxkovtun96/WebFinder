using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebFineder.Web.Services.Abstract
{
    public interface IHtmlLoader
    {
        Task<string> Load(Uri uri, CancellationToken cancelationToken = default);
    }
}

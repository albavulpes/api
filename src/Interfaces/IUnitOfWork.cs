using AlbaVulpes.API.Base;
using Marten;

namespace AlbaVulpes.API.Interfaces
{
    public interface IUnitOfWork
    {
        IDocumentStore GetStore();
        TRepository GetRepository<TRepository>() where TRepository : ApiRepository;
    }
}
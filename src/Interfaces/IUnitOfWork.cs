using AlbaVulpes.API.Base;
using Marten;

namespace AlbaVulpes.API.Interfaces
{
    public interface IUnitOfWork
    {
        IDocumentStore GetStore();
        ApiRepository<TModel> GetRepository<TModel>() where TModel : ApiModel;
        TRepository GetRepository<TModel, TRepository>() where TModel : ApiModel where TRepository : ApiRepository<TModel>;
    }
}
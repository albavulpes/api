using System;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using Marten;

namespace AlbaVulpes.API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly IDocumentStore Store;

        public UnitOfWork(IDocumentStore documentStore)
        {
            Store = documentStore;
        }

        public IDocumentStore GetStore()
        {
            return Store;
        }

        public ApiRepository<TModel> GetRepository<TModel>() where TModel : ApiModel
        {
            return new ApiRepository<TModel>(Store);
        }

        public TRepository GetRepository<TModel, TRepository>() where TModel : ApiModel where TRepository : ApiRepository<TModel>
        {
            return (TRepository)Activator.CreateInstance(typeof(TRepository), Store);
        }
    }
}
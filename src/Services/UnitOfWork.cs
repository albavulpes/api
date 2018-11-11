using System;
using AlbaVulpes.API.Base;
using Marten;

namespace AlbaVulpes.API.Services
{
    public interface IUnitOfWork
    {
        IDocumentStore GetStore();
        TRepository GetRepository<TRepository>() where TRepository : ApiRepository;
    }

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

        public TRepository GetRepository<TRepository>() where TRepository : ApiRepository
        {
            return (TRepository)Activator.CreateInstance(typeof(TRepository), Store);
        }
    }
}
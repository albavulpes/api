using Marten;

namespace AlbaVulpes.API.Base
{
    public class ApiRepository
    {
        protected readonly IDocumentStore Store;

        public ApiRepository(IDocumentStore documentStore)
        {
            Store = documentStore;
        }
    }
}
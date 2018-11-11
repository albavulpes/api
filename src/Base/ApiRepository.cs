using Marten;

namespace AlbaVulpes.API.Base
{
    public class ApiRepository
    {
        protected readonly IDocumentStore _store;

        public ApiRepository(IDocumentStore documentStore)
        {
            _store = documentStore;
        }
    }
}
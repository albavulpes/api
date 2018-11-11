using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AlbaVulpes.API.Interfaces;
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
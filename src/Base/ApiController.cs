using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Marten;
using AlbaVulpes.API.Interfaces;

namespace AlbaVulpes.API.Base
{
    public abstract class ApiController<TModel> : Controller, IRestController<TModel> where TModel : ApiModel
    {
        protected readonly IDocumentStore Store;

        public ApiController(IDocumentStore documentStore)
        {
            Store = documentStore;
        }

        public abstract IEnumerable<TModel> Get();
        public abstract TModel Get(long id);
        public abstract TModel Create([FromBody] TModel data);
        public abstract TModel Update([FromBody] TModel data);
        public abstract TModel Delete(long id);
    }
}

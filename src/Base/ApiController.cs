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

        [HttpGet]
        public abstract IActionResult Get();
        [HttpGet("{id}")]
        public abstract IActionResult Get(Guid id);
        [HttpPost]
        public abstract IActionResult Create([FromBody] TModel data);
        [HttpPut("{id}")]
        public abstract IActionResult Update(Guid id, [FromBody] TModel data);
        [HttpDelete("{id}")]
        public abstract IActionResult Delete(Guid id);
    }
}

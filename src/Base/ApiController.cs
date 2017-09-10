using System;
using Microsoft.AspNetCore.Mvc;
using Marten;
using AlbaVulpes.API.Interfaces;

namespace AlbaVulpes.API.Base
{
    public abstract class ApiController<TModel> : Controller, IRestController<TModel> where TModel : ApiModel
    {
        protected readonly IDocumentStore Store;

        protected ApiController(IDocumentStore documentStore)
        {
            Store = documentStore;
        }

        [HttpPost]
        public abstract IActionResult Create([FromBody] TModel data);
        [HttpGet("{id}")]
        public abstract IActionResult Read(Guid id);
        [HttpPut("{id}")]
        public abstract IActionResult Update(Guid id, [FromBody] TModel data);
        [HttpDelete("{id}")]
        public abstract IActionResult Delete(Guid id);
    }
}

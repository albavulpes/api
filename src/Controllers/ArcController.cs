using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Marten;
using AlbaVulpes.API.Models.Database;
using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Controllers
{
    [Route("arcs")]
    public class ArcController : ApiController<Arc>
    {
        public ArcController(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public override IActionResult Create([FromBody] Arc data)
        {
            throw new NotImplementedException();
        }

        public override IActionResult Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public override IActionResult Get()
        {
            throw new NotImplementedException();
        }

        public override IActionResult Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public override IActionResult Update(Guid id, [FromBody] Arc data)
        {
            throw new NotImplementedException();
        }
    }
}
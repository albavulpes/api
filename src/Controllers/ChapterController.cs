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
    [Route("chapters")]
    public class ChapterController : ApiController<Chapter>
    {
        public ChapterController(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public override IActionResult Create([FromBody] Chapter data)
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

        public override IActionResult Update(Guid id, [FromBody] Chapter data)
        {
            throw new NotImplementedException();
        }
    }
}
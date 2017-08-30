using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Marten;
using AlbaVulpes.API.Models;

namespace AlbaVulpes.API.Controllers
{
    
    [Route("chapters")]
    public class ChapterController : Controller
    {
        private readonly IDocumentStore _documentStore;

        public ChapterController(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }


        [HttpGet]
        public IEnumerable<Chapter> Get()
        {
            using (var session = _documentStore.QuerySession())
            {
                return session.Query<Chapter>();
            }
        }

        [HttpGet("{id}")]
        public Chapter Get(long id)
        {
            using (var session = _documentStore.QuerySession())
            {
                return session
                    .Query<Chapter>()
                    .Where(chapter => chapter.Id == id)
                    .FirstOrDefault();
            }
        }

        [HttpPost]
        public void Create([FromBody]Chapter chapter)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.Store(chapter);
                session.SaveChanges();
            }
        }

        [HttpPut("{id}")]
        public void Update([FromBody]Chapter chapter)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.Update<Chapter>(chapter);
                session.SaveChanges();
            }
        }

        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.DeleteWhere<Chapter>(chapter => chapter.Id == id);
                session.SaveChanges();
            }
        }
    }
}
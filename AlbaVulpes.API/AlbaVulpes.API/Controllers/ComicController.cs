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
    [Route("/comics")]
    public class ComicController : Controller
    {
        private readonly IDocumentStore _documentStore;

        public ComicController(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        
        [HttpGet]
        public IEnumerable<Comic> Get()
        {
            using (var session = _documentStore.QuerySession())
            {
                return session.Query<Comic>();
            }
        }
        
        [HttpGet("{id}")]
        public Comic Get(long id)
        {
            using (var session = _documentStore.QuerySession())
            {
                return session
                    .Query<Comic>()
                    .Where(comic => comic.Id == id)
                    .FirstOrDefault();
            }
        }
        
        [HttpPost]
        public void Create([FromBody]Comic comic)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.Store(comic);
                session.SaveChanges();
            }
        }
        
        [HttpPut("{id}")]
        public void Update([FromBody]Comic comic)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.Update<Comic>(comic);
                session.SaveChanges();
            }
        }
        
        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.DeleteWhere<Comic>(comic => comic.Id == id);
                session.SaveChanges();
            }
        }
    }
}

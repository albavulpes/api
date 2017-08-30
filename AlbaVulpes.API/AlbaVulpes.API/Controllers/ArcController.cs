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
    [Route("arcs")]
    public class ArcController : Controller
    {
        private readonly IDocumentStore _documentStore;

        public ArcController(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }


        [HttpGet]
        public IEnumerable<Arc> Get()
        {
            using (var session = _documentStore.QuerySession())
            {
                return session.Query<Arc>();
            }
        }

        [HttpGet("{id}")]
        public Arc Get(long id)
        {
            using (var session = _documentStore.QuerySession())
            {
                return session
                    .Query<Arc>()
                    .Where(arc => arc.Id == id)
                    .FirstOrDefault();
            }
        }

        [HttpPost]
        public void Create([FromBody]Arc arc)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.Store(arc);
                session.SaveChanges();
            }
        }

        [HttpPut("{id}")]
        public void Update([FromBody]Arc arc)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.Update<Arc>(arc);
                session.SaveChanges();
            }
        }

        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.DeleteWhere<Arc>(arc => arc.Id == id);
                session.SaveChanges();
            }
        }
    }
}
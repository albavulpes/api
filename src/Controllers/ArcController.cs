using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Marten;
using AlbaVulpes.API.Models;
using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Controllers
{
    [Route("arcs")]
    public class ArcController : ApiController<Arc>
    {
        public ArcController(IDocumentStore documentStore) : base(documentStore)
        {
        }

        [HttpGet]
        public IEnumerable<Arc> Get()
        {
            using (var session = Store.QuerySession())
            {
                return session.Query<Arc>();
            }
        }

        [HttpGet("{id}")]
        public Arc Get(Guid id)
        {
            using (var session = Store.QuerySession())
            {
                return session.Query<Arc>().FirstOrDefault(arc => arc.Id == id);
            }
        }

        [HttpPost]
        public Arc Create([FromBody]Arc arc)
        {
            using (var session = Store.LightweightSession())
            {
                session.Store(arc);
                session.SaveChanges();

                return arc;
            }
        }

        [HttpPut("{id}")]
        public Arc Update(Guid id, [FromBody]Arc arc)
        {
            using (var session = Store.LightweightSession())
            {
                session.Update<Arc>(arc);
                session.SaveChanges();

                return arc;
            }
        }

        [HttpDelete("{id}")]
        public Arc Delete(Guid id)
        {
            using (var session = Store.LightweightSession())
            {
                var arc = session.Query<Arc>().FirstOrDefault(a => a.Id == id);
                session.DeleteWhere<Arc>(a => a.Id == id);
                session.SaveChanges();

                return arc;
            }
        }
    }
}
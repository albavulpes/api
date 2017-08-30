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

    [Route("pages")]
    public class PageController : Controller
    {
        private readonly IDocumentStore _documentStore;

        public PageController(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        [HttpGet]
        public IEnumerable<Page> Get()
        {
            using (var session = _documentStore.QuerySession())
            {
                return session.Query<Page>();
            }
        }

        [HttpGet]
        public Page Get(long id)
        {
            using (var session = _documentStore.QuerySession())
            {
                return session
                    .Query<Page>()
                    .Where(page => page.Id == id)
                    .FirstOrDefault();
            }
        }

        [HttpPost]
        public void Create([FromBody]Page page)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.Store(page);
                session.SaveChanges();
            }
        }

        [HttpPut]
        public void Update([FromBody]Page page)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.Update<Page>(page);
                session.SaveChanges();
            }
        }

        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            using (var session = _documentStore.LightweightSession())
            {
                session.DeleteWhere<Page>(page => page.Id == id);
                session.SaveChanges();
            }
        }
    }
}
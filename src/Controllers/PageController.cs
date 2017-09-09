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

    [Route("pages")]
    public class PageController : ApiController<Page>
    {
        public PageController(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public override IEnumerable<Page> Get()
        {
            using (var session = Store.QuerySession())
            {
                return session.Query<Page>();
            }
        }

        public override Page Get(Guid id)
        {
            using (var session = Store.QuerySession())
            {
                return session
                    .Query<Page>()
                    .Where(page => page.Id == id)
                    .FirstOrDefault();
            }
        }

        public override Page Create([FromBody] Page page)
        {
            using (var session = Store.LightweightSession())
            {
                session.Store(page);
                session.SaveChanges();

                return page;
            }
        }

        public override Page Update(Guid id, [FromBody] Page page)
        {
            using (var session = Store.LightweightSession())
            {
                session.Update<Page>(page);
                session.SaveChanges();

                return page;
            }
        }

        public override Page Delete(Guid id)
        {
            using (var session = Store.LightweightSession())
            {
                var page = session.Query<Page>().FirstOrDefault(a => a.Id == id);
                session.DeleteWhere<Page>(p => p.Id == id);
                session.SaveChanges();

                return page;
            }
        }
    }
}
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
    [Route("chapters")]
    public class ChapterController : ApiController<Chapter>
    {
        public ChapterController(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public override IEnumerable<Chapter> Get()
        {
            using (var session = Store.QuerySession())
            {
                return session.Query<Chapter>();
            }
        }

        public override Chapter Get(Guid id)
        {
            using (var session = Store.QuerySession())
            {
                return session
                    .Query<Chapter>()
                    .Where(chapter => chapter.Id == id)
                    .FirstOrDefault();
            }
        }

        public override Chapter Create([FromBody] Chapter data)
        {
            using (var session = Store.LightweightSession())
            {
                session.Store(data);
                session.SaveChanges();

                return data;
            }
        }

        public override Chapter Update(Guid id, [FromBody] Chapter data)
        {
            using (var session = Store.LightweightSession())
            {
                session.Update<Chapter>(data);
                session.SaveChanges();

                return data;
            }
        }

        public override Chapter Delete(Guid id)
        {
            using (var session = Store.LightweightSession())
            {
                var chapter = session.Query<Chapter>().FirstOrDefault(a => a.Id == id);
                session.DeleteWhere<Chapter>(c => c.Id == id);
                session.SaveChanges();

                return chapter;
            }
        }
    }
}
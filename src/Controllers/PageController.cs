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

        public override IActionResult Get()
        {
            using (var session = Store.QuerySession())
            {
                var pages = session.Query<Page>().Take(10);
                return Ok(pages);
            }
        }

        public override IActionResult Get(Guid id)
        {
            using (var session = Store.QuerySession())
            {
                var page = session.Query<Page>().FirstOrDefault(x => x.Id == id);

                if (page == null)
                {
                    return NotFound();
                }

                return Ok(page);
            }
        }

        public override IActionResult Create([FromBody] Page page)
        {
            if (page == null)
            {
                return BadRequest();
            }

            var newComic = new Page
            {
                Number = page.Number,
                Image = page.Image
            };

            using (var session = Store.OpenSession())
            {
                session.Store(newComic);
                session.SaveChanges();

                return Ok(newComic);
            }
        }

        public override IActionResult Update(Guid id, [FromBody] Page page)
        {
            if (page == null)
            {
                return BadRequest();
            }

            using (var session = Store.OpenSession())
            {
                var pageToUpdate = session.Query<Page>().FirstOrDefault(x => x.Id == id);

                if (pageToUpdate == null)
                {
                    return NotFound();
                }

                pageToUpdate.Number = page.Number;
                pageToUpdate.Image = page.Image;

                session.Store(pageToUpdate);
                session.SaveChanges();

                return Ok(pageToUpdate);
            }
        }

        public override IActionResult Delete(Guid id)
        {
            using (var session = Store.OpenSession())
            {
                var page = session.Query<Page>().FirstOrDefault(x => x.Id == id);

                if (page == null)
                {
                    return NotFound();
                }

                session.DeleteWhere<Page>(x => x.Id == id);
                session.SaveChanges();

                return Ok(page);
            }
        }
    }
}
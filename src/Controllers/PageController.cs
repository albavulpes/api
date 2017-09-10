using System;
using System.Linq;
using System.Net;
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

        public override IActionResult Create([FromBody] Page page)
        {
            if (page == null)
            {
                return BadRequest();
            }

            var newPage = new Page
            {
                Number = page.Number,
                Image = page.Image
            };

            using (var session = Store.OpenSession())
            {
                session.Store(newPage);
                session.SaveChanges();

                newPage.ComputeHash();

                session.Update(newPage);
                session.SaveChanges();

                Response.Headers["ETag"] = newPage.Hash;

                return Ok(newPage);
            }
        }

        public override IActionResult Read(Guid id)
        {
            using (var session = Store.QuerySession())
            {
                var page = session.Query<Page>().FirstOrDefault(x => x.Id == id);

                if (page == null)
                {
                    return NotFound();
                }

                var requestHash = Request.Headers["If-None-Match"];
                if (!string.IsNullOrEmpty(requestHash))
                {
                    // Match the requested hash with the database hash
                    if (requestHash == page.Hash)
                    {
                        return StatusCode((int)HttpStatusCode.NotModified);
                    }
                }

                Response.Headers["ETag"] = page.Hash;

                return Ok(page);
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
                var dbPage = session.Query<Page>().FirstOrDefault(x => x.Id == id);

                if (dbPage == null)
                {
                    return NotFound();
                }

                session.Update(page);
                session.SaveChanges();

                return Ok(page);
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
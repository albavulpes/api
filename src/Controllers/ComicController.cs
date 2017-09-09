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
    [Route("/comics")]
    public class ComicController : ApiController<Comic>
    {
        public ComicController(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public override IActionResult Get()
        {
            using (var session = Store.QuerySession())
            {
                var comics = session.Query<Comic>().Take(10);
                return Ok(comics);
            }
        }

        public override IActionResult Get(Guid id)
        {
            using (var session = Store.QuerySession())
            {
                var comic = session.Query<Comic>().FirstOrDefault(x => x.Id == id);

                if (comic == null)
                {
                    return NotFound();
                }

                return Ok(comic);
            }
        }

        public override IActionResult Create([FromBody] Comic comic)
        {
            if (comic == null)
            {
                return BadRequest();
            }

            var newComic = new Comic
            {
                Title = comic.Title,
                Author = comic.Author,
                Arcs = comic.Arcs
            };

            using (var session = Store.OpenSession())
            {
                session.Store(newComic);
                session.SaveChanges();

                return Ok(newComic);
            }
        }

        public override IActionResult Update(Guid id, [FromBody] Comic comic)
        {
            if (comic == null)
            {
                return BadRequest();
            }

            using (var session = Store.OpenSession())
            {
                var comicToUpdate = session.Query<Comic>().FirstOrDefault(x => x.Id == id);

                if (comicToUpdate == null)
                {
                    return NotFound();
                }

                comicToUpdate.Title = comic.Title;
                comicToUpdate.Author = comic.Author;
                comicToUpdate.Arcs = comic.Arcs;

                session.Store(comicToUpdate);
                session.SaveChanges();

                return Ok(comicToUpdate);
            }
        }

        public override IActionResult Delete(Guid id)
        {
            using (var session = Store.OpenSession())
            {
                var comic = session.Query<Comic>().FirstOrDefault(x => x.Id == id);

                if (comic == null)
                {
                    return NotFound();
                }

                session.DeleteWhere<Comic>(x => x.Id == id);
                session.SaveChanges();

                return Ok(comic);
            }
        }
    }
}

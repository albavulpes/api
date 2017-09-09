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
        public IEnumerable<Comic> GetAll()
        {
            using (var session = _documentStore.QuerySession())
            {
                return session.Query<Comic>();
            }
        }
        
        [HttpGet("{id}")]
        public Comic GetById(Guid id)
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
        public IActionResult Create([FromBody]Comic comic)
        {
            if(comic == null)
            {
                return BadRequest();
            }

            var newComic = new Comic
            {
                Id = comic.Id,
                Title = comic.Title,
                Author = comic.Author,
                Arcs = comic.Arcs
            };

            using (var session = _documentStore.OpenSession())
            {
                session.Store(newComic);
                session.SaveChanges();
                return Ok();
            }
        }
        
        [HttpPut("{id}")]
        public IActionResult Update([FromBody]Comic comic, Guid id)
        {
            using (var session = _documentStore.OpenSession())
            {
                var comicToUpdate = session
                    .Query<Comic>()
                    .Where(c => c.Id == id)
                    .FirstOrDefault();

                if(comicToUpdate == null)
                {
                    BadRequest();
                }
                
                comicToUpdate.Title = comic.Title;
                comicToUpdate.Author = comic.Author;
                comicToUpdate.Arcs = comic.Arcs;

                session.Store(comicToUpdate);
                session.SaveChanges();
                return Ok();
            }
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            using (var session = _documentStore.OpenSession())
            {
                if(session.Query<Comic>().Where(comic => comic.Id == id).FirstOrDefault() == null)
                {
                    BadRequest();
                }
                session.DeleteWhere<Comic>(comic => comic.Id == id);
                session.SaveChanges();
                return Ok();
            }
        }
    }
}

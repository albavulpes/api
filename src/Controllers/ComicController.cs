using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Marten;
using AlbaVulpes.API.Models.Database;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;

namespace AlbaVulpes.API.Controllers
{
    [Produces("application/json")]
    [Route("comics")]
    public class ComicController : ApiController<Comic>
    {
        public ComicController(IUnitOfWork unitOfWork): base(unitOfWork)
        {

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
            UnitOfWork.GetRepository<Comic>().Create(newComic);

            Response.Headers["ETag"] = newComic.Hash;

            return CreatedAtRoute("comics", new { id = newComic.Id }, newComic);
        }

        public override IActionResult Read(Guid id)
        {
            var comic = UnitOfWork.GetRepository<Comic>().GetSingle(id);

            if (comic == null)
            {
                return NotFound();
            }

            var requestHash = Request.Headers["If-None-Match"];
            if(!string.IsNullOrEmpty(requestHash))
            {
                if (requestHash == comic.Hash)
                {
                    return StatusCode((int)HttpStatusCode.NotModified);
                }
            }

            Response.Headers["ETag"] = comic.Hash;

            return Ok(comic);

        }

        public override IActionResult Update(Guid id, [FromBody] Comic comic)
        {
            if (comic == null)
            {
                return BadRequest();
            }

            var updatedComic = UnitOfWork.GetRepository<Comic>().Update(id, comic);

            if (updatedComic == null)
            {
                return NotFound();
            }

            return Ok(updatedComic);
        }

        public override IActionResult Delete(Guid id)
        {
            var deletedComic = UnitOfWork.GetRepository<Comic>().RemoveSingle(id);

            if (deletedComic == null)
            {
                return NotFound();
            }

            return Ok(deletedComic);
        }
    }
}
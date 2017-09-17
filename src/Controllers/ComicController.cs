using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;

namespace AlbaVulpes.API.Controllers
{
    [Produces("application/json")]
    [Route("comics")]
    public class ComicController : ApiController<Comic>
    {
        public ComicController(IUnitOfWork unitOfWork) : base(unitOfWork)
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
                Arcs = comic.Arcs,
                CoverImageFullSize = comic.CoverImageFullSize,
                CoverImageThumbnail = comic.CoverImageThumbnail
            };
            UnitOfWork.GetRepository<Comic>().Create(newComic);

            Response.Headers["ETag"] = newComic.Hash;

            return CreatedAtAction("Read", new { id = newComic.Id }, newComic);
        }

        public override IActionResult Read(Guid id)
        {
            var comic = UnitOfWork.GetRepository<Comic>().GetSingle(id);

            if (comic == null)
            {
                return NotFound();
            }

            var requestHash = Request.Headers["If-None-Match"];
            if (!string.IsNullOrEmpty(requestHash))
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

            var comicToUpdate = UnitOfWork.GetRepository<Comic>().Update(id, comic);

            if (comicToUpdate == null)
            {
                return NotFound();
            }

            return Ok(comicToUpdate);
        }

        public override IActionResult Delete(Guid id)
        {
            var comicToDelete = UnitOfWork.GetRepository<Comic>().RemoveSingle(id);

            if (comicToDelete == null)
            {
                return NotFound();
            }

            return Ok(comicToDelete);
        }
    }
}
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Repositories;

namespace AlbaVulpes.API.Controllers
{
    [Route("comics")]
    [Produces("application/json")]
    public class ComicController : ApiController<Comic>
    {
        public ComicController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override IActionResult Read(Guid id)
        {
            var comic = UnitOfWork.GetRepository<Comic, ComicRepository>().Get(id);

            if (comic == null)
            {
                return NotFound();
            }

            return Ok(comic);
        }

        public override IActionResult Create([FromBody] Comic comic)
        {
            if (comic == null)
            {
                return BadRequest();
            }

            var savedComic = UnitOfWork.GetRepository<Comic>().Create(comic);

            return CreatedAtAction("Read", new { id = savedComic.Id }, savedComic);
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
            var comicToDelete = UnitOfWork.GetRepository<Comic>().Delete(id);

            if (comicToDelete == null)
            {
                return NotFound();
            }

            return Ok(comicToDelete);
        }
    }
}
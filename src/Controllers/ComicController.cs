using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Repositories;
using AlbaVulpes.API.Validators;

namespace AlbaVulpes.API.Controllers
{
    [Route("comics")]
    [Produces("application/json")]
    public class ComicController : ApiController<Comic>
    {
        public ComicController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comics = UnitOfWork.GetRepository<Comic, ComicRepository>().GetAll();

            return Ok(comics);
        }

        public override async Task<IActionResult> Get(Guid id)
        {
            var comic = UnitOfWork.GetRepository<Comic, ComicRepository>().Get(id);

            if (comic == null)
            {
                return NotFound();
            }

            return Ok(comic);
        }

        [HttpGet("{id}/arcs")]
        public async Task<IActionResult> GetAllArcs(Guid id)
        {
            var arcs = UnitOfWork.GetRepository<Comic, ComicRepository>().GetArcsForComic(id);

            return Ok(arcs);
        }

        public override async Task<IActionResult> Create([FromBody] Comic comic)
        {
            var validation = await ValidatorService.Validate<ComicValidator>(comic);

            if (comic == null)
            {
                return BadRequest();
            }

            var savedComic = UnitOfWork.GetRepository<Comic>().Create(comic);

            return CreatedAtAction("Get", new { id = savedComic.Id }, savedComic);
        }

        public override async Task<IActionResult> Update(Guid id, [FromBody] Comic comic)
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

        public override async Task<IActionResult> Delete(Guid id)
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
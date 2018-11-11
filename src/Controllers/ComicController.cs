using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Repositories.Resource;
using AlbaVulpes.API.Services;
using AlbaVulpes.API.Validators;

namespace AlbaVulpes.API.Controllers
{
    [Route("comics")]
    [Produces("application/json")]
    public class ComicController : ApiController, IRestController<Comic>
    {
        public ComicController(IUnitOfWork unitOfWork, IValidatorService validator) : base(unitOfWork, validator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comics = await UnitOfWork.GetRepository<ComicRepository>().GetAll();

            return Ok(comics);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var comic = await UnitOfWork.GetRepository<ComicRepository>().Get(id);

            if (comic == null)
            {
                return NotFound();
            }

            return Ok(comic);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Comic comic)
        {
            var validation = await ValidatorService.GetValidator<ComicValidator>().ValidateAsync(comic);

            if (!validation.IsValid)
            {
                var error = validation.Errors.FirstOrDefault();
                return BadRequest(error?.ErrorMessage);
            }

            var savedComic = await UnitOfWork.GetRepository<ComicRepository>().Create(comic);

            return CreatedAtAction("Get", new { id = savedComic.Id }, savedComic);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Comic comic)
        {
            if (comic == null)
            {
                return BadRequest();
            }

            var updatedComic = await UnitOfWork.GetRepository<ComicRepository>().Update(id, comic);

            if (updatedComic == null)
            {
                return NotFound();
            }

            return Ok(updatedComic);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comicToDelete = await UnitOfWork.GetRepository<ComicRepository>().Delete(id);

            if (comicToDelete == null)
            {
                return NotFound();
            }

            return Ok(comicToDelete);
        }
    }
}
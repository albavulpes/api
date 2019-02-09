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
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "Creator")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Comic comic)
        {
            var validation = await ValidatorService.GetValidator<ComicValidator>().ValidateAsync(comic);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.FirstOrDefault()?.ErrorMessage);
            }

            var savedComic = await UnitOfWork.GetRepository<ComicRepository>().Create(comic);

            return CreatedAtAction("Get", new { id = savedComic.Id }, savedComic);
        }

        [Authorize(Roles = "Creator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Comic comic)
        {
            var validation = await ValidatorService.GetValidator<ComicValidator>().ValidateAsync(comic);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.FirstOrDefault()?.ErrorMessage);
            }

            var updatedComic = await UnitOfWork.GetRepository<ComicRepository>().Update(id, comic);

            if (updatedComic == null)
            {
                return NotFound();
            }

            return Ok(updatedComic);
        }

        [Authorize(Roles = "Creator")]
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
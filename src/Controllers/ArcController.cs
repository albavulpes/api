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
    [Route("arcs")]
    [Produces("application/json")]
    public class ArcController : ApiController, IRestController<Arc>
    {
        public ArcController(IUnitOfWork unitOfWork, IValidatorService validator) : base(unitOfWork, validator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForComic(Guid comicId)
        {
            var arcs = await UnitOfWork.GetRepository<ArcRepository>().GetAllArcsForComic(comicId);

            if (arcs == null)
            {
                return NotFound();
            }

            return Ok(arcs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var arc = await UnitOfWork.GetRepository<ArcRepository>().Get(id);

            if (arc == null)
            {
                return NotFound();
            }

            return Ok(arc);
        }

        [Authorize(Roles = "Creator")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Arc arc)
        {
            var validation = await ValidatorService.GetValidator<ArcValidator>().ValidateAsync(arc);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.FirstOrDefault()?.ErrorMessage);
            }

            var savedArc = await UnitOfWork.GetRepository<ArcRepository>().Create(arc);

            if (savedArc == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("Get", new { id = savedArc.Id }, savedArc);
        }

        [Authorize(Roles = "Creator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Arc arc)
        {
            if (arc == null)
            {
                return BadRequest();
            }

            var updatedArc = await UnitOfWork.GetRepository<ArcRepository>().Update(id, arc);

            if (updatedArc == null)
            {
                return NotFound();
            }

            return Ok(updatedArc);
        }

        [Authorize(Roles = "Creator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedArc = await UnitOfWork.GetRepository<ArcRepository>().Delete(id);

            if (deletedArc == null)
            {
                return NotFound();
            }

            return Ok(deletedArc);
        }
    }
}
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
    [Route("chapters")]
    [Produces("application/json")]
    public class ChapterController : ApiController, IRestController<Chapter>
    {
        public ChapterController(IUnitOfWork unitOfWork, IValidatorService validator) : base(unitOfWork, validator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForComic(Guid comicId)
        {
            var chapters = await UnitOfWork.GetRepository<ChapterRepository>().GetAllChaptersForComic(comicId);

            return Ok(chapters);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var chapter = await UnitOfWork.GetRepository<ChapterRepository>().Get(id);

            if (chapter == null)
            {
                return NotFound();
            }

            return Ok(chapter);
        }

        [Authorize(Roles = "Creator")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Chapter chapter)
        {
            var validation = await ValidatorService.GetValidator<ChapterValidator>().ValidateAsync(chapter);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.FirstOrDefault()?.ErrorMessage);
            }

            var savedChapter = await UnitOfWork.GetRepository<ChapterRepository>().Create(chapter);

            if (savedChapter == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("Get", new { id = savedChapter.Id }, savedChapter);
        }

        [Authorize(Roles = "Creator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Chapter chapter)
        {
            var validation = await ValidatorService.GetValidator<ChapterValidator>().ValidateAsync(chapter);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.FirstOrDefault()?.ErrorMessage);
            }

            var updatedChapter = await UnitOfWork.GetRepository<ChapterRepository>().Update(id, chapter);

            if (updatedChapter == null)
            {
                return NotFound();
            }

            return Ok(updatedChapter);
        }

        [Authorize(Roles = "Creator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var chapterToDelete = await UnitOfWork.GetRepository<ChapterRepository>().Delete(id);

            if (chapterToDelete == null)
            {
                return NotFound();
            }

            return Ok(chapterToDelete);
        }

        [Authorize(Roles = "Creator")]
        [HttpPut("{id}/publish")]
        public async Task<IActionResult> Publish(Guid id, bool state = true)
        {
            try
            {
                var chapterToPublish = await UnitOfWork.GetRepository<ChapterRepository>().Publish(id, state);

                if (chapterToPublish == null)
                {
                    return NotFound();
                }

                return Ok(chapterToPublish);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Creator")]
        [HttpPut("{id}/reorder/{index}")]
        public async Task<IActionResult> Reorder(Guid id, int index)
        {
            try
            {
                var chapterToReorder = await UnitOfWork.GetRepository<ChapterRepository>().Reorder(id, index);

                if (chapterToReorder == null)
                {
                    return NotFound();
                }

                return Ok(chapterToReorder);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}


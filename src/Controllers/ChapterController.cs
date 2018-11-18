using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Repositories.Resource;
using AlbaVulpes.API.Services;
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
        public async Task<IActionResult> GetAllForArc(Guid arcId)
        {
            var chapters = await UnitOfWork.GetRepository<ChapterRepository>().GetAllChaptersForArc(arcId);

            if (chapters == null)
            {
                return NotFound();
            }

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

            if (chapter == null)
            {
                return BadRequest();
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
            if (chapter == null)
            {
                return BadRequest();
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
    }
}


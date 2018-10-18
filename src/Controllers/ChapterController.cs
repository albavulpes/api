using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Repositories;

namespace AlbaVulpes.API.Controllers
{
    [Route("chapters")]
    [Produces("application/json")]
    public class ChapterController : ApiController<Chapter>
    {
        public ChapterController(IUnitOfWork unitOfWork, IValidatorService validator) : base(unitOfWork, validator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForArc(Guid arcId)
        {
            var chapters = await UnitOfWork.GetRepository<Chapter, ChapterRepository>().GetAllChaptersForArc(arcId);

            if (chapters == null)
            {
                return NotFound();
            }

            return Ok(chapters);
        }

        public override async Task<IActionResult> Create([FromBody] Chapter chapter)
        {

            if (chapter == null)
            {
                return BadRequest();
            }

            var savedChapter = await UnitOfWork.GetRepository<Chapter, ChapterRepository>().Create(chapter);

            if (savedChapter == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("Get", new { id = savedChapter.Id }, savedChapter);
        }

        public override async Task<IActionResult> Get(Guid id)
        {
            var chapter = await UnitOfWork.GetRepository<Chapter>().Get(id);

            if (chapter == null)
            {
                return NotFound();
            }

            return Ok(chapter);
        }


        public override async Task<IActionResult> Update(Guid id, [FromBody] Chapter chapter)
        {
            if (chapter == null)
            {
                return BadRequest();
            }

            var updatedChapter = await UnitOfWork.GetRepository<Chapter>().Update(id, chapter);

            if (updatedChapter == null)
            {
                return NotFound();
            }

            return Ok(updatedChapter);
        }

        public override async Task<IActionResult> Delete(Guid id)
        {
            var chapterToDelete = await UnitOfWork.GetRepository<Chapter>().Delete(id);

            if (chapterToDelete == null)
            {
                return NotFound();
            }

            return Ok(chapterToDelete);
        }
    }
}


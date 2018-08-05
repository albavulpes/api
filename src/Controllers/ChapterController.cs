using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;

namespace AlbaVulpes.API.Controllers
{
    [Route("chapters")]
    [Produces("application/json")]
    public class ChapterController : ApiController<Chapter>
    {
        public ChapterController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override IActionResult Create([FromBody] Chapter chapter)
        {
            if (chapter == null)
            {
                return BadRequest();
            }

            var savedChapter = UnitOfWork.GetRepository<Chapter>().Create(chapter);

            return CreatedAtAction("Get", new { id = savedChapter.Id }, savedChapter);
        }

        public override IActionResult Get(Guid id)
        {
            var chapter = UnitOfWork.GetRepository<Chapter>().Get(id);

            if (chapter == null)
            {
                return NotFound();
            }

            return Ok(chapter);
        }


        public override IActionResult Update(Guid id, [FromBody] Chapter chapter)
        {
            if (chapter == null)
            {
                return BadRequest();
            }

            var updatedChapter = UnitOfWork.GetRepository<Chapter>().Update(id, chapter);

            if (updatedChapter == null)
            {
                return NotFound();
            }

            return Ok(updatedChapter);
        }

        public override IActionResult Delete(Guid id)
        {
            var chapterToDelete = UnitOfWork.GetRepository<Chapter>().Delete(id);

            if (chapterToDelete == null)
            {
                return NotFound();
            }

            return Ok(chapterToDelete);
        }
    }
}


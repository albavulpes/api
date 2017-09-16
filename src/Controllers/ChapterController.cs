using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Models.Database;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;


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

            var newChapter = new Chapter
            {
                Number = chapter.Number,
                Title = chapter.Title,
                Pages = chapter.Pages
            };

            UnitOfWork.GetRepository<Chapter>().Create(newChapter);
            Response.Headers["ETag"] = newChapter.Hash;

            return CreatedAtRoute("chapters", new { id = newChapter.Id }, newChapter);
        }

        public override IActionResult Read(Guid id)
        {
            var chapter = UnitOfWork.GetRepository<Chapter>().GetSingle(id);

            if (chapter == null)
            {
                return NotFound();
            }

            var requestHash = Request.Headers["If-None-Match"];

            if (!string.IsNullOrEmpty(requestHash))
            {
                if (requestHash == chapter.Hash)
                {
                    return StatusCode((int)HttpStatusCode.NotModified);
                }
            }

            Response.Headers["ETag"] = chapter.Hash;

            return Ok(chapter);
        }


        public override IActionResult Update(Guid id, [FromBody] Chapter chapter)
        {
            if (chapter == null)
            {
                return BadRequest();
            }

            var chapterToUpdate = UnitOfWork.GetRepository<Chapter>().Update(id, chapter);

            if (chapterToUpdate == null)
            {
                NotFound();
            }

            return Ok(chapterToUpdate);
        }

        public override IActionResult Delete(Guid id)
        {
            var chapterToDelete = UnitOfWork.GetRepository<Chapter>().RemoveSingle(id);

            if (chapterToDelete == null)
            {
                return NotFound();
            }

            return Ok(chapterToDelete);
        }
    }
}


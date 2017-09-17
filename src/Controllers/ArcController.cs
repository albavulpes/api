using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;

namespace AlbaVulpes.API.Controllers
{
    [Route("api/Arc")]
    [Produces("application/json")]
    public class ArcController : ApiController<Arc>
    {
        public ArcController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override IActionResult Create([FromBody] Arc arc)
        {
            if (arc == null)
            {
                return BadRequest();
            }

            var newArc = new Arc
            {
                Number = arc.Number,
                Title = arc.Title,
                Chapters = arc.Chapters
            };

            UnitOfWork.GetRepository<Arc>().Create(newArc);

            Response.Headers["ETag"] = newArc.Hash;

            return CreatedAtAction("Read", new { id = newArc.Id }, newArc);
        }

        public override IActionResult Read(Guid id)
        {
            var arc = UnitOfWork.GetRepository<Arc>().GetSingle(id);

            if (arc == null)
            {
                return NotFound();
            }

            var requestHash = Request.Headers["If-None-Match"];

            if (!string.IsNullOrEmpty(requestHash))
            {
                if (requestHash == arc.Hash)
                {
                    return StatusCode((int)HttpStatusCode.NotModified);
                }
            }
            Response.Headers["ETag"] = arc.Hash;

            return Ok(arc);
        }

        public override IActionResult Update(Guid id, [FromBody] Arc arc)
        {
            if (arc == null)
            {
                return BadRequest();
            }

            var updatedArc = UnitOfWork.GetRepository<Arc>().Update(id, arc);

            if (updatedArc == null)
            {
                return NotFound();
            }

            Response.Headers["ETag"] = updatedArc.Hash;

            return Ok(updatedArc);
        }

        public override IActionResult Delete(Guid id)
        {
            var arcToDelete = UnitOfWork.GetRepository<Arc>().RemoveSingle(id);

            if (arcToDelete == null)
            {
                return NotFound();
            }

            return Ok(arcToDelete);
        }
    }
}
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Models.Database;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;

namespace AlbaVulpes.API.Controllers
{
    [Route("arcs")]
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

            return CreatedAtRoute("arcs", new { id = newArc.Id }, newArc);
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

            var arcToUpdate = UnitOfWork.GetRepository<Arc>().Update(id, arc);

            if (arcToUpdate == null)
            {
                NotFound();
            }

            return Ok(arcToUpdate);
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
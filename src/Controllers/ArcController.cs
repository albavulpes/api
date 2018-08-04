using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Repositories;

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

            var savedArc = UnitOfWork.GetRepository<Arc>().Create(arc);

            return CreatedAtAction("Read", new { id = savedArc.Id }, savedArc);
        }

        public override IActionResult Read(Guid id)
        {
            var arc = UnitOfWork.GetRepository<Arc>().Get(id);

            if (arc == null)
            {
                return NotFound();
            }

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

            return Ok(updatedArc);
        }

        public override IActionResult Delete(Guid id)
        {
            var deletedArc = UnitOfWork.GetRepository<Arc>().Delete(id);

            if (deletedArc == null)
            {
                return NotFound();
            }

            return Ok(deletedArc);
        }
    }
}
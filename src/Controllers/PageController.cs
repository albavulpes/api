using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Models.Database;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;

namespace AlbaVulpes.API.Controllers
{
    [Route("pages")]
    [Produces("application/json")]
    public class PageController : ApiController<Page>
    {
        public PageController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override IActionResult Create([FromBody] Page page)
        {
            if (page == null)
            {
                return BadRequest();
            }

            var newPage = new Page
            {
                Number = page.Number,
                Image = page.Image
            };

            UnitOfWork.GetRepository<Page>().Create(newPage);

            Response.Headers["ETag"] = newPage.Hash;

            return CreatedAtRoute("pages", new { id = newPage.Id }, newPage);
        }

        public override IActionResult Read(Guid id)
        {
            var page = UnitOfWork.GetRepository<Page>().GetSingle(id);

            if (page == null)
            {
                return NotFound();
            }

            var requestHash = Request.Headers["If-None-Match"];
            if (!string.IsNullOrEmpty(requestHash))
            {
                // Match the requested hash with the database hash
                if (requestHash == page.Hash)
                {
                    return StatusCode((int)HttpStatusCode.NotModified);
                }
            }

            Response.Headers["ETag"] = page.Hash;

            return Ok(page);
        }

        public override IActionResult Update(Guid id, [FromBody] Page page)
        {
            if (page == null)
            {
                return BadRequest();
            }

            var pageToUpdate = UnitOfWork.GetRepository<Page>().Update(id, page);

            if (pageToUpdate == null)
            {
                return NotFound();
            }

            return Ok(pageToUpdate);
        }

        public override IActionResult Delete(Guid id)
        {
            var pageToDelete = UnitOfWork.GetRepository<Page>().RemoveSingle(id);

            if (pageToDelete == null)
            {
                return NotFound();
            }

            return Ok(pageToDelete);
        }
    }
}
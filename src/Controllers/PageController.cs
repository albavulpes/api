using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;

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

            var savedPage = UnitOfWork.GetRepository<Page>().Create(page);

            return CreatedAtAction("Get", new { id = savedPage.Id }, savedPage);
        }

        public override IActionResult Get(Guid id)
        {
            var page = UnitOfWork.GetRepository<Page>().Get(id);

            if (page == null)
            {
                return NotFound();
            }

            return Ok(page);
        }

        public override IActionResult Update(Guid id, [FromBody] Page page)
        {
            if (page == null)
            {
                return BadRequest();
            }

            var updatedPage = UnitOfWork.GetRepository<Page>().Update(id, page);

            if (updatedPage == null)
            {
                return NotFound();
            }

            return Ok(updatedPage);
        }

        public override IActionResult Delete(Guid id)
        {
            var pageToDelete = UnitOfWork.GetRepository<Page>().Delete(id);

            if (pageToDelete == null)
            {
                return NotFound();
            }

            return Ok(pageToDelete);
        }
    }
}
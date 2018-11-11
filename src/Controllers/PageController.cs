using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Repositories.Resource;

namespace AlbaVulpes.API.Controllers
{
    [Route("pages")]
    [Produces("application/json")]
    public class PageController : ApiController, IRestController<Page>
    {
        public PageController(IUnitOfWork unitOfWork, IValidatorService validator) : base(unitOfWork, validator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForChapter(Guid chapterId)
        {
            var pages = await UnitOfWork.GetRepository<PageRepository>().GetAllPagesForChapter(chapterId);

            if (pages == null)
            {
                return NotFound();
            }

            return Ok(pages);
        }

        public async Task<IActionResult> Create([FromBody] Page page)
        {
            if (page == null)
            {
                return BadRequest();
            }

            var savedPage = await UnitOfWork.GetRepository<PageRepository>().Create(page);

            return CreatedAtAction("Get", new { id = savedPage.Id }, savedPage);
        }

        public async Task<IActionResult> Get(Guid id)
        {
            var page = await UnitOfWork.GetRepository<PageRepository>().Get(id);

            if (page == null)
            {
                return NotFound();
            }

            return Ok(page);
        }

        public async Task<IActionResult> Update(Guid id, [FromBody] Page page)
        {
            if (page == null)
            {
                return BadRequest();
            }

            var updatedPage = await UnitOfWork.GetRepository<PageRepository>().Update(id, page);

            if (updatedPage == null)
            {
                return NotFound();
            }

            return Ok(updatedPage);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var pageToDelete = await UnitOfWork.GetRepository<PageRepository>().Delete(id);

            if (pageToDelete == null)
            {
                return NotFound();
            }

            return Ok(pageToDelete);
        }
    }
}
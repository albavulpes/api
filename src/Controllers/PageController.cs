﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Repositories.Resource;
using AlbaVulpes.API.Services;
using AlbaVulpes.API.Validators;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Get(Guid chapterId, int? pageNumber)
        {
            if (pageNumber != null)
            {
                var page = await UnitOfWork.GetRepository<PageRepository>().GetPageByPageNumber(chapterId, pageNumber.Value);

                if (page == null)
                {
                    return NotFound();
                }

                return Ok(page);
            }

            var pages = await UnitOfWork.GetRepository<PageRepository>().GetAllPagesForChapter(chapterId);

            if (pages == null)
            {
                return NotFound();
            }

            return Ok(pages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var page = await UnitOfWork.GetRepository<PageRepository>().Get(id);

            if (page == null)
            {
                return NotFound();
            }

            return Ok(page);
        }

        [HttpGet("{id}/previous")]
        public async Task<IActionResult> GetPrevious(Guid id)
        {
            var page = await UnitOfWork.GetRepository<PageRepository>().GetPrevious(id);

            if (page == null)
            {
                return NotFound();
            }

            return Ok(page);
        }

        [HttpGet("{id}/next")]
        public async Task<IActionResult> GetNext(Guid id)
        {
            var page = await UnitOfWork.GetRepository<PageRepository>().GetNext(id);

            if (page == null)
            {
                return NotFound();
            }

            return Ok(page);
        }

        [Authorize(Roles = "Creator")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Page page)
        {
            if (page == null)
            {
                return BadRequest();
            }

            var validation = await ValidatorService.GetValidator<PageValidator>().ValidateAsync(page);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.FirstOrDefault()?.ErrorMessage);
            }

            var savedPage = await UnitOfWork.GetRepository<PageRepository>().Create(page);

            return CreatedAtAction("Get", new { id = savedPage.Id }, savedPage);
        }

        [Authorize(Roles = "Creator")]
        [HttpPut("{id}")]
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

        [Authorize(Roles = "Creator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var pageToDelete = await UnitOfWork.GetRepository<PageRepository>().Delete(id);

            if (pageToDelete == null)
            {
                return NotFound();
            }

            return Ok(pageToDelete);
        }

        [Authorize(Roles = "Creator")]
        [HttpPut("{id}/publish")]
        public async Task<IActionResult> Publish(Guid id, bool state = true)
        {
            try
            {
                var pageToPublish = await UnitOfWork.GetRepository<PageRepository>().Publish(id, state);

                if (pageToPublish == null)
                {
                    return NotFound();
                }

                return Ok(pageToPublish);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Creator")]
        [HttpPut("{id}/reorder/{index}")]
        public async Task<IActionResult> Reorder(Guid id, int index)
        {
            try
            {
                var pageToReorder = await UnitOfWork.GetRepository<PageRepository>().Reorder(id, index);

                if (pageToReorder == null)
                {
                    return NotFound();
                }

                return Ok(pageToReorder);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
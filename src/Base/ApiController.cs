using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Marten;
using AlbaVulpes.API.Interfaces;

namespace AlbaVulpes.API.Base
{
    public abstract class ApiController<TModel> : Controller, IRestController<TModel> where TModel : ApiModel
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IValidatorService ValidatorService;

        protected ApiController(IUnitOfWork unitOfWork, IValidatorService validator)
        {
            UnitOfWork = unitOfWork;
            ValidatorService = validator;
        }

        [HttpPost]
        public abstract Task<IActionResult> Create([FromBody] TModel data);

        [HttpGet("{id}")]
        public abstract Task<IActionResult> Get(Guid id);

        [HttpPut("{id}")]
        public abstract Task<IActionResult> Update(Guid id, [FromBody] TModel data);

        [HttpDelete("{id}")]
        public abstract Task<IActionResult> Delete(Guid id);
    }
}

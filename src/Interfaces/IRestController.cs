using AlbaVulpes.API.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AlbaVulpes.API.Interfaces
{
    public interface IRestController<in TModel> where TModel : ApiModel
    {
        Task<IActionResult> Get(Guid id);

        Task<IActionResult> Create([FromBody]TModel data);

        Task<IActionResult> Update(Guid id, [FromBody]TModel data);

        Task<IActionResult> Delete(Guid id);
    }
}

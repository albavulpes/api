using AlbaVulpes.API.Base;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AlbaVulpes.API.Interfaces
{
    public interface IRestController<in TModel> where TModel : ApiModel
    {
        IActionResult Get(Guid id);

        IActionResult Create([FromBody]TModel data);

        IActionResult Update(Guid id, [FromBody]TModel data);

        IActionResult Delete(Guid id);
    }
}

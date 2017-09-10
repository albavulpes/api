using AlbaVulpes.API.Base;
using Marten;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbaVulpes.API.Interfaces
{
    public interface IRestController<in TModel> where TModel : ApiModel
    {
        IActionResult Read(Guid id);

        IActionResult Create([FromBody]TModel data);

        IActionResult Update(Guid id, [FromBody]TModel data);

        IActionResult Delete(Guid id);
    }
}

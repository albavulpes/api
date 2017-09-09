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
        [HttpGet]
        IActionResult Get();

        [HttpGet("{id}")]
        IActionResult Get(Guid id);

        [HttpPost]
        IActionResult Create([FromBody]TModel data);

        [HttpPut("{id}")]
        IActionResult Update(Guid id, [FromBody]TModel data);

        [HttpDelete("{id}")]
        IActionResult Delete(Guid id);
    }
}

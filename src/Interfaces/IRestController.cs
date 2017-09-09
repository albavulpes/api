﻿using AlbaVulpes.API.Base;
using Marten;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbaVulpes.API.Interfaces
{
    interface IRestController<TModel> where TModel : ApiModel
    {
        [HttpGet]
        IEnumerable<TModel> Get();

        [HttpGet("{id}")]
        TModel Get(Guid id);

        [HttpPost]
        TModel Create([FromBody]TModel data);

        [HttpPut("{id}")]
        TModel Update(Guid id, [FromBody]TModel data);

        [HttpDelete("{id}")]
        TModel Delete(Guid id);
    }
}

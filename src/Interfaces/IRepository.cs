using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Interfaces
{
    public interface IRepository<TModel> where TModel : ApiModel
    {
        Task<IReadOnlyList<TModel>> GetAll();
        Task<TModel> Get(Guid id);
        Task<TModel> Create(TModel data);
        Task<TModel> Update(Guid id, TModel data);
        Task<TModel> Delete(Guid id);
    }
}
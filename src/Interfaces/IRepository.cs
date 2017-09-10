using System;
using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Interfaces
{
    public interface IRepository<TModel> where TModel : ApiModel
    {
        TModel GetSingle(Guid id);
        TModel Create(TModel data);
        TModel Update(Guid id, TModel data);
        TModel RemoveSingle(Guid id);
    }
}
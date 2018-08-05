using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AlbaVulpes.API.Interfaces;
using Marten;

namespace AlbaVulpes.API.Base
{
    public class ApiRepository<TModel> : IRepository<TModel> where TModel : ApiModel
    {
        protected readonly IDocumentStore Store;

        public ApiRepository(IDocumentStore documentStore)
        {
            Store = documentStore;
        }

        public virtual async Task<IReadOnlyList<TModel>> GetAll()
        {
            using (var session = Store.QuerySession())
            {
                var data = await session.Query<TModel>().ToListAsync();

                return data;
            }
        }

        public virtual async Task<TModel> Get(Guid id)
        {
            using (var session = Store.QuerySession())
            {
                var data = await session.LoadAsync<TModel>(id);

                return data;
            }
        }

        public virtual async Task<TModel> Create(TModel data)
        {
            using (var session = Store.OpenSession())
            {
                session.Insert(data);
                await session.SaveChangesAsync();

                return data;
            }
        }

        public virtual async Task<TModel> Update(Guid id, TModel data)
        {
            using (var session = Store.QuerySession())
            {
                var dbData = await session.LoadAsync<TModel>(id);

                if (dbData == null)
                {
                    return null;
                }

                // Ensure that the ID is provided in the model, and if not, set it anyway
                data.Id = dbData.Id;
            }

            using (var session = Store.OpenSession())
            {
                session.Update(data);
                await session.SaveChangesAsync();

                return data;
            }
        }

        public virtual async Task<TModel> Delete(Guid id)
        {
            using (var session = Store.OpenSession())
            {
                var data = await session.LoadAsync<TModel>(id);

                if (data == null)
                {
                    return null;
                }

                session.Delete<TModel>(id);
                await session.SaveChangesAsync();

                return data;
            }
        }
    }
}
using System;
using System.Linq;
using System.Net;
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

        public virtual TModel GetSingle(Guid id)
        {
            using (var session = Store.QuerySession())
            {
                var data = session.Load<TModel>(id);

                return data;
            }
        }

        public virtual TModel Create(TModel data)
        {
            using (var session = Store.OpenSession())
            {
                session.Insert(data);
                session.SaveChanges();

                data.ComputeHash();

                session.Update(data);
                session.SaveChanges();

                return data;
            }
        }

        public virtual TModel Update(Guid id, TModel data)
        {
            using (var session = Store.OpenSession())
            {
                var dbData = session.Load<TModel>(id);

                if (dbData == null)
                {
                    return null;
                }

                // Ensure that the ID is provided in the model, and if not, set it anyway
                data.Id = dbData.Id;

                data.ComputeHash();

                session.Update(data);
                session.SaveChanges();

                return data;
            }
        }

        public virtual TModel RemoveSingle(Guid id)
        {
            using (var session = Store.OpenSession())
            {
                var data = session.Load<TModel>(id);

                if (data == null)
                {
                    return null;
                }

                session.Delete<TModel>(id);
                session.SaveChanges();

                return data;
            }
        }
    }
}
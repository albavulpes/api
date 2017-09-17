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
                var data = session.Query<TModel>().FirstOrDefault(x => x.Id == id);

                return data;
            }
        }

        public virtual TModel Create(TModel data)
        {
            using (var session = Store.OpenSession())
            {
                session.Store(data);
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
                var dbData = session.Query<TModel>().FirstOrDefault(x => x.Id == id);

                if (dbData == null)
                {
                    return null;
                }

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
                var data = session.Query<TModel>().FirstOrDefault(x => x.Id == id);

                if (data == null)
                {
                    return null;
                }

                session.DeleteWhere<TModel>(x => x.Id == id);
                session.SaveChanges();

                return data;
            }
        }
    }
}
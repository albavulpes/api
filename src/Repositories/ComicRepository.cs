using System;
using System.Collections.Generic;
using System.Linq;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Resource;
using Marten;

namespace AlbaVulpes.API.Repositories
{
    public class ComicRepository : ApiRepository<Comic>
    {
        public ComicRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public override Comic Get(Guid id)
        {
            using (var session = Store.QuerySession())
            {
                var comic = session.Load<Comic>(id);
                comic.Arcs = session.Query<Arc>()
                    .Where(arc => arc.ComicId == comic.Id)
                    .ToList();

                return comic;
            }
        }
    }
}
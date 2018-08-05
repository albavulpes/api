using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IReadOnlyList<Arc>> GetArcsForComic(Guid id)
        {
            using (var session = Store.QuerySession())
            {
                var arcs = await session.Query<Arc>()
                    .Where(arc => arc.ComicId == id)
                    .ToListAsync();

                return arcs;
            }
        }
    }
}
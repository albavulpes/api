using System;
using System.Collections.Generic;
using System.Linq;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Resource;
using Marten;

namespace AlbaVulpes.API.Repositories
{
    public class ArcRepository : ApiRepository<Arc>
    {
        public ArcRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public override Arc Create(Arc arc)
        {
            var comicId = arc.ComicId;

            if (comicId == Guid.Empty)
            {
                return null;
            }

            using (var session = Store.QuerySession())
            {
                if (!session.Query<Comic>().Any(c => c.Id == comicId))
                {
                    return null;
                }

                var arcsCountInComic = session.Query<Arc>().Count(a => a.ComicId == comicId);

                arc.Number = arcsCountInComic + 1;

                return base.Create(arc);
            }
        }
    }
}
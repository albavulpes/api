using System;
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

        public Comic AddArc(Guid comicId, Guid arcId)
        {
            using (var session = Store.OpenSession())
            {
                var comic = session.Load<Comic>(comicId);
                var arc = session.Load<Arc>(arcId);

                comic.Arcs.Add(new ArcInfo
                {
                    Id = arc.Id,
                    Title = arc.Title,
                    Number = arc.Number,
                    CoverImageThumbnail = arc.CoverImageThumbnail
                });

                Update(comicId, comic);

                return comic;
            }
        }

        public Comic RemoveArc(Guid comicId, Guid arcId)
        {
            using (var session = Store.OpenSession())
            {
                var comic = session.Load<Comic>(comicId);

                comic.Arcs.RemoveAll(a => a.Id == arcId);

                Update(comicId, comic);

                return comic;
            }
        }
    }
}
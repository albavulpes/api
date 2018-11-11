using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Models.View;
using AutoMapper;
using Marten;

namespace AlbaVulpes.API.Repositories.Resource
{
    public class ArcRepository : RestRepository<Arc>
    {
        public ArcRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public async Task<IReadOnlyList<Arc>> GetAllArcsForComic(Guid comicId)
        {
            if (comicId == Guid.Empty)
            {
                return null;
            }
            using (var session = _store.QuerySession())
            {
                var arcs = await session.Query<Arc>()
                    .Where(arc => arc.ComicId == comicId)
                    .ToListAsync();

                var results = arcs
                    .Select(async (arc) =>
                    {
                        var viewModel = Mapper.Map<ArcViewModel>(arc);
                        viewModel.ChaptersCount = await session.Query<Chapter>().CountAsync(chapter => chapter.ArcId == arc.Id);

                        return viewModel;
                    });

                return await Task.WhenAll(results.ToList());
            }
        }

        public override async Task<Arc> Create(Arc arc)
        {
            var comicId = arc.ComicId;

            if (comicId == Guid.Empty)
            {
                return null;
            }

            using (var session = _store.QuerySession())
            {
                if (!session.Query<Comic>().Any(c => c.Id == comicId))
                {
                    return null;
                }

                var arcsCountInComic = await session.Query<Arc>().CountAsync(a => a.ComicId == comicId);

                arc.Number = arcsCountInComic + 1;

                return await base.Create(arc);
            }
        }
    }
}
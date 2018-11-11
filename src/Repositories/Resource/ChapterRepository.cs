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
    public class ChapterRepository : RestRepository<Chapter>
    {
        public ChapterRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public async Task<IReadOnlyList<Chapter>> GetAllChaptersForArc(Guid arcId)
        {
            if (arcId == Guid.Empty)
            {
                return null;
            }
            using (var session = _store.QuerySession())
            {
                var chapters = await session.Query<Chapter>()
                    .Where(chapter => chapter.ArcId == arcId)
                    .ToListAsync();

                var results = chapters
                    .Select(async (chapter) =>
                    {
                        var viewModel = Mapper.Map<ChapterViewModel>(chapter);
                        viewModel.PagesCount = await session.Query<Page>().CountAsync(page => page.ChapterId == chapter.Id);

                        return viewModel;
                    });

                return await Task.WhenAll(results.ToList());
            }
        }

        public override async Task<Chapter> Create(Chapter chapter) 
        {
            var arcId = chapter.ArcId;

            if (arcId == Guid.Empty)
            {
                return null;
            }

            using (var session = _store.QuerySession())
            {
                if (!session.Query<Arc>().Any(a => a.Id == arcId))
                {
                    return null;
                }

                var chaptersCountInArc = await session.Query<Chapter>().CountAsync(ch => ch.ArcId == arcId);

                chapter.ChapterNumber = chaptersCountInArc + 1;

                return await base.Create(chapter);
            }
        }
    }
}

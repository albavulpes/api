using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Resource;
using Marten;

namespace AlbaVulpes.API.Repositories.Resource
{
    public class PageRepository : RestRepository<Page>
    {
        public PageRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public async Task<IReadOnlyList<Page>> GetAllPagesForChapter(Guid chapterId)
        {
            if(chapterId == Guid.Empty)
            {
                return null;
            }
            using (var session = Store.QuerySession())
            {
                var pages = await session.Query<Page>()
                    .Where(page => page.ChapterId == chapterId)
                    .ToListAsync();

                return pages.ToList();
            }
        }

        public override async Task<Page> Create(Page page)
        {
            var chapterId = page.ChapterId;

            if (chapterId == Guid.Empty)
            {
                return null;
            }

            using (var session = Store.QuerySession())
            {
                if (!session.Query<Chapter>().Any(ch => ch.Id == chapterId))
                {
                    return null;
                }

                var pagesCountInChapter = await session.Query<Page>().CountAsync(p => p.ChapterId == chapterId);

                page.PageNumber = pagesCountInChapter + 1;

                return await base.Create(page);
            }
        }
    }
}

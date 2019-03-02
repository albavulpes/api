using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Helpers;
using AlbaVulpes.API.Models.Resource;
using Marten;

namespace AlbaVulpes.API.Repositories.Resource
{
    public class PageRepository : ApiRepository
    {
        public PageRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public async Task<IReadOnlyList<Page>> GetAllPagesForChapter(Guid chapterId)
        {
            if (chapterId == Guid.Empty)
            {
                return null;
            }

            using (var session = _store.QuerySession())
            {
                var pages = await session.Query<Page>()
                    .Where(page => page.ChapterId == chapterId)
                    .ToListAsync();

                var sortedPages = pages
                    .OrderBy(page => page.PageNumber)
                    .ToList();

                return sortedPages;
            }
        }

        public async Task<Page> Get(Guid id)
        {
            using (var session = _store.QuerySession())
            {
                var data = await session.LoadAsync<Page>(id);

                return data;
            }
        }

        public async Task<Page> Create(Page data)
        {
            var chapterId = data.ChapterId;

            if (chapterId == Guid.Empty)
            {
                return null;
            }

            using (var session = _store.QuerySession())
            {
                if (!session.Query<Chapter>().Any(ch => ch.Id == chapterId))
                {
                    return null;
                }

                var pagesCountInChapter = await session.Query<Page>().CountAsync(p => p.ChapterId == chapterId);

                data.PageNumber = pagesCountInChapter + 1;
            }

            using (var session = _store.OpenSession())
            {
                session.Insert(data);
                await session.SaveChangesAsync();

                return data;
            }
        }

        public async Task<Page> Update(Guid id, Page data)
        {
            using (var session = _store.QuerySession())
            {
                var dbData = await session.LoadAsync<Page>(id);

                if (dbData == null)
                {
                    return null;
                }

                data.Id = dbData.Id;
                data.ChapterId = dbData.ChapterId;
                data.CreatedDate = dbData.CreatedDate;
            }

            using (var session = _store.OpenSession())
            {
                session.Update(data);
                await session.SaveChangesAsync();

                return data;
            }
        }

        public async Task<Page> Delete(Guid id)
        {
            using (var session = _store.OpenSession())
            {
                var data = await session.LoadAsync<Page>(id);

                if (data == null)
                {
                    return null;
                }

                session.Delete<Page>(id);
                await session.SaveChangesAsync();

                return data;
            }
        }

        public async Task<Page> Reorder(Guid id, int index)
        {
            using (var session = _store.OpenSession())
            {
                var pageToReorder = await session.LoadAsync<Page>(id);

                if (pageToReorder == null)
                {
                    return null;
                }

                var allPagesForChapter = await session.Query<Page>()
                    .Where(p => p.ChapterId == pageToReorder.ChapterId)
                    .OrderBy(p => p.PageNumber)
                    .ToListAsync();

                var pagesList = allPagesForChapter.ToList();

                pagesList.Remove(pageToReorder);
                pagesList.Insert(index, pageToReorder);

                pagesList.Reorder(p => p.PageNumber);
                
                session.Update(pagesList.ToArray());
                await session.SaveChangesAsync();

                return pageToReorder;
            }
        }
    }
}

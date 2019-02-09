using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Models.Responses;
using AutoMapper;
using Baseline;
using Marten;

namespace AlbaVulpes.API.Repositories.Resource
{
    public class ChapterRepository : ApiRepository
    {
        private readonly IMapper _mapper;

        public ChapterRepository(IDocumentStore documentStore, IMapper autoMapper) : base(documentStore)
        {
            _mapper = autoMapper;
        }

        public async Task<IReadOnlyList<ChapterGroupResponse>> GetAllChaptersForComic(Guid comicId)
        {
            if (comicId == Guid.Empty)
            {
                return null;
            }

            using (var session = _store.QuerySession())
            {
                var chapters = await session.Query<Chapter>()
                    .Where(chapter => chapter.ComicId == comicId)
                    .ToListAsync();

                var chapterResponses = chapters
                    .Select(chap => _mapper.Map<ChapterResponse>(chap))
                    .ToList();

                foreach (var viewModel in chapterResponses)
                {
                    viewModel.PagesCount = await session.Query<Page>().CountAsync(page => page.ChapterId == viewModel.Id);
                }

                var chapterArcIds = chapterResponses
                    .Where(chapter => chapter.ArcId != null)
                    .Select(chapter => chapter.ArcId.Value)
                    .Distinct()
                    .ToList();

                var chapterArcs = await session.Query<Arc>()
                    .Where(arc => chapterArcIds.Contains(arc.Id))
                    .ToListAsync();

                var chapterArcsMap = chapterArcs.ToDictionary(arc => arc.Id);

                var groupedResults = chapterResponses
                    .GroupBy(chapter => chapterArcsMap.Get(chapter.ArcId ?? Guid.Empty))
                    .Select(group => new ChapterGroupResponse
                    {
                        Arc = group.Key,
                        Chapters = group.ToList()
                    })
                    .ToList();

                return groupedResults;
            }
        }

        public async Task<ChapterResponse> Get(Guid id)
        {
            using (var session = _store.QuerySession())
            {
                var data = await session.LoadAsync<Chapter>(id);

                var result = _mapper.Map<ChapterResponse>(data);

                result.PagesCount = await session.Query<Page>().CountAsync(arc => arc.ChapterId == data.Id);

                return result;
            }
        }

        public async Task<Chapter> Create(Chapter data)
        {
            var comicId = data.ComicId;

            if (comicId == Guid.Empty)
            {
                return null;
            }

            // TODO: Chapter number logic here (especially with Arc grouping)
            //using (var session = _store.QuerySession())
            //{
            //    if (!session.Query<Comic>().Any(c => c.Id == comicId))
            //    {
            //        return null;
            //    }

            //    var chaptersCountInArc = await session.Query<Chapter>().CountAsync(ch => ch.ArcId == comicId);
            //    var chaptersCountInArc = await session.Query<Chapter>().CountAsync(ch => ch.ArcId == comicId);

            //    data.ChapterNumber = chaptersCountInArc + 1;
            //}

            using (var session = _store.OpenSession())
            {
                session.Insert(data);
                await session.SaveChangesAsync();

                return data;
            }
        }

        public async Task<Chapter> Update(Guid id, Chapter data)
        {
            using (var session = _store.QuerySession())
            {
                var dbData = await session.LoadAsync<Chapter>(id);

                if (dbData == null)
                {
                    return null;
                }

                data.Id = dbData.Id;
                data.ComicId = dbData.ComicId;
                data.CreatedDate = dbData.CreatedDate;
            }

            using (var session = _store.OpenSession())
            {
                session.Update(data);
                await session.SaveChangesAsync();

                return data;
            }
        }

        public virtual async Task<Chapter> Delete(Guid id)
        {
            using (var session = _store.OpenSession())
            {
                var data = await session.LoadAsync<Chapter>(id);

                if (data == null)
                {
                    return null;
                }

                session.Delete<Chapter>(id);
                await session.SaveChangesAsync();

                return data;
            }
        }
    }
}

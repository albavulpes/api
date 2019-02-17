using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Models.Responses;
using AutoMapper;
using Marten;

namespace AlbaVulpes.API.Repositories.Resource
{
    public class ComicRepository : ApiRepository
    {
        private readonly IMapper _mapper;

        public ComicRepository(IDocumentStore documentStore, IMapper autoMapper) : base(documentStore)
        {
            _mapper = autoMapper;
        }

        public async Task<IReadOnlyList<Comic>> GetAll()
        {
            using (var session = _store.QuerySession())
            {
                var comics = await session.Query<Comic>().ToListAsync();

                var results = comics
                    .OrderByDescending(comic => comic.CreatedDate)
                    .Select(comic => _mapper.Map<ComicResponse>(comic))
                    .ToList();

                foreach (var comicViewModel in results)
                {
                    comicViewModel.ChaptersCount = await session.Query<Chapter>().CountAsync(chapter => chapter.ComicId == comicViewModel.Id);
                }

                return results;
            }
        }

        public async Task<ComicResponse> Get(Guid id)
        {
            using (var session = _store.QuerySession())
            {
                var data = await session.LoadAsync<Comic>(id);

                if (data == null)
                {
                    return null;
                }

                var result = _mapper.Map<ComicResponse>(data);

                result.ChaptersCount = await session.Query<Chapter>().CountAsync(chapter => chapter.ComicId == data.Id);

                return result;
            }
        }

        public virtual async Task<Comic> Create(Comic data)
        {
            data.CreatedDate = DateTime.UtcNow;

            using (var session = _store.OpenSession())
            {
                session.Insert(data);
                await session.SaveChangesAsync();

                return data;
            }
        }

        public async Task<Comic> Update(Guid id, Comic data)
        {
            using (var session = _store.QuerySession())
            {
                var dbData = await session.LoadAsync<Comic>(id);

                if (dbData == null)
                {
                    return null;
                }

                data.Id = dbData.Id;
                data.CreatedDate = dbData.CreatedDate;
            }

            using (var session = _store.OpenSession())
            {
                session.Update(data);
                await session.SaveChangesAsync();

                return data;
            }
        }

        public virtual async Task<Comic> Delete(Guid id)
        {
            using (var session = _store.OpenSession())
            {
                var data = await session.LoadAsync<Comic>(id);

                if (data == null)
                {
                    return null;
                }

                session.Delete<Comic>(id);
                await session.SaveChangesAsync();

                return data;
            }
        }

        public virtual async Task<Comic> Publish(Guid id, bool state)
        {
            using (var session = _store.OpenSession())
            {
                var comicToPublish = await session.LoadAsync<Comic>(id);

                if (comicToPublish == null)
                {
                    return null;
                }

                // If comic is already published
                if (comicToPublish.PublishDate != null && comicToPublish.PublishDate < DateTime.UtcNow)
                {
                    if (state)
                    {
                        throw new Exception("Comic has already been published");
                    }

                    comicToPublish.PublishDate = null;
                }
                else
                {
                    if (!state)
                    {
                        throw new Exception("Comic is not published yet");
                    }

                    comicToPublish.PublishDate = DateTime.UtcNow;
                }


                session.Update(comicToPublish);
                await session.SaveChangesAsync();

                return comicToPublish;
            }
        }
    }
}
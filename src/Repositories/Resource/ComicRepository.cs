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
    public class ComicRepository : RestRepository<Comic>
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
                    .Select(comic => _mapper.Map<ComicResponse>(comic))
                    .ToList();

                foreach (var comicViewModel in results)
                {
                    comicViewModel.ArcsCount = await session.Query<Arc>().CountAsync(arc => arc.ComicId == comicViewModel.Id);
                }

                return results;
            }
        }

        public new virtual async Task<ComicResponse> Get(Guid id)
        {
            using (var session = _store.QuerySession())
            {
                var data = await session.LoadAsync<Comic>(id);

                var result = _mapper.Map<ComicResponse>(data);

                result.ArcsCount = await session.Query<Arc>().CountAsync(arc => arc.ComicId == data.Id);

                return result;
            }
        }
    }
}
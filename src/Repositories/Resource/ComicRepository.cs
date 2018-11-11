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
        public ComicRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public async Task<IReadOnlyList<Comic>> GetAll()
        {
            using (var session = _store.QuerySession())
            {
                var comics = await session.Query<Comic>().ToListAsync();

                var results = comics
                    .Select(async (comic) =>
                    {
                        var viewModel = Mapper.Map<ComicViewModel>(comic);
                        viewModel.ArcsCount = await session.Query<Arc>().CountAsync(arc => arc.ComicId == comic.Id);

                        return viewModel;
                    });

                return await Task.WhenAll(results.ToList());
            }
        }

        public new virtual async Task<ComicViewModel> Get(Guid id)
        {
            using (var session = _store.QuerySession())
            {
                var data = await session.LoadAsync<Comic>(id);

                var result = Mapper.Map<ComicViewModel>(data);

                result.ArcsCount = await session.Query<Arc>().CountAsync(arc => arc.ComicId == data.Id);

                return result;
            }
        }
    }
}
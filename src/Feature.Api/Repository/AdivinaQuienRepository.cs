using Feature.Api.Config;
using Feature.Api.Entities;
using Feature.Api.Entities.Filters;
using Feature.Core;
using RepoDb;

namespace Feature.Api.Repository
{
    public class AdivinaQuienRepository : FeatureRepositoryBase
    {
        private ApiConfig _config;

        public AdivinaQuienRepository(ApiConfig config) : base(config)
        {
            _config = config;
        }

        public async Task<List<Character>> PersonajeListarAsync(Characterfilter filter)
        {
            var personajeList = new List<Character>();
            for (int i = 0; i < 10; i++)
            {
                personajeList.Add(new Character { CharacterId = i,
                Description = $"Personaje {i}"
                });
            }
            return personajeList;
        }

        public async Task<List<Category>> CategoryListAsync()
        {
            var categoryList = new List<Category>();
            for (int i = 0; i < 10; i++)
            {
                categoryList.Add(new Category
                {
                    CategoryId = i,
                    Description = $"Categoría - {i}",

                });
            }
            return categoryList;
        }
    }
}

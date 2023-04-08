using Feature.Api.Config;
using Feature.Api.Entities;
using Feature.Api.Entities.Filters;
using Feature.Api.Repository;

namespace Feature.Api.Business
{
    public class AdivinaQuienBusiness
    {
        private ApiConfig _apiConfig;
        private AdivinaQuienRepository _adivinaQuienRepository;
        private Serilog.ILogger _logger;

        public AdivinaQuienBusiness(AdivinaQuienRepository adivinaQuienRepository,
                             ApiConfig apiConfig,
                             Serilog.ILogger logger
                             )
        {
            _adivinaQuienRepository = adivinaQuienRepository;
            _apiConfig = apiConfig;
            _logger = logger;
        }

        public async Task<List<Character>> PersonajeListarAsync(Characterfilter filter)
        {
            return await _adivinaQuienRepository.PersonajeListarAsync(filter);
        }

        public async Task<List<Category>> CategoryListAsync()
        {
            return await _adivinaQuienRepository.CategoryListAsync();
        }
    }
}

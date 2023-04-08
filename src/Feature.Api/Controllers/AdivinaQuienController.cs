using Feature.Api.Business;
using Feature.Api.Entities;
using Feature.Api.Entities.Filters;
using Feature.Api.Hub;
using Feature.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Feature.Api.Controllers
{
    [AllowAnonymous]
    public class AdivinaQuienControllerInternal : CoreAuthControllerBase
    {
        private AdivinaQuienBusiness _adivinaQuienBusiness { get; set; }
        private readonly IHubContext<CustomHub> _hubContext;

        private ILogger _logger { get; set; }

        public AdivinaQuienControllerInternal(AdivinaQuienBusiness adivinaQuienBusiness,
                                IHubContext<CustomHub> hubContext,
                                ILogger logger)
        {
            _adivinaQuienBusiness = adivinaQuienBusiness;
            _hubContext = hubContext;
            _logger = logger;
        }

        //[HttpGet]
        //[Route("start")]
        //public async Task<IActionResult> Start([FromBody] Player player)
        //{
        //    try
        //    {
        //        await _hubContext.Clients.Group($"{player.Code}").SendAsync("refresh");
        //        var resultado = await _adivinaQuienBusiness.CategoryListAsync();
        //        return Ok(resultado);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"[AdivinaQuienController] CategoryList > {ex.Message}");
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet]
        [Route("category/list")]
        public async Task<IActionResult> CategoryList()
        {
            try
            {
                //await _hubContext.Clients.Group($"{player.Code}").SendAsync("refresh");
                var resultado = await _adivinaQuienBusiness.CategoryListAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.Error($"[AdivinaQuienController] CategoryList > {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("personajes/listar")]
        public async Task<IActionResult> PersonajesListar([FromBody] Characterfilter filter)
        {
            try
            {
                var resultado = await _adivinaQuienBusiness.PersonajeListarAsync(filter);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.Error($"[AdivinaQuienController] PersonajesListar > {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

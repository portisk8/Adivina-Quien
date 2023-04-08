using Feature.Api.Business;
using Feature.Api.Hub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AdivinaQuien.Api.Controllers
{
    [Route("api/[controller]")]
    public class AdivinaQuienController : Feature.Api.Controllers.AdivinaQuienControllerInternal
    {
        public AdivinaQuienController(AdivinaQuienBusiness adivinaQuienBusiness, 
                                IHubContext<CustomHub> hubContext,
                                Serilog.ILogger logger) 
            : base(adivinaQuienBusiness, hubContext, logger)
        {
        }

    }
}

using Microsoft.AspNetCore.Mvc;

namespace RedSocial.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiversion}/[controller]")]
    public abstract class BaseApiController : Controller
    {
        
    }
}

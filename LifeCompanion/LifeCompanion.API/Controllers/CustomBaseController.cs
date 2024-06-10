using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeCompanion.API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomBaseController : ControllerBase {
    }
}

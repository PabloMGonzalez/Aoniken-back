using Microsoft.AspNetCore.Mvc;


namespace Aoniken.Controllers
{

    [ApiController]
    [Route("user")]

    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public dynamic listarClientes()
        {
            return "Hola";
        }
    }
}
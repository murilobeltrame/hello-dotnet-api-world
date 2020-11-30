using ApiShowcase.Auth.Models;
using ApiShowcase.Auth.Repository;
using ApiShowcase.Auth.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiShowcase.Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : Controller
    {
        [HttpPost]
        public IActionResult Authenticate([FromBody]User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);
            if (user == null) return NotFound(new { message = "Usuário ou senha inválidos" });
            var token = TokenService.GenerateToken(user);
            return Created("", new { token = token });
        }
    }
}

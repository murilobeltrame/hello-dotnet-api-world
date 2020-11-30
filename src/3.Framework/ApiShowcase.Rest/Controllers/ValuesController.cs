using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiShowcase.Rest.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        [Route("anonymous")]
        public string GetAnonymous()
        {
            return "Anonymous";
        }

        [Authorize]
        [HttpGet]
        [Route("authenticated")]
        public string GetAuthenticated()
        {
            return $"Authenticaded with {User.Identity.Name}";
        }

        [Authorize(Roles = "employee,manager")]
        [HttpGet]
        [Route("employees")]
        public string GetForEmployees()
        {
            return "This is for Managers and Employees";
        }

        [Authorize(Roles = "manager")]
        [HttpGet]
        [Route("managers")]
        public string GetForManagers()
        {
            return "This is for Managers only";
        }
    }
}

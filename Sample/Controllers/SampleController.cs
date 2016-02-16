using System.Web.Http;

namespace WebApi.BasicAuth.Sample.Controllers
{
    public class SampleController : ApiController
    {
        /// <summary>
        /// Required no authentication.
        /// </summary>
        [HttpGet, Route("no-auth")]
        public string NoAuth()
        {
            return "Some content";
        }

        /// <summary>
        /// Requires authentication, accepts all users.
        /// </summary>
        [HttpGet, Route("auth"), Authorize]
        public string Auth()
        {
            return "Some content";
        }

        /// <summary>
        /// Requires authentication, accepts only a specific user.
        /// </summary>
        [HttpGet, Route("auth-user"), Authorize(Users = "JohnDoe")]
        public string AuthUser()
        {
            return "Some content";
        }

        /// <summary>
        /// Requires authentication, accepts only users with a specific role.
        /// </summary>
        [HttpGet, Route("auth-role"), Authorize(Roles = "Operator")]
        public string AuthRole()
        {
            return "Some content";
        }
    }
}
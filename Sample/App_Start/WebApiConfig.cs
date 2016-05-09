using System.Web.Http;
using WebApi.BasicAuth;

namespace BasicAuthSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.EnableBasicAuth();

            config.EnsureInitialized();
        }
    }
}
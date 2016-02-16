using System.Web.Http;

namespace WebApi.BasicAuth.Sample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.EnableBasicAuth();
        }
    }
}
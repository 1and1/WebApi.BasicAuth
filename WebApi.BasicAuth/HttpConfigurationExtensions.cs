using System.Web.Http;

namespace WebApi.BasicAuth
{
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        /// Enables HTTP Basic Auth with users stored in the application configuration file (Web.config or App.config).
        /// </summary>
        public static HttpConfiguration EnableBasicAuth(this HttpConfiguration configuration)
        {
            var basicAuthConfig = BasicAuthSection.Load();
            if (basicAuthConfig != null)
                configuration.MessageHandlers.Add(new BasicAuthHandler(basicAuthConfig));
            return configuration;
        }
    }
}
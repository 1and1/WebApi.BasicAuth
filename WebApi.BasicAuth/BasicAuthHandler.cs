using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.BasicAuth
{
    /// <summary>
    /// Matches HTTP Basic Authentication headers against a fixed list of users.
    /// </summary>
    public class BasicAuthHandler : DelegatingHandler
    {
        private readonly BasicAuthSection _config;

        /// <summary>
        /// Creates a new HTTP Basic Authentication handler.
        /// </summary>
        /// <param name="config">The configuration specifying the valid users.</param>
        public BasicAuthHandler(BasicAuthSection config)
        {
            _config = config;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var credentials = ParseAuthentication(request.Headers.Authorization);
            if (credentials != null)
            {
                var user = _config.Users.OfType<User>().FirstOrDefault(x =>
                    x.Username.Equals(credentials.UserName, StringComparison.InvariantCultureIgnoreCase));
                if (user != null)
                {
                    if (string.IsNullOrEmpty(user.HashAlgorithm))
                    {
                        if (credentials.Password == user.Password)
                            SetPrincipal(request, user);
                    }
                    else
                    {
                        var algo = HashAlgorithm.Create(user.HashAlgorithm);
                        if (algo == null)
                            throw new ConfigurationErrorsException($"No known hash algorithm called {user.HashAlgorithm}.");

                        string hashedPassword = BitConverter.ToString(algo.ComputeHash(Encoding.UTF8.GetBytes(credentials.Password))).Replace("-", "");
                        if (hashedPassword == user.Password.ToUpperInvariant())
                            SetPrincipal(request, user);
                    }
                }
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized && response.Headers.WwwAuthenticate.Count == 0)
                SetAuthenticationChallenge(response);

            return response;
        }

        private static NetworkCredential ParseAuthentication(AuthenticationHeaderValue authentication)
        {
            if (authentication == null || authentication.Scheme != "Basic" ||
                string.IsNullOrWhiteSpace(authentication.Parameter))
                return null;

            var credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authentication.Parameter)).Split(':');
            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[1]))
                return null;

            return new NetworkCredential(credentials[0], credentials[1]);
        }

        private static void SetPrincipal(HttpRequestMessage request, User user)
        {
            request.GetRequestContext().Principal = new GenericPrincipal(
                new GenericIdentity(user.Username),
                user.Roles.OfType<Role>().Select(x => x.Name).ToArray());
        }

        private void SetAuthenticationChallenge(HttpResponseMessage response)
        {
            response.Headers.WwwAuthenticate.Add(string.IsNullOrEmpty(_config?.Realm)
                ? new AuthenticationHeaderValue("Basic")
                : new AuthenticationHeaderValue("Basic", $"realm=\"{_config.Realm}\""));
        }
    }
}
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace WebApi.BasicAuth
{
    /// <summary>
    /// Matches HTTP Basic Authentication headers against a fixed list of users.
    /// </summary>
    public class BasicAuthFilter : IAuthenticationFilter
    {
        private readonly BasicAuthSection _config;

        /// <summary>
        /// Creates a new HTTP Basic Authentication handler.
        /// </summary>
        /// <param name="config">The configuration specifying the valid users.</param>
        public BasicAuthFilter(BasicAuthSection config)
        {
            _config = config;
        }

        public bool AllowMultiple => false;

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var credentials = ParseAuthentication(context.Request.Headers.Authorization);
            if (credentials != null)
            {
                var user = _config.Users.OfType<User>().FirstOrDefault(x =>
                    x.Username.Equals(credentials.UserName, StringComparison.InvariantCultureIgnoreCase));
                if (user != null)
                {
                    if (string.IsNullOrEmpty(user.HashAlgorithm))
                    {
                        if (credentials.Password == user.Password)
                            context.Principal = BuildPrincipal(user);
                    }
                    else
                    {
                        var algo = HashAlgorithm.Create(user.HashAlgorithm);
                        if (algo == null)
                            throw new ConfigurationErrorsException($"No known hash algorithm called {user.HashAlgorithm}.");

                        string hashedPassword = BitConverter.ToString(algo.ComputeHash(Encoding.UTF8.GetBytes(credentials.Password))).Replace("-", "");
                        if (hashedPassword == user.Password.ToUpperInvariant())
                            context.Principal = BuildPrincipal(user);
                    }
                }
            }

            return Task.FromResult(true);
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

        protected virtual IPrincipal BuildPrincipal(User user)
        {
            return new GenericPrincipal(
                new GenericIdentity(user.Username),
                user.Roles.OfType<Role>().Select(x => x.Name).ToArray());
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new BasicAuthChallengeResult(context.Result, _config?.Realm);
            return Task.FromResult(true);
        }
    }
}
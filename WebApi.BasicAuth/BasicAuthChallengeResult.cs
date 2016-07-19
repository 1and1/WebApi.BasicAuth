using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.BasicAuth
{
    internal class BasicAuthChallengeResult : IHttpActionResult
    {
        private readonly IHttpActionResult _result;
        private readonly string _realm;

        public BasicAuthChallengeResult(IHttpActionResult result, string realm)
        {
            _result = result;
            _realm = realm;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await _result.ExecuteAsync(cancellationToken);
            response.Headers.WwwAuthenticate.Add(string.IsNullOrEmpty(_realm)
                ? new AuthenticationHeaderValue("Basic")
                : new AuthenticationHeaderValue("Basic", $"realm=\"{_realm}\""));
            return response;
        }
    }
}
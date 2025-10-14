using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MtlsDemo.Auth
{
    public class MtlsAuthenticationOptions : AuthenticationSchemeOptions { }

    public class MtlsAuthenticationHandler : AuthenticationHandler<MtlsAuthenticationOptions>
    {
        private readonly MtlsSettings _settings;

        public MtlsAuthenticationHandler(
            IOptionsMonitor<MtlsAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IOptions<MtlsSettings> mtlsSettings
        )
            : base(options, logger, encoder)
        {
            _settings = mtlsSettings.Value;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var clientCert = Context.Connection.ClientCertificate;

            if (_settings.RequireClientCertificate)
            {
                if (clientCert == null)
                    return Task.FromResult(
                        AuthenticateResult.Fail("No client certificate provided")
                    );

                if (!clientCert.Issuer.Contains(_settings.TrustedIssuer))
                    return Task.FromResult(AuthenticateResult.Fail("Invalid issuer"));
            }
            else
            {
                if (clientCert == null)
                    return Task.FromResult(AuthenticateResult.NoResult());
            }

            var claims = new[] { new Claim(ClaimTypes.Name, clientCert?.Subject ?? "Anonymous") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}

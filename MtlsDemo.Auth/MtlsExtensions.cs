using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MtlsDemo.Auth
{
    public static class MtlsExtensions
    {
        public static IServiceCollection AddMtls(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var mtlsSection = configuration.GetSection("Mtls");
            services.Configure<MtlsSettings>(mtlsSection);

            var mtlsSettings = mtlsSection.Get<MtlsSettings>() ?? new MtlsSettings();

            if (mtlsSettings.Enabled)
            {
                services
                    .AddAuthentication("mtls")
                    .AddScheme<MtlsAuthenticationOptions, MtlsAuthenticationHandler>("mtls", null);
                services.AddAuthorization();
            }

            return services;
        }

        public static void ConfigureKestrelMtls(
            this KestrelServerOptions kestrelOptions,
            MtlsSettings mtlsSettings
        )
        {
            if (mtlsSettings.Enabled)
            {
                kestrelOptions.ConfigureHttpsDefaults(httpsOptions =>
                {
                    if (!string.IsNullOrEmpty(mtlsSettings.ServerCertificatePath))
                    {
                        httpsOptions.ServerCertificate = new X509Certificate2(
                            mtlsSettings.ServerCertificatePath,
                            mtlsSettings.ServerCertificatePassword
                        );
                    }
                    httpsOptions.ClientCertificateMode = mtlsSettings.RequireClientCertificate
                        ? ClientCertificateMode.RequireCertificate
                        : ClientCertificateMode.NoCertificate;
                });
            }
        }
    }
}
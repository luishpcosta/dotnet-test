using MtlsDemo.Auth;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMtls(builder.Configuration);


var mtlsSettings = builder.Configuration.GetSection("Mtls").Get<MtlsSettings>() ?? new MtlsSettings();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps(mtlsSettings.ServerCertificatePath, mtlsSettings.ServerCertificatePassword, httpsOptions =>
        {
            httpsOptions.ClientCertificateMode = mtlsSettings.RequireClientCertificate
                ? Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.RequireCertificate
                : Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.NoCertificate;
        });
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/test", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name ?? "Anonymous"}!");

app.Run();
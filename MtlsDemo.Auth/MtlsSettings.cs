namespace MtlsDemo.Auth
{
    public class MtlsSettings
    {
        public bool Enabled { get; set; } = true;
        public string TrustedIssuer { get; set; } = "MyRootCA";
        public bool RequireClientCertificate { get; set; } = true;
        public string ServerCertificatePath { get; set; } = string.Empty;
        public string ServerCertificatePassword { get; set; } = string.Empty;
    }
}
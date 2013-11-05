namespace Wdsf.Api.Client
{
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;

#if DEBUG

    /// <summary>
    /// This class allows all certificates to be seen as valid. Use for local debugging only!
    /// </summary>
    internal class CertificatePolicy
    {
        internal static bool ValidateSSLCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;    // All certificates are considered valid, no matter what.
        }
    }
#endif
}

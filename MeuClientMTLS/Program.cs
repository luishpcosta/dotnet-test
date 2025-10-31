using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Pkcs;

class Program
{
    static async Task Main()
    {
        // Suponha que você já tenha o conteúdo base64 do arquivo combinado
        string base64Combined = "LS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSUVxekNDQXBNQ0ZDVy8rZStVYzcyc1UrZHNzS2x2c3VHWVpVdUpNQTBHQ1NxR1NJYjNEUUVCQ3dVQU1CTXgKRVRBUEJnTlZCQU1NQ0UxNVVtOXZkRU5CTUI0WERUSTFNVEF6TVRBek1UQXlPVm9YRFRNMU1UQXlPVEF6TVRBeQpPVm93RVRFUE1BMEdBMVVFQXd3R1kyeHBaVzUwTUlJQ0lqQU5CZ2txaGtpRzl3MEJBUUVGQUFPQ0FnOEFNSUlDCkNnS0NBZ0VBd213UnBmNkl5Z0ZnMHpTT0JncnZ6d1hqRVlnTTdFT21nR0QxVndwVUl1bkF0cnJsSXYvREEvUjQKWHlXc2Irb0pzbFQzM0EyTFVGZmJKQkpGQk13MWFVYnlMSG5MZWRITHRpY1RJQWZQVXlIblcvR3Q1QnpSRE9yZwpsWmZpM2svN05JUG9OV2RuYWRPMUk2OUIxMHlNSlczekZlaWpUOEExa1AvYW1DTmNyekJwaUdJVkR4aXJCUGhnCnlDMEhIMS84WmlZaks3aVBpTzQvMmhjNzZhcGw3TW5DWElZVXJ0RHVTMjhERlBXQTAyNUE1MjA0ajJJQ0VaeFEKRTl5bXBrT0hhdEZ6MWF4R3NpUHltNnlkRkE4Y05taTYwalNtMGZ1d0h0ZVNMcUVwaTBXMGN2aWhSc1RyWVEzNAo0RFpMbitZNEk5Ty8xa1dXTENOM1cwSTJnWXg3QS9tSXhxdW45NTNaWCs4QUlmK1JJcmRVWE5qdXJhMXE3K1hRCkNWNHc0dFUvSnpOMVkvNEUrQTdFM2cvK04wZXgvanVuMGRkcndsV2g5T3k5WWJFWmkzQWM4ZnpsdHVVSXpVSEUKb3ZIRGFxa3RXUDlhNlV6ZUkzRW00NVpqVEh4M2VYTGpXV051NDJIbXhZMlNUVXo5c0xXMlNDcGN3Z1FzMStwZApTcFU0VHdWMFJYak5aeG9LVkYvNlpEK2RpYkRJdWw2MFpOSjI5L0MwTHRzQlZPNjRpdHM3eS9vOUNWNU96a3ptCk1RM2h2YUVTZjlOZ25xZFhnWEpGS3I4OWU4emxaMWZBQ243MFlmdk5sbW9VSjAzVmhDN3ViZlJIeE53Wkx4bWkKRG5TcFhBalJqQUFRRGZTYmEzWDNaYlJHWFNGUFVHU1Qzd2F6czFFZ0pEOWFFM1plSVlrQ0F3RUFBVEFOQmdrcQpoa2lHOXcwQkFRc0ZBQU9DQWdFQWF1SHdYNHQ5UC9iYXlWSjBvQ1dvQzU1VExTTWYxY29lQWhocXN4cktsdFlLCkFZTFhLVE1CbmdXNzYyeHpHY2t4eVpmU282dkVhL0NZbjg0ZEVYVk9GOVZYb21MUkVPeXZuRVdTTkRtZmlFUzAKQ01qQkVxUU50eHFDU1o1QUdwdHhsZWZPMW81MncwM2tWSkhKdE9EN21uZWNtRXZxV1orV040aUlTN3U5N3NKVgppVHcxa1Uyc213d3hoNE5tTThNKzJiYlk5QUFGS1hJc0hkeUxkKy9Ed2NLMzJGL3M4UXBMcGdQdXFVeis3cWxnCnF5NUtDdnkwRnJRWnBZZzZ0dTlOTHUxR0k4NE5aRVJzeUVtWlZnWUlxL1g1TkdIUkh2T0I3RHpHODhzNkZaMDgKNGJYYm8vaVh0clRZQk9HMDFnV0V6SnFydzNubktlck5BVUltZHQwOENBUXl5b0hLeXpPR0RmcDB0a1NCcHlEWgptb3ZpQlVKbm5VbXpSRm90R2hvMFZ6bVJLRXBmSnMyUnNSK092ZUJ1VEdYbXl1ZkI4YndndWR4ZG94QVAwbDVzCmQ5WFVtc001b0szb0NKNWVzOVMvVThGTHN1S0Jid1RNcWV6SDkySEtFcEQwZXZRbVdSdk9PMVVUcHJlczduRkQKa045eTZwZWlZTEpiMTVDSU9KT2ZIUlFLSmlHZTA4MkdsNHRLNkxTMG9uanVzODNsbU9DdmQ1Qnl1MzFzTjQ3eQpyWHJCeU1mTDByTU9vQkVnVk0wNUxUTmo2Uk1BZlo2OXZNcm1qeHRsdWdFWURXRUNTRmNtYVJ0Zm1TSFovZnZxCnBHTThCRU84Q2k3N2FvTHl3ZHJHU21Pb2s3dFVRZXBzV2YwOXIxd1FFVVowMTZ6dytFV1VuNDY1Y2djS1JEVT0KLS0tLS1FTkQgQ0VSVElGSUNBVEUtLS0tLQotLS0tLUJFR0lOIFBSSVZBVEUgS0VZLS0tLS0KTUlJSlFnSUJBREFOQmdrcWhraUc5dzBCQVFFRkFBU0NDU3d3Z2drb0FnRUFBb0lDQVFEQ2JCR2wvb2pLQVdEVApOSTRHQ3UvUEJlTVJpQXpzUTZhQVlQVlhDbFFpNmNDMnV1VWkvOE1EOUhoZkpheHY2Z215VlBmY0RZdFFWOXNrCkVrVUV6RFZwUnZJc2VjdDUwY3UySnhNZ0I4OVRJZWRiOGEza0hORU02dUNWbCtMZVQvczBnK2cxWjJkcDA3VWoKcjBIWFRJd2xiZk1WNktOUHdEV1EvOXFZSTF5dk1HbUlZaFVQR0tzRStHRElMUWNmWC94bUppTXJ1SStJN2ovYQpGenZwcW1Yc3ljSmNoaFN1ME81TGJ3TVU5WURUYmtEbmJUaVBZZ0lSbkZBVDNLYW1RNGRxMFhQVnJFYXlJL0tiCnJKMFVEeHcyYUxyU05LYlIrN0FlMTVJdW9TbUxSYlJ5K0tGR3hPdGhEZmpnTmt1ZjVqZ2owNy9XUlpZc0kzZGIKUWphQmpIc0QrWWpHcTZmM25kbGY3d0FoLzVFaXQxUmMyTzZ0cldydjVkQUpYakRpMVQ4bk0zVmovZ1Q0RHNUZQpELzQzUjdIK082ZlIxMnZDVmFIMDdMMWhzUm1MY0J6eC9PVzI1UWpOUWNTaThjTnFxUzFZLzFycFRONGpjU2JqCmxtTk1mSGQ1Y3VOWlkyN2pZZWJGalpKTlRQMnd0YlpJS2x6Q0JDelg2bDFLbFRoUEJYUkZlTTFuR2dwVVgvcGsKUDUySnNNaTZYclJrMG5iMzhMUXUyd0ZVN3JpSzJ6dkwrajBKWGs3T1RPWXhEZUc5b1JKLzAyQ2VwMWVCY2tVcQp2ejE3ek9WblY4QUtmdlJoKzgyV2FoUW5UZFdFTHU1dDlFZkUzQmt2R2FJT2RLbGNDTkdNQUJBTjlKdHJkZmRsCnRFWmRJVTlRWkpQZkJyT3pVU0FrUDFvVGRsNGhpUUlEQVFBQkFvSUNBR3lOQ0NHRkJ2eTZYa1F1RUtuaEMxbXMKTU1hR0Rxa1BjSFEzTG1RbG1TcFJGczh2ZEdYaEpSSWRPZVB0MWVRNmFLV1ArVDk4dEc3c1RxZEtLQXp3czFXUApUZ0YxRXgxMS9HWThWTnBLWWdCSmFyS1VNb0d0Y1M3a0tQWjg4aWNyd3Ria1d3OE9KOWdYWXdpRWZwOWwxczNECklMajN4Yzd1UXpvdEQvQm50Z1QrWTczSS9YY0xUOWtia0hYVFppbW5Ydlh0ankrSEFXVHptT1Q4bWUveHVxekYKVFdaNVdpenM3RTlqc1BYUHlOakRzeXREZVJQZEpjaU5YTXhxSXdXcnFIMEVhc2MvUTgyTWNwVm1mK0ZHT250SwphQnliVnhUZ2tNbjNsRi9DamI1NzhhOUxrRWhDQkNXUk5vYUE2aUpoMkxCd05xaTgrNGF6V3AyeVQxcHZnL0tPClpaRGdrUzMzTGJIVDdmbTlMTzZNamtaZWlid1ljN3NzVnNQbGFockduNDFDNGdZR1MvdDRnZm5HL2VGYlVKQy8KTXVKdTdKd0tITEx3dGdlK0RHMjlCZ3JZeWFtK09vdk5CQVNhRGxtRTRTQTloakhJR0Nxd0dhV0lhQ29uMGVzRApYcDRLQnY4MEFITXk1ZGlnUEt2V2QrT1QrTjVoUEh3WStsOXBIYU1lcmJndzgrZkZhMjA5UGZlVUtkZmZhUTkwCkRDK2hLSGF4Q1Vic0Q3dlZSdk1wWnphNWtaMWZYZlNYR1htdEswdDhaU2lpYTFsdkQ5NDhKSHlKQURYeHZTdU0KdFpPYzl3ZitPbjhOMDBXODRucElGSUFZTzZsM2oxN0FiUTVqYkljalFtV2Q5ZTFwWGpUZ2g2TWtzSUpRbEI4bQpkUTkvWGVRWWU5UE13K3pTSXcyOUFvSUJBUUQ2VnE3cmJoZjg4RlpHNFYvcU81dHFxWVR0OU1rQ3JzNEV0NWJ6CmpIcEExSjQ3ak1LaTFqUWdkOWhGM3JNSCsvaS9CTVB5MzVTWmNHQVFEMVIrV2RkSVJhblJzSXROSGt5K2c0akoKa3daTzcxZUdMWEQ1UU5pWkJNN3ptejJCdElWeVlzWGttSEtPdk9RV3JnV3J5UVNkRTkyU0hrVGdYODY0cVFDUQpNaDVuNHgwbXdTeXlUSjZMWVhPWUhZR3FvSk1CU2FNbFBqOEFHVnJMcnRiSzhOeVhDWlVGallsMnlWSExDa3lqCjlwUW94VGlPRnNacXN4QjM0VGFyNkN5VmdTRHUwd2c0cWJ0RTlUQW16UmxRQzAvY2lSSDUrcHBVeU9MbnZibEYKOVZjQ0JCcjVRU1QzUUYxZ1RjeDhvMnVwTFp6RlBMYmpYWmp1a1RDRDA0UUhia2lUQW9JQkFRREcwYWxWeFVqQwpQL1dMakg4cHdHQ1VLNXBoVDJJUFBNRU92eHBsT2lRYit2V2xLME5tdG1PdjJ5MXptSWRGRjNqTE51YlhTOTZMCjVMTURWWnZVa2pBRmE4QVh3U2xpSkFuOE5yNVBRM0llbWdNKzhBL091bGVqZUUvaE9hVlNldVpkUDBqRGtaRm0KUVZ2WEtyaXdTYUFodEJoNVAxLzR3c3FDN3pyWks5c3BPR3pHTDFnakZkMlFnL0lQUkFEU3VJbFZMWTQ1U1RUbgpMSStIZHQrK2xLZHFJbHc1OC81T1RSYi9pZ3RybktVNmx5TStMaXp1UVJKSW4rRHNZVDdVM05VZXpuMHNqRUZpCnVkU3lTcmQ5RWp5V2FIYmpIK0duYlhoSG5CSFkzQmNiUEIyNzBZMVdMczFPNFpzWERPVCtqRE5YanJHQnE4K2IKUEZFN3UrTitNSXJ6QW9JQkFCQUNRUmRmSi91TjV3YUxYUlhlai9iSE5tLzlLbW1ZZ2lZOUUxTXAxWnVWdlRTegpzTEszSGdoQmdid2VITGJTL3pvLzlNUHdWbEZscFU4Qm0vYWtpaWJlQlprYVFCNURaTWIrSkY0M243eW1nNk5aCi9wS0IraGYyL2NFSVdXdVUvcitzMzFqYUF1RnI1TzRxWE1SNmkyZjhJRHc2NFU2ejhjVlBVWERuTTVpVEpvY1cKN1dQTmtJbjdSbWdnNHV6bGJlRTBNMUdyWkYzK1FSY3JiQ0w0T2pNU2Nzd3pHK3BUdjM1TGYxRXc4SktXNGlIOApCTjVINnpLdlhVNnEvMGJrUmhYZnV2UGNydFFmRmVBMVYzd043U0orUlg4RjdwNmpaaGRKdzdKY21SN1E0bVMwCkxNZisvcm1zMy9uTWZaTUpEckJ5VElQSHRZTDBGL1VtTHBoSERpOENnZ0VBSkVQRjhXRWszbG13UGg0SWErTXQKT3BtaGRUSjNublBrZXY0Y29HNDBlKzRRdEducHgyaHBVb0JXRUFtNFVyL3ZScEhqRlU0L2I2RHRqaVFsWW1hdgozNjFCd3BOM0t2R0w2TGRMaTBXd0lTalNncjVqQThBMWtCbTM2RXNOZnJmd0lQNFVGYWdOcjJzb2gxekJoMVpHCm9SOHFmNk14ckR4TG1BcHUzNElDWGNodS9zOVA4a1pndmFWemw1azkxVEhYeXZPZVI1QzAxNTNuS0R4a24yeWEKQk4vemxwYU13bGVKWkY4WHFpOU9iQnQ1VURRMytkVmdzbUtITWxhaWtVMGVpcG43ZHc2Mk5uM1M4QnAwMFpscgo5N3RpUkJkaW41bDJJUW0wcjZ1bGcxQTUxREpxNTFkdUYzOUxQSzhiYmRyQTJxMnFidzZadGFVUTVDWm5Ybk1ZClJRS0NBUUVBa2gyZ1BvanZkc3ZqRGYvUko4ZjI1T3JnMkJyTTlqNktQVGVCRG1DV2VmUDBYY0EreWNuVmJFM08KcHFDTGh3MFNHVlR5WXY1V3l0aG9NS1VYalEzQ1RiSzZNUUNBY2xVeERXcGlvM0dSUDRPd2F5b29JVFFQUkROdQpyV1RZWjBhYk1kN2ljMytJM3V2S24vSjIxcFp0QjlmYmJJS1BpWTVVc2FJYmp5OFRTNEFMMS9RMW83U0JFVCtwCkw0RllkdHJKNUJKdDhMVlQzcGlHMldLR0N3M0QrS1F1LzJCYUxqeGJOREs2ektDcFNhbkY1VHpiNS9DQTFtZTAKS3c1b3d5N3loUkNrOTU3eHZ2aVM0L05tWW0vZE9kWE9ZL3FCQ2ZYOVpZTkxudFpEZXkwYW5sZG9tU3R1MHNUVwp6NHlNNmdRUEdocnJYdVFacldYMGxjcmtTWmMwVXc9PQotLS0tLUVORCBQUklWQVRFIEtFWS0tLS0tCg=="; // ou pegue de uma variável

        // Decodifica o base64
        string combinedPem = Encoding.UTF8.GetString(Convert.FromBase64String(base64Combined));

        // Extrai certificado e chave privada usando BouncyCastle
        var (cert, key) = ParsePem(combinedPem);

        // Cria PFX em memória
        var store = new Pkcs12Store();
        var certEntry = new X509CertificateEntry(cert);
        string alias = "client";
        store.SetCertificateEntry(alias, certEntry);
        store.SetKeyEntry(alias, new AsymmetricKeyEntry(key), new[] { certEntry });

        using var ms = new MemoryStream();
        string pfxPassword = ""; // pode ser vazio
        store.Save(ms, pfxPassword.ToCharArray(), new SecureRandom());
        ms.Position = 0;

        // Cria X509Certificate2 a partir do PFX em memória
        var x509 = new X509Certificate2(ms.ToArray(), pfxPassword, X509KeyStorageFlags.Exportable);

        // Configura HttpClientHandler com o certificado do cliente
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(x509);

        // Ignora validação do certificado do servidor (apenas para DEV!)
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        using var client = new HttpClient(handler);
        var response = await client.GetAsync("https://localhost:8443/");
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
    }

    static (Org.BouncyCastle.X509.X509Certificate, AsymmetricKeyParameter) ParsePem(string pem)
    {
        Org.BouncyCastle.X509.X509Certificate? cert = null;
        AsymmetricKeyParameter? key = null;
        using var reader = new StringReader(pem);
        var pemReader = new PemReader(reader);
        object? obj;
        while ((obj = pemReader.ReadObject()) != null)
        {
            if (obj is Org.BouncyCastle.X509.X509Certificate c) cert = c;
            else if (obj is AsymmetricCipherKeyPair kp) key = kp.Private;
            else if (obj is AsymmetricKeyParameter k && k.IsPrivate) key = k;
        }
        if (cert == null || key == null)
            throw new Exception("Certificado ou chave privada não encontrados no PEM.");
        return (cert, key);
    }
}
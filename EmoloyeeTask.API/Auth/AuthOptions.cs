using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace EmoloyeeTask.API.Auth
{
    internal static class AuthOptions
    {
        internal const int TTL = 1;
        internal const string Issuer = "MyIssuer";
        internal const string Audience = "MyServer";

        internal static SecurityKey PublicKey = GetPublicKey();
        internal static SecurityKey PrivateKey = GetPrivateKey();

        private const string PublicKeyString = "MIIBCgKCAQEAzaNkuI9pHcU7kPR1jnJqva3P4Xuqz//CVZ9krB\u002BIhT70Gr2uBl1bPqV1bcoNKPA5Y3f\u002B7pSiOJZYc4YOsRwjT/Qgqcgf8GtyiLNEGF7cO5gAndY4ARgw1EIWKBm1cZJYKUiOdSZ59ypjVJOpdyq8Boh2mL0yF4KdhAT/zLMUWY0X8kqJL9a9kn64giF73rDv4U42F02zK1xvB8VIkldpUjIgSd4DymT1tWBbM7wCIOGRwH7Vrv8RLRlvXFZK274BH9L2hDAQpR3nwRxA7qBgv\u002BBnXwbQySICJ5aWyS8KMRddlHlawNNZhL16BufuSIBJXsvqG0baPP9gypVuee\u002BnRQIDAQAB";
        private const string PrivateKeyString = "MIIEpAIBAAKCAQEAzaNkuI9pHcU7kPR1jnJqva3P4Xuqz//CVZ9krB\u002BIhT70Gr2uBl1bPqV1bcoNKPA5Y3f\u002B7pSiOJZYc4YOsRwjT/Qgqcgf8GtyiLNEGF7cO5gAndY4ARgw1EIWKBm1cZJYKUiOdSZ59ypjVJOpdyq8Boh2mL0yF4KdhAT/zLMUWY0X8kqJL9a9kn64giF73rDv4U42F02zK1xvB8VIkldpUjIgSd4DymT1tWBbM7wCIOGRwH7Vrv8RLRlvXFZK274BH9L2hDAQpR3nwRxA7qBgv\u002BBnXwbQySICJ5aWyS8KMRddlHlawNNZhL16BufuSIBJXsvqG0baPP9gypVuee\u002BnRQIDAQABAoIBAApNHBeOv5JJNhGt3VG0c5pq4ZYHDuh6hG6rnGxu6/TY0Xuhct1IwfBic357d6tMVWOhODkVYwb4qReoaIYAxdgUf6phpw/xIfytFkUuQ1QM6kL93zJW1uNbKCb8GrXEwfvXcaN2uPdjvTQYbY3VqzOclGxUVfUNivx5D6NeeS9/JcugKZdygfbC/gTn\u002BrpNBcJnOVWTVpn3PGneyuoxKo4e8VfpIvTXR96VHjNnfQ0vJuDx3z9mvMLRmWJgLAPO7L64/5exvxnrBdRdRS9FOnioJ9\u002Bc4sjqiM5/xoU9IziMnnKwhHDvLM/iXlEYYNMrqo\u002B5yOGasDUkX1ga6GOXN2UCgYEAzedjxu7V63TQHkiGTH6vLbn8FcH1ULcFhm8OX8spvq79m1EsF74wKHRfGtTIY7Ydtdhl/DLdVWjdKt02mLdAqME98Cr9UVUnKXvfodmjjF\u002BUAj2ARHwrD9WDzbXrNXSVH7E7QIWAZKQi1zqFl66YMtUHx\u002BIvd0zo2W0KlWKl438CgYEA/6t11JvqzI78mNRQRvr3sdK3cIKFDUeD3zsgHh2mKVQY4RMDkye2vBwLZ8lqQ9UwPiaTkjEyVHskd/aypyY1t/9QTVT2BNEBwfuyPyCm6yePVhlvR54xN6PTi1Dk0ZBO2tUNqxQhndmSbsBSR/oDtoRvyu\u002BXmJl84BEG3yJkRzsCgYAdw\u002B92mJwwF4jmd9GCSWYFlDdO5eUSsAKKdo97ZaE5QY7D3y4imInZr0j4Vf5J\u002B8\u002B7s/a6SJgaerhONABqf0NHoffUOkK4eTjaFGA1w4KTDlYVJNLgE5seBN2y5TOFXL8Z/8LzBtmSsu\u002BecYVSchjqcEVZrebythmLJOo8wi/8/QKBgQCPRRDpjFneOX45UjiOHhLxdghPpwRSNbIjROmxlqHHZRXwAoytSMLw1LJbfZXr3yqCmXF2qdTvXHZKN1dTrWjK6\u002Bqe\u002By5F8q2Oe7IeB/khq3B6KhSeyHoCGO7xkeo3uStHG77mEIjqAMM7BtDU/rhX7g0LZonH/LLqiQAVyBrzgwKBgQCarnticEJpqXpDzxWA69SuiUCVDQIqTWiC\u002BSVMZaSwgnItGH1Xv3FZaGnt9USKu2L6And3pZM7VcqS/yfPtmC24rvhxGDjyB07vmrurj1IDF4FBaqX5NNMwI0MohIdpNaseRhPJ563WIBWnYeE66ajAWGUcxpt24mV9Os9v1ufIA==";
    
        private static SecurityKey GetPublicKey()
        {
            var key = RSA.Create();
            key.ImportRSAPublicKey(source: Convert.FromBase64String(PublicKeyString), bytesRead: out _);
            return new RsaSecurityKey(key);
        }
        private static SecurityKey GetPrivateKey()
        {
            var key = RSA.Create();
            key.ImportRSAPrivateKey(source: Convert.FromBase64String(PrivateKeyString), bytesRead: out _);
            return new RsaSecurityKey(key);
        }
    }
}

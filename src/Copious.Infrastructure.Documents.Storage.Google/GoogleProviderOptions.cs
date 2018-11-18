using Google.Apis.Storage.v1;

namespace Copious.Infrastructure.Documents.Storage.Google {
    public class GoogleProviderOptions {
        public string Email { get; set; }

        public string Bucket { get; set; }

        public string P12PrivateKey { get; set; }
    }
}
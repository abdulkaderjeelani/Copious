namespace Copious.Infrastructure.Documents.Storage.Amazon {
    public class AmazonProviderOptions {
        public string PublicKey { get; set; }

        public string SecretKey { get; set; }

        public string Bucket { get; set; }

        public string ServiceUrl { get; set; }
    }
}
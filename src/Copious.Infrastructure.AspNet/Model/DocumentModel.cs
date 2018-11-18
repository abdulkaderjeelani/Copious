using Copious.Document.Interface.State;
using Microsoft.AspNetCore.Http;

namespace Copious.Infrastructure.AspNet.Model {
    public class DocumentModel {
        public DocumentMetadata Metadata { get; set; }
        public DocumentDetail Detail { get; set; }
        public DocumentSecurity Security { get; set; }
        public File File { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
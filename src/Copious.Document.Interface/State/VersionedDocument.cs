using Copious.Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Copious.Utilities;

namespace Copious.Document.Interface.State
{
    /// <remarks>
    /// Typically this shall be stored in a separate no sql database, and index shall be stored in a rdbms,
    /// as we store now in a ef backed db,  we use backing properties
    /// </remarks>
    public class VersionedDocument : Entity
    {
        public Guid DocumentId { get; set; }
        public int VersionNo { get; set; }
        public DocumentKind DocumentKind { get; set; }

        /*Json fields*/
        public string _Metadata { get; set; }
        public string _RelatedDocumentIds { get; set; }
        public string _Detail { get; set; }
        public string _File { get; set; }        
        public string _Access { get; set; }
        public string _Security { get; set; }

        /*Json backing properties*/

        [NotMapped]
        private DocumentMetadata _metadata;

        [NotMapped]
        public DocumentMetadata Metadata {

            get
            {
                if (_metadata == null)
                    _metadata = JsonUtils.To<DocumentMetadata>(_Metadata);

                return _metadata;
            }
            set
            {
                _metadata = value;
                _Metadata = JsonUtils.From(_metadata);
            }
        }
                
        [NotMapped]
        private List<Guid> _relatedDocumentIds;

        [NotMapped]
        public List<Guid> RelatedDocumentIds
        {

            get
            {
               if( _relatedDocumentIds == null)
                    _relatedDocumentIds = JsonUtils.To<List<Guid>>(_RelatedDocumentIds);

                return _relatedDocumentIds;
            }
            set
            {
                _relatedDocumentIds = value;
                _RelatedDocumentIds = JsonUtils.From(_relatedDocumentIds);
            }
        }

        [NotMapped]
        private DocumentDetail _detail;

        [NotMapped]
        public DocumentDetail Detail
        {
            get
            {
                if (_detail == null)
                    switch (DocumentKind)
                    {
                        case DocumentKind.Business:
                            _detail = JsonUtils.To<BusinessDetail>(_Detail);
                            break;

                        case DocumentKind.Content:
                        default:
                            _detail = JsonUtils.To<ContentDetail>(_Detail);
                            break;
                    }
                return _detail;
            }
            set
            {
                _detail = value;
                _Detail = JsonUtils.From(_detail);
            }
        }

        [NotMapped]
        private File _file;

        [NotMapped]
        public File File
        {
            get
            {
                if (_file == null)
                    _file = JsonUtils.To<File>(_File);

                return _file;
            }
            set
            {
                _file = value;
                _File = JsonUtils.From(_file);
            }
        }

        [NotMapped]
        private DocumentAccess _access;

        [NotMapped]
        public DocumentAccess Access {

            get
            {
                if (_access == null)
                    _access = JsonUtils.To<DocumentAccess>(_Access);

                return _access;
            }
            set
            {
                _access = value;
                _Access = JsonUtils.From(_access);
            }
        }

        [NotMapped]
        private DocumentSecurity _security;

        [NotMapped]
        public DocumentSecurity Security { 

            get
            {
                if (_security == null)
                    _security = JsonUtils.To<DocumentSecurity>(_Security);

                return _security;
            }
            set
            {
                _security = value;
                _Security = JsonUtils.From(_file);
            }
        }

        /*Computed properties*/
        [NotMapped]
        public bool IsMaster => VersionNo == 1;
    }
}
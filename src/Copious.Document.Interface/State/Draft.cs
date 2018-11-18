using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Copious.Foundation;
using Copious.Utilities;

namespace Copious.Document.Interface.State {
    public class Draft : Entity<Guid> {
        public ActorKind ActorKind { get; set; }
        public Guid ActorId { get; set; }
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public string _File { get; set; }

        [NotMapped]
        File _file;

        [NotMapped]
        public File File {
            get {
                if (_file == null)
                    _file = JsonUtils.To<File> (_File);

                return _file;
            }
            set {
                _file = value;
                _File = JsonUtils.From (_file);
            }
        }
    }
}
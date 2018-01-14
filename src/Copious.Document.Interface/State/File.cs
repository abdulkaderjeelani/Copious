using Copious.Foundation;
using Copious.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Document.Interface.State
{
    public class File :  Entity<Guid>
    {
        public StorageProvider Provider { get; set; }
        public Dictionary<string,string> ProviderOptions { get;set; }
        public string Container { get; set; }
        public string Blob { get; set; }


        /// <summary>
        /// If this fileld is null then the target file has no extension
        /// </summary>
        public string DotlessExtension { get; set; }
        
    }
}
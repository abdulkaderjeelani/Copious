using Copious.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Document.Interface.State
{
    /// <summary>
    /// Holds the biz information of a doc if document kind is business
    /// </summary>
    public class BusinessDetail : DocumentDetail
    {
        public string ComponentName { get; set; }
        public Guid ComponentId { get; set; }
    }
}
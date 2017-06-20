using System;
using System.Security.Principal;

namespace Copious.Foundation
{
    /// <summary>
    /// Actor is on who performs a query / command. It can be a User / Systm.
    /// </summary>
    public class Actor
    {
        private string _name;

        public Guid Id { get; set; }
        public ActorKind Kind { get; set; }

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name) && Principal != null)
                    _name = Principal.Identity.Name;
                return _name;
            }
            set => _name = value;
        }

        public IPrincipal Principal { get; set; }

        public Guid SystemId { get; set; }
        public Guid SubSystemId { get; set; }
    }
}
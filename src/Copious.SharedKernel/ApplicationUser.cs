using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.SharedKernel
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string userName) : base(userName)
        {
        }

        public ApplicationUser(string userName, int systemId) : this(userName)
        {
            SystemId = systemId;
        }

        public int SystemId { get; set; }
    }
}
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Bl
{
    public class ApplicationRole :IdentityRole
    {
        public string RolePermissions { get; set; } //json
    }
}

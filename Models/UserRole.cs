using System;
using System.Collections.Generic;

namespace DroneWorks.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            WorksUser = new HashSet<WorksUser>();
        }

        public int UserRolePk { get; set; }
        public string RoleName { get; set; }
        public string RoleFunction { get; set; }

        public virtual ICollection<WorksUser> WorksUser { get; set; }
    }
}

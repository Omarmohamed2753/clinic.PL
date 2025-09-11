using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.DAL.Entities
{
    using clinic.DAL.Enum;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }

        public virtual List<LoginHistory> LoginHistories { get; set; } = new List<LoginHistory>();
    }

}

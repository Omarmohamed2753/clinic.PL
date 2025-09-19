using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.DAL.Entities
{
    public class LoginHistory
    {
        public int LoginHistoryID { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual User User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.DAL.Enum
{
    public enum AppointmentStatus
    {
        PendingPayment = 0,
        Scheduled = 1,
        Completed = 2,
        Cancelled = 3
    }
}

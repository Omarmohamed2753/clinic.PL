using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.BLL.ModelVM.Appointment
{
    public class GetAllAppointmentVM
    {
        public int Id { get; set; }
        public string? PatientName { get; set; }
        public string? PatientImg { get; set; } 
        public int? Age { get; set; }
        public string? DoctorName { get; set; }
        public string? Department { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
    }
}

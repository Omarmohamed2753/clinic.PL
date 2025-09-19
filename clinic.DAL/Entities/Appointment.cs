
using clinic.DAL.Enum;

namespace clinic.DAL.Entities
{
    public class Appointment
    {
        public Appointment(int iD, DateTime appointmentDate, AppointmentStatus status)
        {
            ID = iD;
            AppointmentDate = appointmentDate;
            Status = status;
        }

        public int ID { get; private set; } // Primary Key
        public DateTime AppointmentDate { get; private set; }
        public AppointmentStatus Status { get; set; }

        // Foreign Keys
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }

        // Navigation Properties
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        [ForeignKey("MedicalRecord")]
        public int? MedicalRecordID { get; set; }
        public virtual MedicalRecord MedicalRecord { get; set; }



        // Stripe Payment Fields
        public string? StripeSessionId { get; set; }
        public string? PaymentIntentId { get; set; }

    }
}

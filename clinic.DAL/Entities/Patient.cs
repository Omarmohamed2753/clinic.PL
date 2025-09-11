
namespace clinic.DAL.Entities
{
    public class Patient
    {
        public Patient() { }
        public Patient(int iD, string bloodType, string allergies, string medicalHistory, string emergencyContactName, string emergencyContactPhone)
        {
            Id = iD;
            BloodType = bloodType;
            Allergies = allergies;
            MedicalHistory = medicalHistory;
            EmergencyContactName = emergencyContactName;
            EmergencyContactPhone = emergencyContactPhone;
        }

            public int Id { get; private set; } // Primary Key
            public string? BloodType { get; private set; }
            public string? Allergies { get; private set; }
            public string? MedicalHistory { get; private set; }
            public string? EmergencyContactName { get; private set; }
            public string? EmergencyContactPhone { get; private set; }
            // Foreign Keys

            [ForeignKey("Person")]
            public int PersonID { get; set; }
            // Navigation Properties
            public virtual Person Person { get; set; }
            public virtual List<Appointment> Appointments { get; set; }
            

    }
}

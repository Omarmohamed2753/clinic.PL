
using Microsoft.AspNetCore.Http;

namespace clinic.DAL.Entities
{
    public class Patient
    {
        public Patient() { }
        public Patient(int iD, string bloodType, string allergies, string medicalHistory, string emergencyContactPhone,string? imgPath)
        {
            Id = iD;
            BloodType = bloodType;
            Allergies = allergies;
            MedicalHistory = medicalHistory;
            EmergencyContactPhone = emergencyContactPhone;
            this.imgPath = imgPath;
        }

            public int Id { get; private set; } // Primary Key
            public string BloodType { get; private set; }
            public string Allergies { get; private set; }
            public string MedicalHistory { get; private set; }
            public string? imgPath { get; private set; }
            public string EmergencyContactPhone { get; private set; }
            // Foreign Keys

            [ForeignKey("Person")]
            public int PersonID { get; set; }
            // Navigation Properties
            public virtual Person Person { get; set; }
            public virtual List<Appointment> Appointments { get; set; }
            [ForeignKey("User")]
            public string UserId { get; set; }
            public virtual User User { get; set; }
        public void UpdatePatient(string bloodType, string allergies,string emergencyContactPhone,string medicalHistory)
        {
            BloodType = bloodType;
            Allergies = allergies;
            EmergencyContactPhone=emergencyContactPhone;
            MedicalHistory=medicalHistory;
        }


    }
}

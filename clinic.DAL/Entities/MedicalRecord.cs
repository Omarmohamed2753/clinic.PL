
namespace clinic.DAL.Entities
{
    public class MedicalRecord
    {
        public MedicalRecord(int iD, string diagnosis, string prescription, string notes)
        {
            ID = iD;
            Diagnosis = diagnosis;
            Prescription = prescription;
            Notes = notes;
        }

        public int ID { get; private set; } // Primary Key
        public string Diagnosis { get; private set; }
        public string Prescription { get; private set; }
        public string Notes { get; private set; }

        [ForeignKey("Appointment")]
        public int AppointmentID { get; set; }

        public virtual Appointment Appointment { get; set; }
    }
}

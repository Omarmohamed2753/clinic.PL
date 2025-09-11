
namespace clinic.DAL.Entities
{
    public class Receptionist
    {
        public Receptionist(int receptionistID, DateTime hireDate, DateTime? endDate, string status)
        {
            ReceptionistID = receptionistID;
            HireDate = hireDate;
            EndDate = endDate;
            Status = status;
        }

        public int ReceptionistID { get; private set; } // Primary Key
        public DateTime HireDate { get; private set; }
        public DateTime? EndDate { get; private set; } // Nullable DateTime
        public string Status { get; private set; }

        // Foreign Key
        [ForeignKey("Person")]
        public int PersonID { get; set; }

        // Navigation Property
        public virtual Person Person { get; set; }
    }
}

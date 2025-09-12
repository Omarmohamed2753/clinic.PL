
namespace clinic.DAL.Entities
{
    public class Doctor  
    {
        //This is a parameterless constructor
        public Doctor() { }
        public Doctor(int iD, string licenseNumber, string specialization, int yearsOfExperience, DateTime hireDate, string status, decimal consultationFee, string imgPath)
        {
            Id = iD;
            LicenseNumber = licenseNumber;
            Specialization = specialization;
            YearsOfExperience = yearsOfExperience;
            HireDate = hireDate;
            Status = status;
            ConsultationFee = consultationFee;
            this.imgPath = imgPath;
        }
        public int Id { get; private set; }
        public string imgPath { get; private set; }
        public string LicenseNumber { get; private set; }
        public string Specialization { get; private set; }
        public int YearsOfExperience { get;  private set; }
        public DateTime HireDate { get; private set; }
        public string Status { get; private set; }
        public decimal ConsultationFee { get; private set; }
        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }
        public virtual List<Appointment> Appointments { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual User User { get; set; }
        public void UpdateDoctor(string licenseNumber, string specialization, int yearsOfExperience, string imgPath)
        {
            LicenseNumber = licenseNumber;
            Specialization = specialization;
            YearsOfExperience = yearsOfExperience;
            this.imgPath = imgPath;
        }



    }
}

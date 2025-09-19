
using System.ComponentModel.DataAnnotations;
using clinic.DAL.Enum;


namespace clinic.DAL.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NationalID { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? imgPath { get;  set; }
        public string? CreatedBy { get;  set; }
        public DateTime? CreatedOn { get;  set; }
        public string? ModifiedBy { get; set; } 
        public DateTime? ModifiedOn { get; set; }
        public bool Update(string fname,string lname, string nationalID,
            string phone,string address, int age, DateTime birthDate, string modifiedBy="Admin")
        {
            if (string.IsNullOrEmpty(modifiedBy))
                modifiedBy = "Admin";

            FirstName = fname;
            LastName = lname;
            NationalID = nationalID;
            Age = age;
            BirthDate = birthDate;
            Address = address;
            Phone = phone;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
            return true;
        }
      
    }
}

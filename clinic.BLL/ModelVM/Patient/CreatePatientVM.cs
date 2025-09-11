
namespace clinic.BLL.ModelVM.Patient
{
    public class CreatePatientVM
    {
        public string BloodType { get; set; }
        public string Allergies { get; set; }
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
      
        public int Age { get; set; }
        
        public string Gender { get; set; }
    
        public string Phone { get; set; }

        public IFormFile ImageFile { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using clinic.BLL.ModelVM.AccountVM;
using clinic.BLL.ModelVM.Person;
using clinic.BLL.ModelVM.User;

namespace clinic.BLL.ModelVM.Doctor
{
    public class CreateDoctorVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "License number is required")]
        [StringLength(30, ErrorMessage = "License number cannot be longer than 30 characters")]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(100, ErrorMessage = "Specialization cannot be longer than 100 characters")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "Years of experience is required")]
        [Range(0, 60, ErrorMessage = "Years of experience must be between 0 and 60")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Doctor image is required")]
        public IFormFile DoctorImg { get; set; }

        [Required(ErrorMessage = "Personal information is required")]
        public virtual PersonVM Person { get; set; }

        [Required(ErrorMessage = "User information is required")]
        public virtual UserVM User { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentID { get; set; }
    }
}

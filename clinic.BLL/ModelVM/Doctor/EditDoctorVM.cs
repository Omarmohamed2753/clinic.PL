using System.ComponentModel.DataAnnotations;
using clinic.BLL.ModelVM.User;

namespace clinic.BLL.ModelVM.Doctor
{
    public class EditDoctorVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(25, 80, ErrorMessage = "Age must be between 25 and 80")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Birth date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "License number is required")]
        [StringLength(20, ErrorMessage = "License number cannot exceed 20 characters")]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "Specialization is required")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "Years of experience is required")]
        [Range(0, 50, ErrorMessage = "Years of experience must be between 0 and 50")]
        public int YearsOfExperience { get; set; }

        public IFormFile? DoctorImg { get; set; }

        public string? ExistingImgPath { get; set; }

        [Required]
        public UserVM User { get; set; }
    }
}

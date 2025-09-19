
using clinic.BLL.ModelVM.User;
using System.ComponentModel.DataAnnotations;

namespace clinic.BLL.ModelVM.Patient
{
    public class EditPatientVM
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
        [Required(ErrorMessage ="patient Image is required")]
        public IFormFile? PatientImg { get; set; }
        [Required(ErrorMessage = "Blood type is required")]
        [RegularExpression("^(A|B|AB|O)[+-]$", ErrorMessage = "Invalid blood type format (e.g., A+, O-)")]
        public string BloodType { get; set; }

        [MaxLength(250, ErrorMessage = "Allergies must not exceed 250 characters")]
        public string Allergies { get; set; }

        public IFormFile? MedicalHistory { get; set; }

        [Required(ErrorMessage = "Emergency contact phone is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Phone number must be between 10 and 15 digits")]
        public string EmergencyContactPhone { get; set; }
        public string? ExistingImgPath { get; set; }

        [Required]
        public UserVM User { get; set; }
    }
}

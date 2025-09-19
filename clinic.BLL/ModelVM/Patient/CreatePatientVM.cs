using clinic.BLL.ModelVM.Person;
using clinic.BLL.ModelVM.User;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace clinic.BLL.ModelVM.Patient
{
    public class CreatePatientVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient image is required")]
        public IFormFile PatientImg { get; set; }

        [Required(ErrorMessage = "Personal information is required")]
        public virtual PersonVM Person { get; set; }

        [Required(ErrorMessage = "Blood type is required")]
        [RegularExpression("^(A|B|AB|O)[+-]$", ErrorMessage = "Invalid blood type format (e.g., A+, O-)")]
        public string BloodType { get; set; }

        [MaxLength(250, ErrorMessage = "Allergies must not exceed 250 characters")]
        public string Allergies { get; set; }

        [Required( ErrorMessage = "Medical history File is required")]
        public IFormFile MedicalHistory { get; set; }

        [Required(ErrorMessage = "Emergency contact phone is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Phone number must be between 10 and 15 digits")]
        public string EmergencyContactPhone { get; set; }

        [Required(ErrorMessage = "User information is required")]
        public virtual UserVM User { get; set; }
    }
}

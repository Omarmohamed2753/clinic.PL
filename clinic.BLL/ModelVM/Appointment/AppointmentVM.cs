using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace clinic.BLL.ModelVM.Appointment
{
    public class AppointmentVM
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        // Appointment Details
        [Required]
        [Display(Name = "Appointment Date")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Please select a department.")]
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Please select a doctor.")]
        [Display(Name = "Doctor")]
        public int DoctorID { get; set; }

        public string Message { get; set; }

        // Properties to hold the dropdown list data
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Doctors { get; set; }

        public AppointmentVM()
        {
            // Initialize lists to prevent null reference errors
            Departments = new List<SelectListItem>();
            Doctors = new List<SelectListItem>();
        }
    }
}

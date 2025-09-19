using clinic.DAL.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace clinic.BLL.ModelVM.Appointment
{
    public class CreateAppointmentVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient is required")]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Doctor is required")]
        [Display(Name = "Doctor")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Appointment Date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public AppointmentStatus Status { get; set; }


        // Dropdowns
        public List<SelectListItem> Patients { get; set; } = new();
        public List<SelectListItem> Doctors { get; set; } = new();
        public List<SelectListItem> Departments { get; set; } = new();
    }
}

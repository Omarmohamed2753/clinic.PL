
namespace clinic.BLL.ModelVM.Appointment
{
    public class AppointmentDashboardVM
    {
        public int Id { get; set; }
        public string? PatientName { get; set; }
        public string? PatientLocation { get; set; }
        public string? DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}

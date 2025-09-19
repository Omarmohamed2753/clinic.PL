using clinic.BLL.ModelVM.Appointment;
using clinic.BLL.ModelVM.PatientDashBoard;
namespace clinic.BLL.ModelVM.AdminDashBoard
{
    public class AdminDashBoardVM
    {
            // الـ Counters
            public int DoctorCount { get; set; }
            public int PatientCount { get; set; }
            public int AppointmentCount { get; set; }

            // Upcoming Appointments
            public List<AppointmentDashboardVM> UpcomingAppointments { get; set; }

            // Doctors
            public List<GetAllDoctorVM> Doctors { get; set; }

            // New Patients
            public List<PatientDashBoardVM> NewPatients { get; set; }

            // Hospital Management (Progress Bars)
            public int OPDPercent { get; set; }
            public int NewPatientPercent { get; set; }
            public int LabTestPercent { get; set; }
            public int TreatmentPercent { get; set; }
            public int DischargePercent { get; set; }
        }
    }

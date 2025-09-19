using clinic.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace clinic.DAL.Repo.abstraction
{
    public interface IAppointmentRepo
    {
        
        Task<Patient> GetPatientByUserIdAsync(string userId);

        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int departmentId);

        Task AddAsync(Appointment appointment);
        Task<List<Appointment>> GetAllAsync();

        Task<int> SaveChangesAsync();

        Task<Doctor> GetDoctorByIdAsync(int doctorId);
        Task<Appointment> GetAppointmentBySessionIdAsync(string sessionId);

        void UpdateAppointment(Appointment appointment);
    }
}

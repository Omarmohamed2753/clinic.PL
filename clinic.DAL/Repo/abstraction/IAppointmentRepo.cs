using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.DAL.Repo.abstraction
{
    public interface IAppointmentRepo
    {
        Task<Person> GetPersonByEmailAsync(string email);
        Task<Patient> GetPatientByPersonIdAsync(int personId);

   
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int departmentId);

       
        Task AddAsync(Appointment appointment);
        Task<List<Appointment>> GetAllAsync();

        Task<int> SaveChangesAsync();
    }
}

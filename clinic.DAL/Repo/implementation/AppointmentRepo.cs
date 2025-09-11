using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic.DAL.Repo.implementation
{
    public class AppointmentRepo :IAppointmentRepo
    {
        private readonly ClinicDbcontext _context;

        public AppointmentRepo(ClinicDbcontext context)
        {
            _context = context;
        }

        public async Task<Person> GetPersonByEmailAsync(string email)
        {
            return await _context.Person.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<Patient> GetPatientByPersonIdAsync(int personId)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.PersonID == personId);
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int departmentId)
        {
            return await _context.Doctors
                .Include(d => d.Person)
                .Where(d => d.DepartmentID == departmentId)
                .ToListAsync();
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                                 .Include(a => a.Doctor)   
                                 .Include(a => a.Patient)
                                 .ToListAsync();
        }

    }
}

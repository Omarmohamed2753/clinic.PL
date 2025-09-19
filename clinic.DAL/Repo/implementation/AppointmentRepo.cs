using clinic.DAL.DataBase;
using clinic.DAL.Entities;
using clinic.DAL.Repo.abstraction;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clinic.DAL.Repo.implementation
{
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly ClinicDbcontext _context;

        public AppointmentRepo(ClinicDbcontext context)
        {
            _context = context;
        }

        // ✅ بديل GetPersonByEmailAsync
        public async Task<Patient> GetPatientByUserIdAsync(string userId)
        {
            return await _context.Patients
                                 .Include(p => p.Person)
                                 .FirstOrDefaultAsync(p => p.UserId == userId);
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
                                 .Include(a => a.Doctor).ThenInclude(d => d.Person)
                                 .Include(a => a.Patient).ThenInclude(p => p.Person)
                                 .ToListAsync();
        }

        public async Task<Doctor> GetDoctorByIdAsync(int doctorId)
        {
            return await _context.Doctors
                                 .Include(d => d.Person)
                                 .FirstOrDefaultAsync(d => d.Id == doctorId);
        }

        public async Task<Appointment> GetAppointmentBySessionIdAsync(string sessionId)
        {
            return await _context.Appointments
                                 .Include(a => a.Patient).ThenInclude(p => p.Person)
                                 .Include(a => a.Doctor).ThenInclude(d => d.Person)
                                 .FirstOrDefaultAsync(a => a.StripeSessionId == sessionId);
        }

        public void UpdateAppointment(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
        }
        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.Doctors
                                 .Include(d => d.Person)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _context.Patients
                                 .Include(p => p.Person)
                                 .ToListAsync();
        }

    }
}

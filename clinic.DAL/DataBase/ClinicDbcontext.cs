
using clinic.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace clinic.DAL.DataBase
{
    public class ClinicDbcontext : IdentityDbContext<User>
    {
        public ClinicDbcontext(DbContextOptions<ClinicDbcontext> options) : base(options)
        {
        }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Receptionist> Receptionists { get; set; }
        public virtual DbSet<User> Users { get; set; }
        }
}

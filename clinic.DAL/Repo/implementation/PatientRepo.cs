using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace clinic.DAL.Repo.implementation
{
    public class PatientRepo : IPatientRepo
    {
        private readonly ClinicDbcontext _db;

        public PatientRepo(ClinicDbcontext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(Patient patient)
        {
            try
            {
                var result = await _db.Patients.AddAsync(patient);
                await _db.SaveChangesAsync();
                return result.Entity.Id > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Patient patient, string deletedBy = "system")
        {
            if (patient == null) return false;

            _db.Patients.Remove(patient);

            if (patient.Person.ToggleStatus(deletedBy))
            {
                return await _db.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> EditAsync(Patient patient, string updatedBy = "system")
        {
            var found = await _db.Patients.FirstOrDefaultAsync(s => s.Id == patient.Id);
            if (found == null) throw new ArgumentNullException(nameof(patient));

            if (found.Person.Update(
                    patient.Person.FirstName,
                    patient.Person.LastName,
                    patient.Person.NationalID,
                    patient.Person.Email,
                    patient.Person.Phone,
                    patient.Person.Address,
                    patient.Person.Age,
                    patient.Person.BirthDate,
                    patient.Person.ModifiedBy))
            {
                return await _db.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<Patient?> GetPatientByIdAsync(int patientId)
        {
            try
            {
                return await _db.Patients.FirstOrDefaultAsync(a => a.Id == patientId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Patient>> GetPatientsAsync(Expression<Func<Patient, bool>>? filter = null)
        {
            try
            {
                if (filter != null)
                {
                    return await _db.Patients.Where(filter).ToListAsync();
                }

                return await _db.Patients.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            var found = await _db.Patients.ToListAsync();
            if (found == null) throw new ArgumentNullException();
            return found;
        }

        public async Task<List<Patient>> GetAllWithPersonAndRecordAsync()
        {
            return await _db.Patients
                .Include(p => p.Person)
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.MedicalRecord)
                .ToListAsync();
        }
    }
}

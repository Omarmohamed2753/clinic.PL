
using System.Linq.Expressions;
using clinic.DAL.Entities;

namespace clinic.DAL.Repo.abstraction
{
    public interface IPatientRepo
    {
        Task<bool> CreateAsync(Patient patient);
        Task<bool> DeletedAsync(int patientId);
        Task<bool> EditAsync(Patient patient, string updatedBy = "system");
        Task<Patient?> GetPatientByIdAsync(int patientId);
        Task<List<Patient>> GetPatientsAsync(Expression<Func<Patient, bool>>? filter = null);
        Task<List<Patient>> GetAllPatientsAsync();
        Task<List<Patient>> GetAllWithPersonAndRecordAsync();
    }

}

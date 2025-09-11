
using AutoMapper;
using clinic.BLL.ModelVM.Patient;
using clinic.DAL.Repo.abstraction;

namespace clinic.BLL.Service.abstraction
{
    public interface IPatientServicecs
    {
        Task<(bool success, string? errorMessage)> CreateAsync(CreatePatientVM patientVM);
        Task<(bool success, string? errorMessage)> DeleteAsync(int id);
        Task<(bool fail, string? errorMessage, List<IndexPatientVM>? patients)> GetAllAsync();
        Task<(bool success, string? errorMessage)> EditAsync(EditPatientVM patientVM);
        Task<List<Patient>> GetAllWithPersonAndRecordAsync();
    }

}

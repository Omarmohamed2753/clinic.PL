public interface IDoctorService
{
    Task<(bool success, string? errorMessage)> CreateAsync(CreateDoctorVM Doctor);
    Task<(bool isError, string? errorMessage, List<GetAllDoctorVM>? doctors)> GetAllAsync();
    Task<(bool isError, string? errorMessage, DoctorVM? doctor)> GetDoctorByIdAsync(int id);
    Task<(bool isError, string? errorMessage, EditDoctorVM? doctor)> GetDoctorForEditAsync(int id);
    Task<(bool success, string? errorMessage)> EditAsync(EditDoctorVM DoctorVM);
    Task<(bool success, string? errorMessage)> DeleteAsync(int id);
}

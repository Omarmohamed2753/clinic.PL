public interface IDoctorRepo
{
    Task<bool> CreateAsync(Doctor doctor);
    Task<bool> DeletedAsync(int DoctorId);
    Task<bool> EditAsync(Doctor doctor);
    Task<Doctor?> GetDoctorByIdAsync(int UserId);
    Task<List<Doctor>> GetDoctorsAsync(Expression<Func<Doctor, bool>>? filter = null);
}

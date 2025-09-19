
namespace clinic.DAL.Repo.abstraction
{
    public interface IDepartmentRepo
    {
        Task<bool> CreateAsync(Department department);
        Task<bool> DeletedAsync(int departmentId);
        Task<bool> EditAsync(int DepartmentId, Department department);
        Task<Department?> GetdepartmentByIdAsync(int UserId);
        Task<List<Department>> GetdepartmentsAsync(Expression<Func<Department, bool>>? filter = null);
    }

}

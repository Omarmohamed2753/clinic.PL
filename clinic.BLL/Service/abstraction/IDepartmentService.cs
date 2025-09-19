
using clinic.BLL.ModelVM.Department;
using clinic.BLL.ModelVM.Doctor;

namespace clinic.BLL.Service.abstraction
{
    public interface IDepartmentService
    {
        Task<(bool, string?)> CreateAsync(CreateDepartmentVM department);
        Task<(bool, string?, List<GetAllDepartmentVM>?)> GetAllAsync();
        Task<(bool, string?, DepartmentVM?)> GetDepartmentByIdAsync(int id);
        Task<(bool success, string? errorMessage)> EditAsync(EditDepartmentVM departmentVM);
        Task<(bool success, string? errorMessage)> DeleteAsync(int id);
    }

}

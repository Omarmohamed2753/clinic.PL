using AutoMapper;
using clinic.BLL.Helper;
using clinic.BLL.ModelVM.Department;
using clinic.DAL.Entities;
using clinic.DAL.Repo.abstraction;

namespace clinic.BLL.Service.implementation
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo departmentRepo;
        private readonly IMapper mapper;

        public DepartmentService(IMapper mapper, IDepartmentRepo departmentRepo)
        {
            this.departmentRepo = departmentRepo;
            this.mapper = mapper;
        }

        public async Task<(bool, string?)> CreateAsync(CreateDepartmentVM department)
        {
            try
            {
                var imagePath = Upload.UploadFile("files", department.DepartmentImg);
                var mapped = new Department(department.DepartmentName, department.Description, imagePath);
                var result = await departmentRepo.CreateAsync(mapped);

                if (result)
                    return (true, null);

                return (true, "Failed to create department");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string?, List<GetAllDepartmentVM>?)> GetAllAsync()
        {
            try
            {
                var result = await departmentRepo.GetdepartmentsAsync();
                var depts = mapper.Map<List<GetAllDepartmentVM>>(result);
                return (false, null, depts);
            }
            catch (Exception ex)
            {
                return (true, ex.Message, null);
            }
        }

        public async Task<(bool, string?, DepartmentVM?)> GetDepartmentByIdAsync(int id)
        {
            try
            {
                var result = await departmentRepo.GetdepartmentByIdAsync(id);
                var mapp = mapper.Map<DepartmentVM>(result);
                return (false, null, mapp);
            }
            catch (Exception ex)
            {
                return (true, ex.Message, null);
            }
        }

        public async Task<(bool success, string? errorMessage)> EditAsync(EditDepartmentVM departmentVM)
        {
            var existingDepartment = await departmentRepo.GetdepartmentByIdAsync(departmentVM.Id);
            if (existingDepartment == null)
                return (false, "Department is not found");

            string newImageUrl = existingDepartment.imgPath;
            if (departmentVM.DepartmentImg != null)
            {
                if (!string.IsNullOrEmpty(existingDepartment.imgPath))
                {
                    var oldFilePath = Path.Combine("wwwroot", "Files", existingDepartment.imgPath);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                newImageUrl = Upload.UploadFile("Files", departmentVM.DepartmentImg);
            }

            mapper.Map(departmentVM, existingDepartment);

            bool updated = await departmentRepo.EditAsync(existingDepartment.Id, existingDepartment);
            return updated ? (true, null) : (false, "Failed to update department");
        }

        public async Task<(bool success, string? errorMessage)> DeleteAsync(int id)
        {
            var department = await departmentRepo.GetdepartmentByIdAsync(id);
            if (department == null)
                return (false, "Department not found");

            bool deleted = await departmentRepo.DeletedAsync(id);
            return deleted ? (true, null) : (false, "Failed to delete department");
        }
    }
}


namespace clinic.BLL.ModelVM.Department
{
    public class EditDepartmentVM:CreateDepartmentVM
    {
        public int Id { get; set; }
        public IFormFile DepartmentImg { get; set; }
    }
}

namespace clinic.BLL.ModelVM.Department
{
    public class EditDepartmentVM
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }   
        public string Description { get; set; }
        public IFormFile? DepartmentImg { get; set; }
    }
}

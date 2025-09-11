
namespace clinic.BLL.ModelVM.Doctor
{
    public class EditDoctorVM : CreateDoctorVM
    {
        public int Id { get; set; }
        public IFormFile DoctorImg { get; set; }
        
    }
}

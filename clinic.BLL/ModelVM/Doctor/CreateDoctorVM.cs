
using clinic.BLL.ModelVM.AccountVM;
using clinic.BLL.ModelVM.User;

namespace clinic.BLL.ModelVM.Doctor
{
    public class CreateDoctorVM
    {
        public int Id { get;  set; }
        public string LicenseNumber { get;  set; }
        public string Specialization { get;  set; }
        public int YearsOfExperience { get;  set; }
        public IFormFile DoctorImg { get; set; }
        public virtual Person Person { get; set; }
        public virtual UserVM User { get; set; }
        public int DepartmentID { get; set; }


    }
}

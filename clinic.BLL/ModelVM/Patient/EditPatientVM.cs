
namespace clinic.BLL.ModelVM.Patient
{
    public class EditPatientVM :CreatePatientVM
    {
        public int Id { get; set; }
        //public int PersonId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}

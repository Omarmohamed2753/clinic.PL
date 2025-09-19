
namespace clinic.DAL.Entities
{
    public class Department
    {
        public Department() { }
        public Department(string departmentName, string description, string imgPaht)
        {
            DepartmentName = departmentName;
            Description = description;
            this.imgPath = imgPaht;
        }

        public int Id { get; private set; }
        public string imgPath { get; private set; }
        public string DepartmentName { get; private set; }
        public string Description { get; private set; }
        public bool isDeleted { get; private set; } = false;
        public DateTime? DeletedOn { get; private set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }


        public virtual List<Doctor> Doctors { get; set; }
        public bool Update(string Description, string DepartmentName, string modifiedBy)
        {
            if (string.IsNullOrEmpty(modifiedBy))
                return false;
            this.DepartmentName = DepartmentName;
            this.Description = Description;
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.Now;
            return true;
        }
    }
}

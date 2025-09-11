using clinic.DAL.DataBase;
using Microsoft.AspNetCore.Identity;

namespace clinic.BLL.Service.implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepo DoctorRepo;
        private readonly UserManager<User> _userManager;
        private readonly ClinicDbcontext _dbContext; // لازم يبقى عندك DbContext
        private readonly IMapper mapper;

        public DoctorService(IMapper mapper, IDoctorRepo doctorRepo, UserManager<User> userManager, ClinicDbcontext dbContext)
        {
            this.DoctorRepo = doctorRepo;
            this.mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<(bool success, string? errorMessage)> CreateAsync(CreateDoctorVM Doctor)
        {
            try
            {
                // 1) رفع صورة الدكتور لو موجودة
                string imagePath = "default-doctor.png";
                if (Doctor.DoctorImg != null)
                {
                    imagePath = Upload.UploadFile("files", Doctor.DoctorImg);
                }

                // 2) إنشاء Person وحفظه أولًا
                var person = new Person
                {
                    FirstName = Doctor.Person.FirstName,
                    LastName = Doctor.Person.LastName,
                    NationalID = Doctor.Person.NationalID,
                    Email = Doctor.Person.Email,
                    Phone = Doctor.Person.Phone,
                    Address = Doctor.Person.Address,
                    Age = Doctor.Person.Age,
                    BirthDate = Doctor.Person.BirthDate,
                    imgPath = imagePath
                };

                await _dbContext.Person.AddAsync(person);
                await _dbContext.SaveChangesAsync(); // دلوقتي عنده Id

                // 3) إنشاء User وربطه بالـ PersonId
                var appUser = new User
                {
                    UserName = Doctor.User.UserName,
                    Email = Doctor.User.Email,
                    PersonID = person.Id
                };

                var userResult = await _userManager.CreateAsync(appUser, Doctor.User.Password);
                if (!userResult.Succeeded)
                {
                    return (false, string.Join(", ", userResult.Errors.Select(e => e.Description)));
                }
                
                // 4) إنشاء Doctor وربطه بالـ User و Person
                var doctorEntity = new Doctor(
                iD: 0,
                licenseNumber: Doctor.LicenseNumber,
                specialization: Doctor.Specialization,
                yearsOfExperience: Doctor.YearsOfExperience,
                hireDate: DateTime.Now,
                status: "Active",
                consultationFee: 100,
                imgPath:imagePath
                );
                doctorEntity.Person = person;
                doctorEntity.User = appUser;
                doctorEntity.UserID = appUser.Id;
                doctorEntity.PersonID = person.Id;
                doctorEntity.DepartmentID = Doctor.DepartmentID;

                var result = await DoctorRepo.CreateAsync(doctorEntity);

                if (result)
                    return (true, null);

                return (false, "Failed to create doctor");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool isError, string? errorMessage, List<GetAllDoctorVM>? doctors)> GetAllAsync()
        {
            try
            {
                var result = await DoctorRepo.GetDoctorsAsync();

                var doctors = result.Select(d => new GetAllDoctorVM
                {
                    Id = d.Id,
                    Name = d.Person.FirstName + " " + d.Person.LastName,
                    Specialization = d.Specialization,
                    YearsOfExperience = d.YearsOfExperience,
                    DoctorImgUrl = string.IsNullOrEmpty(d.imgPath) ? "default-doctor.png" : d.imgPath
                }).ToList();

                return (false, null, doctors);
            }
            catch (Exception ex)
            {
                return (true, ex.Message, null);
            }
        }


        public async Task<(bool isError, string? errorMessage, DoctorVM? doctor)> GetDoctorByIdAsync(int id)
        {
            try
            {
                var result = await DoctorRepo.GetDoctorByIdAsync(id);
                if (result == null)
                    return (true, "Doctor not found", null);

                var mapped = mapper.Map<DoctorVM>(result);
                return (false, null, mapped);
            }
            catch (Exception ex)
            {
                return (true, ex.Message, null);
            }
        }

        public async Task<(bool success, string? errorMessage)> EditAsync(EditDoctorVM DoctorVM)
        {
            var existingDoctor = await DoctorRepo.GetDoctorByIdAsync(DoctorVM.Id);
            if (existingDoctor == null)
                return (false, "Doctor not found");

            string newImageUrl = existingDoctor.Person.imgPath;
            if (DoctorVM.DoctorImg != null)
            {
                if (!string.IsNullOrEmpty(existingDoctor.Person.imgPath))
                {
                    var oldFilePath = Path.Combine("wwwroot", "Files", existingDoctor.Person.imgPath);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                newImageUrl = Upload.UploadFile("Files", DoctorVM.DoctorImg);
            }

            mapper.Map(DoctorVM, existingDoctor);
            existingDoctor.Person.imgPath = newImageUrl;

            bool updated = await DoctorRepo.EditAsync(existingDoctor);
            return updated ? (true, null) : (false, "Failed to update doctor");
        }

        public async Task<(bool success, string? errorMessage)> DeleteAsync(int id)
        {
            var doctor = await DoctorRepo.GetDoctorByIdAsync(id);
            if (doctor == null)
                return (false, "Doctor not found");

            bool deleted = await DoctorRepo.DeletedAsync(id);
            return deleted ? (true, null) : (false, "Failed to delete doctor");
        }
    }
}

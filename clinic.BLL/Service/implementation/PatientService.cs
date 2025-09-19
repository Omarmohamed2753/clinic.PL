using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using clinic.DAL.DataBase;
using clinic.BLL.ModelVM.User;

namespace clinic.BLL.Service.implementation
{
    public class PatientService : IPatientServicecs
    {
        private readonly IPatientRepo _patientRepo;
        private readonly ClinicDbcontext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PatientService(IPatientRepo patientRepo, IMapper mapper, ClinicDbcontext dbContext, UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
        {
            _patientRepo = patientRepo;
            _mapper = mapper;
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<(bool success, string? errorMessage)> CreateAsync(CreatePatientVM patient)
        {
            try
            {
                string imagePath = "default-patient.png";
                if (patient.PatientImg != null)
                {
                    imagePath = Upload.UploadFile("files", patient.PatientImg);
                }

                string filePath = "default-patient.png";
                if (patient.MedicalHistory != null)
                {
                    filePath = Upload.UploadFile("MedicalRecordFiles", patient.MedicalHistory);
                }

                var person = new Person
                {
                    FirstName = patient.Person.FirstName,
                    LastName = patient.Person.LastName,
                    NationalID = patient.Person.NationalID,
                    Phone = patient.Person.Phone,
                    Address = patient.Person.Address,
                    Age = patient.Person.Age,
                    BirthDate = patient.Person.BirthDate,
                    imgPath = imagePath
                };
                await _dbContext.Person.AddAsync(person);
                await _dbContext.SaveChangesAsync();


                var existingUserByName = await _userManager.FindByNameAsync(patient.User.UserName.Trim());
                var existingUserByEmail = await _userManager.FindByEmailAsync(patient.User.Email.Trim());

                if (existingUserByName != null || existingUserByEmail != null)
                {
                    return (false, "Username or Email already exists");
                }

                var appUser = new User
                {
                    UserName = patient.User.UserName.Trim(),
                    Email = patient.User.Email.Trim(),
                    PersonID = person.Id
                };

                var userResult = await _userManager.CreateAsync(appUser, patient.User.Password);
                if (!userResult.Succeeded)
                {
                    return (false, string.Join(", ", userResult.Errors.Select(e => e.Description)));
                }

                if (!await _roleManager.RoleExistsAsync("Patient"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Patient"));
                }

                var roleResult = await _userManager.AddToRoleAsync(appUser, "Patient");
                if (!roleResult.Succeeded)
                {
                    return (false, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }

                var patientEntity = new Patient(
                    iD: patient.Id,
                    bloodType: patient.BloodType,
                    allergies: patient.Allergies,
                    medicalHistory: filePath,
                    emergencyContactPhone: patient.EmergencyContactPhone,
                    imgPath: imagePath
                );

                patientEntity.Person = person;
                patientEntity.User = appUser;
                patientEntity.UserId = appUser.Id;
                patientEntity.PersonID = person.Id;

                var result = await _patientRepo.CreateAsync(patientEntity);

                if (result)
                    return (true, null);

                return (false, "Failed to create patient");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }



        public async Task<(bool success, string? errorMessage)> DeleteAsync(int id)
        {
            var patient = await _patientRepo.GetPatientByIdAsync(id);
            if (patient == null)
                return (false, "Patient not found");

            bool deleted = await _patientRepo.DeletedAsync(patient.Id);
            return deleted ? (true, null) : (false, "Failed to delete patient");
        }

        public async Task<(bool fail, string? errorMessage, List<IndexPatientVM>? patients)> GetAllAsync()
        {
            var temp = await _patientRepo.GetAllPatientsAsync();
            if (temp != null && temp.Any())
            {
                var mappedPatients = _mapper.Map<List<IndexPatientVM>>(temp);
                return (false, null, mappedPatients);
            }
            return (true, "No patients found", null);
        }

        public async Task<(bool success, string? errorMessage)> EditAsync(EditPatientVM patientVM)
        {
            var existingPatient = await _patientRepo.GetPatientByIdAsync(patientVM.Id);
            if (existingPatient == null)
                return (false, "Doctor not found");

            string newImageUrl = existingPatient.Person.imgPath;
            if (patientVM.PatientImg != null)
            {
                if (!string.IsNullOrEmpty(existingPatient.Person.imgPath))
                {
                    var oldFilePath = Path.Combine("wwwroot", "Files", existingPatient.Person.imgPath);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                newImageUrl = Upload.UploadFile("Files", patientVM.PatientImg);
            }

            existingPatient.Person.FirstName = patientVM.FirstName;
            existingPatient.Person.LastName = patientVM.LastName;
            existingPatient.Person.Phone = patientVM.Phone;
            existingPatient.Person.Address = patientVM.Address;
            existingPatient.Person.Age = patientVM.Age;
            existingPatient.Person.BirthDate = patientVM.BirthDate;
            existingPatient.Person.imgPath = newImageUrl;

            existingPatient.User.UserName = patientVM.User.UserName;
            existingPatient.User.Email = patientVM.User.Email;

            existingPatient.UpdatePatient(
                patientVM.BloodType,
                patientVM.Allergies,
                patientVM.EmergencyContactPhone,
                newImageUrl
            );

            bool updated = await _patientRepo.EditAsync(existingPatient);
            return updated ? (true, null) : (false, "Failed to update patient");
        }

        public async Task<List<Patient>> GetAllWithPersonAndRecordAsync()
        {
            return await _patientRepo.GetAllWithPersonAndRecordAsync();
        }
        public async Task<(bool isError, string? errorMessage, EditPatientVM? patientVM)> GetPatientForEditAsync(int id)
        {
            var patient = await _dbContext.Patients
                .Include(d => d.Person)
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (patient == null)
                return (true, "Doctor not found", null);

            var vm = new EditPatientVM
            {
                Id = patient.Id,
                FirstName = patient.Person.FirstName,
                LastName = patient.Person.LastName,
                Phone = patient.Person.Phone,
                Address = patient.Person.Address,
                Age = patient.Person.Age,
                BirthDate = patient.Person.BirthDate,
                BloodType = patient.BloodType,
                Allergies = patient.Allergies,
                EmergencyContactPhone = patient.EmergencyContactPhone,
                ExistingImgPath = patient.imgPath,
                User = new UserVM
                {
                    UserName = patient.User.UserName,
                    Email = patient.User.Email
                }
            };

            return (false, null, vm);
        }

    }
}

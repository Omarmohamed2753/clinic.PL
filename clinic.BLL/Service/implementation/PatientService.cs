using AutoMapper;
using clinic.BLL.Helper;
using clinic.BLL.ModelVM.Patient;
using clinic.DAL.Entities;
using clinic.DAL.Repo.abstraction;

namespace clinic.BLL.Service.implementation
{
    public class PatientService : IPatientServicecs
    {
        private readonly IPatientRepo _patientRepo;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepo patientRepo, IMapper mapper)
        {
            _patientRepo = patientRepo;
            _mapper = mapper;
        }

        public async Task<(bool success, string? errorMessage)> CreateAsync(CreatePatientVM patientVM)
        {
            var mappedPatient = _mapper.Map<Patient>(patientVM);
            var created = await _patientRepo.CreateAsync(mappedPatient);

            return created ? (true, null) : (false, "Failed to create patient");
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
                return (false, "Patient not found");

            string newImageUrl = existingPatient.Person.imgPath;
            if (patientVM.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(existingPatient.Person.imgPath))
                {
                    var oldFilePath = Path.Combine("wwwroot", "Files", existingPatient.Person.imgPath);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                newImageUrl = Upload.UploadFile("Files", patientVM.ImageFile);
            }

            _mapper.Map(patientVM, existingPatient);
            existingPatient.Person.imgPath = newImageUrl;

            bool updated = await _patientRepo.EditAsync(existingPatient);
            return updated ? (true, null) : (false, "Failed to update patient");
        }

        public async Task<List<Patient>> GetAllWithPersonAndRecordAsync()
        {
            return await _patientRepo.GetAllWithPersonAndRecordAsync();
        }
    }
}

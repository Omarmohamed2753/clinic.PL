using clinic.BLL.ModelVM.AccountVM;
using clinic.BLL.ModelVM.Appointment;
using clinic.BLL.ModelVM.User;
using clinic.BLL.ModelVM.Person;
using clinic.BLL.ModelVM.Doctor;

namespace clinic.BLL.mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            // Department
            CreateMap<Department, DepartmentVM>().ReverseMap();
            CreateMap<Department, GetAllDepartmentVM>().ReverseMap();
            CreateMap<Department, CreateDepartmentVM>().ReverseMap();
            CreateMap<EditDepartmentVM, Department>().ReverseMap();

            // Doctor
            CreateMap<Doctor, DoctorVM>().ReverseMap();
            CreateMap<Doctor, GetAllDoctorVM>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
                .ReverseMap();

            CreateMap<CreateDoctorVM, Doctor>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person))
                .ReverseMap();


            // Patient
            CreateMap<Patient, IndexPatientVM>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Person.Age.ToString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Person.Address))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Person.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => src.BloodType));

            CreateMap<Patient, DeletePatientVM>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName));

            CreateMap<CreatePatientVM, Patient>().ReverseMap();
            CreateMap<EditPatientVM, Patient>().ReverseMap();

            // User
            CreateMap<UserVM, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();

            // Person 🟢
            CreateMap<PersonVM, Person>().ForMember(dest => dest.Id, opt => opt.Ignore());


            // Account
            CreateMap<SignUpVM, Person>();
            CreateMap<SignUpVM, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            // Appointment
            CreateMap<Appointment, AppointmentVM>().ReverseMap();
        }
    }
}

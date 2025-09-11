
using clinic.BLL.ModelVM.AccountVM;
using clinic.BLL.ModelVM.Appointment;
using clinic.BLL.ModelVM.User;

namespace clinic.BLL.mapper
{
    public class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<Department, DepartmentVM>().ReverseMap();
            CreateMap<Department, GetAllDepartmentVM>().ReverseMap();
            CreateMap<Department, CreateDepartmentVM>().ReverseMap();

            CreateMap<Doctor, DoctorVM>().ReverseMap();
            CreateMap<Doctor, GetAllDoctorVM>().ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
                .ReverseMap();

            CreateMap<CreateDoctorVM, Doctor>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person))
            .ReverseMap();


            CreateMap<Patient, IndexPatientVM>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName))
            .ForMember(dest => dest.Phone,
                opt => opt.MapFrom(src => src.Person.Phone));

            CreateMap<UserVM, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)).ReverseMap();


            CreateMap<Patient, DeletePatientVM>()
           .ForMember(dest => dest.FullName,
               opt => opt.MapFrom(src => src.Person.FirstName + " " + src.Person.LastName));


            CreateMap<CreatePatientVM, Patient>().ReverseMap();
            CreateMap<EditPatientVM, Patient>().ReverseMap();

            CreateMap<SignUpVM, Person>();
            CreateMap<Appointment, AppointmentVM>().ReverseMap();
            CreateMap<SignUpVM, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        }
        
    }
}

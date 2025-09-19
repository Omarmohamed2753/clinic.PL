using clinic.BLL.ModelVM.Appointment;
using clinic.BLL.ModelVM.Common;
namespace clinic.BLL.Service.abstraction
{
    public interface IAppointmentService
    {
        Task<AppointmentVM> PrepareAppointmentViewModel();
        Task<Stripe.Checkout.Session> CreateCheckoutSessionAsync(AppointmentVM viewModel, string successUrl, string cancelUrl, string userId);
        Task<Appointment> FulfillAppointmentOrderAsync(string sessionId);
        Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int departmentId);
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task AddAppointmentFromAdminAsync(CreateAppointmentVM vm);
        Task<IEnumerable<DropdownVM>> GetAllPatientsForDropdownAsync();
        Task<IEnumerable<DropdownVM>> GetAllDoctorsForDropdownAsync();
        Task<IEnumerable<DropdownVM>> GetAllDepartmentsForDropdownAsync();
    }
}

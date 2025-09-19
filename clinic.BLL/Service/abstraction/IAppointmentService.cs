using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clinic.BLL.ModelVM.Appointment;
namespace clinic.BLL.Service.abstraction
{
    public interface IAppointmentService
    {
        Task<AppointmentVM> PrepareAppointmentViewModel();

        Task<Stripe.Checkout.Session> CreateCheckoutSessionAsync(AppointmentVM viewModel, string successUrl, string cancelUrl, string userId);
        Task<Appointment> FulfillAppointmentOrderAsync(string sessionId);


        Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int departmentId);
        Task<List<Appointment>> GetAllAppointmentsAsync();






    }
}

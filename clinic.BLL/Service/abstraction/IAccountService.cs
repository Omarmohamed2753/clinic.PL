
using clinic.BLL.ModelVM.AccountVM;


namespace clinic.BLL.Service.abstraction
{
    public interface IAccountService
    {
        Task<(bool, IEnumerable<string>)> Register(SignUpVM signUp);
        Task<(User user, string ErrorMessage)> Login(string email, string password);
        Task<IList<string>> GetUserRoles(User user);
        Task SeedSuperAdmin();
    }
}

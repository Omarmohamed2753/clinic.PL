using clinic.BLL.ModelVM.AccountVM;
using clinic.DAL.DataBase;
using Microsoft.AspNetCore.Identity;

namespace clinic.BLL.Service.implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ClinicDbcontext _dbContext;
        private readonly IMapper _mapper;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ClinicDbcontext dbContext, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<(User user, string ErrorMessage)> Login(string email, string password)
        {
            email = email.Trim();
            var user = await _userManager.FindByEmailAsync(email);
            user.EmailConfirmed = true;
            if (user == null)
                return (null, "Invalid email or password");

            if (!user.EmailConfirmed)
                return (null, "Email not confirmed yet");

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded)
                return (user, null);

            return (null, "Invalid email or password");
        }

        public async Task<(bool, IEnumerable<string>)> Register(SignUpVM signUp)
        {
            signUp.Email = signUp.Email.Trim();
            signUp.UserName = signUp.UserName.Trim();

            var existingUser = await _userManager.FindByNameAsync(signUp.UserName)
                               ?? await _userManager.FindByEmailAsync(signUp.Email);

            if (existingUser != null)
                return (false, new[] { "Username or Email already exists" });

            var person = _mapper.Map<Person>(signUp);
            _dbContext.Person.Add(person);
            await _dbContext.SaveChangesAsync();

            var user = _mapper.Map<User>(signUp);
            user.PersonID = person.Id;
            user.EmailConfirmed = true;
            user.UserName = signUp.UserName;

            var result = await _userManager.CreateAsync(user, signUp.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Patient"))
                    await _roleManager.CreateAsync(new IdentityRole("Patient"));

                await _userManager.AddToRoleAsync(user, "Patient");
                return (true, null);
            }

            var errors = result.Errors.Select(e => e.Description);
            return (false, errors);
        }

        public async Task<IList<string>> GetUserRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task SeedSuperAdmin()
        {
            var superAdminEmail = "admin@clinic.com";

            var existingAdmin = await _userManager.FindByEmailAsync(superAdminEmail);
            if (existingAdmin == null)
            {
                var person = new Person
                {
                    FirstName = "System Admin",
                    Age = 0,
                    Gender = "N/A"
                };
                _dbContext.Person.Add(person);
                await _dbContext.SaveChangesAsync();

                var superAdmin = new User
                {
                    UserName = "superadmin",
                    Email = superAdminEmail,
                    EmailConfirmed = true,
                    PersonID = person.Id
                };

                var result = await _userManager.CreateAsync(superAdmin, "StrongPassword!23");
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Admin"))
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));

                    await _userManager.AddToRoleAsync(superAdmin, "Admin");
                }
            }
        }
    }
}

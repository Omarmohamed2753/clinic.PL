using clinic.BLL.ModelVM.AccountVM;
using clinic.DAL.DataBase;
using Microsoft.AspNetCore.Identity;

namespace clinic.BLL.Service.implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ClinicDbcontext dbContext;
        private readonly IMapper mapper;


        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ClinicDbcontext dbContext, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<(User user, string ErrorMessage)> Login(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return (null, "Invalid email or password");

            var result = await signInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded)
                return (user, null);
            else
                return (null, "Invalid email or password");
        }

        public async Task<(bool, IEnumerable<string>)> Register(SignUpVM signUp)
        {
            var person = mapper.Map<Person>(signUp);

            dbContext.Person.Add(person);
            await dbContext.SaveChangesAsync();

            var user = mapper.Map<User>(signUp);
            user.PersonID = person.Id;

            var result = await userManager.CreateAsync(user, signUp.Password);

            if (result.Succeeded)
            {
                var roleExist = await roleManager.RoleExistsAsync("Patient");
                if (!roleExist)
                    await roleManager.CreateAsync(new IdentityRole("Patient"));

                await userManager.AddToRoleAsync(user, "Patient");

                return (true, null);
            }

            var errors = result.Errors.Select(e => e.Description);
            return (false, errors);
        }


        public async Task<IList<string>> GetUserRoles(User user)
        {
            return await userManager.GetRolesAsync(user);
        }

        public async Task SeedSuperAdmin()
        {
            var superAdminEmail = "admin@clinic.com";

            var existingAdmin = await userManager.FindByEmailAsync(superAdminEmail);
            if (existingAdmin == null)
            {
                var person = new Person
                {
                    FirstName = "System Admin",
                    Age = 0,
                    Gender = "N/A"
                };

                dbContext.Person.Add(person);
                await dbContext.SaveChangesAsync();

                var superAdmin = new User
                {
                    UserName = "superadmin",
                    Email = superAdminEmail,
                    EmailConfirmed = true,
                    PersonID = person.Id
                };

                var result = await userManager.CreateAsync(superAdmin, "StrongPassword!23");

                if (result.Succeeded)
                {
                    
                    var roleExist = await roleManager.RoleExistsAsync("Admin");
                    if (!roleExist)
                        await roleManager.CreateAsync(new IdentityRole("Admin"));

                    await userManager.AddToRoleAsync(superAdmin, "Admin");
                }
            }
        }



    }
}

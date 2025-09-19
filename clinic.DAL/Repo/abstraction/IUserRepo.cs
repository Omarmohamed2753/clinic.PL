
namespace clinic.DAL.Repo.abstraction
{
    public interface IUserRepo
    {
        Task<bool> CreateAsync(User user);
        Task<bool> DeletedAsync(int userId);
        Task<bool> EditAsync(User user, string updatedBy = "system");
        Task<User?> GetUserByIdAsync(string userId);
        Task<List<User>> GetUsersAsync(Expression<Func<User, bool>>? filter = null);
        Task<User?> GetByEmailAsync(string email);
    }

}

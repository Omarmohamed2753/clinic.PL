using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace clinic.DAL.Repo.implementation
{
    public class UserRepo : IUserRepo
    {
        private readonly ClinicDbcontext _db;

        public UserRepo(ClinicDbcontext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(User user)
        {
            try
            {
                var result = await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                return result.Entity.Person.Id > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeletedAsync(int userId)
        {
            try
            {
                var user = await _db.Users
                    .Include(d => d.Person)
                    .FirstOrDefaultAsync(d => d.Id == userId.ToString());

                if (user != null)
                {

                    if (user.Person != null)
                    {
                        _db.Person.Remove(user.Person);
                    }

                    _db.Users.Remove(user);

                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> EditAsync(User user, string updatedBy = "system")
        {
            var found = await _db.Users.FirstOrDefaultAsync(s => s.Id == user.Id);
            if (found == null) throw new ArgumentNullException(nameof(user));

            if (found.Person.Update(
                    user.Person.FirstName,
                    user.Person.LastName,
                    user.Person.NationalID,
                    user.Person.Phone,
                    user.Person.Address,
                    user.Person.Age,
                    user.Person.BirthDate,
                    user.Person.ModifiedBy))
            {
                return await _db.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            try
            {
                return await _db.Users.FirstOrDefaultAsync(a => a.Id == userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<User>> GetUsersAsync(Expression<Func<User, bool>>? filter = null)
        {
            try
            {
                if (filter != null)
                {
                    return await _db.Users.Where(filter).ToListAsync();
                }

                return await _db.Users.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}

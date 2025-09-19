using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace clinic.DAL.Repo.implementation
{
    public class DepartmentRepo : IDepartmentRepo
    {
        private readonly ClinicDbcontext Db;

        public DepartmentRepo(ClinicDbcontext Db)
        {
            this.Db = Db;
        }

        public async Task<bool> CreateAsync(Department department)
        {
            try
            {
                var result = await Db.Departments.AddAsync(department);
                await Db.SaveChangesAsync();
                return result.Entity.Id > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeletedAsync(int departmentId)
        {
            try
            {
                var department = await Db.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);

                Db.Departments.Remove(department);
                await Db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> EditAsync(int DepartmentId, Department department)
        {
            try
            {
                var result = await Db.Departments.FirstOrDefaultAsync(a => a.Id == DepartmentId);
                if (result != null)
                {
                    var r = result.Update(department.Description, department.DepartmentName, result.ModifiedBy);
                    if (r)
                    {
                        await Db.SaveChangesAsync();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Department?> GetdepartmentByIdAsync(int UserId)
        {
            try
            {
                return await Db.Departments.FirstOrDefaultAsync(a => a.Id == UserId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Department>> GetdepartmentsAsync(Expression<Func<Department, bool>>? filter = null)
        {
            try
            {
                if (filter != null)
                {
                    return await Db.Departments.Where(filter).ToListAsync();
                }
                else
                {
                    return await Db.Departments.ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

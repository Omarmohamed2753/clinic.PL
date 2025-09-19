using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace clinic.DAL.Repo.implementation
{
    public class DoctorRepo : IDoctorRepo
    {
        private readonly ClinicDbcontext Db;
        public DoctorRepo(ClinicDbcontext Db)
        {
            this.Db = Db;
        }

        public async Task<bool> CreateAsync(Doctor doctor)
        {
            try
            {
                var result = await Db.Doctors.AddAsync(doctor);
                await Db.SaveChangesAsync();

                return result.Entity.Id > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeletedAsync(int doctorId)
        {
            try
            {
                var doctor = await Db.Doctors
                    .Include(d => d.Person)
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(d => d.Id == doctorId);

                if (doctor != null)
                {
                    if (doctor.User != null)
                    {
                        Db.Users.Remove(doctor.User);
                    }

                    if (doctor.Person != null)
                    {
                        Db.Person.Remove(doctor.Person);
                    }

                    Db.Doctors.Remove(doctor);

                    await Db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> EditAsync(Doctor doctor)
        {
            try
            {
                var result = await Db.Doctors
                                     .Where(a => a.Id == doctor.Id)
                                     .FirstOrDefaultAsync();

                if (result != null)
                {
                    var r = result.Person.Update(
                        doctor.Person.FirstName,
                        doctor.Person.LastName,
                        doctor.Person.NationalID,
                        doctor.Person.Phone,
                        doctor.Person.Address,
                        doctor.Person.Age,
                        doctor.Person.BirthDate,
                        doctor.Person.ModifiedBy
                    );

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

        public async Task<Doctor?> GetDoctorByIdAsync(int UserId)
        {
            try
            {
                return await Db.Doctors
                               .Where(a => a.Id == UserId)
                               .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
         }

        public async Task<List<Doctor>> GetDoctorsAsync(Expression<Func<Doctor, bool>>? filter = null)
        {
            try
            {
                if (filter != null)
                {
                    return await Db.Doctors.Where(filter).ToListAsync();
                }
                else
                {
                    return await Db.Doctors.ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

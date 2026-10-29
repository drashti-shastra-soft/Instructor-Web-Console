using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;

namespace InstructorWebClient.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DataAccess.Models.InstructorDbContext _context;

        public StudentRepository(DataAccess.Models.InstructorDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.StudentLister> GetAllAsync(Domain.StudentLister mLister)
        {
            try
            {
                var query = from s in _context.Students
                            where s.ToDate == null
                            orderby s.Id descending
                            select new Domain.Student
                            {
                                Id = s.Id,
                                FirstName = s.FirstName ?? string.Empty,
                                LastName = s.LastName ?? string.Empty,
                                EmailAddress = s.EmailAddress ?? string.Empty,
                                MobileNumber = s.MobileNumber ?? string.Empty,
                                LicenseNumber = s.LicenseNumber ?? string.Empty,
                                Sin = s.Sin ?? string.Empty,
                                IsActive = s.IsActive
                            };

                // Optional filtering
                var criteria = mLister.SearchCriteria;
                if (criteria != null)
                {
                    if (criteria.AddressId > default(int))
                        query = query.Where(x => x.AddressId == criteria.AddressId);

                    if (criteria.FirstName.IsNotNullOrEmpty())
                        query = query.Where(x => x.FirstName != null && x.FirstName.ToUpper().Contains(criteria.FirstName.ToUpper()));

                    if (criteria.LastName.IsNotNullOrEmpty())
                        query = query.Where(x => x.LastName != null && x.LastName.ToUpper().Contains(criteria.LastName.ToUpper()));

                    if (criteria.EmailAddress.IsNotNullOrEmpty())
                        query = query.Where(x => x.EmailAddress != null && x.EmailAddress.ToUpper().Contains(criteria.EmailAddress.ToUpper()));

                    if (criteria.MobileNumber.IsNotNullOrEmpty())
                        query = query.Where(x => x.MobileNumber != null && x.MobileNumber.ToUpper().Contains(criteria.MobileNumber.ToUpper()));

                    if (criteria.LicenseNumber.IsNotNullOrEmpty())
                        query = query.Where(x => x.LicenseNumber != null && x.LicenseNumber.ToUpper().Contains(criteria.LicenseNumber.ToUpper()));

                    if (criteria.Sin.IsNotNullOrEmpty())
                        query = query.Where(x => x.Sin != null && x.Sin.ToUpper().Contains(criteria.Sin.ToUpper()));

                    if (criteria.Status.IsNotNullOrEmpty())
                    {
                        if (criteria.Status == "Active")
                            query = query.Where(x => x.IsActive);
                        else if (criteria.Status == "Disabled")
                            query = query.Where(x => !x.IsActive);
                    }
                }

                // Pagination setup
                var pagination = mLister.Pagination ?? new Domain.Pagination();
                pagination.TotalRecord = await query.CountAsync(); // ✅ Count before pagination

                if (pagination.Take <= 0)
                {
                    pagination.Take = pagination.TotalRecord;
                    pagination.Skip = 0;
                }

                pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;
                pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                // Apply pagination in SQL
                var pagedQuery = query.Skip(pagination.Skip).Take(pagination.Take);

                var efStudent = await pagedQuery.ToListAsync();

                mLister.Pagination = pagination;

                // Convert anonymous type to Domain.Student
                mLister.List = JsonSerializer.Deserialize<List<Domain.Student>>(JsonSerializer.Serialize(efStudent)) ?? new List<Domain.Student>();
            }
            catch
            {
                throw;
            }

            return mLister;
        }

        public async Task<Domain.Student> UpsertAsync(Domain.Student mStudent)
        {
            try
            {
                var efStudent = await _context.Students.Where(x => x.Id == mStudent.Id && x.ToDate == null).FirstOrDefaultAsync();
                if (efStudent == null || efStudent.Id <= default(int))
                {
                    efStudent = new DataAccess.Models.Student
                    {
                        FromDate = DateTime.UtcNow
                    };
                    _context.Students.Add(efStudent);
                }

                efStudent.FirstName = mStudent.FirstName;
                efStudent.LastName = mStudent.LastName;
                efStudent.EmailAddress = mStudent.EmailAddress;
                efStudent.MobileNumber = mStudent.MobileNumber;
                efStudent.LicenseNumber = mStudent.LicenseNumber;
                efStudent.Sin = mStudent.Sin;
                efStudent.IsActive = mStudent.IsActive;

                await _context.SaveChangesAsync();

                mStudent.Id = efStudent.Id;
            }
            catch (Exception)
            {
                throw;
            }
            return mStudent;
        }

        public async Task<Domain.Student> GetByAsync(int id)
        {
            var mStudent = new Domain.Student();
            try
            {
                mStudent = await _context.Students
                                         .AsNoTracking()
                                         .Where(s => s.ToDate == null && s.Id == id)
                                         .Select(s => new Domain.Student
                                         {
                                             Id = s.Id,                                             
                                             FirstName = s.FirstName,
                                             LastName = s.LastName,
                                             EmailAddress = s.EmailAddress,
                                             MobileNumber = s.MobileNumber,
                                             LicenseNumber = s.LicenseNumber,
                                             Sin = s.Sin,
                                             IsActive = s.IsActive
                                         }).FirstOrDefaultAsync();
            }

            catch (Exception)
            {
                throw;
            }
            return mStudent;
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var efStudent = await _context.Students.Where(u => u.Id == id && u.ToDate == null).FirstOrDefaultAsync(); // ✅ async query
                if (efStudent != null && efStudent.Id > default(int))
                {
                    efStudent.ToDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Domain.Student>> GetActiveStudent()
        {
            return await _context.Students
                .Where(s => s.IsActive && s.ToDate == null)
                .Select(s => new Domain.Student
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    LicenseNumber = s.LicenseNumber
                })
                .ToListAsync();
        }

        public async Task<List<Domain.Student>> GetActiveStudents(List<int> mStudents)
        {
            var result = await (from s in _context.Students
                                where s.IsActive && s.ToDate == null && mStudents.Contains(s.Id)
                                select new Domain.Student
                                {
                                    Id = s.Id,
                                    FirstName = s.FirstName,
                                    LastName = s.LastName,
                                    EmailAddress = s.EmailAddress
                                }).ToListAsync();
            return result;
        }
    }
}

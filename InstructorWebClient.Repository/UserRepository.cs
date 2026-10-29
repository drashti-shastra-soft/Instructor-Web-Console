using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InstructorWebClient.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataAccess.Models.InstructorDbContext _context;

        public UserRepository(DataAccess.Models.InstructorDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.User> GetByAsync(string username)
        {
            var mUser = new Domain.User();

            try
            {
                mUser = await (from u in _context.Users.AsNoTracking()
                               join ut in _context.UserTypes on u.UserTypeId equals ut.Id
                               where u.ToDate == null && u.Username == username
                               select new Domain.User()
                               {
                                   Id = u.Id,
                                   FirstName = u.FirstName,
                                   LastName = u.LastName,
                                   EmailAddress = u.EmailAddress,
                                   MobileNumber = u.MobileNumber,
                                   Password = u.Password,
                                   IsActive = u.IsActive,
                                   UserTypeId = u.UserTypeId,
                                   UserType = ut.Name
                               }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return mUser;

        }

        public async Task<Domain.UserLister> GetAllAsync(Domain.UserLister mLister)
        {
            try
            {
                var query = from u in _context.Users.AsNoTracking()
                            join ut in _context.UserTypes on u.UserTypeId equals ut.Id
                            where u.ToDate == null && ut.ToDate == null
                            orderby u.Id descending
                            select new
                            {
                                Id = u.Id,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                EmailAddress = u.EmailAddress,
                                MobileNumber = u.MobileNumber,
                                Username = u.Username,
                                Password = u.Password,
                                IsActive = u.IsActive,
                                UserTypeId = u.UserTypeId,
                                UserType = u.UserType.Name
                            };

                // Optional filtering
                if (mLister.SearchCriteria != null)
                {
                    var criteria = mLister.SearchCriteria;

                    if (criteria.FirstName.IsNotNullOrEmpty())
                        query = query.Where(x => x.FirstName != null && x.FirstName.ToUpper().Contains(criteria.FirstName.ToUpper()));

                    if (criteria.LastName.IsNotNullOrEmpty())
                        query = query.Where(x => x.LastName != null && x.LastName.ToUpper().Contains(criteria.LastName.ToUpper()));

                    if (criteria.EmailAddress.IsNotNullOrEmpty())
                        query = query.Where(x => x.EmailAddress != null && x.EmailAddress.ToUpper().Contains(criteria.EmailAddress.ToUpper()));

                    if (criteria.MobileNumber.IsNotNullOrEmpty())
                        query = query.Where(x => x.MobileNumber != null && x.MobileNumber.ToUpper().Contains(criteria.MobileNumber.ToUpper()));

                    if (criteria.Username.IsNotNullOrEmpty())
                        query = query.Where(x => x.Username != null && x.Username.ToUpper().Contains(criteria.Username.ToUpper()));

                    if (criteria.UserType.IsNotNullOrEmpty())
                        query = query.Where(x => x.UserType != null && x.UserType.ToUpper().Contains(criteria.UserType.ToUpper()));

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
                pagination.TotalRecord = await query.CountAsync();

                if (pagination.Take <= 0)
                {
                    pagination.Take = pagination.TotalRecord;
                    pagination.Skip = 0;
                }

                pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;
                pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                // Apply pagination in SQL
                var pagedQuery = query.Skip(pagination.Skip).Take(pagination.Take);

                var efUser = await pagedQuery.ToListAsync();

                mLister.Pagination = pagination;

                // Convert anonymous type to Domain.User
                mLister.List = JsonSerializer.Deserialize<List<Domain.User>>(JsonSerializer.Serialize(efUser)) ?? new List<Domain.User>();
            }
            catch
            {
                throw;
            }

            return mLister;
        }

        public async Task<Domain.User> UpsertAsync(Domain.User mUser)
        {
            try
            {
                var efUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == mUser.Id && x.ToDate == null);
                if (efUser == null || efUser.Id <= default(int))
                {
                    efUser = new DataAccess.Models.User
                    {
                        FromDate = DateTime.UtcNow
                    };
                    _context.Users.Add(efUser);
                }
                efUser.FirstName = mUser.FirstName;
                efUser.LastName = mUser.LastName;
                efUser.EmailAddress = mUser.EmailAddress;
                efUser.MobileNumber = mUser.MobileNumber;
                efUser.Username = mUser.Username;
                efUser.Password = mUser.Password;
                efUser.IsActive = mUser.IsActive;
                efUser.UserTypeId = mUser.UserTypeId;

                await _context.SaveChangesAsync();

                mUser.Id = efUser.Id;
            }
            catch (Exception)
            {
                throw;
            }
            return mUser;
        }

        public async Task<Domain.User> GetByAsync(int id)
        {
            var mUser = new Domain.User();

            try
            {
                mUser = await (from u in _context.Users.AsNoTracking()
                               where u.ToDate == null && u.Id == id
                               select new Domain.User()
                               {
                                   Id = u.Id,
                                   FirstName = u.FirstName,
                                   LastName = u.LastName,
                                   Username = u.Username,
                                   Password = u.Password,
                                   EmailAddress = u.EmailAddress,
                                   MobileNumber = u.MobileNumber,
                                   IsActive = u.IsActive,
                                   UserTypeId = u.UserTypeId
                               }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return mUser;

        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var efUser = await _context.Users.Where(x => x.Id == id && x.ToDate == null).FirstOrDefaultAsync();
                if (efUser != null)
                {
                    efUser.ToDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

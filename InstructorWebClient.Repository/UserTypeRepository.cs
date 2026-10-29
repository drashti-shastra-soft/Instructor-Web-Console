using InstructorWebClient.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InstructorWebClient.Repository
{
    public class UserTypeRepository : IUserTypeRepository
    {
        private readonly DataAccess.Models.InstructorDbContext _context;

        public UserTypeRepository(DataAccess.Models.InstructorDbContext context)
        {
            _context = context;
        }

        public async Task<List<Domain.UserType>> GetAllAsync()
        {
            var mUserTypes = new List<Domain.UserType>();
            try
            {
                mUserTypes = await (from userType in _context.UserTypes.AsNoTracking()
                                    where userType.ToDate == null && userType.IsActive == true
                                    select new Domain.UserType
                                    {
                                        Id = userType.Id,
                                        Name = userType.Name,
                                    }).ToListAsync();
            }
            catch
            {
                throw;
            }
            return mUserTypes;
        }
    }
}

using InstructorWebClient.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstructorWebClient.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DataAccess.Models.InstructorDbContext _context;

        public AddressRepository(DataAccess.Models.InstructorDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Address> UpsertAsync(Domain.Address mAddress)
        {
            try
            {
                var efAddres = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == mAddress.Id && x.ToDate == null);
                if (efAddres == null || efAddres.Id <= default(int))
                {
                    efAddres = new DataAccess.Models.Address
                    {
                        FromDate = DateTime.UtcNow
                    };
                    _context.Addresses.Add(efAddres);
                }
                efAddres.StudentId = mAddress.StudentId;
                efAddres.Line1 = mAddress.Line1;
                efAddres.Line2 = mAddress.Line2;
                efAddres.Line3 = mAddress.Line3;
                efAddres.City = mAddress.City;
                efAddres.State = mAddress.State;
                efAddres.ZipCode = mAddress.ZipCode;

                await _context.SaveChangesAsync();

                mAddress.Id = efAddres.Id;
            }
            catch (Exception)
            {
                throw;
            }
            return mAddress;
        }

        public async Task<Domain.Address> GetByAsync(int id)
        {
            var mAddress = new Domain.Address();

            try
            {
                mAddress = await (from a in _context.Addresses.AsNoTracking()
                               where a.ToDate == null && a.StudentId == id 
                               select new Domain.Address()
                               {
                                   Id = a.Id,
                                   StudentId = a.StudentId,
                                   Line1 = a.Line1,
                                   Line2 = a.Line2,
                                   Line3 = a.Line3,
                                   City = a.City,
                                   State = a.State,
                                   ZipCode = a.ZipCode,
                               }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return mAddress;

        }
    }
}

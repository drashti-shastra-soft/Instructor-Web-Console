using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstructorWebClient.Infrastructure.IRepository
{
    public interface IAddressRepository
    {
        Task<Domain.Address> UpsertAsync(Domain.Address mAddress);
        Task<Domain.Address> GetByAsync(int id);
    }
}

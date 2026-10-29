using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstructorWebClient.Infrastructure.IService
{
    public interface IAddressService
    {
        Task<Domain.Address> UpsertAsync(Domain.Address mAddress);
        Task<Domain.Address> GetByAsync(int id);
    }
}

using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.IService;

namespace InstructorWebClient.Service
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repo;

        public AddressService(IAddressRepository repo)
        {
            _repo = repo;
        }

        public async Task<Domain.Address> UpsertAsync(Domain.Address mAddress)
        {
            return await _repo.UpsertAsync(mAddress);
        }

        public async Task<Domain.Address> GetByAsync(int id)
        {
            return await _repo.GetByAsync(id);
        }
    }
}

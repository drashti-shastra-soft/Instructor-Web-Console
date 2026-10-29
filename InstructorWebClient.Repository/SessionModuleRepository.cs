using InstructorWebClient.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;

namespace InstructorWebClient.Repository
{
    public class SessionModuleRepository : ISessionModuleRepository
    {
        private readonly DataAccess.Models.InstructorDbContext _context;

        public SessionModuleRepository(DataAccess.Models.InstructorDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Domain.Module mModule)
        {
            int id = 0;
            try
            {
                var efSessionModule = new DataAccess.Models.SessionModule();
                efSessionModule.SessionId = mModule.SessionId;
                efSessionModule.Module = mModule.ModuleNumber;
                efSessionModule.FromDate = DateTime.UtcNow;
                _context.SessionModules.Add(efSessionModule);
                await _context.SaveChangesAsync();
                id = efSessionModule.Id;
            }
            catch
            {
                throw;
            }
            return id;
        }

        public async Task DeleteAsync(int sessionId)
        {
            try
            {
                var efRecords = await _context.SessionModules.Where(x => x.SessionId == sessionId && x.ToDate == null).ToListAsync();
                if (efRecords != null && efRecords.Count > default(int))
                {
                    foreach (var efRecord in efRecords)
                    {
                        efRecord.ToDate = DateTime.UtcNow;
                    }
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

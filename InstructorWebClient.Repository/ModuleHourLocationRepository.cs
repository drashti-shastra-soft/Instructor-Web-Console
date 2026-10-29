using InstructorWebClient.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;

namespace InstructorWebClient.Repository
{
    public class ModuleHourLocationRepository : IModuleHourLocationRepository
    {
        private readonly DataAccess.Models.InstructorDbContext _context;

        public ModuleHourLocationRepository(DataAccess.Models.InstructorDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Domain.HourLocation mHourLocation)
        {
            try
            {
                var efModuleHourLocation = new DataAccess.Models.ModuleHourLocation();
                efModuleHourLocation.SessionId = mHourLocation.SessionId;
                efModuleHourLocation.ModuleId = mHourLocation.ModuleId;
                efModuleHourLocation.Location = mHourLocation.Location;
                efModuleHourLocation.Hour = mHourLocation.Hour;
                efModuleHourLocation.FromDate = DateTime.UtcNow;
                _context.ModuleHourLocations.Add(efModuleHourLocation);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteAsync(int sessionId)
        {
            try
            {
                var efRecords = await _context.ModuleHourLocations.Where(x => x.SessionId == sessionId && x.ToDate == null).ToListAsync();
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

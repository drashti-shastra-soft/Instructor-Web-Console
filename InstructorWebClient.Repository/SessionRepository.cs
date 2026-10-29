using InstructorWebClient.DataAccess.Models;
using InstructorWebClient.Domain;
using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InstructorWebClient.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DataAccess.Models.InstructorDbContext _context;

        public SessionRepository(DataAccess.Models.InstructorDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.SessionLister> GetAllAsync(Domain.SessionLister mLister)
        {
            try
            {
                // Base query: sessions + user
                var efRecords = from s in _context.Sessions
                                join u in _context.Users on s.UserId equals u.Id
                                join ss in _context.StudentSessions on s.Id equals ss.SessionId
                                join stud in _context.Students on ss.StudentId equals stud.Id
                                where s.ToDate == null
                                group stud by new
                                {
                                    s.Id,
                                    s.UserId,
                                    s.ClassType,
                                    Username = u.FirstName + " " + u.LastName,
                                    s.Date,
                                    s.EndTime,
                                    s.StartTime
                                } into g
                                select new Domain.Session
                                {
                                    Id = g.Key.Id,
                                    UserId = g.Key.UserId,
                                    ClassType = g.Key.ClassType,
                                    Username = g.Key.Username,
                                    Date = g.Key.Date,
                                    EndTime = g.Key.EndTime,
                                    StartTime = g.Key.StartTime,                                    
                                    Student = string.Join(", ", g.Select(x => x.FirstName + " " + x.LastName))
                                };

                // Apply filters BEFORE touching modules/hour locations
                var criteria = mLister.SearchCriteria;
                if (criteria != null)
                {
                    if (criteria.Username.IsNotNullOrEmpty())
                        efRecords = efRecords.Where(x => !string.IsNullOrEmpty(x.Username) && x.Username.ToUpper().Contains(criteria.Username.ToUpper()));

                    if (criteria.ClassType != default(int))
                        efRecords = efRecords.Where(x => x.ClassType != default(int) && x.ClassType == criteria.ClassType);

                    if (criteria.Date != DateOnly.MinValue)
                        efRecords = efRecords.Where(x => x.Date == criteria.Date);

                    if (criteria.StartTime != TimeOnly.MinValue)
                        efRecords = efRecords.Where(x => x.StartTime >= criteria.StartTime);

                    if (criteria.EndTime != TimeOnly.MinValue)
                        efRecords = efRecords.Where(x => x.EndTime <= criteria.EndTime);

                    if (criteria.Student.IsNotNullOrEmpty())
                        efRecords = efRecords.Where(x => !string.IsNullOrEmpty(x.Student) &&
                                                         x.Student.ToUpper().Contains(criteria.Student.ToUpper()));

                    if (criteria.UserId > 0)
                        efRecords = efRecords.Where(x => x.UserId == criteria.UserId);
                }

                // Pagination setup
                var pagination = mLister.Pagination ?? new Domain.Pagination();
                pagination.TotalRecord = await efRecords.CountAsync();

                if (pagination.Take <= 0)
                {
                    pagination.Take = pagination.TotalRecord;
                    pagination.Skip = 0;
                }

                pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;
                pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                // Apply pagination in SQL
                var pagedSessions = await efRecords.OrderByDescending(x => x.Id).Skip(pagination.Skip).Take(pagination.Take).ToListAsync();

                // Now attach modules/hour locations AFTER filters & pagination
                foreach (var session in pagedSessions)
                {
                    session.Modules = await _context.SessionModules
                                                    .Where(m => m.SessionId == session.Id && m.ToDate == null)
                                                    .Select(m => new Domain.Module
                                                    {
                                                        ModuleNumber = m.Module,
                                                        HourLocations = _context.ModuleHourLocations
                                                                                .Where(h => h.ModuleId == m.Id && h.ToDate == null)
                                                                                .Select(h => new Domain.HourLocation
                                                                                {
                                                                                    Location = h.Location,
                                                                                    Hour = h.Hour
                                                                                }).ToList()
                                                    }).ToListAsync();
                }

                mLister.Pagination = pagination;
                mLister.List = pagedSessions;
            }
            catch (Exception)
            {
                throw;
            }

            return mLister;
        }

        public async Task<Domain.Session> UpsertAsync(Domain.Session mSession)
        {
            try
            {
                var efsession = await _context.Sessions.Where(x => x.Id == mSession.Id && x.ToDate == null).FirstOrDefaultAsync();
                if (efsession == null || efsession.Id <= default(int))
                {
                    efsession = new DataAccess.Models.Session();
                    efsession.FromDate = DateTime.UtcNow;
                    efsession.UserId = mSession.UserId;
                    _context.Sessions.Add(efsession);
                }
                efsession.ClassType = mSession.ClassType;
                efsession.Date = mSession.Date;
                efsession.StartTime = mSession.StartTime;
                efsession.EndTime = mSession.EndTime;
                //efsession.StudentId = mSession.StudentId;

                await _context.SaveChangesAsync();

                mSession.Id = efsession.Id;
            }
            catch
            {
                throw;
            }
            return mSession;
        }

        public async Task<Domain.Session> GetByAsync(int id)
        {
            var mSession = new Domain.Session();
            try
            {
                var efRecords = from s in _context.Sessions.AsNoTracking()
                                where s.ToDate == null && s.Id == id
                                select new Domain.Session()
                                {
                                    Id = s.Id,
                                    ClassType = s.ClassType,
                                    UserId = s.UserId,
                                    Date = s.Date,
                                    EndTime = s.EndTime,
                                    StartTime = s.StartTime,
                                    Student = s.Student,
                                };

                mSession = await efRecords.FirstOrDefaultAsync();

                if (mSession != null && mSession.ClassType == 1)
                {
                    mSession.Modules = await _context.SessionModules
                                                     .Where(m => m.SessionId == mSession.Id && m.ToDate == null)
                                                     .Select(m => new Domain.Module
                                                     {
                                                         ModuleNumber = m.Module,
                                                         HourLocations = _context.ModuleHourLocations
                                                                                 .Where(h => h.ModuleId == m.Id && h.ToDate == null)
                                                                                 .Select(h => new Domain.HourLocation
                                                                                 {
                                                                                     Location = h.Location,
                                                                                     Hour = h.Hour
                                                                                 }).ToList()
                                                     }).ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mSession;
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var efSession = await _context.Sessions.Where(x => x.Id == id && x.ToDate == null).FirstOrDefaultAsync();
                if (efSession != null)
                {
                    efSession.ToDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<int> Years, List<int> Months)> FetchDistinctYearMonthForUserAsync(int userId)
        {
            try
            {
                var uniqueYearMonths = await (from x in _context.Sessions
                                              where x.UserId == userId
                                              select new { x.Date.Year, x.Date.Month })
                                                 .Distinct()
                                                 .OrderBy(x => x.Year)
                                                 .ThenBy(x => x.Month)
                                                 .ToListAsync();

                var years = uniqueYearMonths.Select(x => x.Year).Distinct().ToList();
                var months = uniqueYearMonths.Select(x => x.Month).Distinct().ToList();

                return (years, months);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(TimeSpan InYardTotal, TimeSpan InVehicleTotal, TimeSpan InClassroomTotal)> GetSessionHourSumsAsync(int userId, int year, int month)
        {
            try
            {
                var filteredData = await (from mhl in _context.ModuleHourLocations
                                          join s in _context.Sessions on mhl.SessionId equals s.Id
                                          where s.UserId == userId && s.ToDate == null && mhl.ToDate == null && s.Date.Month == month && s.Date.Year == year
                                          select new
                                          {
                                              mhl.Location,
                                              mhl.Hour
                                          }).ToListAsync();

                var result = filteredData.GroupBy(x => x.Location)
                                         .Select(g => new
                                         {
                                             Location = g.Key,
                                             TotalHours = g.Sum(x =>
                                             {
                                                 // Try parsing as TimeSpan
                                                 if (TimeSpan.TryParse(x.Hour, out var ts))
                                                 {
                                                     return ts.TotalHours;
                                                 }

                                                 // Try parsing as float (e.g., "2", "1.5")
                                                 if (float.TryParse(x.Hour, out var f))
                                                 {
                                                     return f;
                                                 }

                                                 return 0;
                                             })
                                         }).ToList();

                var inYardTotal = TimeSpan.FromHours(result.Where(x => x.Location == EnumHelper.GetEnumString(Enums.Location.In_Yard)).Select(s => s.TotalHours).FirstOrDefault());
                var inVehicleTotal = TimeSpan.FromHours(result.Where(x => x.Location == EnumHelper.GetEnumString(Enums.Location.In_Vehicle)).Select(s => s.TotalHours).FirstOrDefault());
                var inClassroomTotal = TimeSpan.FromHours(result.Where(x => x.Location == EnumHelper.GetEnumString(Enums.Location.In_Classroom)).Select(s => s.TotalHours).FirstOrDefault());

                return (inYardTotal, inVehicleTotal, inClassroomTotal);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

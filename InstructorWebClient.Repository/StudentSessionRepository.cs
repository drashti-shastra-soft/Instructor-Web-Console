using InstructorWebClient.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InstructorWebClient.Repository
{
    public class StudentSessionRepository : IStudentSessionRepository
    {
        private readonly DataAccess.Models.InstructorDbContext _context;

        public StudentSessionRepository(DataAccess.Models.InstructorDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.StudentSessionLister> GetAllAsync(Domain.StudentSessionLister mLister)
        {
            try
            {
                var query = from ss in _context.StudentSessions.AsNoTracking()
                            where ss.ToDate == null
                            orderby ss.Id descending
                            select new
                            {
                                Id = ss.Id,
                                StudentId = ss.StudentId,
                                SessionId = ss.SessionId
                            };

                var criteria = mLister.SearchCriteria;
                if (criteria != null)
                {
                    if (criteria.StudentId > 0)
                        query = query.Where(x => x.StudentId == criteria.StudentId);
                    if (criteria.SessionId > 0)
                        query = query.Where(x => x.SessionId == criteria.SessionId);
                }

                // Convert anonymous type to Domain.StudentSession
                mLister.List = JsonSerializer.Deserialize<List<Domain.StudentSession>>(JsonSerializer.Serialize(await query.ToListAsync())) ?? new List<Domain.StudentSession>();
            }
            catch
            {
                throw;
            }
            return mLister;
        }

        public async Task<Domain.StudentSession> CreateAsync(Domain.StudentSession mStudentSession)
        {
            try
            {
                var efStudentSession = await _context.StudentSessions.FirstOrDefaultAsync(x => x.Id == mStudentSession.Id && x.ToDate == null);
                if (efStudentSession == null || efStudentSession.Id <= default(int))
                {
                    efStudentSession = new DataAccess.Models.StudentSession
                    {
                        FromDate = DateTime.UtcNow
                    };
                    _context.StudentSessions.Add(efStudentSession);
                }

                efStudentSession.StudentId = mStudentSession.StudentId;
                efStudentSession.SessionId = mStudentSession.SessionId;

                await _context.SaveChangesAsync();

                mStudentSession.Id = efStudentSession.Id;
            }
            catch (Exception)
            {
                throw;
            }
            return mStudentSession;
        }

        public async Task<Domain.StudentSession> GetByAsync(int id)
        {
            var mStudent = new Domain.StudentSession();
            try
            {
                mStudent = await _context.StudentSessions
                                         .AsNoTracking()
                                         .Where(ss => ss.ToDate == null && ss.Id == id)
                                         .Select(ss => new Domain.StudentSession
                                         {  
                                             Id = ss.Id,
                                             StudentId = ss.StudentId,
                                             SessionId = ss.SessionId,
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
                var efStudentSession = await _context.StudentSessions.Where(u => u.Id == id && u.ToDate == null).FirstOrDefaultAsync(); 
                if (efStudentSession != null && efStudentSession.Id > default(int))
                {
                    efStudentSession.ToDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync(); 
                }
            }
            catch (Exception)
            {
                throw; 
            }
        }
    }
}

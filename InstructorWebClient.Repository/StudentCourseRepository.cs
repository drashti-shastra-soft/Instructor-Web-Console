using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InstructorWebClient.Repository
{
    public class StudentCourseRepository : IStudentCourseRepository
    {
        private readonly DataAccess.Models.InstructorDbContext _context;

        public StudentCourseRepository(DataAccess.Models.InstructorDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.StudentCourseLister> GetAllAsync(Domain.StudentCourseLister mLister)
        {
            try
            {
                var efRecords = from sc in _context.StudentCourses
                                join c in _context.Courses on sc.CourseId equals c.Id
                                where sc.ToDate == null
                                orderby sc.Id descending
                                select new Domain.StudentCourse
                                {
                                    Id = sc.Id,
                                    StudentId = sc .StudentId,
                                    CourseId = sc.CourseId,
                                    StartDate = sc.StartDate,
                                    EndDate = sc.EndDate,
                                    IsActive = sc.IsActive,
                                    Course = c.Name
                                };

                // Apply filters BEFORE touching modules/hour locations
                var criteria = mLister.SearchCriteria;
                if (criteria != null)
                {
                    if (criteria.StudentId > default(int))
                        efRecords = efRecords.Where(x => x.StudentId == criteria.StudentId);

                    if (criteria.StartDate != DateOnly.MinValue)
                        efRecords = efRecords.Where(x => x.StartDate == criteria.StartDate);

                    if (criteria.EndDate != DateOnly.MinValue)
                        efRecords = efRecords.Where(x => x.EndDate == criteria.EndDate);

                    if (criteria.CourseId > default(int))
                        efRecords = efRecords.Where(x => x.CourseId == criteria.CourseId);

                    if (criteria.Status.IsNotNullOrEmpty())
                    {
                        if (criteria.Status == "Active")
                            efRecords = efRecords.Where(x => x.IsActive);
                        else if (criteria.Status == "Disabled")
                            efRecords = efRecords.Where(x => !x.IsActive);
                    }
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
                var pagedQuery = efRecords.Skip(pagination.Skip).Take(pagination.Take);

                var efStudentCourse = await pagedQuery.ToListAsync();

                mLister.Pagination = pagination;

                // Convert anonymous type to Domain.User
                mLister.List = JsonSerializer.Deserialize<List<Domain.StudentCourse>>(JsonSerializer.Serialize(efStudentCourse)) ?? new List<Domain.StudentCourse>();
            }
            catch (Exception)
            {
                throw;
            }

            return mLister;
        }

        public async Task<Domain.StudentCourse> UpsertAsync(Domain.StudentCourse mStudentCourse)
        {
            try
            {
                var efStudentCourse = await _context.StudentCourses.FirstOrDefaultAsync(x => x.Id == mStudentCourse.Id && x.ToDate == null);
                if (efStudentCourse == null || efStudentCourse.Id <= default(int))
                {
                    efStudentCourse = new DataAccess.Models.StudentCourse
                    {
                        FromDate = DateTime.UtcNow
                    };
                    _context.StudentCourses.Add(efStudentCourse);
                }
                efStudentCourse.CourseId = mStudentCourse.CourseId;
                efStudentCourse.StudentId = mStudentCourse.StudentId;
                efStudentCourse.StartDate = mStudentCourse.StartDate;
                efStudentCourse.EndDate = mStudentCourse.EndDate;
                efStudentCourse.ToDate = mStudentCourse.ToDate;
                efStudentCourse.IsActive = mStudentCourse.IsActive;

                await _context.SaveChangesAsync();

                mStudentCourse.Id = efStudentCourse.Id;
            }
            catch (Exception)
            {
                throw;
            }
            return mStudentCourse;
        }

        public async Task<Domain.StudentCourse> GetByAsync(int id)
        {
            var mStudentCourse = new Domain.StudentCourse();

            try
            {
                mStudentCourse = await (from sc in _context.StudentCourses.AsNoTracking()
                               where sc.ToDate == null && sc.Id == id 
                               select new Domain.StudentCourse()
                               {
                                   Id = sc.Id,
                                   StudentId = sc.StudentId,
                                   CourseId = sc.CourseId,
                                   StartDate = sc.StartDate,
                                   EndDate = sc.EndDate,
                                   IsActive = sc.IsActive,
                               }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return mStudentCourse;

        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var efStudentCourse = await _context.StudentCourses.Where(x => x.Id == id && x.ToDate == null).FirstOrDefaultAsync();
                if (efStudentCourse != null)
                {
                    efStudentCourse.ToDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<Domain.StudentCourse> UpdateActiveAsync(Domain.StudentCourse mStudentCourse)
        {
            var studentCourses = await _context.StudentCourses
                .Where(x => x.StudentId == mStudentCourse.StudentId)
                .ToListAsync();

            foreach (var course in studentCourses)
            {
                course.IsActive = false;
            }

            var efStudentCourse = studentCourses.FirstOrDefault(x => x.Id == mStudentCourse.Id);
            if (efStudentCourse != null)
            {
                efStudentCourse.IsActive = true;
                efStudentCourse.StartDate = mStudentCourse.StartDate;
                efStudentCourse.EndDate = mStudentCourse.EndDate;
                efStudentCourse.ToDate = mStudentCourse.ToDate;
            }

            await _context.SaveChangesAsync();

            return mStudentCourse;
        }

    }
}

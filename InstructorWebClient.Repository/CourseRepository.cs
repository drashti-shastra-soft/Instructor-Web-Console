using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace InstructorWebClient.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DataAccess.Models.InstructorDbContext _context;

        public CourseRepository(DataAccess.Models.InstructorDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.CourseLister> GetAllAsync(Domain.CourseLister mLister)
        {
            try
            {
                var query = from c in _context.Courses
                            where c.ToDate == null
                            orderby c.Id descending
                            select new Domain.Course
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Description = c.Description,
                                TotalCumulativeHours = c.TotalCumulativeHours,
                                IsActive = c.IsActive,
                            };

                // Apply filters BEFORE touching modules/hour locations
                var criteria = mLister.SearchCriteria;
                if (criteria != null)
                {
                    if (!string.IsNullOrEmpty(criteria.Name))
                    {
                        query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(criteria.Name.ToUpper()));
                    }

                    if (!string.IsNullOrEmpty(criteria.Status))
                    {
                        if (criteria.Status == "Active")
                            query = query.Where(x => x.IsActive);
                        else if (criteria.Status == "Disabled")
                            query = query.Where(x => !x.IsActive);
                    }
                }

                // Pagination setup
                var pagination = mLister.Pagination ?? new Domain.Pagination();
                pagination.TotalRecord = await query.CountAsync();

                if (pagination.Take <= 0)
                {
                    pagination.Take = pagination.TotalRecord;
                    pagination.Skip = 0;
                }

                pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;
                pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                // Apply pagination in SQL
                var pagedQuery = query.Skip(pagination.Skip).Take(pagination.Take);

                var efCourse = await pagedQuery.ToListAsync();

                mLister.Pagination = pagination;

                mLister.List = JsonSerializer.Deserialize<List<Domain.Course>>(JsonSerializer.Serialize(efCourse)) ?? new List<Domain.Course>();
            }
            catch
            {
                throw;
            }

            return mLister;
        }

        public async Task<Domain.Course> UpsertAsync(Domain.Course mCourse)
        {
            try
            {
                var efCourse = await _context.Courses.FirstOrDefaultAsync(x => x.Id == mCourse.Id && x.ToDate == null);
                if (efCourse == null || efCourse.Id <= default(int))
                {
                    efCourse = new DataAccess.Models.Course
                    {
                        FromDate = DateTime.UtcNow
                    };
                    _context.Courses.Add(efCourse);
                }
                efCourse.Name = mCourse.Name;
                efCourse.Description = mCourse.Description;
                efCourse.TotalCumulativeHours = mCourse.TotalCumulativeHours;
                efCourse.IsActive = mCourse.IsActive;

                await _context.SaveChangesAsync();

                mCourse.Id = efCourse.Id;
            }
            catch (Exception)
            {
                throw;
            }
            return mCourse;
        }

        public async Task<Domain.Course> GetByAsync(int id)
        {
            var mCourse = new Domain.Course();

            try
            {
                mCourse = await (from c in _context.Courses.AsNoTracking()
                               where c.ToDate == null && c.Id == id
                               select new Domain.Course()
                               {
                                   Id = c.Id,
                                   Name = c.Name,
                                   Description = c.Description,
                                   TotalCumulativeHours = c.TotalCumulativeHours,
                                   IsActive = c.IsActive,
                               }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return mCourse;

        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var efCourse = await _context.Courses.Where(x => x.Id == id && x.ToDate == null).FirstOrDefaultAsync();
                if (efCourse != null)
                {
                    efCourse.ToDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Domain.Course>> GetActiveCourses()
        {
            return await _context.Courses
                .Where(c => c.IsActive && c.ToDate == null)
                .OrderBy(c => c.Name)
                .Select(c => new Domain.Course
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}

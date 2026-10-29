using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.IService;
using InstructorWebClient.Infrastructure.Utils;
using System.Text.RegularExpressions;

namespace InstructorWebClient.Service
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _repo;
        private readonly ISessionModuleRepository _sessionModuleRepo;
        private readonly IModuleHourLocationRepository _moduleHourLocationRepo;
        private readonly IStudentSessionRepository _studentSessionRepo;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IFileReaderService _fileReaderService;
        private readonly IStudentRepository _studentRepo;
        private readonly ISessionManagerService _sessionManagerService;

        public SessionService(ISessionRepository repo, ISessionModuleRepository sessionModuleRepo, IModuleHourLocationRepository moduleHourLocationRepo, IStudentSessionRepository studentSessionRepo, IEmailSenderService emailSenderService, IFileReaderService fileReaderService, IStudentRepository studentRepo, IUserRepository userRepo, ISessionManagerService sessionManagerService)
        {
            _repo = repo;
            _sessionModuleRepo = sessionModuleRepo;
            _moduleHourLocationRepo = moduleHourLocationRepo;
            _studentSessionRepo = studentSessionRepo;
            _emailSenderService = emailSenderService;
            _fileReaderService = fileReaderService;
            _studentRepo = studentRepo;
            _sessionManagerService = sessionManagerService;
        }

        public async Task<Domain.SessionLister> GetAllAsync(Domain.SessionLister mLister)
        {
            return await _repo.GetAllAsync(mLister);
        }

        public async Task<Domain.Session> UpsertAsync(Domain.Session mSession)
        {
            mSession = await _repo.UpsertAsync(mSession);
            if (mSession != null && mSession.Id > default(int))
            {
                await _moduleHourLocationRepo.DeleteAsync(mSession.Id);
                await _sessionModuleRepo.DeleteAsync(mSession.Id);
                await _studentSessionRepo.DeleteAsync(mSession.Id);

                foreach (var mModule in mSession.Modules)
                {
                    mModule.SessionId = mSession.Id;
                    mModule.Id = await _sessionModuleRepo.CreateAsync(mModule);
                    if (mModule != null && mModule.Id > 0)
                    {
                        foreach (var mHourLocation in mModule.HourLocations)
                        {
                            mHourLocation.ModuleId = mModule.Id;
                            mHourLocation.SessionId = mModule.SessionId;
                            await _moduleHourLocationRepo.CreateAsync(mHourLocation);
                        }
                    }
                }

                string instructor = string.Empty;
                var mLoginUser = _sessionManagerService.GetLoginUser();
                if (mLoginUser != null)
                    instructor = mLoginUser.FirstName + " " + mLoginUser.LastName;

                var mStudents = await _studentRepo.GetActiveStudents(mSession.StudentSessions.Select(s => s.StudentId).ToList());
                foreach (var mStudenSession in mSession.StudentSessions)
                {
                    mStudenSession.SessionId = mSession.Id;
                    await _studentSessionRepo.CreateAsync(mStudenSession);

                    var mStudent = mStudents.Where(x => x.Id == mStudenSession.StudentId).FirstOrDefault();
                    if (mStudent != null && mStudent.Id > default(int))
                    {
                        _ = Task.Run(async () =>
                        {
                            await Task.Delay(3000);
                            mSession.Student = $"{mStudent.FirstName} {mStudent.LastName}";
                            SendEmailOnCreateSession(mSession, mStudent.EmailAddress);
                        });
                    }
                }
            }
            return mSession;
        }

        public async Task<Domain.Session> GetByAsync(int id)
        {
            return await _repo.GetByAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _moduleHourLocationRepo.DeleteAsync(id);
            await _sessionModuleRepo.DeleteAsync(id);
            await _repo.DeleteAsync(id);
        }

        public async Task<(List<int> Years, List<int> Months)> FetchDistinctYearMonthForUserAsync(int userId)
        {
            return await _repo.FetchDistinctYearMonthForUserAsync(userId);
        }

        public async Task<Domain.SessionSummary> GetSessionHourSumsAsync(int userId, int year, int month)
        {
            var mSummary = new Domain.SessionSummary();

            var totalHours = await _repo.GetSessionHourSumsAsync(userId, year, month);

            mSummary.InYard = StaticMethods.FormatTimeDuration(totalHours.InYardTotal);

            mSummary.InVehicle = StaticMethods.FormatTimeDuration(totalHours.InVehicleTotal);

            mSummary.InClassroom = StaticMethods.FormatTimeDuration(totalHours.InClassroomTotal);

            mSummary.Total = StaticMethods.FormatTimeDuration(totalHours.InYardTotal + totalHours.InVehicleTotal + totalHours.InClassroomTotal);

            return mSummary;
        }


        #region Private Methods
        private void SendEmailOnCreateSession(Domain.Session mSession, string studentEmail)
        {
            string content = _fileReaderService.ReadFileContent("EmailTemplates/StudentEmailSender.html");
            string finalContent = PopulateEmailTemplate(content, mSession);
            _emailSenderService.SendEmail(finalContent, studentEmail);
        }

        private string PopulateEmailTemplate(string template, Domain.Session mSession)
        {
            var replacements = new Dictionary<string, string>
            {
                { "{{Student}}", mSession.Username },
                { "{{Instructor}}", mSession.Instructor },
                { "{{Date}}", mSession.Date.ToString("dd MMM yyyy") },
                { "{{StartTime}}", mSession.StartTime.ToString("hh:mm tt")},
                { "{{EndTime}}", mSession.EndTime.ToString("hh:mm tt")},
                { "{{Duration}}", CalculatePeriod(mSession.StartTime, mSession.EndTime) }
            };

            if (mSession.ClassType == 1)
            {

                replacements.Add("{{Modules}}", "Special class type 1 details");
            }
            else
            {
                // If template contains {{ExtraInfo}}, strip it out cleanly
                template = Regex.Replace(
                    template,
                    @"<tr>\s*<td.*?>.*?ExtraInfo.*?</td>\s*<td>{{Modules}}</td>\s*</tr>",
                    string.Empty,
                    RegexOptions.IgnoreCase | RegexOptions.Singleline
                );
            }

            foreach (var pair in replacements)
            {
                template = template.Replace(pair.Key, pair.Value);
            }

            return template;
        }

        private string CalculatePeriod(TimeOnly start, TimeOnly end)
        {
            TimeSpan duration = end.ToTimeSpan() - start.ToTimeSpan();
            return $"{duration.Hours} hours {duration.Minutes} minutes";
        }
        #endregion
    }
}

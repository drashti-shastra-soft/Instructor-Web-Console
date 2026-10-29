using InstructorWebClient.Domain;
using InstructorWebClient.Infrastructure.IService;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace InstructorWebClient.Service
{
    public class SessionManagerService : ISessionManagerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string LoginUserKey = "LoginUser";

        public SessionManagerService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void ClearLoginUser()
        {
            _httpContextAccessor.HttpContext?.Session.Remove(LoginUserKey);
        }

        public User? GetLoginUser()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var json = session?.GetString(LoginUserKey);
            return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<Domain.User>(json);

        }

        public void SetLoginUser(User user)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var json = JsonSerializer.Serialize(user);
            session?.SetString(LoginUserKey, json);
        }
    }
}

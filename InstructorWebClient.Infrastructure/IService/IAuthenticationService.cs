using System;
using System.Linq;

namespace InstructorWebClient.Infrastructure.IService
{
    public interface IAuthenticationService
    {
        Task<Domain.User> ValidateAsync(Domain.Credential mCredential);
    }
}

using System;
using System.Linq;

namespace InstructorWebClient.Infrastructure.Utils
{
    public class Enums
    {
        public enum Operation
        {
            User = 1,
            Session = 2,
            Student = 3,
            StudentCourse = 4,
            Course = 5
        }

        public enum Result
        {
            Success = 1,
            Error = 2,
            Warning = 3
        }

        public enum UserType
        {
            Admin = 1,
            Instructor = 2
        }

        public enum Location
        {
            In_Yard = 1,
            In_Vehicle = 2,
            In_Classroom = 3
        }

    }
}

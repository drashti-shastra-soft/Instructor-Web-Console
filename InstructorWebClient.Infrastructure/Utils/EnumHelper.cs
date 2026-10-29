using System;
using System.Linq;
using static InstructorWebClient.Infrastructure.Utils.Enums;

namespace InstructorWebClient.Infrastructure.Utils
{
    public class EnumHelper
    {
        public static string GetEnumString(Operation operation)
        {
            switch (operation)
            {
                case Operation.User:
                    return nameof(Operation.User);
                case Operation.Session:
                    return nameof(Operation.Session);
                case Operation.Student:
                    return nameof(Operation.Student);
                case Operation.StudentCourse:
                    return nameof(Operation.StudentCourse);
                case Operation.Course:
                    return nameof(Operation.Course);
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }

        public static string GetEnumString(Result result)
        {
            switch (result)
            {
                case Result.Success:
                    return nameof(Result.Success);
                case Result.Warning:
                    return nameof(Result.Warning);
                case Result.Error:
                    return nameof(Result.Error);
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }

        public static string GetEnumString(UserType userType)
        {
            switch (userType)
            {
                case UserType.Admin:
                    return nameof(UserType.Admin);
                case UserType.Instructor:
                    return nameof(UserType.Instructor);
                default:
                    throw new ArgumentOutOfRangeException(nameof(userType), userType, null);
            }
        }

        public static int GetEnumIndex(UserType userType)
        {
            switch (userType)
            {
                case UserType.Admin:
                    return (int)UserType.Admin;
                case UserType.Instructor:
                    return (int)UserType.Instructor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(userType), userType, null);
            }
        }

        public static string GetEnumString(Location location)
        {
            switch (location)
            {
                case Location.In_Yard:
                    return "In Yard";
                case Location.In_Vehicle:
                    return "In Vehicle";
                case Location.In_Classroom:
                    return "In Classroom";
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }


    }
}

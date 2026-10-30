# Driving Instructor App

Driving Instructor App is a responsive ASP.NET web console for driving instructors in Canada. Instructors record driver details, in-car and in-class lessons, skills assessed, and progress as per Canadian driving regulations. Admins track each instructor's logged hours, lesson history, and student completion through reports. The console features role-based access and secure login. The solution is organized into separate Domain, Infrastructure, Service, and Web Console projects.

## Features

- **Authentication** — secure sign-in with role-based access for instructors and admins.
- **Dashboard** — landing page with key highlights (upcoming lessons, logged hours, student progress) after entering the console.
- **Driver (student) management** — record driver details, licence information, contact details, and enrolment status.
- **Lesson management** — record in-car and in-class lessons with date, time, duration, location, and lesson type.
- **Skills assessment** — record skills assessed per lesson as per Canadian driving regulations (e.g., observation, signalling, lane changes, parking, highway driving).
- **Progress tracking** — track each student's progress against required skills and lesson hours.
- **Instructor hour logging** — instructors log their hours per lesson and per day; admins can review logged hours.
- **Lesson history** — full history of lessons per student and per instructor.
- **Student completion** — track and mark student completion against regulation requirements.
- **Reports for admins** — instructor-wise logged hours, lesson history, and student completion reports.
- **Role-based access** — instructors see their own students and lessons; admins see all instructors and students.
- **Shared web UI** — Razor views, shared layout, pagination helper, and client-side validation.
- **Responsive design** — mobile-ready layout for instructors using phones and tablets in the field.

> Feature descriptions are based on the controllers, domain models, services, and views present in this repository. Actual permissions and backend behavior may depend on the connected database and configuration.

## Technology stack

- C# and .NET Framework 4.8 (or `<your target framework>`)
- ASP.NET MVC with Razor views
- Visual Studio solution (`DrivingInstructorApp.sln`)
- Dependency injection configured through Unity
- Entity Framework + SQL Server for instructors, students, lessons, skills, and reports
- Role-based authorization (instructor vs admin)
- HTML, CSS, Bootstrap, JavaScript, and jQuery-based MVC views

## Solution structure

```text
V4/
├── DrivingInstructorApp.sln
├── DrivingInstructorApp/                       # ASP.NET MVC web application
│   ├── Controllers/                            # MVC request handling
│   ├── Views/                                  # Razor UI pages
│   ├── App_Start/                              # Routes, filters, Unity setup
│   ├── Config/                                 # Environment-specific app settings
│   └── Web.config
├── DrivingInstructorApp.Domain/                # Domain models and list/pagination models
├── DrivingInstructorApp.Infrastructure/        # Service interfaces, helpers, constants, exceptions
└── DrivingInstructorApp.Service/               # Service implementations and data access
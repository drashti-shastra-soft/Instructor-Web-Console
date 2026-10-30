# Instructor App

Instructor App is a responsive ASP.NET web console for instructors in Canada. Instructors record student details, in-person and in-class sessions, skills assessed, and progress as per Canadian regulations, and schedule online meeting sessions with students. Admins track each instructor's logged hours, session history, student completion, and session schedules through reports. The console features role-based access and secure login. The solution is organized into separate Domain, Infrastructure, Service, and Web Console projects.

## Features

- **Authentication** — secure sign-in with role-based access for instructors and admins.
- **Dashboard** — landing page with key highlights (upcoming sessions, logged hours, student progress) after entering the console.
- **Student management** — record student details, registration information, contact details, and enrolment status.
- **Session scheduling** — schedule in-person, in-class, and online meeting sessions with date, time, duration, location/meeting link, and session type.
- **Online meeting sessions** — schedule and manage online sessions with meeting links (e.g., Zoom, Google Meet, Microsoft Teams) and join details for students.
- **Session recording** — record sessions delivered with notes, attendance, and session outcome.
- **Skills assessment** — record skills assessed per session as per Canadian regulations.
- **Progress tracking** — track each student's progress against required skills and session hours.
- **Instructor hour logging** — instructors log their hours per session and per day; admins can review logged hours.
- **Session history** — full history of sessions per student and per instructor, including online meetings.
- **Student completion** — track and mark student completion against regulation requirements.
- **Reports for admins** — instructor-wise logged hours, session history, online meeting schedules, and student completion reports.
- **Role-based access** — instructors see their own students and sessions; admins see all instructors and students.
- **Shared web UI** — Razor views, shared layout, pagination helper, and client-side validation.
- **Responsive design** — mobile-ready layout for instructors using phones and tablets in the field.

> This app is a **session and progress tracking console**, not a general lecture or course management platform. It focuses on student details, sessions (in-person, in-class, online), skills assessment, and regulation-based progress. Actual permissions and backend behavior may depend on the connected database and configuration.

## Technology stack

- C# and .NET Framework 4.8 (or `<your target framework>`)
- ASP.NET MVC with Razor views
- Visual Studio solution (`InstructorApp.sln`)
- Dependency injection configured through Unity
- Entity Framework + SQL Server for instructors, students, sessions, skills, and reports
- Role-based authorization (instructor vs admin)
- Online meeting integration via `<Zoom / Google Meet / Microsoft Teams / custom link>`
- HTML, CSS, Bootstrap, JavaScript, and jQuery-based MVC views

## Solution structure

```text
V4/
├── InstructorApp.sln
├── InstructorApp/                       # ASP.NET MVC web application
│   ├── Controllers/                     # MVC request handling
│   ├── Views/                           # Razor UI pages
│   ├── App_Start/                       # Routes, filters, Unity setup
│   ├── Config/                          # Environment-specific app settings
│   └── Web.config
├── InstructorApp.Domain/                # Domain models and list/pagination models
├── InstructorApp.Infrastructure/        # Service interfaces, helpers, constants, exceptions
└── InstructorApp.Service/               # Service implementations and data access
namespace SchoolApp.Services
{
    public interface IApplicationService
    {
        UserService UserService { get;  }
        TeacherService TeacherService { get; }
        // Other services can be added here as needed
    }
}

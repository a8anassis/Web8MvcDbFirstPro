using SchoolApp.Data;

namespace SchoolApp.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(Mvc8DbProContext context) : base(context)
        {
        }

        public Task<List<Student>> GetCourseStudentsAsync(int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<Teacher?> GetCourseTeacherAsync(int courseId)
        {
            throw new NotImplementedException();
        }
    }
}

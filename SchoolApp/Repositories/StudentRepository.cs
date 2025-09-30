using SchoolApp.Data;
using SchoolApp.Models;
using System.Linq.Expressions;

namespace SchoolApp.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(Mvc8DbProContext context) : base(context)
        {
        }

        public Task<Student?> GetByam(string? am)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<Student>> GetPaginatedUsersStudentsAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<Student>> GetPaginatedUsersStudentsFilteredAsync(int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates)
        {
            throw new NotImplementedException();
        }

        public Task<List<Course>> GetStudentCoursesAsync(int studentId)
        {
            throw new NotImplementedException();
        }
    }
}

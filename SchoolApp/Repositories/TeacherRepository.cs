using Microsoft.EntityFrameworkCore;
using SchoolApp.Core.Enums;
using SchoolApp.Data;
using SchoolApp.Models;
using System.Linq.Expressions;

namespace SchoolApp.Repositories
{
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(Mvc8DbProContext context) : base(context)
        {
        }

        public async Task<Teacher?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await context.Teachers
                .Where(t => t.PhoneNumber == phoneNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Course>> GetTeacherCoursesAsync(int teacherId)
        {
            List<Course> courses;

            courses = await context.Teachers
                .Where(t => t.Id == teacherId)
                .SelectMany(t => t.Courses)
                .ToListAsync();

            return courses;

        }

        public async Task<List<User>> GetAllUsersTeachersAsync()
        {
            var usersWithRoleTeacher = await context.Users
                .Where(u => u.UserRole == UserRole.Teacher)
                .Include(u => u.Teacher) // Εager loading της σχετικής οντότητας Teacher
                .ToListAsync();

            return usersWithRoleTeacher;
        }

        public Task<PaginatedResult<User>> GetPaginatedUsersTeachersAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        

        public  Task<PaginatedResult<User>> GetPaginatedUsersTeachersFilteredAsync(int pageNumber, int pageSize, 
            List<Expression<Func<User, bool>>> predicates)
        {
            throw new NotImplementedException();
        }

       

        public Task<User?> GetUserTeacherByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}

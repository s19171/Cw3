using cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Services
{
    public class EfStudentsDbService : IStudentsDbService
    {

        private readonly StudentDbContext _context;
        public EfStudentsDbService(StudentDbContext context)
        {
            _context = context;
        }

        public Enrollment EnrollStudent(EnrollStudentRequest request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudents()
        {
            return _context.Students.ToList();
        }

        public Enrollment PromoteStudents(int semester, string studies)
        {
            throw new NotImplementedException();
        }
    }
}

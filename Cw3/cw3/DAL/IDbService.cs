using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw3.Models;

namespace Wyklad3.Services
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        public Student GetStudent(int id);
        public void AddStudent(Student student);
        public void DeleteStudent(int id);
    }
}

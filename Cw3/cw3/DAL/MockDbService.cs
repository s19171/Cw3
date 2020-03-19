using System.Collections.Generic;
using Wyklad3.Models;

namespace Wyklad3.Services
{
    public class MockDbService : IDbService
    {
        private static List<Student> _students = new List<Student>
        {
            new Student{IdStudent=1, FirstName="Jan", LastName="Kowalski", IndexNumber="s1234"},
            new Student{IdStudent=2, FirstName="Anna", LastName="Malewski", IndexNumber="s2342"},
            new Student{IdStudent=3, FirstName="Krzysztof", LastName="Andrzejewicz", IndexNumber="s5432"}
        };

        public void AddStudent(Student student)
        {
            _students.Add(student);
        }

        public void DeleteStudent(int id)
        {
            _students.Remove(GetStudent(id));
        }

        public Student GetStudent(int id)
        {
            return _students.Find(x => x.IdStudent == id);
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}

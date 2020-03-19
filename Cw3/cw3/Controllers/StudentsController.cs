using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Wyklad3.Models;
using Wyklad3.Services;

namespace Wyklad3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents([FromQuery]string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent([FromRoute]int id)
        {
            Student s = _dbService.GetStudent(id);
            if(!(s==null))return Ok(s);
            else return NotFound("Student was not found");
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody]Student student)
        {
            student.IndexNumber=$"s{new Random().Next(1, 20000)}";
            _dbService.AddStudent(student);
            return Ok(student); //JSON
        }

        [HttpPut("{id:int}")]
        public IActionResult PutStudent(int id)
        {
            return Ok($"put {id}");
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteStudent(int id)
        {
            _dbService.DeleteStudent(id);
            return Ok($"deleted student with id: {id}");
        }

    }
}
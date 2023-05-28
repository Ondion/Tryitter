using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tryitter.Models;
using tryitter.Repository;
using tryitter.Entities;

namespace tryitter.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly StudentRepository _repository;
    public StudentController(StudentRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public IActionResult CreateStudent(Student student)
    {
        if (student.Name == null || student.Email == null || student.Status == null || student.Password == null)
        {
            return BadRequest();
        }
        _repository.AddStudent(student);
        return Ok();
    }

    [HttpPost("/Login")]
    public IActionResult Login(StudentLogin studentlogin)
    {
        if (studentlogin.Name == null || studentlogin.Password == null)
        {
            BadRequest("all fields are not filled in");
        }
        var token = _repository.Login(studentlogin);
        return Ok(token);
    }

    [HttpPut("{id}")]
    [Authorize]
    public IActionResult UpdateStudent(int id, Student student)
    {
        if (student.Name == null || student.Email == null || student.Status == null || student.Password == null)
        {
            return BadRequest();
        }
        var response = _repository.UpdateStudent(id, student);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "StudentName")]
    public IActionResult DeleteStudent(int id)
    {
        if (_repository.GetStudentById(id) == null)
        {
            return BadRequest("student not found");
        }
        var student = _repository.GetStudentById(id);
        var response = _repository.DeleteStudent(student);
        return Ok(response);
    }

    [HttpGet("Id/{id}")]
    public IActionResult GetStudent(int id)
    {
        var student = _repository.GetStudentById(id);
        if (student != null)
        {
            var studentResult = new StudentResponse
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Email = student.Email,
                Status = student.Status,
            };
            return Ok(studentResult);
        }
        return BadRequest("student not found");
    }

    [HttpGet("Name/{name}")]
    public IActionResult GetStudent(string name)
    {
        var student = _repository.GetStudent(name);
        if (student != null)
        {
            var studentResult = new StudentResponse
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Email = student.Email,
                Status = student.Status,
            };
            return Ok(studentResult);
        }
        return BadRequest("student not found");
    }

    [HttpGet]
    public IActionResult GetAllStudents()
    {
        var listStudents = new List<StudentResponse>();
        var students = _repository.GetAllStudents();
        foreach (Student student in students)
        {
            listStudents.Add(new StudentResponse
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Email = student.Email,
                Status = student.Status
            });
        }
        return Ok(listStudents);
    }
}

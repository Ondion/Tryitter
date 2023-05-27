using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tryitter.Models;
using tryitter.Repository;


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
}

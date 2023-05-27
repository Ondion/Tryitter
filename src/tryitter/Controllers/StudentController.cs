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
    public IActionResult CreateUser(Student student)
    {
        if (student.Name == null || student.Email == null || student.Status == null || student.Password == null)
        {
            return BadRequest();
        }
        _repository.AddStudent(student);
        return Ok();
    }
}

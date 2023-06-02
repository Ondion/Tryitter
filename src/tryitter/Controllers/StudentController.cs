using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tryitter.Models;
using tryitter.Repository;
using tryitter.Entities;
using System.Text.Json;

namespace tryitter.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
  private const string ErrorStudentNotFound = "Student not found";
  private readonly StudentRepository _repository;
  public StudentController(StudentRepository repository)
  {
    _repository = repository;
  }

  [HttpPost]
  public IActionResult CreateStudent(Student student)
  {
    var response = _repository.AddStudent(student);
    if (response == "Email already exists") return BadRequest(response);
    return Ok(response);
  }

  [HttpPost("/Login")]
  public IActionResult Login(StudentLogin studentlogin)
  {
    var response = _repository.Login(studentlogin);
    if (response == "Student not found") return BadRequest(response);
    return Ok(response);
  }

  [HttpPut("{id}")]
  [Authorize]
  public IActionResult UpdateStudent(int id, Student student)
  {
    var response = _repository.UpdateStudent(id, student);
    if (response == "Student not found") return BadRequest(response);
    if (response == "Email already exists") return BadRequest(response);
    return Ok(response);
  }

  [HttpDelete("{id}")]
  [Authorize]

  public IActionResult DeleteStudent(int id)
  {
    if (_repository.GetStudentById(id) == null)
    {
      return BadRequest(ErrorStudentNotFound);
    }
    var student = _repository.GetStudentById(id);
    var response = _repository.DeleteStudent(student);
    return Ok(response);
  }

  [HttpGet("{id}")]
  public IActionResult GetStudent(int id)
  {
    var student = _repository.GetStudentById(id);
    if (student == null) return BadRequest(ErrorStudentNotFound);
    var studentResult = new StudentResponse
    {

      StudentId = student.StudentId,
      Name = student.Name,
      Email = student.Email,
      Status = student.Status,
    };
    return Ok(studentResult);

  }

  [HttpGet("Name")]
  public IActionResult GetStudent([FromBody] JsonElement name)
  {
    string jsonBody = JsonSerializer.Serialize(name);
    var stringsStudentName = jsonBody.Split('"');
    var student = _repository.GetStudent(stringsStudentName[3]);
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
    return BadRequest(ErrorStudentNotFound);
  }

  [HttpGet]
  public IActionResult GetAllStudents()
  {
    var students = _repository.GetAllStudents();
    return Ok(students);
  }

}

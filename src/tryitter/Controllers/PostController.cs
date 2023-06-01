using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tryitter.Repository;
using tryitter.Entities;
using System.Text.Json;
using tryitter.Models;

namespace tryitter.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
  private readonly PostRepository _postRepository;
  private readonly StudentRepository _studentRepository;
  public PostController(PostRepository repository, StudentRepository studentRepository)
  {
    _postRepository = repository;
    _studentRepository = studentRepository;
  }

  [HttpPost]
  [Authorize]
  public IActionResult CreatePost(PostRequest postRequest)
  {
    var postCreated = _postRepository.CreatePost(postRequest);
    return Ok(postCreated);
  }

  [HttpPut("{id}")]
  [Authorize]
  public IActionResult UpdatePost(int id, PostRequest postRequest)
  {
    var response = _postRepository.UpdatePost(id, postRequest);
    if (response == "Not Alowed") return Unauthorized(response);
    if (response == "Post not found") return BadRequest(response);
    return Ok(response);
  }

  [HttpDelete("{id}")]
  [Authorize]
  public IActionResult DeletePost(int id, [FromBody] JsonElement studentEmail)
  {
    string jsonBody = JsonSerializer.Serialize(studentEmail);
    var stringsStudentEmail = jsonBody.Split('"');
    var response = _postRepository.DeletePost(id, stringsStudentEmail[3]);
    if (response == "Post not found") return BadRequest(response);
    if (response == "Not Alowed") return Unauthorized(response);
    return Ok(response);
  }

  [HttpGet]
  public IActionResult GetAllPosts()
  {
    var posts = _postRepository.GetAllPosts();
    return Ok(posts);
  }

  [HttpGet("{id}")]
  public IActionResult GetPostsById(int id)
  {
    var response = _postRepository.GetPostById(id);
    if (response == null) return BadRequest(response);
    var postResponse = new PostResponse(
    response.PostId,
    response.Content,
    response.CreatAt,
    response.UpdatetAt,
    response.Image,
    response.StudentId);
    return Ok(postResponse);
  }

  [HttpGet("Student/{id}")]
  public IActionResult GetPostByStudentId(int id)
  {
    if (_studentRepository.GetStudentById(id) == null)
    {
      return BadRequest("Student not found");
    }
    var posts = _postRepository.GetPostByStudentId(id);
    return Ok(posts);
  }

  [HttpGet("Last/Student/{id}")]
  public IActionResult GetLastPostByStudentId(int id)
  {
    if (_studentRepository.GetStudentById(id) == null)
    {
      return BadRequest("Student not found");
    }
    var post = _postRepository.GetLastPostByStudentId(id);
    return Ok(post);
  }

  [HttpGet("StudentName")]
  public IActionResult GetPostByStudentName([FromBody] JsonElement name)
  {

    string jsonBody = JsonSerializer.Serialize(name);
    var stringsStudentName = jsonBody.Split('"');
    if (_studentRepository.GetStudent(stringsStudentName[3]) == null)
    {
      return BadRequest("Student not found");
    }
    var posts = _postRepository.GetPostByStudentName(stringsStudentName[3]);
    return Ok(posts);
  }

  [HttpGet("Last/StudentName")]
  public IActionResult GetLastPostByStudentName([FromBody] JsonElement name)
  {
    string jsonBody = JsonSerializer.Serialize(name);
    var stringsStudentName = jsonBody.Split('"');
    if (_studentRepository.GetStudent(stringsStudentName[3]) == null)
    {
      return BadRequest("Student not found");
    }
    var post = _postRepository.GetLastPostByStudentName(stringsStudentName[3]);
    return Ok(post);
  }

}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tryitter.Repository;
using tryitter.Entities;
using System.Text.Json;

namespace tryitter.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
  private readonly PostRepository _repository;
  public PostController(PostRepository repository)
  {
    _repository = repository;
  }

  [HttpPost]
  [Authorize]
  public IActionResult CreatePost(PostRequest postRequest)
  {
    var postCreated = _repository.CreatePost(postRequest);
    return Ok(postCreated);
  }

  [HttpPut("{id}")]
  [Authorize]
  public IActionResult UpdatePost(int id, PostRequest postRequest)
  {
    var response = _repository.UpdatePost(id, postRequest);
    if (response == "Not Alowed") return Unauthorized(response);
    return Ok(response);
  }

  [HttpDelete("{id}")]
  [Authorize]
  public IActionResult DeletePost(int id, [FromBody] JsonElement studentEmail)
  {
    string jsonBody = JsonSerializer.Serialize(studentEmail);
    var stringsStudentEmail = jsonBody.Split('"');
    var response = _repository.DeletePost(id, stringsStudentEmail[3]);
    if (response == "Post not found") return BadRequest(response);
    if (response == "Not Alowed") return Unauthorized(response);
    return Ok(response);
  }

  [HttpGet]
  public IActionResult GetAllPosts()
  {
    var posts = _repository.GetAllPosts();
    return Ok(posts);
  }

  [HttpGet("{id}")]
  public IActionResult GetPostsById(int id)
  {
    var post = _repository.GetPostById(id);
    return Ok(post);
  }

  [HttpGet("Student/{id}")]
  public IActionResult GetPostByStudentId(int id)
  {
    var posts = _repository.GetPostByStudentId(id);
    return Ok(posts);
  }

  [HttpGet("Last/Student/{id}")]
  public IActionResult GetLastPostByStudentId(int id)
  {
    var post = _repository.GetLastPostByStudentId(id);
    return Ok(post);
  }

  [HttpGet("StudentName")]
  public IActionResult GetPostByStudentName([FromBody] JsonElement name)
  {
    string jsonBody = JsonSerializer.Serialize(name);
    var stringsStudentName = jsonBody.Split('"');
    var posts = _repository.GetPostByStudentName(stringsStudentName[3]);
    return Ok(posts);
  }

  [HttpGet("Last/StudentName")]
  public IActionResult GetLastPostByStudentName([FromBody] JsonElement name)
  {
    string jsonBody = JsonSerializer.Serialize(name);
    var stringsStudentName = jsonBody.Split('"');
    var post = _repository.GetLastPostByStudentName(stringsStudentName[3]);
    return Ok(post);
  }

}

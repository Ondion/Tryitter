using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tryitter.Models;
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
    string jsonBody = System.Text.Json.JsonSerializer.Serialize(studentEmail);
    var stringStudentEmail = jsonBody.Split('"');
    var response = _repository.DeletePost(id, stringStudentEmail[3]);
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

  // [HttpGet("StudentName/{name}")]
  // public IActionResult GetPostByStudentName(string name)
  // {
  //     var posts = _repository.GetPostByStudentName(name);
  //     return Ok(posts);
  // }

  // [HttpGet("Last/StudentName/{name}")]
  // public IActionResult GetLastPostByStudentName(string name)
  // {
  //     var post = _repository.GetLastPostByStudentName(name);
  //     return Ok(post);
  // }

}

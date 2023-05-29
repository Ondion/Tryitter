using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tryitter.Models;
using tryitter.Repository;
using tryitter.Entities;

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
    _repository.CreatePost(postRequest);
    return Ok();
  }

  [HttpGet]
  public IActionResult GetAllPosts()
  {
    var posts = _repository.GetAllPosts();
    return Ok(posts);
  }

  [HttpPut("{id}")]
  [Authorize]
  public IActionResult UpdatePost(int id, PostRequest postRequest)
  {
    var response = _repository.UpdatePost(id, postRequest);
    if (response == "Not Alowed") return Unauthorized(response);
    return Ok(response);
  }
  [HttpGet("Student/{id}")]
  public IActionResult GetPostByStudentId(int id)
  {
    var posts = _repository.GetPostByStudentId(id);
    return Ok(posts);
  }

}

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

    [HttpPost("teste")]
    public IActionResult CreateStudent(PostRequest postRequest)
    {
        // if (student.Name == null || student.Email == null || student.Status == null || student.Password == null)
        // {
        //     return BadRequest();
        // }
        _repository.CreatePost(postRequest);
        return Ok();
        // }

    }

    [HttpGet]
    public IActionResult GetAllPosts()
    {
        var posts = _repository.GetAllPosts();
        return Ok(posts);
    }

}

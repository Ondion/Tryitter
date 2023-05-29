using Microsoft.EntityFrameworkCore;
using tryitter.Models;
using tryitter.Entities;
using tryitter.Repository;

namespace tryitter.Repository
{
  public class PostRepository
  {
    private readonly TryitterContext _context;
    public PostRepository(TryitterContext context)
    {
      _context = context;
    }
    public Post CreatePost(PostRequest postRequest)
    {
      // string authHeader = _context.Posts.Request.Headers["Authorization"];
      var student = _context.Students.FirstOrDefault(x => x.Email == postRequest.StudentEmail);
      var postCreated = new Post
      {
        Content = postRequest.Content,
        Image = postRequest.Image,
        StudentId = student.StudentId,
        CreatAt = DateTime.Now,
        UpdatetAt = DateTime.Now
      };
      _context.Posts.Add(postCreated);
      _context.SaveChanges();
      return postCreated;
    }

    public List<Post> GetAllPosts()
    {
      var posts = _context.Posts.ToList();
      _context.SaveChanges();
      return posts;
    }

    public Post GetPostById(int id)
    {
      var post = _context.Posts.Find(id);
      return post;
    }

    public string UpdatePost(int id, PostRequest postRequest)
    {
      var oldPost = _context.Posts.AsNoTracking().Where(c => c.PostId == id).FirstOrDefault();
      var student = _context.Students.AsNoTracking().Where(c => c.Email == postRequest.StudentEmail).FirstOrDefault();
      if (oldPost.StudentId != student.StudentId) return "Not Alowed";
      var post = new Post
      {
        PostId = id,
        StudentId = oldPost.StudentId,
        Content = postRequest.Content,
        CreatAt = oldPost.CreatAt,
        UpdatetAt = DateTime.Now,
        Image = postRequest.Image ?? oldPost.Image,
      };
      _context.Posts.Update(post);
      _context.SaveChanges();
      return "Post updated";
    }

  }
}
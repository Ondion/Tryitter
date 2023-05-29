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

    // public List<Post> GetAllPosts()
    // {
    //   List<PostResponse> listPosts = new List<PostResponse>();
    //   var posts = _context.Posts?.ToList();
    //   foreach (Post post in posts)
    //   {
    //     PostResponse postResponse = new PostResponse
    //     {
    //       PostId = post.PostId,
    //       Content = post.Content,
    //       Image = post.Image,
    //       CreatAt = post.CreatAt,
    //       UpdatetAt = post.UpdatetAt,
    //       StudentId = post.StudentId
    //     };
    //     listPosts.Add(postResponse);
    //   }
    //   return posts;
    // }

    public Post GetPostById(int id)
    {
      var post = _context.Posts.Find(id);
      return post;
    }

    public List<Post> GetPostByStudentId(int id)
    {
      var posts = _context.Posts.Where(p => p.StudentId == id).ToList();
      return posts;
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
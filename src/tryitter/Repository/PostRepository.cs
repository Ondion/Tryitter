using Microsoft.EntityFrameworkCore;
using tryitter.Models;
using tryitter.Entities;

namespace tryitter.Repository
{
  public class PostRepository
  {
    private readonly TryitterContext _context;
    public PostRepository(TryitterContext context)
    {
      _context = context;
    }

    //Create a new post
    public string CreatePost(PostRequest postRequest)
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
      return "Post Created";
    }

    // Update a post if the student who made the request is the same author of the post
    public string UpdatePost(int id, PostRequest postRequest)
    {
      var oldPost = _context.Posts.AsNoTracking().Where(c => c.PostId == id).FirstOrDefault();
      var student = _context.Students.AsNoTracking().Where(c => c.Email == postRequest.StudentEmail).FirstOrDefault();
      if (oldPost == null) return "Post not found";
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

    // Delete a post if the post exists and if the student who made the request is the same author of the post 
    public string DeletePost(int id, string studentEmail)
    {
      var post = _context.Posts.AsNoTracking().Where(c => c.PostId == id).FirstOrDefault();
      if (post == null) return "Post not found";
      var student = _context.Students.AsNoTracking().Where(c => c.Email == studentEmail).FirstOrDefault();
      if (student.StudentId != post.StudentId) return "Not Alowed";
      _context.Posts.Remove(post);
      _context.SaveChanges();
      return "Post deleted";
    }

    //Get all post in DB
    public List<PostResponse> GetAllPosts()
    {
      List<PostResponse> listPosts = new List<PostResponse>();
      var posts = _context.Posts.ToList();
      foreach (Post post in posts)
      {
        var postResponse = new PostResponse(
            post.PostId,
            post.Content,
            post.CreatAt,
            post.UpdatetAt,
            post.Image,
            post.StudentId);

        listPosts.Add(postResponse);
      }
      return listPosts;
    }
    //Get post by postId
    public PostResponse GetPostById(int id)
    {
      var post = _context.Posts.Find(id);

      var postResponse = new PostResponse(
          post.PostId,
          post.Content,
          post.CreatAt,
          post.UpdatetAt,
          post.Image,
          post.StudentId);
      return postResponse;
    }

    //Get all posts done by a studentId 
    public List<PostResponse> GetPostByStudentId(int id)
    {
      List<PostResponse> listPosts = new List<PostResponse>();
      var posts = _context.Posts.Where(p => p.StudentId == id).ToList();
      foreach (Post post in posts)
      {
        var postResponse = new PostResponse(
            post.PostId,
            post.Content,
            post.CreatAt,
            post.UpdatetAt,
            post.Image,
            post.StudentId);

        listPosts.Add(postResponse);
      }
      return listPosts;
    }

    //Get the last post done by studentId 
    public PostResponse GetLastPostByStudentId(int id)
    {
      var posts = _context.Posts.Where(p => p.StudentId == id).ToList();
      var post = posts.LastOrDefault();
      var postResponse = new PostResponse(
          post.PostId,
          post.Content,
          post.CreatAt,
          post.UpdatetAt,
          post.Image,
          post.StudentId);
      return postResponse;
    }
    //get all posts of student by student name
    public List<PostResponse> GetPostByStudentName(string name)
    {
      List<PostResponse> listPosts = new List<PostResponse>();
      var student = _context.Students.FirstOrDefault(s => s.Name == name);
      var posts = _context.Posts.Where(p => p.StudentId == student.StudentId).ToList();
      foreach (Post post in posts)
      {
        var postResponse = new PostResponse(
            post.PostId,
            post.Content,
            post.CreatAt,
            post.UpdatetAt,
            post.Image,
            post.StudentId);

        listPosts.Add(postResponse);
      }
      return listPosts;
    }
    //Get last post done by a student using student name 
    public PostResponse GetLastPostByStudentName(string name)
    {
      var student = _context.Students.FirstOrDefault(s => s.Name == name);
      var posts = _context.Posts.Where(p => p.StudentId == student.StudentId).ToList();
      var post = posts.LastOrDefault();
      var postResponse = new PostResponse(
          post.PostId,
          post.Content,
          post.CreatAt,
          post.UpdatetAt,
          post.Image,
          post.StudentId);
      return postResponse;
    }
  }
}
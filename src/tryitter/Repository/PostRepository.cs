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
        public Post CreatePost(PostRequest postRequest)
        {
            // string authHeader = _context.Posts.Request.Headers["Authorization"];
            var postCreated = new Post
            {
                Content = postRequest.Content,
                Image = postRequest.Image,
                StudentId = 1,
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

    }
}
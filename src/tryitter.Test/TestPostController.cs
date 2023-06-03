using System.Net.Http.Headers;
using System.Text;
using tryitter.Services;
using tryitter.Models;

namespace tryitter.Test;

public class TestPostController : IClassFixture<TestTryitterContext<Program>>
{
  private readonly HttpClient _client;
  public TestPostController(TestTryitterContext<Program> factory)
  {
    _client = factory.CreateClient();
  }
  [Fact]
  public async Task CreatePost()
  {
    var student = new Student { Name = "Ana", Email = "ana@gmail.com", Password = "xft@ff", Status = "Focada" };
    //Create Post
    var jsonToAdd = "{\"content\":\"postagem\",\"image\":\"string\",\"studentEmail\":\"ana@gmail.com\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    //token for request
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var result = await _client.PostAsync("/Post", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("Post Created");
    //Delete Post in inMemory DB
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri("/Post/4"),
      Content = new StringContent("{\"studentEmail\":\"ana@gmail.com\"}", Encoding.UTF8, "application/json"),
    };
    await _client.SendAsync(request).ConfigureAwait(false);
  }

  [Fact]
  public async Task CreatePostWithoutToken()
  {
    //Create Post
    var jsonToAdd = "{\"content\":\"postagem\",\"image\":\"string\",\"studentEmail\":\"ana@gmail.com\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var result = await _client.PostAsync("/Post", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
  }

  [Fact]
  public async Task UpdatePost()
  {
    var student = new Student { Name = "Ana", Email = "ana@gmail.com", Password = "xft@ff", Status = "Focada" };
    //Update Post
    var jsonToAdd = "{\"content\":\"Texto 1-1\",\"image\":\"string\",\"studentEmail\":\"ana@gmail.com\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    //token for request
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var result = await _client.PutAsync("/Post/1", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("Post updated");
  }

  [Fact]
  public async Task UpdatePostWithouToken()
  {
    //Update Post
    var jsonToAdd = "{\"content\":\"Texto 1-1\",\"image\":\"string\",\"studentEmail\":\"ana@gmail.com\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var result = await _client.PutAsync("/Post/1", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
  }
  [Fact]
  public async Task UpdateANonStudentPost()
  {
    var student = new Student { Name = "Paulo", Email = "paulo@gmail.com", Password = "xft@ff", Status = "Focada" };
    //Update Post
    var jsonToAdd = "{\"content\":\"Texto 1-1\",\"image\":\"string\",\"studentEmail\":\"paulo@gmail.com\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    //token for request
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var result = await _client.PutAsync("/Post/1", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("Not Alowed");
  }

  [Fact]
  public async Task UpdateANonExintingPost()
  {
    var student = new Student { Name = "Paulo", Email = "paulo@gmail.com", Password = "xft@ff", Status = "Focada" };
    //Update Post
    var jsonToAdd = "{\"content\":\"Texto 1-1\",\"image\":\"string\",\"studentEmail\":\"paulo@gmail.com\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    //token for request
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var result = await _client.PutAsync("/Post/99", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("Post not found");
  }

  [Fact]
  public async Task DeletePost()
  {
    var student = new Student { Name = "Paulo", Email = "paulo@gmail.com", Password = "xft@ff", Status = "Focada" };
    //token for request
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //Delete Post
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri("/Post/3"),
      Content = new StringContent("{\"studentEmail\":\"paulo@gmail.com\"}", Encoding.UTF8, "application/json"),
    };
    var result = await _client.SendAsync(request).ConfigureAwait(false);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("Post deleted");
  }

  [Fact]
  public async Task DeletePostWithoutToken()
  {
    //Delete Post
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri("/Post/3"),
      Content = new StringContent("{\"studentEmail\":\"paulo@gmail.com\"}", Encoding.UTF8, "application/json"),
    };
    var result = await _client.SendAsync(request).ConfigureAwait(false);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
  }

  [Fact]
  public async Task DeleteANonExintingPost()
  {
    var student = new Student { Name = "Paulo", Email = "paulo@gmail.com", Password = "xft@ff", Status = "Focada" };
    //token for request
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //Delete Post
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri("/Post/99"),
      Content = new StringContent("{\"studentEmail\":\"paulo@gmail.com\"}", Encoding.UTF8, "application/json"),
    };
    var result = await _client.SendAsync(request).ConfigureAwait(false);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("Post not found");
  }

  [Fact]
  public async Task DeleteANonStudentPost()
  {
    var student = new Student { Name = "Paulo", Email = "paulo@gmail.com", Password = "xft@ff", Status = "Focada" };
    //token for request
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //Delete Post
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri("/Post/1"),
      Content = new StringContent("{\"studentEmail\":\"paulo@gmail.com\"}", Encoding.UTF8, "application/json"),
    };
    var result = await _client.SendAsync(request).ConfigureAwait(false);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("Not Alowed");
  }

  [Fact]
  public async Task GetPostById()
  {
    var result = await _client.GetAsync("Post/2");
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("{\"postId\":2,\"content\":\"Texto 2\",\"creatAt\":\"2022-10-02T08:35:00\",\"updatetAt\":\"2022-10-04T08:35:00\",\"image\":null,\"studentId\":2}");
  }

  [Fact]
  public async Task GetPostByNonExintingId()
  {
    var result = await _client.GetAsync("Post/99");
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
  }

  [Fact]
  public async Task GetAllPosts()
  {
    var result = await _client.GetAsync("Post");
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
  }

  [Fact]
  public async Task GetPostsByStudentId()
  {
    var result = await _client.GetAsync("Post/Student/2");
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("[{\"postId\":3,\"content\":\"Texto 3\",\"creatAt\":\"2022-10-02T08:35:00\",\"updatetAt\":\"2022-10-05T08:35:00\",\"image\":null,\"studentId\":2},{\"postId\":2,\"content\":\"Texto 2\",\"creatAt\":\"2022-10-02T08:35:00\",\"updatetAt\":\"2022-10-04T08:35:00\",\"image\":null,\"studentId\":2}]");
  }
  [Fact]
  public async Task GetPostsByNonExitingStudentId()
  {
    var result = await _client.GetAsync("Post/Student/99");
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("Student not found");
  }


}
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
    await _client.DeleteAsync("/Student/5");
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

}
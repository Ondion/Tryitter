using System.Net.Http.Headers;
using System.Text;
using tryitter.Services;
using tryitter.Models;

namespace tryitter.Test;

public class TestController : IClassFixture<TestTryitterContext<Program>>
{
  private readonly HttpClient _client;
  public TestController(TestTryitterContext<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task CreateStudent()
  {

    var student = new Student { Name = "Maria", Email = "maria@gmail.com", Password = "xft@ff", Status = "Focada" };
    //Create Student
    var jsonToAdd = "{\"name\":\"Maria\",\"email\":\"maria@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var result = await _client.PostAsync("/Student", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("student created");
    //Delete Maria in inMemory DB
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    await _client.DeleteAsync("/Student/5");
  }

  [Fact]
  public async Task CreateStudentWithAExistingEmail()
  {
    //Create a Student with a exiting email inMemory in DB
    var jsonToAdd = "{\"name\":\"Tom\",\"email\":\"tom@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var result = await _client.PostAsync("/Student", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var resultString = result.Content.ReadAsStringAsync().Result;
    resultString.Should().Be("Email already exists");
  }

  [Fact]
  public async Task LoginWithAExistingStudent()
  {
    var student = new Student { Name = "Pedro", Email = "pedro@gmail.com", Password = "xft@ff", Status = "Focada" };

    //Create Student
    var jsonToAdd = "{\"name\":\"Pedro\",\"email\":\"pedro@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    await _client.PostAsync("/Student", stringContent);

    // login
    var jsonToAddLogin = "{\"email\":\"pedro@gmail.com\",\"password\":\"xft@ff\"}";
    var stringContentLogin = new StringContent(jsonToAddLogin, Encoding.UTF8, "application/json");
    var resultLogin = await _client.PostAsync("/Login", stringContentLogin);
    resultLogin.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    // Reset DB
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    await _client.DeleteAsync("/Student/6");
  }

  [Fact]
  public async Task LoginWithANonExistingStudent()
  {
    //login
    var jsonToAddLoginWithError = "{\"email\":\"carlos@gmail\",\"password\":\"xf\"}";
    var stringContentLogin = new StringContent(jsonToAddLoginWithError, Encoding.UTF8, "application/json");
    var resultLogin = await _client.PostAsync("/Login", stringContentLogin);
    resultLogin.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var resultLoginString = resultLogin.Content.ReadAsStringAsync().Result;
    resultLoginString.Should().Be("Student not found");
  }

  [Fact]
  public async Task GetAllStudents()
  {
    //login
    var getAllStudentsResult = await _client.GetAsync("/Student");
    getAllStudentsResult.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var getAllStudentsResultString = getAllStudentsResult.Content.ReadAsStringAsync().Result;
    getAllStudentsResultString.Should().Be("[{\"studentId\":1,\"name\":\"Ana\",\"email\":\"ana@gmail.com\",\"status\":\"Focada\"},{\"studentId\":2,\"name\":\"Paulo\",\"email\":\"paulo@gmail.com\",\"status\":\"Focada\"},{\"studentId\":3,\"name\":\"Tom\",\"email\":\"tom@gmail.com\",\"status\":\"Focada\"},{\"studentId\":4,\"name\":\"Joao\",\"email\":\"joao@gmail.com\",\"status\":\"Focada\"}]");
  }

  [Fact]
  public async Task GetStudentWithACorrectId()
  {
    //GetStudentById
    var resultGetStudentById = await _client.GetAsync("Student/1");
    resultGetStudentById.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var resultGetStudentByIdString = resultGetStudentById.Content.ReadAsStringAsync().Result;
    resultGetStudentByIdString.Should().Be("{\"studentId\":1,\"name\":\"Ana\",\"email\":\"ana@gmail.com\",\"status\":\"Focada\"}");
  }

  [Fact]
  public async Task GetStudentWithANonExintingId()
  {
    //GetStudentByWrongId
    var resultgetStudentById = await _client.GetAsync("Student/99");
    resultgetStudentById.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var resultgetStudentByIdString = resultgetStudentById.Content.ReadAsStringAsync().Result;
    resultgetStudentByIdString.Should().Be("Student not found");
  }

  [Fact]
  public async Task GetStudentWithAExintingName()
  {
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri("/Student/Name"),
      Content = new StringContent("{\"name\":\"Paulo\"}", Encoding.UTF8, "application/json"),
    };
    var result = await _client.SendAsync(request).ConfigureAwait(false);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var getStudentsResultString = result.Content.ReadAsStringAsync().Result;
    getStudentsResultString.Should().Be("{\"studentId\":2,\"name\":\"Paulo\",\"email\":\"paulo@gmail.com\",\"status\":\"Focada\"}");
  }

  [Fact]
  public async Task GetStudentWithANonExintingName()
  {
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri("/Student/Name"),
      Content = new StringContent("{\"name\":\"Mickey\"}", Encoding.UTF8, "application/json"),
    };
    var result = await _client.SendAsync(request).ConfigureAwait(false);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var getStudentsResultString = result.Content.ReadAsStringAsync().Result;
    getStudentsResultString.Should().Be("Student not found");
  }

  [Fact]
  public async Task DeleteStudentWithACorrectId()
  {
    var student = new Student { Name = "Joaquim", Email = "joka@gmail.com", Password = "xft@ff", Status = "Focada" };

    //Create Student
    var jsonToAdd = "{\"name\":\"Joaquim\",\"email\":\"joka@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var result = await _client.PostAsync("/Student", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    //DeleteStudent
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var resultDeleteStudent = await _client.DeleteAsync("Student/6");
    resultDeleteStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var resultDeleteStudentString = resultDeleteStudent.Content.ReadAsStringAsync().Result;
    resultDeleteStudentString.Should().Be("student remove");
  }

  [Fact]
  public async Task DeleteStudentWithANonExitingId()
  {
    var student = new Student { Name = "Joaquim", Email = "joka@gmail.com", Password = "xft@ff", Status = "Focada" };
    //DeleteStudent
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var resultDeleteStudent = await _client.DeleteAsync("Student/99");
    resultDeleteStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var resultDeleteStudentString = resultDeleteStudent.Content.ReadAsStringAsync().Result;
    resultDeleteStudentString.Should().Be("Student not found");
  }

  [Fact]
  public async Task DeleteStudentWithoutToken()
  {
    //DeleteStudent
    var resultDeleteStudent = await _client.DeleteAsync("Student/1");
    resultDeleteStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
  }

  [Fact]
  public async Task UpdateStudent()
  {
    var student = new Student { Name = "Ana", Email = "ana@gmail.com", Password = "xft@ff", Status = "Focada" };
    //UpdateStudent
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var jsonToAdd = "{\"name\":\"Aninha\",\"email\":\"ana@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var resultUpdateStudent = await _client.PutAsync("Student/1", stringContent);
    resultUpdateStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var resultUpdateStudentString = resultUpdateStudent.Content.ReadAsStringAsync().Result;
    resultUpdateStudentString.Should().Be("Student updated");
  }

  [Fact]
  public async Task UpdateStudentWithoutToken()
  {
    //UpdateStudent
    var jsonToAdd = "{\"name\":\"Aninha\",\"email\":\"ana@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var resultUpdateStudent = await _client.PutAsync("Student/1", stringContent);
    resultUpdateStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
  }

  [Fact]
  public async Task UpdateStudentWithAExitingEmail()
  {
    var student = new Student { Name = "Ana", Email = "ana@gmail.com", Password = "xft@ff", Status = "Focada" };
    //DeleteStudent
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var jsonToAdd = "{\"name\":\"Aninha\",\"email\":\"tom@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var resultUpdateStudent = await _client.PutAsync("Student/1", stringContent);
    resultUpdateStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var resultUpdateStudentString = resultUpdateStudent.Content.ReadAsStringAsync().Result;
    resultUpdateStudentString.Should().Be("Email already exists");
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
      RequestUri = new Uri("/Post/6"),
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
  public async Task UpdatePostWithoutToken()
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

    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri("/Post/3"),
      Content = new StringContent("{\"studentEmail\":\"paulo@gmail.com\"}", Encoding.UTF8, "application/json"),
    };
    //token for request
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //Delete Post
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
    var result = await _client.GetAsync("Post/Student/3");
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("[{\"postId\":4,\"content\":\"Texto 4\",\"creatAt\":\"2022-10-02T08:35:00\",\"updatetAt\":\"2022-10-05T08:35:00\",\"image\":null,\"studentId\":3},{\"postId\":5,\"content\":\"Texto 5\",\"creatAt\":\"2022-10-02T08:35:00\",\"updatetAt\":\"2022-10-06T08:35:00\",\"image\":null,\"studentId\":3}]");
  }

  [Fact]
  public async Task GetPostsByNonExitingStudentId()
  {
    var result = await _client.GetAsync("Post/Student/99");
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("Student not found");
  }

  [Fact]
  public async Task GetLastPostsByStudentId()
  {
    var result = await _client.GetAsync("Post/Last/Student/3");
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("{\"postId\":5,\"content\":\"Texto 5\",\"creatAt\":\"2022-10-02T08:35:00\",\"updatetAt\":\"2022-10-06T08:35:00\",\"image\":null,\"studentId\":3}");
  }

  [Fact]
  public async Task GetLastPostsByNonExitingStudentId()
  {
    var result = await _client.GetAsync("Post/Last/Student/99");
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("Student not found");
  }

  [Fact]
  public async Task GetPostsByStudentName()
  {
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri("/Post/StudentName"),
      Content = new StringContent("{\"name\":\"Tom\"}", Encoding.UTF8, "application/json"),
    };
    var result = await _client.SendAsync(request).ConfigureAwait(false);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("[{\"postId\":4,\"content\":\"Texto 4\",\"creatAt\":\"2022-10-02T08:35:00\",\"updatetAt\":\"2022-10-05T08:35:00\",\"image\":null,\"studentId\":3},{\"postId\":5,\"content\":\"Texto 5\",\"creatAt\":\"2022-10-02T08:35:00\",\"updatetAt\":\"2022-10-06T08:35:00\",\"image\":null,\"studentId\":3}]");
  }

  [Fact]
  public async Task GetPostByNonExistingStudentName()
  {
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri("/Post/StudentName"),
      Content = new StringContent("{\"name\":\"Tomy\"}", Encoding.UTF8, "application/json"),
    };
    var result = await _client.SendAsync(request).ConfigureAwait(false);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("Student not found");
  }

  [Fact]
  public async Task GetLastPostByStudentName()
  {
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri("/Post/Last/StudentName"),
      Content = new StringContent("{\"name\":\"Tom\"}", Encoding.UTF8, "application/json"),
    };
    var result = await _client.SendAsync(request).ConfigureAwait(false);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("{\"postId\":5,\"content\":\"Texto 5\",\"creatAt\":\"2022-10-02T08:35:00\",\"updatetAt\":\"2022-10-06T08:35:00\",\"image\":null,\"studentId\":3}");
  }

  [Fact]
  public async Task GetLastPostByNonExistingStudentName()
  {
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri("/Post/Last/StudentName"),
      Content = new StringContent("{\"name\":\"Tomy\"}", Encoding.UTF8, "application/json"),
    };
    var result = await _client.SendAsync(request).ConfigureAwait(false);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("Student not found");
  }

}
// export PATH = "$PATH:/home/tamires/.dotnet/tools"
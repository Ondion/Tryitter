using System.Net.Http.Headers;
using System.Text;
using tryitter.Services;
using tryitter.Models;

namespace tryitter.Test;

public class TestStudentController : IClassFixture<TestTryitterContext<Program>>
{
  private readonly HttpClient _client;
  public TestStudentController(TestTryitterContext<Program> factory)
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
    getAllStudentsResultString.Should().Be("[{\"studentId\":1,\"name\":\"Aninha\",\"email\":\"ana@gmail.com\",\"status\":\"Focada\"},{\"studentId\":2,\"name\":\"Paulo\",\"email\":\"paulo@gmail.com\",\"status\":\"Focada\"},{\"studentId\":3,\"name\":\"Tom\",\"email\":\"tom@gmail.com\",\"status\":\"Focada\"},{\"studentId\":4,\"name\":\"Joao\",\"email\":\"joao@gmail.com\",\"status\":\"Focada\"}]");
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

}
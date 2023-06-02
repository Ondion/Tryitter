using System.Net.Http.Headers;
using System.Text;
using tryitter.Services;
using tryitter.Models;
using Moq;
using tryitter.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using tryitter.Models;
using tryitter.Repository;


namespace tryitter.Test;

public class TestStudentController : IClassFixture<TestTryitterContext<Program>>
{
  private readonly HttpClient _client;
  // private readonly Mock<IStudentRepository> _repositoryMock;
  // private readonly string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiI4IiwibmFtZSI6IkFuYSIsImVtYWlsIjoiYW5hQGdtYWlsIiwibmJmIjoxNjg1NjIyMTYzLCJleHAiOjE2ODU4ODEzNjMsImlhdCI6MTY4NTYyMjE2M30.luQMd2NH755p7fDyda64Tl4-2BLm6YLUV-5p9Z9r6iw";

  // public TestTryitterContext<Program> _factory { get; }

  public TestStudentController(TestTryitterContext<Program> factory)
  {
    _client = factory.CreateClient();
    // _repositoryMock = new Mock<IStudentRepository>();

    // _client = factory.WithWebHostBuilder(builder =>
    // {
    //   builder.ConfigureServices(services =>
    //       {
    //         services.AddScoped<IStudentRepository>(st => _repositoryMock.Object);
    //       });
    // }).CreateClient();
  }

  [Fact]
  public async Task CreateStudent()
  {

    var student = new Student { Name = "Maria", Email = "maria@gmail.com", Password = "xft@ff", Status = "Focada" };
    //Create Student
    var jsonToAdd = "{\"name\":\"Maria\",\"email\":\"maria@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");

    // _repositoryMock.Setup(db => db.AddStudent(student)).Returns("student created");

    var result = await _client.PostAsync("/Student", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("student created");
    //Delete Maria in inMemory DB
    var token = new TokenGenerator().Generate(student);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    await _client.DeleteAsync("/Student/5");
    // var teste = await _client.GetAsync("/Student");
    // var resultAddSameUserTwiceString = teste.Content.ReadAsStringAsync().Result;
    // resultAddSameUserTwiceString.Should().Be("Email already exists");

  }

  [Fact]
  public async Task CreateStudentWithAExistingEmail()
  {
    //Create a Student with a exiting email in DB
    var jsonToAdd = "{\"name\":\"Tom\",\"email\":\"tom@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var result = await _client.PostAsync("/Student", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var resultString = result.Content.ReadAsStringAsync().Result;
    resultString.Should().Be("Email already exists");
    //Reset in memoryDB
    // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    // await _client.DeleteAsync("/Student/1");
  }

  [Fact]
  public async Task LoginWithAExistingStudent()
  {
    //login
    // var jsonToAddLogin = "{\"name\":\"Joao\",\"password\":\"xft@ff\"}";
    // var stringContentLogin = new StringContent(jsonToAddLogin, Encoding.UTF8, "application/json");
    // var resultLogin = await _client.PostAsync("/Login", stringContentLogin);
    // resultLogin.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    //Reset DB
    // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    // await _client.DeleteAsync("/Student/1");
  }

  [Fact]
  public async Task LoginWithANonExistingStudent()
  {
    //login
    var jsonToAddLoginWithError = "{\"name\":\"Carlos\",\"password\":\"xf\"}";
    var stringContentLogin = new StringContent(jsonToAddLoginWithError, Encoding.UTF8, "application/json");
    var resultLogin = await _client.PostAsync("/Login", stringContentLogin);
    resultLogin.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var resultLoginString = resultLogin.Content.ReadAsStringAsync().Result;
    resultLoginString.Should().Be("Student not found");
  }

  [Fact]
  public async Task GetAllStudents()
  {
    //Create 2 Students
    // var jsonToAdd = "{\"name\":\"Ana\",\"email\":\"an@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    // var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    // await _client.PostAsync("/Student", stringContent);

    // var jsonToAdd2 = "{\"name\":\"Paulo\",\"email\":\"paulo@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focado\"}";
    // var stringContent2 = new StringContent(jsonToAdd2, Encoding.UTF8, "application/json");
    // await _client.PostAsync("/Student", stringContent2);
    //login
    // var getAllStudentsResult = await _client.GetAsync("/Student");
    // getAllStudentsResult.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    // var getAllStudentsResultString = getAllStudentsResult.Content.ReadAsStringAsync().Result;
    // getAllStudentsResultString.Should().Be("[{\"studentId\":1,\"name\":\"Ana\",\"email\":\"an@gmail.com\",\"status\":\"Focada\"},{\"studentId\":2,\"name\":\"Paulo\",\"email\":\"paulo@gmail.com\",\"status\":\"Focado\"}]");
    //Reset DB
    // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    // await _client.DeleteAsync("/Student/1");
    // await _client.DeleteAsync("/Student/2");
  }

  // [Fact]
  // public async Task GetStudentWithACorrectId()
  // {
  //   //Create Student
  //   var jsonToAdd = "{\"name\":\"Maria\",\"email\":\"maria@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
  //   var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
  //   var result = await _client.PostAsync("/Student", stringContent);
  //   result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
  //   //GetStudentById
  //   var resultGetStudentById = await _client.GetAsync("Student/1");
  //   resultGetStudentById.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
  //   var resultGetStudentByIdString = resultGetStudentById.Content.ReadAsStringAsync().Result;
  //   resultGetStudentByIdString.Should().Contains("");
  //   //Reset DB
  //   _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  //   await _client.DeleteAsync("/Student/1");


  // }

  // [Fact]
  // public async Task GetStudentWithANonExintingId()
  // {
  //   //GetStudentByWrongId
  //   var resultgetStudentById = await _client.GetAsync("Student/99");
  //   resultgetStudentById.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
  //   var resultgetStudentByIdString = resultgetStudentById.Content.ReadAsStringAsync().Result;
  //   resultgetStudentByIdString.Should().Be("Student not found");
  // }

  // [Fact]
  // public async Task DeleteStudentWithACorrectId()
  // {
  //   //Create Student
  //   var jsonToAdd = "{\"name\":\"Ana\",\"email\":\"an@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
  //   var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
  //   var result = await _client.PostAsync("/Student", stringContent);
  //   result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
  //   //DeleteStudent
  //   _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  //   var resultDeleteStudent = await _client.DeleteAsync("Student/1");
  //   resultDeleteStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
  //   var resultDeleteStudentString = resultDeleteStudent.Content.ReadAsStringAsync().Result;
  //   resultDeleteStudentString.Should().Be("student remove");
  // }

  // [Fact]
  // public async Task DeleteStudentWithANonExitingId()
  // {
  //   //DeleteStudent
  //   _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  //   var resultDeleteStudent = await _client.DeleteAsync("Student/99");
  //   resultDeleteStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
  //   var resultDeleteStudentString = resultDeleteStudent.Content.ReadAsStringAsync().Result;
  //   resultDeleteStudentString.Should().Be("Student not found");
  // }

  // [Fact]
  // public async Task DeleteStudentWithoutToken()
  // {
  //   //DeleteStudent
  //   var resultDeleteStudent = await _client.DeleteAsync("Student/1");
  //   resultDeleteStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
  // }

  //!falta o update e o get com nome
}
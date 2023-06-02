using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Text;
using tryitter.Test;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



namespace tryitter.Test;

public class TestStudentController : IClassFixture<TestTryitterContext<Program>>
{
  private readonly HttpClient _client;

  public TestStudentController(TestTryitterContext<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task CreateStudentShouldReturnStatus200AndCorrectStringResponse()
  {
    var jsonToAdd = "{\"name\":\"Ana\",\"email\":\"x@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var result = await _client.PostAsync("/Student", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    var stringResult = result.Content.ReadAsStringAsync().Result;
    stringResult.Should().Be("student created");
    //Reset DB
    await _client.DeleteAsync("/Student/1");

  }
  [Fact]
  public async Task CreateStudentWithExistentEmailShouldReturnStatus400AndCorrectStringResponse()
  {
    var jsonToAdd = "{\"name\":\"Ana\",\"email\":\"an@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var result = await _client.PostAsync("/Student", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    //add same student twice
    var resultAddSameUserTwice = await _client.PostAsync("/Student", stringContent);
    resultAddSameUserTwice.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var resultAddSameUserTwiceString = resultAddSameUserTwice.Content.ReadAsStringAsync().Result;
    resultAddSameUserTwiceString.Should().Be("Email already exists");
    //Reset DB
    await _client.DeleteAsync("/Student/1");
  }
  [Fact]
  public async Task LoginWithAExistingStudent()
  {
    //Create user
    var jsonToAdd = "{\"name\":\"Ana\",\"email\":\"an@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var result = await _client.PostAsync("/Student", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    //login
    var jsonToAddLogin = "{\"name\":\"Ana\",\"password\":\"xft@ff\"}";
    var stringContentLogin = new StringContent(jsonToAddLogin, Encoding.UTF8, "application/json");
    var resultLogin = await _client.PostAsync("/Login", stringContentLogin);
    resultLogin.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    //Reset DB
    await _client.DeleteAsync("/Student/1");
  }
  [Fact]
  public async Task LoginWithANonExistingStudent()
  {
    //Create user
    var jsonToAdd = "{\"name\":\"Ana\",\"email\":\"an@gmail.com\",\"password\":\"xft@ff\",\"status\":\"Focada\"}";
    var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
    var result = await _client.PostAsync("/Student", stringContent);
    result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    //login
    var jsonToAddLoginWithError = "{\"name\":\"Ana\",\"password\":\"xf\"}";
    var stringContentLogin = new StringContent(jsonToAddLoginWithError, Encoding.UTF8, "application/json");
    var resultLogin = await _client.PostAsync("/Login", stringContentLogin);
    resultLogin.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    var resultLoginString = resultLogin.Content.ReadAsStringAsync().Result;
    resultLoginString.Should().Be("Student not found");
    //Reset DB
    await _client.DeleteAsync("/Student/1");
  }
}
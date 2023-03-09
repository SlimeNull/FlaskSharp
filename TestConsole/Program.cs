using FlaskSharp;
using System.Net;

MyFlaskApp app = new MyFlaskApp();

await app.RunAsync();   

class MyFlaskApp : FlaskApp
{
    [FlaskRoute("/api/get_age")]
    public string GetAge(int birthYear)
    {
        int age = DateTime.Now.Year - birthYear;
        return $"Your age is {age}";
    }
}
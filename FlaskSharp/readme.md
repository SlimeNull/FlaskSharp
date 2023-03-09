# FlaskSharp

Simple, Easy and Lightweight web framework.\
简单, 易用且轻量的 Web 框架.

## Usage

Create a new FlaskApp.

```csharp
class MyFlaskApp : FlaskApp
{
    // Use FlaskRoute attribute to handle request
    [FlaskRoute("/api/get_age")]
    public string GetAge(int birthYear)
    {
        int age = DateTime.Now.Year - birthYear;
        return $"Your age is {age}";
    }
}
```

Run the FlaskApp.

```csharp
MyFlaskApp app = new MyFlaskApp();

await app.RunAsync();  
```
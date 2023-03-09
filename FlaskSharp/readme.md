# FlaskSharp

Simple, Easy and Lightweight web framework.\
简单, 易用且轻量的 Web 框架.

## Usage

Create a new FlaskApp. \
创建一个新的 Flask 应用程序.

```csharp
class MyFlaskApp : FlaskApp
{
    // Use FlaskRoute attribute to handle request
    // 使用 FlaskRoute 特性来处理请求
    [FlaskRoute("/api/get_age")]
    public string GetAge(int birthYear)
    {
        int age = DateTime.Now.Year - birthYear;
        return $"Your age is {age}";
    }
}
```

Run the FlaskApp. \
启动 Flask 应用程序

```csharp
MyFlaskApp app = new MyFlaskApp();

await app.RunAsync();  
```
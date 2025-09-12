# WebBoost - Complex Binder for ASP.NET Core

**WebBoost** extends the **ASP.NET Core** model binding pipeline, providing additional features to handle **complex binds** directly in controller actions. It allows you to specify properties from `QueryString` or `Body` in a simplified way.  

---

## ✨ Motivation

**ASP.NET Core** already has a powerful `Model Binding` mechanism, but in scenarios where we need to **map complex parameters** partially from the query string and/or body, we often end up repeating manual mapping code inside actions or services.  

`WebBoost.Binders` introduces a new attribute called **`[ComplexBinder]`** that makes this process much easier.  

---

## 🚀 Key Features

- Create **complex binds** directly in controller parameters.  
- Support for **query strings** with multiple fields.  
- Automatic type conversion (`int`, `decimal`, `DateTime`, `enum`, etc).  
- Bind support for **JSON objects** (via `BindBody`).  
- Clear exceptions when properties cannot be found.  

---

## 📦 Installation

In the future, this will be published as a NuGet package:  

```powershell
dotnet add package WebBoost.Binders
```

For now, clone the repository and add the reference to your project:  

```powershell
git clone https://github.com/your-username/webboost-binders.git
```

---

## 🛠️ Usage

### 1. Example without ComplexBinder

```csharp
[HttpGet("search")]
public IActionResult Search(string id, string name)
{
    var model = new UserFilter
    {
        Id = id,
        Name = name
    };

    return Ok(model);
}
```

---

### 2. Example with `[ComplexBinder]`

```csharp
[HttpGet("search")]
public IActionResult Search([ComplexBinder("id", "name")] UserFilter filter)
{
    return Ok(filter);
}
```

Now the binder automatically builds the **`UserFilter` object** from the query string:

```
GET /search?id=123&name=Caio
```

Response:
```json
{
  "id": "123",
  "name": "Caio"
}
```

---

## 🔥 Advanced Example

Mixed binding from `query string` + `body` JSON:

```csharp
[HttpPost("register")]
public IActionResult Register(
    [ComplexBinder("id", "email")] User user,
    [FromBody] Address address)
{
    user.Address = address;
    return Ok(user);
}
```

---

## 🧰 Exception Handling

- `ComplexBindNotFoundPropertyException`: thrown when a specified property cannot be found on the model.  
- `InvalidOperationException`: thrown when the value cannot be converted to the target type.  

---

## 📜 License

MIT © 2025 - [Pedro Henrique Rodrigues Oliveira]  

# WebBoost - QueryBinder and HeaderBinder for ASP.NET Core

**WebBoost** extends the **ASP.NET Core Model Binding pipeline**, adding support for **custom binders** directly in action parameters.  
It allows you to map properties from `QueryString` or `Headers` in a simple way, avoiding repetitive code.

---

## ✨ Motivation

**ASP.NET Core** already provides a powerful *Model Binding* mechanism, but in many scenarios we need to map parameters from query strings or headers into model objects.  
This usually requires repetitive code inside controllers.  

`WebBoost.Binders` solves this with attributes like **`[QueryBinder]`** and **`[HeaderBinder]`**.

---

## 🚀 Features

- Direct binding from query string or headers to objects.  
- Automatic type conversion (`int`, `decimal`, `DateTime`, `enum`, etc).  
- Clear exceptions when a property is missing or cannot be converted.  

---

## 📦 Installation

In the future, this will be available as a NuGet package:

```powershell
dotnet add package WebBoost.Binders
```

For now, clone the repository and add it as a reference in your project:

```powershell
git clone https://github.com/PedroHenriqDev/WebBoost
```

---

## 🛠️ Usage

### Example model: `UserDto`

```csharp
namespace WebBoost.Test.Models
{
    public class UserDto
    {
        public int Id { get; set; }              // Can be bound via QueryBinder("Id")
        public string Name { get; set; } = "";   // Can be bound via QueryBinder("Name")
        public int Version { get; set; }         // Can be bound via HeaderBinder("Version")
        public string Email { get; set; } = "";  // Can be bound via QueryBinder("Email")
    }
}
```

---

### 1. Bind from **QueryString**

```csharp
[HttpPost("qs")]
public IActionResult BindUserByQs([QueryBinder("Id", "Name", "Email")] UserDto userDto)
{
    return Ok(userDto);
}
```

**Example request:**
```
POST /api/users/qs?Id=10&Name=Pedro&Email=pedro@email.com
```

**Response:**
```json
{
  "id": 10,
  "name": "Pedro",
  "version": 0,
  "email": "pedro@email.com"
}
```

---

### 2. Bind from **Header**

```csharp
[HttpPost("header")]
public IActionResult BindUserByHeader([HeaderBinder("Version")] UserDto userDto)
{
    return Ok(userDto);
}
```

**Example request with header:**
```
POST /api/users/header
Header: Version: 2
```

**Response:**
```json
{
  "id": 0,
  "name": "",
  "version": 2,
  "email": ""
}
```

---

## 🧰 Exception Handling

- `ComplexBindNotFoundPropertyException`: thrown when the property does not exist on the model.  
- `InvalidOperationException`: thrown when the value cannot be converted to the target type.  

---

## 📜 License

MIT © 2025 - [Pedro Henrique Rodrigues Oliveira]

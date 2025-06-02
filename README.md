# 🤖 DotBot

**DotBot** is an intelligent chatbot built with **ASP.NET Core** that allows users to log in securely, chat with AI models (OpenAI and Gemini), and save chat history by session—just like ChatGPT.

This project showcases advanced integration of technologies such as JWT authentication, Entity Framework, persistent conversation storage, and AI services.

---

## 🚀 Features

- 🔐 Secure login and JWT-based authentication
- 🧠 Support for two AI providers:
  - **OpenAI** (ChatGPT)
  - **Gemini** (Google)
- 💬 Conversations saved by session in the database
- 🗃️ Data persistence with Entity Framework and SQL Server
- 🧱 Scalable and clean architecture
- 🌐 RESTful API + Chat-like frontend interface

---

## 🧰 Tech Stack

- `ASP.NET Core 8`
- `Entity Framework Core`
- `JWT Bearer Authentication`
- `SQL Server`
- `OpenAI API`
- `Gemini API`
- `C#`
- `JS`
- `Blazor` and `Razor Pages`

---

## 🏗️ Project Structure

DotBot/
- `Controllers/`
- `Services/`
- `Models/`
- `SQL Server`
- `Data/`
- `wwwroot/`

---

## ⚙️ Configuration

### 🔧 Required Settings

You can configure these values in the `appsettings.json` file (only for local development) or as environment variables (recommended for production):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=DotBotDB;Integrated Security=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "https://localhost:7156",
    "Audience": "https://localhost:7156",
    "Subject": "DotBot"
  },
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY",
    "Endpoint": "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent"
  },
  "ChatGpt": {
    "ApiKey": "YOUR_OPENAI_API_KEY",
    "Endpoint": "https://api.openai.com/v1/chat/completions"
  }
}

```
```
## 🧱 Database Migrations

Using .NET CLI
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Using Visual Studio 2022

1. Open Package Manager Console
2. Run the following commands:
```powershell
Add-Migration InitialCreate
Update-Database
```
✅ Make sure the Default Project in the console is set to the main project.

## ▶️ Running the Project

Using .NET CLI
```bash
dotnet run
```
Using Visual Studio 2022
Press F5 or click on the Start Debugging button.

## 📬 Contact
Jordy Moreno Arias
📧 yordimorenoarias.11@gmail.com
🔗 [LinkedIn](https://www.linkedin.com/in/jordymorenoarias/)

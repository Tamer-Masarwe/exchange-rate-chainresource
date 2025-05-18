# Mize Exchange Rate ChainResource Assignment

This project demonstrates a generic `ChainResource<T>` system that retrieves exchange rate data from multiple sources using C# and .NET 8.

## ✅ Features

- Chain of Responsibility pattern
- In-memory caching (1-hour expiration)
- File system storage as JSON (4-hour expiration)
- Web API fallback via [Open Exchange Rates](https://openexchangerates.org/)
- Structured logging via `ILogger<T>`
- Configuration via `appsettings.json`

## 🔧 Requirements

- .NET 8 SDK
Free API key from https://openexchangerates.org/signup
Note: For assignment review purposes, I’ve temporarily included my API key in the appsettings.json file. This file will be removed after submission.

## 🔑 Setup
1. Clone the repository
2. Create `Config/appsettings.json` with your API key:

```json
{
  "APIKeys": {
    "AppId": "your_api_key_here",
    "BaseUrl": "https://openexchangerates.org/api/latest.json"
  }
}

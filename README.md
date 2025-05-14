<h1 align="center" style="margin-top: 0px; margin-bottom: 5px">Crypto Simulator</h1>

A .NET-based web API simulating cryptocurrency trading for learning purposes. Users can register, log in, buy/sell cryptocurrencies, view wallets, portfolios, transaction history, and calculate profits/losses with simulated real-time price updates.

## Disclaimer

This project is for educational purposes only and does not reflect real cryptocurrency trading platforms. I've got nothing to do with crypto trading.

## Features

- **User Management**: Register, log in, update, and delete accounts.
- **Wallet & Trading**: Track balances, buy/sell cryptocurrencies with real-time prices.
- **Portfolio & Profits**: View holdings, calculate realized/unrealized profits/losses.
- **Transaction History**: Record and retrieve all trades.
- **Authentication**: JWT-based with `User` and `Admin` roles.
- **Swagger UI**: Interactive API documentation.

## Technologies

- .NET 9.0
- Entity Framework Core 9.0.2
- SQL Server
- AutoMapper
- BCrypt.Net
- JWT
- Swagger/OpenAPI

## How to Use

1. **Clone Repository**:
   ```bash
   git clone https://github.com/Nazenano/crypto-simulator.git
   cd crypto-simulator
   ```

2. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

3. **Configure appsettings.json**:
   Update `appsettings.json` and `appsettings.Development.json`:
   ```json
   {
     "Jwt": {
       "Key": "YourSecretKeyHereAtLeast32CharactersLong",
       "Issuer": "CryptoSimulator",
       "Audience": "CryptoSimulator",
       "ExpireDays": 7
     },
     "ConnectionStrings": {
       "SQL": "Server=localhost;Database=CryptoSimulatorDb;Trusted_Connection=True;TrustServerCertificate=True;"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     }
   }
   ```
   - **Jwt:Key**: 32+ character secure key. ([generator website](https://jwtsecret.com/))
   - **ConnectionStrings:SQL**: Change connection string (e.g., `localhost\\SQLEXPRESS` for SQL Server Express).

4. **Set Up Database**:
   Apply migrations to create the database:
   ```bash
   dotnet ef database update
   ```

5. **Run Application**:
   ```bash
   dotnet watch --project CryptoSimulator
   ```
   Access at `http://localhost:5274` (check `launchSettings.json` for port) and explore endpoints via Swagger UI.

## Project Structure

- **CryptoSimulator**: API endpoints (`UserController`, `WalletController`, etc.).
- **CryptoSimulator.Services**: Business logic (`UserService`, `WalletService`, etc.).
- **CryptoSimulator.DataContext**: EF Core setup (`SQL.cs`, entities: `User`, `Wallet`, DTOs, migrations).

**Enjoy!**

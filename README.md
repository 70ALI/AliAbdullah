# AliAbdullah

## Requirements
- .NET SDK (8+)
- SQL Server (LocalDB or SQL Express)
- (Optional) SSMS

## Quick Start (create DB from code-first migrations)

```bash
# 1) Clone
git clone https://github.com/70ALI/AliAbdullah.git
cd AliAbdullah/AliAbdullah

# 2) Configure connection string
# Edit appsettings.json -> ConnectionStrings:DefaultConnection
# Examples:
# LocalDB:
# "Server=(localdb)\\MSSQLLocalDB;Database=AliAbdullah;Trusted_Connection=True;TrustServerCertificate=True;"
# SQL Express:
# "Server=.\\SQLEXPRESS;Database=AliAbdullah;Trusted_Connection=True;TrustServerCertificate=True;"

# 3) Restore & apply migrations (creates DB/tables)
dotnet restore
dotnet tool update --global dotnet-ef   # if dotnet-ef not installed
dotnet ef database update

# 4) Run
dotnet run

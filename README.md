
# AspireIntegration

This repository is a Aspire .NET solution for managing(import/export) Severa customers,

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Visual Studio 2022 (or later) or Visual Studio Code
- Access to Severa API credentials (Client ID and Client Secret)
- Docker desktop
- (Optional) PostgreSQL database if using a real database backend


## Getting Started

1. **Clone the repository:**
	```powershell
	git clone https://github.com/mkdotd/AspireIntegration.git
	cd AspireIntegration
	```
2. **Open the solution in Visual Studio**

3. **Run the project:**
	```powershell
	Set SeveraCustomers.AppHost as startup project to enable orchestration and run the project
	```

## Configuration

- Severa client ID and Severa client secret should be configured to access the severa services.


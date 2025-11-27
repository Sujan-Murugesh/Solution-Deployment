# Sujan Solution Deployer
# Power Platform Solution Deployment Tool

[![NuGet](https://img.shields.io/nuget/v/YourPackageName.svg)](https://www.nuget.org/packages/YourPackageName/)
[![License](https://img.shields.io/badge/license-GPL--3.0-blue.svg)](LICENSE)

A comprehensive solution deployment and management tool for Microsoft Power Platform that streamlines the process of deploying solutions across multiple environments with advanced features like version control, automated backups, and rollback capabilities.

## 🚀 Features

### Core Functionality
- **Multi-Environment Solution Loading**: Load both managed and unmanaged solutions from connected Power Platform environments
- **Batch Deployment**: Deploy single or multiple solutions to one or more target environments simultaneously
- **Target Environment Management**: Easily add, remove, and manage multiple target environments

### Version Control
- **Intelligent Version Increment**: Support for both manual and automatic version incrementation
- **Source Synchronization**: Update solution versions in the source environment automatically
- **Version History Tracking**: Maintain complete version history for audit and compliance

### Deployment Management
- **Real-Time Monitoring**: View live deployment logs and progress indicators
- **Deployment History**: Access complete deployment history with CSV export capability
- **Configurable Options**: Customize deployment settings per environment

### Backup & Recovery
- **Automatic Solution Backup**: Create backups before each deployment
- **Rollback/Downgrade**: Safely revert to previous solution versions when needed
- **Point-in-Time Recovery**: Restore solutions to any previous state

### Notifications
- **Email Notifications**: Automated deployment status notifications
- **SMTP Configuration**: Built-in SMTP setup with diagnostic tools
- **Custom Alerts**: Configure notification triggers and recipients

## 📋 Prerequisites

- .NET 4.6 or higher
- Microsoft Power Platform environment access
- Valid Power Platform credentials with appropriate permissions
- SMTP server details (optional, for email notifications)

## 🔧 Installation

### Via NuGet Package Manager
```bash
Install-Package SujanSolutionDeployer --version 1.0.0
```

### Via .NET CLI
```bash
dotnet add package SujanSolutionDeployer --version 1.0.0
```

### Via Package Manager Console
```powershell
NuGet\Install-Package SujanSolutionDeployer -Version 1.0.0
```

## 🎯 Quick Start

### 1. Connect to Your Environment
```csharp
var connectionString = "AuthType=OAuth;Url=https://yourorg.crm.dynamics.com;...";
var solutionManager = new SolutionDeploymentTool(connectionString);
```

### 2. Load Solutions
```csharp
// Load all solutions from the connected environment
var solutions = await solutionManager.LoadSolutionsAsync();

// Filter managed or unmanaged solutions
var managedSolutions = solutions.Where(s => s.IsManaged).ToList();
```

### 3. Configure Target Environments
```csharp
// Add target environment
solutionManager.AddTargetEnvironment(new TargetEnvironment 
{
    Name = "Production",
    Url = "https://prod.crm.dynamics.com",
    Credentials = credentials
});
```

### 4. Deploy Solutions
```csharp
var deploymentOptions = new DeploymentOptions 
{
    EnableEmailNotification = true,
    AutoBackup = true,
    VersionIncrement = VersionIncrementType.Patch
};

await solutionManager.DeployAsync(solutions, targetEnvironments, deploymentOptions);
```

## 📖 Detailed Usage

### Version Management

#### Automatic Version Increment
```csharp
var options = new DeploymentOptions 
{
    VersionIncrement = VersionIncrementType.Automatic,
    VersionIncrementRule = IncrementRule.Patch // Major, Minor, Patch, Build
};
```

#### Manual Version Increment
```csharp
await solutionManager.UpdateSolutionVersionAsync(
    solutionName: "MySolution",
    newVersion: "1.2.3.4"
);
```

### Backup Configuration

```csharp
var backupOptions = new BackupOptions 
{
    AutoBackup = true,
    BackupLocation = @"C:\Backups\Solutions",
    RetentionDays = 30
};

solutionManager.ConfigureBackup(backupOptions);
```

### Email Notifications

```csharp
var smtpConfig = new SmtpConfiguration 
{
    Host = "smtp.gmail.com",
    Port = 587,
    EnableSsl = true,
    Username = "your-email@domain.com",
    Password = "your-password",
    FromAddress = "noreply@domain.com"
};

solutionManager.ConfigureSmtp(smtpConfig);

// Test SMTP configuration
var diagnosticResult = await solutionManager.TestSmtpAsync();
```

### Rollback Solutions

```csharp
// List available backups
var backups = await solutionManager.GetBackupsAsync("MySolution");

// Rollback to specific version
await solutionManager.RollbackAsync(
    solutionName: "MySolution",
    targetVersion: "1.2.0.0",
    targetEnvironment: productionEnv
);
```

### Deployment History

```csharp
// Get deployment history
var history = await solutionManager.GetDeploymentHistoryAsync(
    startDate: DateTime.Now.AddMonths(-1),
    endDate: DateTime.Now
);

// Export to CSV
await solutionManager.ExportHistoryToCsvAsync(
    history, 
    outputPath: @"C:\Reports\deployment_history.csv"
);
```

## 🔍 Monitoring & Logging

The tool provides real-time deployment monitoring:

```csharp
solutionManager.OnDeploymentProgress += (sender, args) => 
{
    Console.WriteLine($"Progress: {args.PercentComplete}%");
    Console.WriteLine($"Current Step: {args.CurrentStep}");
    Console.WriteLine($"Status: {args.Status}");
};

solutionManager.OnDeploymentLog += (sender, args) => 
{
    Console.WriteLine($"[{args.Timestamp}] {args.Level}: {args.Message}");
};
```

## ⚙️ Configuration

### appsettings.json Example
```json
{
  "PowerPlatform": {
    "SourceEnvironment": {
      "Url": "https://dev.crm.dynamics.com",
      "AuthType": "OAuth"
    },
    "DefaultDeploymentOptions": {
      "AutoBackup": true,
      "EmailNotification": true,
      "VersionIncrement": "Patch"
    }
  },
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "EnableSsl": true
  },
  "Backup": {
    "Location": "./Backups",
    "RetentionDays": 30
  }
}
```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the GPL-3.0 License - see the [LICENSE](LICENSE) file for details.

## 🐛 Known Issues

- Large solution deployments (>100MB) may require increased timeout settings
- Email notifications require proper SMTP server configuration

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/Sujan-Murugesh/Solution-Deployment/issues)
- **Discussions**: [GitHub Discussions](https://github.com/Sujan-Murugesh/Solution-Deployment/discussions)
- **Email**: murugeshsujan22@gmail.com

## 🙏 Acknowledgments

- Microsoft Power Platform team for the excellent SDK

## 📊 Roadmap

- [ ] Support for Power Platform Pipelines integration
- [ ] Advanced solution dependency analysis
- [ ] Multi-tenant deployment support
- [ ] Integration with Azure DevOps
- [ ] Enhanced reporting and analytics dashboard

---

**Made with ❤️ for the Power Platform Community**

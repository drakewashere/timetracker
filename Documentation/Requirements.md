# Technologies Used
- Windows 10 (Tested with Home Edition)
- .Net 6.0 (LTS)
- Sql Server (Tested with v15 LTS)
- Docker Desktop

# Nuget Libraries Used
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore Version=6.0.10
- Microsoft.AspNetCore.Identity.EntityFrameworkCore Version=6.0.10
- Microsoft.AspNetCore.Identity.UI Version=6.0.10
- Microsoft.EntityFrameworkCore.SqlServer Version=6.0.10
- Microsoft.EntityFrameworkCore.Tools Version=6.0.10
- Microsoft.VisualStudio.Azure.Containers.Tools.Targets Version=1.17.0

# DB Setup
- Create a SQL Server login with dbcreate permissions
- Set the location, port if needed, User Id, and Password for the created account
- In Package Manager console, run the following command to create database structure
```
Update-Database
```
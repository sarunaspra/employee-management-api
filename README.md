# employee-management-api
ASP.NET Core three-tier architecture Web Api project.

# Layers
- SP.EmployeeManagement.Api - Presentation layer (Asp.Net Core Web API project);
- SP.EmployeeManagement.BusinessLogic - Business logic layer responsible for data exchange between data access and presentation layers. Consists of:
  - Services;
  - AutomapperProfiles;
  - Custom exception classes;
- SP.EmployeeManagement.DataAccess - Data access layer handles data retrieval and storage operations, interacting with the database. Consists of:
  - Unit of work;
  - Database context;
  - Repositories;
  - Database entity models.
- Additional class libs:
  - SP.EmployeeManagement.Dto - Data transfer objects are added here;
  - SP.EmployeeManagement.Test - Unit tests are added here.

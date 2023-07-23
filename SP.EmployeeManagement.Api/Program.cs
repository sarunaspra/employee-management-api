using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SP.EmployeeManagement.BusinessLogic.AutoMapperProfiles;
using SP.EmployeeManagement.BusinessLogic.Services;
using SP.EmployeeManagement.BusinessLogic.Services.IServices;
using SP.EmployeeManagement.BusinessLogic.Validators;
using SP.EmployeeManagement.DataAccess;
using SP.EmployeeManagement.DataAccess.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(EmployeeProfile),
    typeof(DepartmentProfile), 
    typeof(PositionProfile));

builder.Services.AddValidatorsFromAssemblyContaining<EmployeeDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DepartmentDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PositionDtoValidator>();

builder.Services.Configure<UnitOfWorkOptions>(
    builder.Configuration.GetSection(nameof(UnitOfWorkOptions)));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPositionService, PositionService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

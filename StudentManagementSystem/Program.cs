using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Repositories.AuthRepositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StudentManagementSystem.Repositories.DepartmentRepositories;
using StudentManagementSystem.Repositories.ClassesRepositories;
using StudentManagementSystem.Repositories.SubjectsRepositories;
using StudentManagementSystem.Repositories.AcademicYearsRepository;
using StudentManagementSystem.Repositories.SectionsRepositories;
using StudentManagementSystem.Repositories.TeachersRepositories;
using StudentManagementSystem.Repositories.StudentsRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//add for db connection
builder.Services.AddDbContext<StudentDBContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//for IdentyFramework
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<StudentDBContext>()
    .AddDefaultTokenProviders();

//Dependencies
builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddTransient<IClassesRepository, ClassesRepository>();
builder.Services.AddTransient<ISubjectsRepository, SubjectsRepository>();
builder.Services.AddTransient<IYearsRepository, YearsRepository>();
builder.Services.AddTransient<ISectionsRepository, SectionsRepository>();
builder.Services.AddTransient<ITeachersRepository, TeachersRepository>();
builder.Services.AddTransient<IStudentsRepository, StudentsRepository>();

//jwt
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.RequireHttpsMetadata = true;
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
    };
});

builder.Services.AddControllers();

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

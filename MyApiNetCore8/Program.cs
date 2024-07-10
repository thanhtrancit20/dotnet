using System.Net;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using MyApiNetCore8.Data;
using MyApiNetCore8.Repositories;
using MyApiNetCore8.Repositories.impl;
using dotenv.net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyApiNetCore8.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyApiNetCore8.Services;
using MyApiNetCore8.Services.impl;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                }); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod()
           );
});
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowReactApp",
//        builder => builder.WithOrigins("http://localhost:3000")
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//            .AllowCredentials());
//});


builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<MyContext>().AddDefaultTokenProviders();

builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("dotnetStore"),
                   new MySqlServerVersion(new Version(8, 0, 26)),
                   mysqlOptions =>
                   {
                       mysqlOptions.EnableRetryOnFailure(
                           maxRetryCount: 5,
                           maxRetryDelay: TimeSpan.FromSeconds(5),
                           errorNumbersToAdd: null);
                   });
});

// Configuring AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
// Configuring Repositories
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITaskService, TaskService>();

//Configuring Cloudinary
DotEnv.Load(new DotEnvOptions(probeForEnv: true));
Account account = new Account(
  "drsxhbpwp",
  "552366933472957",
  "JXjOqMvW5-IE6BAPH9sGAF5wc68");
Cloudinary cloudinary = new Cloudinary(account);

// Register the Singleton instance of Cloudinary
builder.Services.AddSingleton(cloudinary);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    //options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new
        Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
        (builder.Configuration["JWT:Secret"]))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();

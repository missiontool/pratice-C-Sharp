using EmployeeApi.Data;
using Microsoft.EntityFrameworkCore;
using EmployeeApi.Services; // 引用Service

var builder = WebApplication.CreateBuilder(args);

// 註冊AppDbContext
// 設定使用SQL Server，連線字串從appsettings.json拿取
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// AddScoped 意思是：每個 HTTP 請求都會產生一個新的 Service 實體
// 註冊 Service
builder.Services.AddScoped<IEmployeeService, EmployeeService>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

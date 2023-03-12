using System;
using BookkeeperRest.New.Email;
using BookkeeperRest.New.Repositories;
using BookkeeperRest.New.Services;

Console.WriteLine("Starting Bookkeeper Rest API.");
Console.WriteLine("Demo mode is: " + Environment.GetEnvironmentVariable("ASPNETCORE_IS_DEMO"));

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

EmailConfig emailConfig;

if (builder.Environment.IsDevelopment())
{
    emailConfig = builder.Configuration.GetSection("EmailConfig").Get<EmailConfig>();
}
else
{
    emailConfig = new()
    {
        Name = Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_NAME") ?? "",
        Address = Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_ADDRESS") ?? "",
        SmtpServer = Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_SMTPSERVER") ?? "",
        Port = Int32.Parse(Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_PORT") ?? "0"),
        UserName = Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_USERNAME") ?? "",
        Password = Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_PASSWORD") ?? "",
        EmailEnabled = (Environment.GetEnvironmentVariable("ASPNETCORE_IS_DEMO") ?? "true") == "false"
    };
}

builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IUserSettingService, UserSettingService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDenominationService, DenominationService>();



builder.Services.AddScoped<IEarningCategoryRepository, EarningCategoryRepository>();
builder.Services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
builder.Services.AddScoped<IDenominationRepository, DenominationRepository>();
builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
builder.Services.AddScoped<IUserSettingRepository, UserSettingRepository>();
builder.Services.AddScoped<IEarningRepository, EarningRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IDenominationRepository, DenominationRepository>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "All",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("All");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
  app.UseHttpsRedirection();
}

// app.UseAuthorization();

app.MapControllers();

app.Run();
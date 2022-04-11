using BookkeeperRest.Repositories.PasswordRepository;
using BookkeeperRest.Security.PasswordHasher;
using BookkeeperRest.Repositories.TransactionRepository;
using BookkeeperRest.Services.TransactionService;
using BookkeeperRest.Services.PasswordService;
using BookkeeperRest.Models.Transaction;
using BookkeeperRest.Services;
using BookkeeperRest.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddSingleton<IPasswordRepository, PasswordRepository>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<ITransactionService, TransactionService>();
builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
builder.Services.AddSingleton<ITransactionConverter, TransactionConverter>();
builder.Services.AddSingleton<IDenominationService, DenominationService>();
builder.Services.AddSingleton<IDenominationRepository, DenominationRepository>();
builder.Services.AddSingleton<ITransactionTypeRepository, TransactionTypeRepository>();
builder.Services.AddSingleton<ITransactionTypeService, TransactionTypeService>();


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

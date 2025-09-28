using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MSCustomer.Application.Services;
using MSCustomer.Infrastructure.Data;
using MSCustomer.Infrastructure.Repositories;
using MSCustomer.Application.DTOs;
using MS.Customer.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<IValidator<CreateCustomerDto>, CreateCustomerDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateCustomerDto>, UpdateCustomerDtoValidator>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// HTTP Client para comunicación entre microservicios
builder.Services.AddHttpClient();

//Se habilita desde cualquier origen para pruebas
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// prueba, permitir desde todos los origenes
app.UseCors("AllowAll");


app.UseAuthorization();
app.MapControllers();

app.Run();
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.ICampaignTypeServices;
using Application.Interfaces.IServices.IClientServices;
using Application.Interfaces.IServices.IInteractionTypeServices;
using Application.Interfaces.IServices.IProjectServices;
using Application.Interfaces.IServices.ITaskStatusServices;
using Application.Interfaces.IServices.IUserServices;
using Application.Interfaces.IValidator;
using Application.Models;
using Application.Request;
using Application.UseCase.CampaignTypesServices;
using Application.UseCase.ClientServices;
using Application.UseCase.InteractionTypeServices;
using Application.UseCase.ProjectServices;
using Application.UseCase.TaskStatusServices;
using Application.UseCase.UserServices;
using Application.Validators;
using FluentValidation;

using Infrastructure.Command;
using Infrastructure.Persistence;
using Infrastructure.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Marketing CRM", Version = "1.0" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

//Custom
var connectionString = builder.Configuration["ConnectionStrings"];
builder.Services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(connectionString));
builder.Services.AddScoped<IUserGetServices,UserGetServices>();
builder.Services.AddScoped<ITaskStatusGetServices,TaskStatusGetServices>();
builder.Services.AddScoped<IInteractionTypeGetServices, InteractionTypeGetServices>();
builder.Services.AddScoped<ICampaignTypeGetServices, CampaignTypeGetServices>();

builder.Services.AddScoped<IClientGetServices, ClientGetServices>();
builder.Services.AddScoped<IClientPostServices, ClientPostServices>();

builder.Services.AddScoped<IProjectGetServices,ProjectGetServices>();
builder.Services.AddScoped<IProjectPostServices,ProjectPostServices>();
builder.Services.AddScoped<IProjectPatchServices,ProjectPatchServices>();
builder.Services.AddScoped<IProjectPutServices,ProjectPutServices>();


builder.Services.AddScoped<IUserQuery,UserQuery>();
builder.Services.AddScoped<ICampaignTypeQuery,CampaignTypeQuery>();
builder.Services.AddScoped<IInteractionTypeQuery,InteractionTypeQuery>();
builder.Services.AddScoped<ITaskStatusQuery,TaskStatusQuery>();
builder.Services.AddScoped<IClientQuery,ClientQuery>();
builder.Services.AddScoped<IProjectQuery,ProjectQuery>();
builder.Services.AddScoped<ITaskQuery, TaskQuery>();
builder.Services.AddScoped<IInteractionQuery,InteractionQuery>();


builder.Services.AddScoped<IClientCommand, ClientCommand>();
builder.Services.AddScoped<IProjectCommand, ProjectCommand>();

builder.Services.AddValidatorsFromAssemblyContaining<ProjectsRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TasksRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<InteractionsRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ClientsRequestValidator>();

builder.Services.AddScoped<IValidatorHandler<ProjectRequest>, ValidatorHandler<ProjectRequest>>();
builder.Services.AddScoped<IValidatorHandler<TasksRequest>, ValidatorHandler<TasksRequest>>();
builder.Services.AddScoped<IValidatorHandler<InteractionsRequest>, ValidatorHandler<InteractionsRequest>>();
builder.Services.AddScoped<IValidatorHandler<ClientsRequest>, ValidatorHandler<ClientsRequest>>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Marketing CRM v1.0");
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();


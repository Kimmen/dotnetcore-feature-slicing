using FluentValidation;

using Kimmen.FeatureSlicing.Api.Web.Endpoints;
using Kimmen.FeatureSlicing.Api.Web.Features;
using Kimmen.FeatureSlicing.Api.Web.Shared.Model;
using Kimmen.FeatureSlicing.Api.Web.Shared.Validation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddValidatorsFromAssemblyContaining<AddTeacherValidator>();
builder.Services.AddMediatR(o =>
{
    //Use any public type in Application to register all handlers
    o.RegisterServicesFromAssemblyContaining(typeof(ValidationBehavior<,>));

    //Use the validation behavior, so I know the input is validated before hitting the handler.
    o.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

//Note: Let the classroom instance exists while the application is alive. 
builder.Services.AddSingleton<Classroom<NamedTeacher, NamedStudent>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapClassroomEndpoints();

app.Run();
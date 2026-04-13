using GoodsApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConnectionCreate();
builder.Services.AddJwtAuthentication();
builder.Services.AddAutoMapperConfiguration();
builder.AddAuthorization();

builder.RegisterServices();

builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(8080));

var app = builder.Build();

app.MappingEndpoints();

app.Run();
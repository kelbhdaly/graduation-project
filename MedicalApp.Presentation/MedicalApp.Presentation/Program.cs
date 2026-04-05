var builder = WebApplication.CreateBuilder(args);
#region Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddExceptionHandler<CustomExceptionHandlerMiddleWare>();
builder.Services.AddProblemDetails();
builder.Services.AddIdentityRegisteration(builder.Configuration);
builder.Services.AddBusinessLogicServices();
builder.Services.AddUtilities();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddJwtRegistration(builder.Configuration);
#endregion

var app = builder.Build();

//Database seeding
await app.AddSeedDataBaseAsync();

#region Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run(); 
#endregion

using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ShoppingAPI.Services.Service>();
builder.Services.AddAuthentication( //Enable Authentication Process
    opt =>
    {
        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; //DefaultScheme → sets default authentication scheme  //JwtBearerDefaults.AuthenticationScheme → "Bearer"
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //DefaultChallengeScheme → sets default challenge scheme (used when authentication fails)
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //DefaultAuthenticateScheme → sets default authentication scheme (used when authenticating requests)
    }).AddJwtBearer(opt => //AddJwtBearer → adds JWT Bearer authentication handler
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]); //Encoding.UTF8.GetBytes(...) → converts string → bytes  //builder.Configuration["Jwt:Key"] → reads secret key
        opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters //TokenValidationParameters → sets parameters for validating JWT tokens
        {
            ValidateIssuer = true, //ValidateIssuer → validates the issuer of the token
            ValidateAudience = true, //ValidateAudience → validates the audience of the token
            ValidateLifetime = true,//  ValidateLifetime → validates the expiration time of the token
            ValidateIssuerSigningKey = true, //     ValidateIssuerSigningKey → validates the signing key of the token

            ValidIssuer = builder.Configuration["Jwt:Issuer"], //ValidIssuer → specifies the expected issuer of the token  //builder.Configuration["Jwt:Issuer"] → reads expected issuer
            ValidAudience = builder.Configuration["Jwt:Audience"], //ValidAudience → specifies the expected audience of the token  //builder.Configuration["Jwt:Audience"] → reads expected audience
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key) //IssuerSigningKey → specifies the signing key to validate the token  //new SymmetricSecurityKey(key) → creates symmetric security key from byte array
        };
    });
builder.Services.AddAuthorization(); //Enable Authorization Process
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthentication(); //UseAuthentication → enables authentication middleware (must be called before UseAuthorization)
app.UseAuthorization();//UseAuthorization → enables authorization middleware (must be called after UseAuthentication)

app.MapControllers();

app.Run();

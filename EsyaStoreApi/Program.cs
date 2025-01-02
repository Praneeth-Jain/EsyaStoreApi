using System.Text;
using EsyaStore.Data.Context;
using EsyaStoreApi.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                string ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
                options.UseSqlServer(ConnectionString);
            }
           );
builder.Services.AddScoped<TokenGenerator>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
            RoleClaimType="UserType"
        };
    }
    );

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policy => policy.RequireClaim("UserType", "User"));
    options.AddPolicy("SellerPolicy", policy => policy.RequireClaim("UserType", "Seller"));
    options.AddPolicy("UsernSellerPolicy", policy => policy.RequireAssertion(c=>
    c.User.HasClaim(c=>
    c.Type=="UserType" && c.Value == "User" || c.Value == "Seller")));
});


var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

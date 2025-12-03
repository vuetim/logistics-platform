using LogisticsPlatform.Application.Authorization;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Repositories.Queries;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Application.Services;
using LogisticsPlatform.Infrastructure.Persistence;
using LogisticsPlatform.Infrastructure.Persistence.Repositories.Queries;
using LogisticsPlatform.Infrastructure.Repositories;
using LogisticsPlatform.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Controllers
builder.Services.AddControllers();

// 2. DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// 3. Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<ICustomerContactRepository, CustomerContactRepository>();
builder.Services.AddScoped<ICustomerNoteRepository, CustomerNoteRepository>();
builder.Services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
builder.Services.AddScoped<ICarrierRepository, CarrierRepository>();
builder.Services.AddScoped<ICarrierContactRepository, CarrierContactRepository>();
builder.Services.AddScoped<ICarrierAddressRepository, CarrierAddressRepository>();
builder.Services.AddScoped<ICarrierNoteRepository, CarrierNoteRepository>();
builder.Services.AddScoped<ICarrierDocumentRepository, CarrierDocumentRepository>();
builder.Services.AddScoped<ICustomerQueryRepository, CustomerQueryRepository>();
builder.Services.AddScoped<ICarrierQueryRepository, CarrierQueryRepository>();
builder.Services.AddScoped<ICarrierDocumentQueryRepository, CarrierDocumentQueryRepository>();
builder.Services.AddScoped<ILoadQueryRepository, LoadQueryRepository>();
builder.Services.AddScoped<ILoadRepository, LoadRepository>();
builder.Services.AddScoped<ILoadStopRepository, LoadStopRepository>();
builder.Services.AddScoped<ILoadEquipmentRepository, LoadEquipmentRepository>();
builder.Services.AddScoped<ILoadDocumentRepository, LoadDocumentRepository>();
builder.Services.AddScoped<ILoadNoteRepository, LoadNoteRepository>();
builder.Services.AddScoped<IOrderQueryRepository, OrderQueryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderRouteRepository, OrderRouteRepository>();










// 4. Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICustomerNoteService, CustomerNoteService>();
builder.Services.AddScoped<ICustomerContactService, CustomerContactService>();
builder.Services.AddScoped<ICustomerAddressService, CustomerAddressService>();
builder.Services.AddScoped<ICarrierService, CarrierService>();
builder.Services.AddScoped<ICarrierContactService, CarrierContactService>();
builder.Services.AddScoped<ICarrierAddressService, CarrierAddressService>();
builder.Services.AddScoped<ICarrierNoteService, CarrierNoteService>();
builder.Services.AddScoped<ICarrierDocumentService, CarrierDocumentService>();
builder.Services.AddScoped<ICustomerQueryService, CustomerQueryService>();
builder.Services.AddScoped<ICarrierQueryService, CarrierQueryService>();
builder.Services.AddScoped<ICarrierDocumentQueryService, CarrierDocumentQueryService>();
builder.Services.AddScoped<ILoadQueryService, LoadQueryService>();
builder.Services.AddScoped<ILoadService, LoadService>();
builder.Services.AddScoped<ILoadStopService, LoadStopService>();
builder.Services.AddScoped<ILoadEquipmentService, LoadEquipmentService>();
builder.Services.AddScoped<ILoadDocumentService, LoadDocumentService>();
builder.Services.AddScoped<ILoadNoteService, LoadNoteService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderQueryService, OrderQueryService>();
builder.Services.AddScoped<IOrderRouteService, OrderRouteService>();














// 5. JWT Authentication
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// 6. Swagger + JWT Support (Correct Version)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LogisticsPlatform API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insert JWT token like: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();

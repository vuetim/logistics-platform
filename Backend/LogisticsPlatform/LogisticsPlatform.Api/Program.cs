using Hangfire;
using LogisticsPlatform.Application.BackgroundJobs;
using LogisticsPlatform.Application.DTOs.Common;
using LogisticsPlatform.Application.Interfaces.Repositories;
using LogisticsPlatform.Application.Interfaces.Repositories.Carriers;
using LogisticsPlatform.Application.Interfaces.Repositories.Customers;
using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Repositories.Orders;
using LogisticsPlatform.Application.Interfaces.Repositories.Queries;
using LogisticsPlatform.Application.Interfaces.Repositories.Security;
using LogisticsPlatform.Application.Interfaces.Repositories.Users;
using LogisticsPlatform.Application.Interfaces.Services;
using LogisticsPlatform.Application.Interfaces.Services.ActivityLog;
using LogisticsPlatform.Application.Interfaces.Services.Auth;
using LogisticsPlatform.Application.Interfaces.Services.Carriers;
using LogisticsPlatform.Application.Interfaces.Services.Customers;
using LogisticsPlatform.Application.Interfaces.Services.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Orders;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Application.Interfaces.Services.Users;
using LogisticsPlatform.Application.Jobs;
using LogisticsPlatform.Application.Options;
using LogisticsPlatform.Application.Security;
using LogisticsPlatform.Application.Services;
using LogisticsPlatform.Application.Services.Financial;
using LogisticsPlatform.Application.Services.Orders;
using LogisticsPlatform.Application.Services.Users;
using LogisticsPlatform.Infrastructure.Persistence;
using LogisticsPlatform.Infrastructure.Persistence.Repositories.Queries;
using LogisticsPlatform.Infrastructure.Repositories;
using LogisticsPlatform.Infrastructure.Repositories.Financial;
using LogisticsPlatform.Infrastructure.Repositories.Security;
using LogisticsPlatform.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(x =>
    x.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddHangfireServer();

//QUESTPDF LICENSE
QuestPDF.Settings.License = LicenseType.Community;

// 1. Add Controllers
builder.Services.AddControllers();

// 2. DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);
//email
builder.Services.Configure<SmtpOptions>(
    builder.Configuration.GetSection("Smtp"));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200" // Angular dev
                                        // 
                                        // "https://app.logisticsplatform.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


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
builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
builder.Services.AddScoped<IActivityLogQueryRepository, ActivityLogQueryRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<ILoadItemRepository, LoadItemRepository>();
builder.Services.AddScoped<IOrderCostRepository, OrderCostRepository>();
builder.Services.AddScoped<ILoadCostRepository, LoadCostRepository>();
builder.Services.AddScoped<IOrderEquipmentRequirementRepository, OrderEquipmentRequirementRepository>();
// Financial Repositories
builder.Services.AddScoped<ICustomerInvoiceRepository, CustomerInvoiceRepository>();
builder.Services.AddScoped<ICarrierSettlementRepository, CarrierSettlementRepository>();
builder.Services.AddScoped<ILoadFinancialSnapshotRepository, LoadFinancialSnapshotRepository>();
//Carrier Assignment
builder.Services.AddScoped<ILoadCarrierAssignmentRepository, LoadCarrierAssignmentRepository>();
//carrier performance 
builder.Services.AddScoped<ICarrierPerformanceRepository, CarrierPerformanceRepository>();
builder.Services.AddScoped<ILoadAlertRepository, LoadAlertRepository>();
builder.Services.AddHostedService<HangfireStartupJob>();
// Delay fault (system)
builder.Services.AddScoped<IDelayFaultAttributionService, DelayFaultAttributionService>();
//Delay responsibility(manual)

builder.Services.AddScoped<IDelayResponsibilityService, DelayResponsibilityService>();

builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IAuthAuditLogRepository, AuthAuditLogRepository>();
builder.Services.AddScoped<IUserQueryRepository, UserQueryRepository>();








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
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderQueryService, OrderQueryService>();
builder.Services.AddScoped<IOrderRouteService, OrderRouteService>();
builder.Services.AddScoped<ILoadStatusCalculatorService, LoadStatusCalculatorService>();
builder.Services.AddScoped<ILoadStopExecutionService, LoadStopExecutionService>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<ILoadItemService, LoadItemService>();
builder.Services.AddScoped<IOrderCostService, OrderCostService>();
builder.Services.AddScoped<ILoadCostService, LoadCostService>();
builder.Services.AddScoped<IOrderEquipmentRequirementService, OrderEquipmentRequirementService>();
// Financial Services
builder.Services.AddScoped<ICustomerInvoiceService, CustomerInvoiceService>();
builder.Services.AddScoped<ICarrierSettlementService, CarrierSettlementService>();
builder.Services.AddScoped<ILoadFinancialSnapshotService, LoadFinancialSnapshotService>();
builder.Services.AddScoped<ILoadFinancialAutomationService, LoadFinancialAutomationService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IEmailService, EmailService>();

//pdf 
builder.Services.AddScoped<IPdfService, PdfService>();
// Carrier Assignment 
builder.Services.AddScoped<ICarrierAssignmentService, CarrierAssignmentService>();
//carrierperformance 
builder.Services.AddScoped<ICarrierPerformanceService, CarrierPerformanceService>();
//carrier score perfromance analytics 
builder.Services.AddScoped<CarrierScoreCardService>();

builder.Services.AddScoped<IEtaPredictionService, EtaPredictionService>();
builder.Services.AddScoped<EtaMonitoringJob>();
builder.Services.AddScoped<ILoadAlertService, LoadAlertService>();
// Delay fault (system)
builder.Services.AddScoped<ILoadDelayResponsibilityRepository, LoadDelayResponsibilityRepository>();

//Delay responsibility(manual)
builder.Services.AddScoped<IDelayResponsibilityRepository, DelayResponsibilityRepository>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IAuthAuditService, AuthAuditService>();
//users
builder.Services.AddScoped<IUserManagementService, UserManagementService>();

builder.Services.AddScoped<IPermissionReadModel, PermissionReadModel>();
builder.Services.AddScoped<IPermissionService, PermissionService>();


builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();







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
//app.UseHangfireDashboard("/hangfire");
app.UseCors("AllowFrontend");
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
//  ETA background job


app.Run();

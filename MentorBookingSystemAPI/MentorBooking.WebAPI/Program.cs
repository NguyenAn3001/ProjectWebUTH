using MentorBooking.Repository.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using MentorBooking.Repository.Entities;
using MentorBooking.Service.Services;
using MentorBooking.Service.Interfaces;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Repository.Repositories;
using MentorBooking.Service.AutoMapper;
using MentorBooking.Service.DTOs.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IMentorProfilesService, MentorProfilesService>();
builder.Services.AddScoped<IStudentProfilesServices,StudentProfilesServices>();
builder.Services.AddScoped<IWorkSchedulesView, WorkSchedulesView>();
builder.Services.AddScoped<IAcceptBookingSession, AcceptBookingSession>();
builder.Services.AddScoped<IMentorFeedbackRepository,MentorFeedbackRepository>();
builder.Services.AddScoped<IMentorSupportSessionRepository,MentorSupportSessionRepository>();
builder.Services.AddScoped<IMentorWorkScheduleRepository,MentorWorkScheduleRepository>();
builder.Services.AddScoped<IBookingMentorService, BookingMentorService>();
builder.Services.AddScoped<IMentorFeedbackService, MentorFeedbackService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticationHandler>();
builder.Services.AddScoped<ISearchAndSortService, SearchAndSortService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserTokenRepository, UserTokenRepository>();
builder.Services.AddScoped<IUpdateInformationService, UpdateInformationHandler>();
builder.Services.AddScoped<IUserInformationFactory, UserInformationFactory>();
builder.Services.AddScoped<IUserInformationUpdate, MentorInfoUpdateService>();
builder.Services.AddScoped<IUserInformationUpdate, StudentInfoUpdateService>();
builder.Services.AddScoped<IMentorRepository, MentorRepository>();
builder.Services.AddScoped<IMentorSkillRepository, MentorSkillRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IUserPointRepository, UserPointRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IImageUploadService, ImageUploadService>();
builder.Services.AddTransient<IImageRepository, ImageRepository>();
builder.Services.AddTransient<ISenderEmail, SenderEmail>();
builder.Services.AddTransient<IConfirmEmailRepository, ConfirmEmailRepository>();
builder.Services.AddScoped<ISchedulesMentor, SchedulesMentor>();
builder.Services.AddScoped<ISchedulesAvailableRepository, SchedulesAvailableRepository>();
builder.Services.AddScoped<IProjectGroupRepository, ProjectGroupRepository>();
builder.Services.AddScoped<IStudentGroupRepository, StudentGroupRepository>();
builder.Services.AddScoped<IGroupOfStudentService, GroupOfStudentService>();
builder.Services.AddScoped<IProjectProgressRepository, ProjectProgressRepository>();
builder.Services.AddSingleton<Dictionary<Guid, List<Guid>>>();
builder.Services.AddScoped<IProjectProgressService, ProjectProgressService>();
builder.Services.AddScoped<IUserPointRepository, UserPointRepository>();
builder.Services.AddScoped<IPointService, PointService>();
// builder.Services.AddScoped<IMentorServices, MentorServices>();
// Add Identity
builder.Services.AddCors(o => o.AddPolicy("MyCors", builder =>
 {
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));
builder.Services.AddIdentity<Users, Roles>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
// DI Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection"));
});
// Add Jwt Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        RoleClaimType = ClaimTypes.Role
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Auth Demo Api enabled with JWT Bearer",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    AddSwaggerOAuth2Configuration(c);

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        //your additional stuff...
        c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> {{ "nonce", "anyNonceStringHere" }});
        c.OAuthClientId("oauth2Config.ClientId");
        c.InjectJavascript("swagger.js");
    });
}
app.UseCors("MyCors");
app.UseHttpsRedirection();
app.UseStaticFiles();
// Add Authentication 
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.Run();
void AddSwaggerOAuth2Configuration(SwaggerGenOptions swaggerGenOptions) 
{
    
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            Implicit = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                Scopes = new Dictionary<string, string> {{"email", "email"}, {"profile", "profile"}}
            }
        },
        Extensions = new Dictionary<string, IOpenApiExtension>
        {
            {"x-tokenName", new OpenApiString("id_token")}
        },
    };
        
    swaggerGenOptions.AddSecurityDefinition("OAuth2", securityScheme) ;

    var securityRequirements = new OpenApiSecurityRequirement 
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
            new List<string> {"email", "profile"}
        } 
    };
    swaggerGenOptions.AddSecurityRequirement(securityRequirements);
}
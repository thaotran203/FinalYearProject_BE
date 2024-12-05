using FinalYearProject_BE.Data;
using FinalYearProject_BE.Models;
using FinalYearProject_BE.Repository.IRepository;
using FinalYearProject_BE.Repository;
using FinalYearProject_BE.Services.IService;
using FinalYearProject_BE.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FinalYearProject_BE.Settings;
using Microsoft.OpenApi.Models;

namespace FinalYearProject_BE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Configure JWT Authentication
            var secretKey = configuration["Jwt:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                    ClockSkew = TimeSpan.Zero,
                };
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Bearer token"
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
                        new string[] {}
                    }
                });
            });


            builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
            {
                build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));



            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSession();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();
            builder.Services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            builder.Services.AddSingleton<VnPayLibrary>();
            builder.Services.AddScoped<Utils>();
            builder.Services.AddHttpContextAccessor();
            //builder.Services.Configure<VnPaySettings>(builder.Configuration.GetSection("VnPaySettings"));
            //builder.Services.AddTransient<VnPayService>();
            //builder.Services.AddTransient<ZaloPayService>();

            // Dang ky cac service va repository
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ILessonRepository, LessonRepository>();
            builder.Services.AddScoped<ILessonVideoRepository, LessonVideoRepository>();
            builder.Services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILessonService, LessonService>();
            builder.Services.AddScoped<ILessonVideoService, LessonVideoService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();



            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Cookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
                options.LoginPath = new PathString("/Login");
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            builder.Services.AddControllersWithViews();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("MyCors");
            app.UseMiddleware<JwtMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}

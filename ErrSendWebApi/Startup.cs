using ErrSendApplication;
using ErrSendApplication.Interfaces;
using ErrSendApplication.Mappings;
using ErrSendPersistensTelegram;
using ErrSendWebApi.ExceptionMidlevare;
using ErrSendWebApi.Middleware.Culture;
using ErrSendWebApi.Serviсe;
using ErrSendWebApi.TimeZone;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ErrSendWebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //Файли конфігів
            //services.Configure<TokenCfg>(Configuration.GetSection("JWT"));

            //Додаємо профілі зборок в ДІ конвеєр через автомапер.
            services.AddAutoMapper(config =>
            {
                config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
                config.AddProfile(new AssemblyMappingProfile(typeof(IHttpClientWr).Assembly));
            });

            //Підключаємо зборки в ДІ через Медіатр
            services.AddApplication(Configuration);
            services.AddPersistenceTelegram(Configuration);
            services.AddControllers();
            
            /*Дописати
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "ErrSendWebApi",
                    ValidAudience = "https://localhost:5001",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(PassHelper.Decrypt(Configuration.GetSection("JWT:TokenKey").Value, "HEnglish")))
                };
            });*/

            //Політика підключення із всіх а не тільки через ІдентітіСрв не працювало б якщо ми б спробували із 1с наприклад підключитись.
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
            //services.ConfigureJwt(Configuration);
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HybrisWebApi.Api",
                    Version = "v1",
                    Description = "BesNilsen"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insert JWT Token."

                });
                c.OperationFilter<AddTimeAndTimeZoneOperationFilter>(); // Додавання кастомного фільтра

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                          },
                         new string[] {}
                    }
                });
            });

            //TODO: Зробити коли підключатимемо токени
            //services.AddPersistenceToken(Configuration);
            services.AddSingleton<ICurrentService, CurrentService>();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //Отримує ІР компа із якого прийшов запит але додатково налаштовуємо в конфігах NGINX.
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.RoutePrefix = string.Empty;
                config.SwaggerEndpoint("swagger/v1/swagger.json", "HybrisWebApi.Api");
                config.InjectStylesheet("/swagger/Ui/theme-feeling-blue.css");
            });

            app.UseCustomExceptionHandler();
            app.UseCulture();
            app.UseRouting();
            //TODO: Дві строки нище це перенаправлення на HTTPS та дозвіл підключення із всіх а не тільки через ІдентітіСрв.
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            //Додаємо авторизацію і аутентифікацію по токену.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

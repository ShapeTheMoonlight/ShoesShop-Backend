using System.Text.Json.Serialization;
using ShoesShop.Application;
using ShoesShop.Application.Requests.Queries.OutputVMs.Profiles;
using ShoesShop.Persistence;
using ShoesShop.WebApi.Dto.Profiles;

namespace ShoesShop.WebAPI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            Configure(app, builder.Environment);
            InitializeDb(app);
            SeedTestData(app);
            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddControllers()
                            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new VmProfiles());
                cfg.AddProfile(new DtoProfiles());
            });
            builder.Services.AddTransient<TestData>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddRouting();

            builder.Services.AddSwaggerGen();
            builder.Services.AddPersistence(builder.Configuration);
            builder.Services.AddApplication();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.AllowAnyOrigin();
                });
            });
        }

        private static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(x => x.SwaggerEndpoint("v1/swagger.json", "MyApi"));
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public static void InitializeDb(IHost app)
        {
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopedFactory.CreateScope())
            {
                var service = scope.ServiceProvider;
                try
                {
                    var context = service.GetRequiredService<ShopDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        private static void SeedTestData(IHost app)
        {
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopedFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<TestData>();
                service.SeedDataContext();
            }
        }
    }
}

using AccountManagement.BLL;
using AccountManagement.Configuration;
using AccountManagement.Configuration.Extensions;
using AccountManagement.DAL;
using Asp.Versioning.ApiExplorer;

namespace AccountManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add Serilog
            builder.Services.ConfigureSerilog(builder.Configuration);


            builder.Services.AddControllers();

            // Add Api Versioning
            builder.Services.AddApiVersioningConfigured();

            // Add DbContext & Data Protection
            builder.Services.ConfigureDbContext(builder.Configuration);
            builder.Services.ConfigureDataProtection();

            //Add Identity & Jwt
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureJwt(builder.Configuration);

            // Add services to the container.
            builder.Services.AddServices();
            builder.Services.AddRepositories();
            builder.Services.AddUnitOfWork();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //Add Swagger
            builder.Services.ConfigureSwagger();

            // Add Mapper
            builder.Services.AddAutoMapper(typeof(MapperConfiguration));

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddLogging();
            builder.Services.AddMemoryCache(); // For Feature Use

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                app.ConfigureSwaggerUi(provider);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
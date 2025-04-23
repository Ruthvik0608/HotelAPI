using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartHotelBookingSystem.BusinessLogicLayer;
using SmartHotelBookingSystem.DataAccess.ADO;
using SmartHotelBookingSystem.DataAccess.EFCore;
using SmartHotelBookingSystem.Models;

namespace HotelAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Register AppDbContext with dependency injection
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register DB1 with dependency injection
            builder.Services.AddScoped<DB1>();

            // Register your scoped services
            builder.Services.AddScoped<BookingRepository>();
            builder.Services.AddScoped<CategoryDAL>();
            builder.Services.AddScoped<LoyaltyDataOperations>();
            builder.Services.AddScoped<HotelBLL>();
            builder.Services.AddScoped<RoomBLL>();
            builder.Services.AddScoped<ReviewsRepository>();
            builder.Services.AddScoped<UserRepository>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.MapControllers();
            app.Run();

        }
    }
}

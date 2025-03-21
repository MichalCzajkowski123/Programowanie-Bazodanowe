using BLL.ServicesInterfaces;
using BLL_DB;
using BLL_DB.Services;
using BLL_EF;
using DAL.Model;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddScoped<IProductService, ProductService>();
            //builder.Services.AddScoped<IBasketService, BasketService>();
            //builder.Services.AddScoped<IOrderService, OrderService>();
            //builder.Services.AddScoped<IProductGroupService, ProductGroupService>();
            //builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IProductService, ProductServiceDB>();
            builder.Services.AddScoped<IBasketService, BasketServiceDB>();
            builder.Services.AddScoped<IOrderService, OrderServiceDB>();
            builder.Services.AddScoped<IProductGroupService, ProductGroupServiceDB>();
            builder.Services.AddScoped<IUserService, UserServiceDB>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<WebStoreContext>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

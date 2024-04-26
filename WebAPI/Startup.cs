using Business.Abstract;
using Business.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace YourNamespace
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(); // Bu, Controller sınıflarının eklenmesini sağlar.
            services.AddSwaggerGen(); // Bu, Swagger belgelerinin oluşturulmasını sağlar (isteğe bağlı).
            services.AddControllers();
            services.AddSingleton<ICarService, CarManager>();
      
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Geliştirme ortamında hata sayfasını gösterir.
                app.UseSwagger(); // Swagger belgelerini etkinleştirir (isteğe bağlı).
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); // Swagger UI'yı etkinleştirir (isteğe bağlı).
                });
            }

            // app.UseHttpsRedirection(); // HTTPS'e yönlendirme yapar (isteğe bağlı).
            app.UseRouting();

            // app.UseAuthorization(); // Kimlik doğrulama ve yetkilendirme kullanılıyorsa bu satır etkinleştirilir.

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Controller endpoint'lerini haritalar.
            });
        }
    }
}

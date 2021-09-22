using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Shop
{
    /*
    Uma classe de inicialização, toda vez que o app inicializar vai pra essa classe
    */
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /*
                Quais servicos do .net ou outros serviços a aplicação irar usar
        */


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop", Version = "v1" });
            });
        }

        /*
        Como ou quais opções desse serviço irá usar
        IWebHostEnvironment = Esta em producao ou em dev
        IApplicationBuilder = Esta na aplicação

        */
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop v1"));
            }

            //Força a api para https
            app.UseHttpsRedirection();

            //Usar o padrão de rota do mvc
            app.UseRouting();


            app.UseAuthorization();

            //Mapeamento dos endpoint
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /*
        O que é API Data Driven ou Orientado a dados ?
        Um espelho do banco de dados CRUD

        DDD voltado pra regra de negocio
        */
    }
}

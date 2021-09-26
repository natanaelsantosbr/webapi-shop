using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shop.Data;

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

            //EF em Memoria
            //services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("DataBase"));

            //EF no SQL Server
            services.AddDbContext<DataContext>(
                opt => opt.UseSqlServer(Configuration.GetConnectionString("connectionString"))
            );








            //AddTransient vai me trazer um novo DataContext
            //AddSingleton = Criar uma DataContext por aplicação (todos usam)
            //Garantir um DataContext por uma requisição
            services.AddScoped<DataContext, DataContext>();
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

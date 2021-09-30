using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
            services.AddCors();

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
            });

            //services.AddResponseCaching();
            services.AddControllers();

            //Gerar um chave Simetrica
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            //Adicionar a autenticação
            services.AddAuthentication(
                    x =>
                    {
                        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //
                        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //
                    }
            ).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            //EF em Memoria
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("DataBase"));

            //EF no SQL Server
            // services.AddDbContext<DataContext>(
            //     opt => opt.UseSqlServer(Configuration.GetConnectionString("connectionString"))
            // );

            //AddTransient vai me trazer um novo DataContext
            //AddSingleton = Criar uma DataContext por aplicação (todos usam)
            //Garantir um DataContext por uma requisição
            //services.AddScoped<DataContext, DataContext>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop Api", Version = "v1" });
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
            }

            //Força a api para https
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
           {
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "SHOP API V1");
           });



            //Usar o padrão de rota do mvc
            app.UseRouting();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();
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

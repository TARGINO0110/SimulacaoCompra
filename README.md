# Sistema web para Simulaçao de compras 
Este sistema tem como o intuito apresentar uma simulação sobre a compra realizada pelo usuário,
na qual será calculado o valor total de uma determinada compra de serviço ou produto sobre
um percentual de juros divido entre o número de parcelas.

# Tecnologias utilizadas para o desenvolvimento
VisualStudio 2019 => ASP.NET Core MVC; 
EF Core;
Sql Server 2019;

# Passos de Inicialização
1- Criar em Models Compra.cs e adicionar código abaixo:
-------------------------------------------------------------------
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SimulacaoCompra.Models
{
    public class Compra
    {
        [Key]
        public int Idcompra { get; set; }

        [Display(Name ="Nome da compra")]
        [StringLength(100, ErrorMessage ="Informe no campo até 100 caracteres")]
        public string NomeDaCompra{ get; set; }

        [Range(1, 10000000000, ErrorMessage = "O minimo para a simulação, é o valor de R$1,00 e o maximo é R$ 1.00000000,00.")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [Display(Name = "Valor da compra")]
        [Column(TypeName = "decimal(10, 2)")]
        [DisplayFormat(DataFormatString = "R$ {0:f}")]
        public decimal Valortotal { get; set; }


        [Display(Name = "% Juros")]
        [Required(ErrorMessage = "Informe o percentual de juros, pois este campo não pode ser nulo ou zerado.")]
        [Column(TypeName = "decimal(10, 4)")]
        [DisplayFormat(DataFormatString = "{0:g} %")]
        public decimal Valorjuros { get; set; }


        [Display(Name = "Qtd Parcelas")]
        [Range(1, 10000000000, ErrorMessage = "Informe no minimo 01 parcela")]
        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [Column(TypeName = "Decimal(10,5)")]
        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal Qtdparcelas { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Este campo é obrigatorio, selecione uma data atual ou posterior")]
        [Display(Name = "Data da compra")]
        public DateTime Datacompra { get; set; }


        [Display(Name = "Tipo de Calculo")]
        [Required(ErrorMessage = "Este campo é obrigatorio, selecione pelo menos 1 das opções")]
        public string TipoCalculo { get; set; }


        [Display(Name = "Valor da parcela")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10, 2)")]
        [DisplayFormat(DataFormatString = "R$ {0:f}")]
        public decimal Valorparcela { get; set; }   
        
    }
}

2- Adicionar classe "BancoDbContext.cs" em Models
---------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SimulacaoCompra.Models
{
    public class BancoDbContext : DbContext
    {
        public virtual DbSet<Compra> Compras { get; set; }

        public BancoDbContext(DbContextOptions<BancoDbContext> options) : base(options) { }
    }
}

3-Acessar a classe "Startup.cs" e Adicionar código abaixo:
-----------------------------------------------------------
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimulacaoCompra.Models;

namespace SimulacaoCompra
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
            services.AddMvc();
            //Parametro para executar serviço de conexão SqlServer no appsettings.json
            services.AddDbContext<BancoDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Compras/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Compras}/{action=Index}/{id?}");
            });
        }
    }
}

4- Acessar o arquivo appsetting.json e adicionar sua string de conexão SQL, segue abaixo:
-----------------------------------------------------------
{

  "ConnectionStrings": {
    "DefaultConnection": "Data Source="Seu host";Initial Catalog=SimularCompra; Integrated Security=True" 
  },

  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Warning"
    }
  }
}

5- utilizar o Console do gerenciador de pacotes para realizar os comando da migrations
-----------------------------------------------------------
1 comando, aguardar finalizar:
PM> add-migration InicialCompras

2 comando, aguardar atualizar a base de dados e verificar em seu sql se foi criado a base:
PM> update-database

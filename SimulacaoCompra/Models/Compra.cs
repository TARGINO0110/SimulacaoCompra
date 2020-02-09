using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SimulacaoCompra.Models
{
    public class Compra
    {
        [Key]
        public int Idcompra { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [Display(Name = "Nome da compra")]
        [StringLength(100, ErrorMessage = "Informe no campo até 100 caracteres")]
        public string NomeDaCompra { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [Range(1, 10000000000, ErrorMessage = "O minimo para a simulação, é o valor de R$1,00 e o maximo é R$ 1.00000000,00.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor da compra")]
        [Column(TypeName = "decimal(10, 2)")]
        [DisplayFormat(DataFormatString = "R$ {0:f}")]
        public decimal Valortotal { get; set; }

        [Required(ErrorMessage = "Informe o percentual de juros, pois este campo não pode ser nulo ou zerado.")]
        [Display(Name = "% Juros")]
        [Column(TypeName = "decimal(10, 4)")]
        [DisplayFormat(DataFormatString = "{0:g} %")]
        public decimal Valorjuros { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [Display(Name = "Qtd Parcelas")]
        [Range(1, 10000000000, ErrorMessage = "Informe no minimo 01 parcela")]
        [Column(TypeName = "Decimal(10,5)")]
        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal Qtdparcelas { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio, selecione uma data atual ou posterior")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data da compra")]
        public DateTime Datacompra { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio, selecione pelo menos 1 das opções")]
        [Display(Name = "Tipo de Calculo")]
        public string TipoCalculo { get; set; }

        [Required(ErrorMessage = "É necessário preencher este campo!")]
        [Display(Name = "Valor da parcela")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10, 2)")]
        [DisplayFormat(DataFormatString = "R$ {0:f}")]
        public decimal Valorparcela { get; set; }

        [Required(ErrorMessage = "É necessário preencher este campo!")]
        public string Categoria { get; set; }
    }
}

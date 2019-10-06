using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SimulacaoCompra.Models
{
    public class Compra
    {
        [Key]
        public int Idcompra { get; set; }

        
        [Range(1,10000000000, ErrorMessage = "O minimo para a simulação, é o valor de R$1,00 e o maximo é R$ 1.00000000,00.")]        
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
        [DisplayFormat(DataFormatString ="{0:0}")]
        public decimal Qtdparcelas { get; set; }

        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode =true)]
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

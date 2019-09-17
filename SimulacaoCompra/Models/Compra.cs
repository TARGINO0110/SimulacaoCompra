using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SimulacaoCompra.Models
{
    public class Compra
    {
        [Key]
        public int Idcompra { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor da compra")]
        [Column(TypeName = "decimal(10, 2)")]
        [DisplayFormat(DataFormatString = "R$ {0:f}")]
        public decimal Valortotal { get; set; }

        [Required]
        [Display(Name = "% Juros")]
        [Column(TypeName = "decimal(10, 4)")]
        [DisplayFormat(DataFormatString = "{0:g} %")]
        public decimal Valorjuros { get; set; }
        
        
        [Display(Name = "Qtd Parcelas")]
        [Column(TypeName = "Decimal(10,5)")]
        [DisplayFormat(DataFormatString ="{0:0}")]
        public decimal Qtdparcelas { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode =true)]
        [Required]
        [Display(Name = "Data da compra")]
        public DateTime Datacompra { get; set; }

        [Required]
        [Display(Name = "Tipo de Calculo")]
        public string TipoCalculo { get; set; }

        [Display(Name = "Valor da parcela")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10, 2)")]
        [DisplayFormat(DataFormatString = "R$ {0:f}")]
        public decimal Valorparcela { get; set; }   
        
    }
}

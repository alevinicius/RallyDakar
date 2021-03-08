using System.ComponentModel.DataAnnotations;

namespace RallyDakar.API.Modelo
{
    public class PilotoModelo
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome é de preenchimento obrigatório")]
        [MinLength(5, ErrorMessage = "Nome deve ter no mínimo 5 caracteres")]
        [MaxLength(50, ErrorMessage = "Nome não pode ter mais do que 50 caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Sobrenome é de preenchimento obrigatório")]
        [MinLength(5, ErrorMessage = "Sobrenome deve ter no mínimo 5 caracteres")]
        [MaxLength(50, ErrorMessage = "Sobrenome não pode ter mais do que 50 caracteres")]
        public string SobreNome { get; set; }
        public int EquipeId { get; set; }

        public string NomeCompleto
        {
            get { return $"{Nome} {SobreNome}"; }
        }
    }
}

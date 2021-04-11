using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Garcons.Application.Dtos
{
    public class GarcomDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public IEnumerable<SetorAtendimentoDto> Setores { get; set; }
    }
}


using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Setores.Application.Dtos
{
    public class SetorDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public IEnumerable<MesaDto> Mesas { get; set; }
    }
}

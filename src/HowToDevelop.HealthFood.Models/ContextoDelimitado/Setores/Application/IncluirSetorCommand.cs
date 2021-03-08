using HowToDevelop.Core.Comunicacao;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.ContextoDelimitado.Setores.Application
{
    public class IncluirSetorCommand :Comando<SetorDto>
    {
        public IncluirSetorCommand(in string nome, in string sigla)
        {
            Nome = nome;
            Sigla = sigla;
        }

        public string Nome { get; }
        public string Sigla { get; }
    }
}

using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.ValidacoesPadrao;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Setores
{
    public sealed class Mesa: Entidade<int>
    {
        public Mesa()
        {

        }

        public Mesa(int id)
            :base(id)
        {

        }

        public ushort Numeracao { get; set; }

        public override Result EhValido()
        {
            return Numeracao.NaoDeveSer(0, SetoresConstantes.NumeracaoNaoPodeSerIgualZero);
        }
    }
}

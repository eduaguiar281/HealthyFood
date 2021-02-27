using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.ValidacoesPadrao;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Setores
{
    public sealed class Mesa: Entidade<int>
    {
        [ExcludeFromCodeCoverage]
        private Mesa()
        {

        }

        private Mesa(in ushort numeracao, in int id)
            :base(id)
        {
            Numeracao = numeracao;
        }

        public ushort Numeracao { get; }

        public static Result<Mesa> Criar(in ushort numeracao, in int id = 0)
        {
            var (_, isFailure, error) = numeracao.NaoDeveSer(0, SetoresConstantes.NumeracaoNaoPodeSerIgualZero);
            if (isFailure)
            {
                return Result.Failure<Mesa>(error);
            }

            return Result.Success(new Mesa(numeracao, id));
        }
    }
}

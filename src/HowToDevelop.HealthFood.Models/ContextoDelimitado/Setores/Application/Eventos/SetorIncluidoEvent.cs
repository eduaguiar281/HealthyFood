using HowToDevelop.Core.Comunicacao;

namespace HowToDevelop.HealthFood.Dominio.ContextoDelimitado.Setores.Application.Eventos
{
    public class SetorIncluidoEvent : EventoDominioBase
    {
        public SetorIncluidoEvent(in string nome, in string sigla)
            : base(0)
        {
            Data = new { Nome = nome, Sigla = sigla };
        }
    }
}

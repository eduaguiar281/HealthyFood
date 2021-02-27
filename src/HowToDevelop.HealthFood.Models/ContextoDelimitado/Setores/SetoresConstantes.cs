
namespace HowToDevelop.HealthFood.Dominio.Setores
{
    public static class SetoresConstantes
    {
        public const string SetorCampoNomeObrigatorio = "Campo Nome é obrigatório!";

        public const string SetorCampoSiglaObrigatorio = "Campo Sigla é obrigatório!";

        public const int SetorTamanhoMaximoNome = 50;

        public static readonly string SetorCampoNomeDeveTerAteNCaracteres = $"Campo Nome é deve ter no máximo {SetorTamanhoMaximoNome} caracteres!";

        public const int SetorTamanhoMaximoSigla = 3;

        public static readonly string SetorCampoSiglaDeveTerAteNCaracteres = $"Campo Sigla deve ter no máximo {SetorTamanhoMaximoSigla} caracteres!";

        public const string NumeracaoNaoPodeSerIgualZero = "Numeração não pode ser igual a zero!";

        public const string JaExisteUmaMesaComEstaNumeracaoParaSetor = "Já existe esta numeracao de mesa para o setor {0}";

        public const string MesaInformadaNaoFoiLocalizada = "Mesa informada {0} não foi localizada!";

    }
}

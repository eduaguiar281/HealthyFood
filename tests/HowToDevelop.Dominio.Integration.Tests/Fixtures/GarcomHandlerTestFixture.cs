namespace HowToDevelop.Dominio.Integration.Tests.Fixtures
{
    public class GarcomHandlerTestFixture
    {
        public const string NomeGarcomIncluido = "José da Silva";
        public const string ApelidoGarcomIncluido = "Zé";

        public const string NomeGarcomAlterado = "José de Souza";
        public const string ApelidoGarcomAlterado = "Sr. Zé";

        public GarcomHandlerTestFixture()
        {

        }
        public int GarcomIdIncluido { get; set; }

        public int? SetorVinculado { get; set; }
    }
}

namespace HowToDevelop.Dominio.Integration.Tests.Fixtures
{
    public class GarcomTestFixture
    {
        public const string NomeGarcomIncluido = "José da Silva";
        public const string ApelidoGarcomIncluido = "Zé";

        public const string NomeGarcomAlterado = "José de Souza";
        public const string ApelidoGarcomAlterado = "Sr. Zé";

        public GarcomTestFixture()
        {

        }
        public int GarcomIdIncluido { get; set; }
        public int? SetorVinculado { get; set; }
        public string NomeSetorVinculado { get; set; }
    }
}

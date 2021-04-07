namespace HowToDevelop.Dominio.Integration.Tests.Fixtures
{
    public class SetoresRepositorioTestFixture
    {
        public const string NomeSetorIncluido = "VIP 01";
        public const string SiglaSetorIncluido = "VP1";
        public const string NomeSetorAlterado = "VIP 01";
        public const string SiglaSetorAlterado = "VP1";
        public const int MesaRemover = 10;
        public readonly ushort[] MesasAdicionar = new ushort[3] { 10, 9, 8 };


        public SetoresRepositorioTestFixture()
        {

        }
        public int SetorIdIncluido { get; set; }
    }
}

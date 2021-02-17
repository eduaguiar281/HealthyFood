using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Garcons
{
    public static class GarconsConstantes
    {
        public const int GarcomTamanhoMaximoNome = 60;
        public const int GarcomTamanhoMaximoApelido = 30;

        public const string SetorJaFoiVinculadoAoGarcom = "Setor já foi vinculado este garçom!";
        public const string SetorInformadoNaoFoiLocalizado = "Setor informado {0} não foi localizado!";
        public static readonly string NomeDeveTerAteCaracteres = $"Campo Nome é deve ter no máximo {GarcomTamanhoMaximoNome} caracteres!";
        public static readonly string ApelidoDeveTerAteCaracteres = $"Campo Nome é deve ter no máximo {GarcomTamanhoMaximoApelido} caracteres!";
        public const string GarcomDeveTerNoMinimoUmSetorVinculado = "Garçom deve ter no mínimo um setor vinculado!";
        public const string GarcomNomeEhObrigatorio = "Campo Nome é obrigatório!";
    }
}

using System;
namespace Core.Impl.Business
{
    public static class CalculoFrete
    {
        public static string Calcular(string cep, int qtdeItens)
        {
            int fatorMultiplicador = (Convert.ToInt32(cep.ToCharArray(cep.Length - 1, 1)[0])) switch
            {
                0 => 9,
                1 => 10,
                2 => 11,
                3 => 12,
                4 => 13,
                5 => 14,
                6 => 15,
                7 => 16,
                8 => 17,
                9 => 18,
                _ => 20,
            };
            double valorFrete = qtdeItens * (fatorMultiplicador / 1.5);
            return valorFrete.ToString("##.##");
        }
    }
}

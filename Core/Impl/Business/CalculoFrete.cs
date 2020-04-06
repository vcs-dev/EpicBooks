using System;
namespace Core.Impl.Business
{
    public static class CalculoFrete
    {
        public static double Calcular(string cep, int qtdeItens)
        {
            if (cep == null || qtdeItens == 0)
                return 00.00;
            
            int fatorMultiplicador = (Convert.ToInt32(cep.Substring(cep.Length - 1))) switch
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
            return valorFrete;
        }
    }
}

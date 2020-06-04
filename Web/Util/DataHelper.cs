using System;
using System.Collections.Generic;

namespace Web.Util
{
    public static class DataHelper
    {
        public static List<string> RetornaPeriodo(string dataInicial, string dataFinal)
        {
            List<string> listaDatas = new List<string>();
            string[] arrayDataInicial = dataInicial.Split('/');
            int mes = Convert.ToInt32(arrayDataInicial[1]);
            listaDatas.Add(dataInicial.Substring(3));
            if (!dataInicial.Substring(3).Equals(dataFinal.Substring(3)))
            {
                while (true)
                {
                    mes += 1;
                    string data;
                    if (mes > 12)
                    {
                        data = "0" + (mes - 12) + "/" + (Convert.ToInt32(arrayDataInicial[2]) + 1);
                        mes -= 12;
                        arrayDataInicial[2] = Convert.ToString(Convert.ToInt32(arrayDataInicial[2]) + 1);
                    }
                    else if (mes > 9)
                        data = mes + "/" + arrayDataInicial[2];
                    else
                        data = "0" + mes + "/" + arrayDataInicial[2];

                    if (!data.Equals(dataFinal.Substring(3)))
                        listaDatas.Add(data);
                    else
                    {
                        listaDatas.Add(data);
                        break;
                    }
                }
            }
            return listaDatas;
        }
    }
}

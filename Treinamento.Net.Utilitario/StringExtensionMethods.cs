using System;
using System.Text.RegularExpressions;

namespace Treinamento.Net.Utilitario
{
    public static class StringExtensionMethods
    {
        public static string SomenteNumeros(this string text)
        {
            string sRet = "";

            if (string.IsNullOrEmpty(text))
                sRet = text;
            else
            {
                for (int i = 0; i < text.Length; i++)
                    if ("0123456789".Contains(text[i].ToString()))
                        sRet = sRet + text[i].ToString();
            }

            return sRet;
        }

        public static bool ValidaRG(this string RG, string UF)
        {
            //Elimina da string os traços, pontos e virgulas,
            RG = RG.RemovePontuacao().Trim();
            UF = UF.ToUpper();

            /* Somente o último digito pode ser letra */
            for (int i = 0; i < RG.Length - 1; i++)
                if ("0123456789".IndexOf(RG[i]) < 0)
                    return false;

            if ((!UF.Equals("SP")) && (!UF.Equals("RJ")))
                return true;

            #region VALIDA SP
            if (UF.Equals("SP"))
            {
                //Verifica se o tamanho da string é 9
                if (RG.Length <= 9)
                {
                    RG = RG.PadLeft(9, '0');

                    int[] n = new int[9];

                    try
                    {
                        n[0] = Convert.ToInt32(RG.Substring(0, 1));
                        n[1] = Convert.ToInt32(RG.Substring(1, 1));
                        n[2] = Convert.ToInt32(RG.Substring(2, 1));
                        n[3] = Convert.ToInt32(RG.Substring(3, 1));
                        n[4] = Convert.ToInt32(RG.Substring(4, 1));
                        n[5] = Convert.ToInt32(RG.Substring(5, 1));
                        n[6] = Convert.ToInt32(RG.Substring(6, 1));
                        n[7] = Convert.ToInt32(RG.Substring(7, 1));
                        if (RG.Substring(8, 1).Equals("x") || RG.Substring(8, 1).Equals("X"))
                            n[8] = 10;
                        else
                            n[8] = Convert.ToInt32(RG.Substring(8, 1)) * 100;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    //obtém cada um dos caracteres do rg

                    //Aplica a regra de validação do RG, multiplicando cada digito por valores pré-determinados
                    n[0] *= 2;
                    n[1] *= 3;
                    n[2] *= 4;
                    n[3] *= 5;
                    n[4] *= 6;
                    n[5] *= 7;
                    n[6] *= 8;
                    n[7] *= 9;
                    //n[8] *= 100;

                    //Valida o RG
                    int somaFinal = n[0] + n[1] + n[2] + n[3] + n[4] + n[5] + n[6] + n[7] + n[8];
                    if ((somaFinal % 11) == 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            #endregion VALIDA SP

            #region VALIDA RJ
            if (UF.Equals("RJ"))
            {
                //Verifica se o tamanho da string é 9
                if (RG.Length <= 9)
                {
                    RG = RG.PadLeft(9, '0');

                    int[] n = new int[9];

                    try
                    {
                        n[0] = Convert.ToInt32(RG.Substring(0, 1));
                        n[1] = Convert.ToInt32(RG.Substring(1, 1));
                        n[2] = Convert.ToInt32(RG.Substring(2, 1));
                        n[3] = Convert.ToInt32(RG.Substring(3, 1));
                        n[4] = Convert.ToInt32(RG.Substring(4, 1));
                        n[5] = Convert.ToInt32(RG.Substring(5, 1));
                        n[6] = Convert.ToInt32(RG.Substring(6, 1));
                        n[7] = Convert.ToInt32(RG.Substring(7, 1));
                        n[8] = Convert.ToInt32(RG.Substring(8, 1));
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    //obtém cada um dos caracteres do rg

                    //Aplica a regra de validação do RG, multiplicando cada digito por valores pré-determinados
                    n[0] = n[0] * 1 > 9 ? (n[0] * 1) - 9 : n[0] * 1;
                    n[1] = n[1] * 2 > 9 ? (n[1] * 2) - 9 : n[1] * 2;
                    n[2] = n[2] * 1 > 9 ? (n[2] * 1) - 9 : n[2] * 1;
                    n[3] = n[3] * 2 > 9 ? (n[3] * 2) - 9 : n[3] * 2;
                    n[4] = n[4] * 1 > 9 ? (n[4] * 1) - 9 : n[4] * 1;
                    n[5] = n[5] * 2 > 9 ? (n[5] * 2) - 9 : n[5] * 2;
                    n[6] = n[6] * 1 > 9 ? (n[6] * 1) - 9 : n[6] * 1;
                    n[7] = n[7] * 2 > 9 ? (n[7] * 2) - 9 : n[7] * 2;

                    //Valida o RG
                    int somaFinal = n[0] + n[1] + n[2] + n[3] + n[4] + n[5] + n[6] + n[7] + n[8];
                    if ((somaFinal % 10) == 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            #endregion VALIDA RJ

            return false;
        }

        public static bool ValidaCpf(this string cpf)
        {
            //variáveis
            int digito1, digito2;
            int adicao = 0;

            string digito = "";
            string calculo = "";

            string cpfComparacao = cpf.RemovePontuacao();

            // Se o tamanho for < 11 entao retorna como inválido
            if (cpfComparacao.Length != 11) return false;

            // Pesos para calcular o primeiro número 
            int[] array1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Pesos para calcular o segundo número
            int[] array2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Caso coloque todos os numeros iguais
            if (Regex.Match(cpfComparacao, "0{11}|1{11}|2{11}|3{11}|4{11}|5{11}|6{11}|7{11}|8{11}|9{11}").Success) return false;

            // Calcula cada número com seu respectivo peso 
            for (int i = 0; i <= array1.GetUpperBound(0); i++)
                adicao += (array1[i] * Convert.ToInt32(cpfComparacao[i].ToString()));

            // Pega o resto da divisão 
            int resto = adicao % 11;

            if (resto == 1 || resto == 0)
                digito1 = 0;
            else
                digito1 = 11 - resto;

            adicao = 0;

            // Calcula cada número com seu respectivo peso 
            for (int i = 0; i <= array2.GetUpperBound(0); i++)
                adicao += (array2[i] * Convert.ToInt32(cpfComparacao[i].ToString()));

            // Pega o resto da divisão 
            resto = adicao % 11;

            if (resto == 1 || resto == 0)
                digito2 = 0;
            else
                digito2 = 11 - resto;

            calculo = digito1.ToString() + digito2.ToString();
            digito = cpfComparacao.Substring(9, 2);

            // Se os ultimos dois digitos calculados bater com 
            // os dois ultimos digitos do cpf entao é válido 
            return calculo == digito;
        }

        public static bool ValidaCnpj(this string cnpj)
        {
            int soma = 0, dig;

            string cnpjOriginal = cnpj.RemovePontuacao();
            if (cnpjOriginal.Length != 14) return false;

            string cnpjComparacao = cnpjOriginal.Substring(0, 12);

            char[] charCnpj = cnpjOriginal.ToCharArray();

            /* Primeira parte */
            for (int i = 0; i < 4; i++)
                if (charCnpj[i] - 48 >= 0 && charCnpj[i] - 48 <= 9)
                    soma += (charCnpj[i] - 48) * (6 - (i + 1));
            for (int i = 0; i < 8; i++)
                if (charCnpj[i + 4] - 48 >= 0 && charCnpj[i + 4] - 48 <= 9)
                    soma += (charCnpj[i + 4] - 48) * (10 - (i + 1));
            dig = 11 - (soma % 11);

            cnpjComparacao += (dig == 10 || dig == 11) ? "0" : dig.ToString();

            /* Segunda parte */
            soma = 0;
            for (int i = 0; i < 5; i++)
                if (charCnpj[i] - 48 >= 0 && charCnpj[i] - 48 <= 9)
                    soma += (charCnpj[i] - 48) * (7 - (i + 1));
            for (int i = 0; i < 8; i++)
                if (charCnpj[i + 5] - 48 >= 0 && charCnpj[i + 5] - 48 <= 9)
                    soma += (charCnpj[i + 5] - 48) * (10 - (i + 1));
            dig = 11 - (soma % 11);
            cnpjComparacao += (dig == 10 || dig == 11) ? "0" : dig.ToString();

            return cnpjOriginal == cnpjComparacao;
        }

        public static string RemovePontuacao(this string text)
        {
            return text.RemovePontuacao(true);
        }

        public static string RemovePontuacao(this string text, bool removeEmpty, bool forAddress = false)
        {
            if (text == null)
                return string.Empty;

            if (forAddress)
            {
                text = Regex.Replace(text, @"['´""]", string.Empty);
                text = Regex.Replace(text, "-", " ");
            }
            else
            {
                text = text.Trim();
                text = Regex.Replace(text, @"[-_/\.,:;()]", string.Empty);
            }

            if (removeEmpty)
                text = text.Replace(" ", string.Empty);

            return text;
        }

        public static int ConverterParaInteiro(this string text)
        {
            if (int.TryParse(text, out int valor))
            {
                return valor;
            }
            return 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace POO.Models
{
    public class Conta
    {
        public int ID { get; set; }
        public bool TipoConta { get; set; } //True = Energia, False = Agua
        public double LeituraMesAnterior { get; set; }
        public double LeituraMesAtual { get; set; }
        public int IDImovel { get; set; }
        public Imovel Imovel { get; set; }

        public static void CriarConta(int idConta, int idImovel, bool tipoConta, double leituraMesAnterior, double leituraMesAtual)
        {
            if (!Imovel.ExisteImovel(idImovel))
            {
                MessageBox.Show($"ID do imóvel {idImovel} não existe no arquivo de imóveis.");
                return;
            }

            string linhaDados = $"{idConta};{idImovel};{tipoConta};{leituraMesAnterior};{leituraMesAtual}";

            string caminhoArquivo = "informacoes.txt";

            if (!File.Exists(caminhoArquivo))
            {
                File.WriteAllText(caminhoArquivo, "");
            }

            File.AppendAllText(caminhoArquivo, linhaDados + "\n");

            MessageBox.Show("Informações salvas com sucesso!");
        }
        public static double GetConsumo(Conta conta)
        {
            return conta.LeituraMesAtual - conta.LeituraMesAnterior;
        }
        public static double GetConsumoReais(Conta conta)
        {
            if (conta.TipoConta.Equals(true)) //Energia
            {
                if (conta.Imovel.TipoImovel.Equals(true)) //Residencial
                {
                    var consumo = GetConsumo(conta);
                    var valorConsumo = consumo * 0.46;

                    if (consumo >= 90.0)
                        valorConsumo += 13.25;

                    valorConsumo = valorConsumo + (valorConsumo * 0.4285);
                    return valorConsumo;
                }
                else //Comercial
                {
                    var consumo = GetConsumo(conta);
                    var valorConsumo = consumo * 0.41;

                    if (consumo >= 90.0)
                        valorConsumo += 13.25;

                    valorConsumo = valorConsumo + (valorConsumo * 0.2195);
                    return valorConsumo;
                }
            }
            else //Agua
            {
                return CalcularVariacaoValorConta(conta);
            }
        }
        public static double GetConsumoMedioReais(Conta conta)
        {
            if (conta.TipoConta.Equals(true)) //Energia
            {
                if (conta.Imovel.TipoImovel.Equals(true)) //Residencial
                {
                    var consumo = (conta.LeituraMesAnterior + conta.LeituraMesAtual) / 2;
                    var valorConsumo = consumo * 0.46;

                    if (consumo >= 90.0)
                        valorConsumo += 13.25;

                    valorConsumo = valorConsumo + (valorConsumo * 0.4285);
                    return valorConsumo;
                }
                else //Comercial
                {
                    var consumo = (conta.LeituraMesAnterior + conta.LeituraMesAtual) / 2;
                    var valorConsumo = consumo * 0.41;

                    if (consumo >= 90.0)
                        valorConsumo += 13.25;

                    valorConsumo = valorConsumo + (valorConsumo * 0.2195);
                    return valorConsumo;
                }
            }
            else //Agua
            {
                return CalcularValorMedioConta(conta);
            }
        }
        public static double GetConsumoMesAnterior(Conta conta)
        {
            return conta.LeituraMesAnterior;
        }
        public static double GetValorTotal(Conta conta)
        {
            if (conta.TipoConta.Equals(true)) //Energia
            {
                if (conta.Imovel.TipoImovel.Equals(true)) //Residencial
                {
                    var consumo = conta.LeituraMesAnterior + conta.LeituraMesAtual;
                    var valorConsumo = consumo * 0.46;

                    if (consumo >= 90.0)
                        valorConsumo += 13.25;

                    valorConsumo = valorConsumo + (valorConsumo * 0.4285);
                    return valorConsumo;
                }
                else //Comercial
                {
                    var consumo = conta.LeituraMesAnterior + conta.LeituraMesAtual;
                    var valorConsumo = consumo * 0.41;

                    if (consumo >= 90.0)
                        valorConsumo += 13.25;

                    valorConsumo = valorConsumo + (valorConsumo * 0.2195);
                    return valorConsumo;
                }
            }
            else //Agua
            {
                return CalcularValorConta(conta);
            }
        }
        public static double GetValorTotalSemImpostos(Conta conta)
        {
            if (conta.TipoConta.Equals(true)) //Energia
            {
                if (conta.Imovel.TipoImovel.Equals(true)) //Residencial
                {
                    var consumo = conta.LeituraMesAnterior + conta.LeituraMesAtual;
                    var valorConsumo = consumo * 0.46;

                    if (consumo >= 90.0)
                        valorConsumo += 13.25;

                    return valorConsumo;
                }
                else //Comercial
                {
                    var consumo = conta.LeituraMesAnterior + conta.LeituraMesAtual;
                    var valorConsumo = consumo * 0.41;
                    if (consumo >= 90.0)
                        valorConsumo += 13.25;

                    return valorConsumo;
                }
            }
            else //Agua
            {
                return CalcularValorContaSemImpostos(conta);
            }
        }
        public static int GerarIDAutoIncrement()
        {
            int proximoID = 1;
            string caminhoArquivo = "informacoes.txt";

            try
            {
                if (File.Exists(caminhoArquivo))
                {
                    string[] linhas = File.ReadAllLines(caminhoArquivo);

                    if (linhas.Length > 0)
                    {
                        int[] idsExistentes = linhas.Select(linha => ObterIDDaLinha(linha)).ToArray();

                        proximoID = idsExistentes.Max() + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }

            return proximoID;
        }

        static int ObterIDDaLinha(string linha)
        {
            int id;
            if (int.TryParse(linha.Split(';')[0], out id))
            {
                return id;
            }

            return 0;
        }

        public static List<Conta> LerInformacoesDoArquivo(string caminhoArquivo)
        {
            List<Conta> contas = new List<Conta>();

            try
            {
                if (File.Exists(caminhoArquivo))
                {
                    string[] linhas = File.ReadAllLines(caminhoArquivo);

                    foreach (string linha in linhas)
                    {
                        if (!string.IsNullOrWhiteSpace(linha) && !linha.StartsWith("//"))
                        {
                            string[] partes = linha.Split(';');

                            if (partes.Length == 5 && int.TryParse(partes[0], out int idConta) &&
                                int.TryParse(partes[1], out int idImovel) && bool.TryParse(partes[2], out bool tipoConta) &&
                                double.TryParse(partes[3], out double leituraMesAnterior) && double.TryParse(partes[4], out double leituraMesAtual))
                            {
                                Imovel imovel = Imovel.ObterImovelPorID(idImovel);

                                Conta conta = new Conta
                                {
                                    ID = idConta,
                                    TipoConta = tipoConta,
                                    LeituraMesAnterior = leituraMesAnterior,
                                    LeituraMesAtual = leituraMesAtual,
                                    IDImovel = idImovel,
                                    Imovel = imovel
                                };

                                contas.Add(conta);
                            }
                            else
                            {
                                MessageBox.Show($"Formato incorreto na linha: {linha}");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Arquivo de informações não encontrado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro: {ex.Message}");
            }

            return contas;
        }
        public static Conta BuscarContaPorID(int idProcurado)
        {
            var caminhoArquivo = "informacoes.txt";
            try
            {
                if (File.Exists(caminhoArquivo))
                {
                    string[] linhas = File.ReadAllLines(caminhoArquivo);

                    foreach (string linha in linhas)
                    {
                        if (!string.IsNullOrWhiteSpace(linha) && !linha.StartsWith("//"))
                        {
                            string[] partes = linha.Split(';');

                            if (partes.Length == 5 && int.TryParse(partes[0], out int idConta) &&
                                int.TryParse(partes[1], out int idImovel) && bool.TryParse(partes[2], out bool tipoConta) &&
                                double.TryParse(partes[3], out double leituraMesAnterior) && double.TryParse(partes[4], out double leituraMesAtual))
                            {
                                if (idConta == idProcurado)
                                {
                                    Imovel imovel = Imovel.ObterImovelPorID(idImovel);

                                    return new Conta
                                    {
                                        ID = idConta,
                                        TipoConta = tipoConta,
                                        LeituraMesAnterior = leituraMesAnterior,
                                        LeituraMesAtual = leituraMesAtual,
                                        IDImovel = idImovel,
                                        Imovel = imovel
                                    };
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Formato incorreto na linha: {linha}");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Arquivo de informações não encontrado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro: {ex.Message}");
            }

            return null;
        }
        public static double CalcularVariacaoValorConta(Conta conta)
        {
            double consumoAgua = conta.LeituraMesAtual - conta.LeituraMesAnterior;

            double tarifaAgua = 0.0;
            double tarifaEsgoto = 0.0;

            if (conta.Imovel.TipoImovel) // Residencial
            {
                tarifaAgua = CalcularTarifaResidencial(consumoAgua);
                tarifaEsgoto = consumoAgua * 0.5;
            }
            else // Comercial
            {
                tarifaAgua = CalcularTarifaComercial(consumoAgua);
                tarifaEsgoto = consumoAgua * 0.6;
            }

            double valorTotalConta = tarifaAgua + tarifaEsgoto;
            double cofins = 0.03 * valorTotalConta;
            double totalComCofins = valorTotalConta + cofins;

            return totalComCofins;
        }
        public static double CalcularValorConta(Conta conta)
        {
            double consumoAgua = conta.LeituraMesAtual + conta.LeituraMesAnterior;

            double tarifaAgua = 0.0;
            double tarifaEsgoto = 0.0;

            if (conta.Imovel.TipoImovel) // Residencial
            {
                tarifaAgua = CalcularTarifaResidencial(consumoAgua);
                tarifaEsgoto = consumoAgua * 0.5;
            }
            else // Comercial
            {
                tarifaAgua = CalcularTarifaComercial(consumoAgua);
                tarifaEsgoto = consumoAgua * 0.6;
            }

            double valorTotalConta = tarifaAgua + tarifaEsgoto;
            double cofins = 0.03 * valorTotalConta;
            double totalComCofins = valorTotalConta + cofins;

            return totalComCofins;
        }
        public static double CalcularValorMedioConta(Conta conta)
        {
            double consumoAgua = (conta.LeituraMesAtual + conta.LeituraMesAnterior)/2;

            double tarifaAgua = 0.0;
            double tarifaEsgoto = 0.0;

            if (conta.Imovel.TipoImovel) // Residencial
            {
                tarifaAgua = CalcularTarifaResidencial(consumoAgua);
                tarifaEsgoto = consumoAgua * 0.5;
            }
            else // Comercial
            {
                tarifaAgua = CalcularTarifaComercial(consumoAgua);
                tarifaEsgoto = consumoAgua * 0.6;
            }

            double valorTotalConta = tarifaAgua + tarifaEsgoto;
            double cofins = 0.03 * valorTotalConta;
            double totalComCofins = valorTotalConta + cofins;

            return totalComCofins;
        }
        public static double CalcularValorContaSemImpostos(Conta conta)
        {
            double consumoAgua = conta.LeituraMesAtual - conta.LeituraMesAnterior;

            double tarifaAgua = 0.0;
            double tarifaEsgoto = 0.0;

            if (conta.Imovel.TipoImovel) // Residencial
            {
                tarifaAgua = CalcularTarifaResidencial(consumoAgua);
                tarifaEsgoto = consumoAgua * 0.5; // Considerando 50% do consumo de água como esgoto
            }
            else // Comercial
            {
                tarifaAgua = CalcularTarifaComercial(consumoAgua);
                tarifaEsgoto = consumoAgua * 0.6; // Considerando 60% do consumo de água como esgoto
            }

            return tarifaAgua + tarifaEsgoto;
        }
        private static double CalcularTarifaResidencial(double consumoAgua)
        {
            if (consumoAgua <= 6)
            {
                return 10.08;
            }
            else if (consumoAgua <= 10)
            {
                return 2.241 * (consumoAgua - 6) + 10.08;
            }
            else if (consumoAgua <= 15)
            {
                return 5.447 * (consumoAgua - 10) + 2.241 * 4 + 10.08;
            }
            else if (consumoAgua <= 20)
            {
                return 5.461 * (consumoAgua - 15) + 5.447 * 5 + 2.241 * 4 + 10.08;
            }
            else if (consumoAgua <= 40)
            {
                return 5.487 * (consumoAgua - 20) + 5.461 * 5 + 5.447 * 5 + 2.241 * 4 + 10.08;
            }
            else
            {
                return 10.066 * (consumoAgua - 40) + 5.487 * 20 + 5.461 * 5 + 5.447 * 5 + 2.241 * 4 + 10.08;
            }
        }
        private static double CalcularTarifaComercial(double consumoAgua)
        {
            if (consumoAgua <= 6)
            {
                return 25.79;
            }
            else if (consumoAgua <= 10)
            {
                return 4.299 * (consumoAgua - 6) + 25.79;
            }
            else if (consumoAgua <= 40)
            {
                return 8.221 * (consumoAgua - 10) + 4.299 * 4 + 25.79;
            }
            else if (consumoAgua <= 100)
            {
                return 8.288 * (consumoAgua - 40) + 8.221 * 30 + 4.299 * 4 + 25.79;
            }
            else
            {
                return 8.329 * (consumoAgua - 100) + 8.288 * 60 + 8.221 * 30 + 4.299 * 4 + 25.79;
            }
        }
    }
}

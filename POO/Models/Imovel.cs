using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace POO.Models
{
    public class Imovel
    {
        public int ID { get; set; }
        public bool TipoImovel { get; set; } //Residencial = True, Comercial = False

        #region Criação de Imovel
        public static void CriarImovel(int idImovel, bool tipoImovel)
        {
            string linhaImovel = $"{idImovel};{tipoImovel}";

            string caminhoArquivoImoveis = "imoveis.txt";

            if (!File.Exists(caminhoArquivoImoveis))
            {
                File.WriteAllText(caminhoArquivoImoveis, "");
            }

            File.AppendAllText(caminhoArquivoImoveis, linhaImovel + "\n");

            MessageBox.Show("Imóvel criado com sucesso!");
        }
        #endregion

        #region Verificação de existência de Imovel ao criar Conta
        public static bool ExisteImovel(int idImovel)
        {
            string caminhoArquivoImoveis = "imoveis.txt";

            if (!File.Exists(caminhoArquivoImoveis))
            {
                MessageBox.Show("Arquivo de imóveis não encontrado.");
                return false;
            }

            string[] linhas = File.ReadAllLines(caminhoArquivoImoveis);

            return linhas.Any(linha => linha.StartsWith($"{idImovel};"));
        }
        #endregion

        #region GerarIDAutoIncrement de acordo com a ordem do arquivo de texto
        public static int GerarIDAutoIncrement()
        {
            int proximoID = 1;
            string caminhoArquivo = "imoveis.txt";

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
        #endregion

        #region ObterImovelPorID
        public static Imovel ObterImovelPorID(int idProcurado)
        {
            var caminhoArquivo = "imoveis.txt";
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

                            if (partes.Length == 2 && int.TryParse(partes[0], out int id) &&
                                bool.TryParse(partes[1], out bool tipoImovel))
                            {
                                if (id == idProcurado)
                                {
                                    return new Imovel { ID = id, TipoImovel = tipoImovel };
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Formato incorreto na linha: {linha}");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Arquivo de imóveis não encontrado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            }

            return null;
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
        #endregion
    }
}

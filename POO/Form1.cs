using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using POO.Models;
using System.IO;

namespace POO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CarregarInformacoesImoveis();

            List<Conta> contas = Conta.LerInformacoesDoArquivo("informacoes.txt");

            MostrarContasNoDropdown(contas);

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Add(new KeyValuePair<bool, string>(true, "Residencial"));
            comboBox1.Items.Add(new KeyValuePair<bool, string>(false, "Comercial"));
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Items.Add(new KeyValuePair<bool, string>(true, "Energia"));
            comboBox2.Items.Add(new KeyValuePair<bool, string>(false, "Água"));
            comboBox2.SelectedIndexChanged += ComboBox2_SelectedIndexChanged;

            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void CarregarInformacoesImoveis()
        {
            string caminhoArquivoImoveis = "imoveis.txt";

            if (!File.Exists(caminhoArquivoImoveis))
            {
                Console.WriteLine("Arquivo de imóveis não encontrado.");
                return;
            }

            string[] linhas = File.ReadAllLines(caminhoArquivoImoveis);

            List<KeyValuePair<int, bool>> dadosImoveis = new List<KeyValuePair<int, bool>>();

            foreach (string linha in linhas)
            {
                string[] partes = linha.Split(';');

                if (partes.Length == 2 && int.TryParse(partes[0], out int idImovel) && bool.TryParse(partes[1], out bool tipoImovel))
                {
                    dadosImoveis.Add(new KeyValuePair<int, bool>(idImovel, tipoImovel));
                }
                else
                {
                    Console.WriteLine($"Formato incorreto na linha: {linha}");
                }
            }

            foreach (var dadoImovel in dadosImoveis)
            {
                comboBox3.Items.Add(dadoImovel);
            }
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValuePair<bool, string> selectedOption = (KeyValuePair<bool, string>)comboBox1.SelectedItem;

            bool valor = selectedOption.Key;
            string texto = selectedOption.Value;
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValuePair<bool, string> selectedOption = (KeyValuePair<bool, string>)comboBox2.SelectedItem;

            bool valor = selectedOption.Key;
            string texto = selectedOption.Value;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
            KeyValuePair<bool, string> selectedOption = (KeyValuePair<bool, string>)comboBox1.SelectedItem;
            Imovel.CriarImovel(Imovel.GerarIDAutoIncrement(), selectedOption.Key);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                KeyValuePair<bool, string> selectedOption2 = (KeyValuePair<bool, string>)comboBox2.SelectedItem;
                KeyValuePair<int, bool> selectedOption3 = (KeyValuePair<int, bool>)comboBox3.SelectedItem;
                Conta.CriarConta(Conta.GerarIDAutoIncrement(), selectedOption3.Key, selectedOption2.Key, Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            CarregarInformacoesImoveis();

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Clear();
            comboBox1.Items.Add(new KeyValuePair<bool, string>(true, "Residencial"));
            comboBox1.Items.Add(new KeyValuePair<bool, string>(false, "Comercial"));
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Items.Clear();
            comboBox2.Items.Add(new KeyValuePair<bool, string>(true, "Energia"));
            comboBox2.Items.Add(new KeyValuePair<bool, string>(false, "Água"));
            comboBox2.SelectedIndexChanged += ComboBox2_SelectedIndexChanged;

            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox4.Items.Clear();
            List<Conta> contas = Conta.LerInformacoesDoArquivo("informacoes.txt");
            MostrarContasNoDropdown(contas);
        }

        void MostrarContasNoDropdown(List<Conta> contas)
        {
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;

            foreach (var conta in contas)
            {
                string textoConta = $"ID: {conta.ID}, TipoConta: {(conta.TipoConta ? "Energia" : "Água")}, Leitura Anterior: {conta.LeituraMesAnterior}, Leitura Atual: {conta.LeituraMesAtual}";
                comboBox4.Items.Add(new { ID = conta.ID, Texto = textoConta });
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem == null)
                MessageBox.Show("Selecione uma conta");
            else
            {
                try
                {
                    var minhaString = Convert.ToString(comboBox4.SelectedItem);
                    char ID = minhaString[7];
                    var conta = Conta.BuscarContaPorID((int)char.GetNumericValue(ID));
                    var consumo = Conta.GetConsumoMesAnterior(conta);
                    if (conta.TipoConta)
                        MessageBox.Show("Consumo em KW/h: " + consumo);
                    else
                        MessageBox.Show("Consumo em m³: " + consumo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem == null)
                MessageBox.Show("Selecione uma conta");
            else
            {
                try
                {
                    var minhaString = Convert.ToString(comboBox4.SelectedItem);
                    char ID = minhaString[7];
                    var conta = Conta.BuscarContaPorID((int)char.GetNumericValue(ID));
                    var valorTotal = Conta.GetValorTotal(conta);
                    MessageBox.Show("Valor total da conta selecionada: " + valorTotal);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem == null)
                MessageBox.Show("Selecione uma conta");
            else
            {
                try
                {
                    var minhaString = Convert.ToString(comboBox4.SelectedItem);
                    char ID = minhaString[7];
                    var conta = Conta.BuscarContaPorID((int)char.GetNumericValue(ID));
                    var valorTotal = Conta.GetValorTotalSemImpostos(conta);
                    MessageBox.Show("Valor total sem impostos da conta selecionada: " + valorTotal);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem == null)
                MessageBox.Show("Selecione uma conta");
            else
            {
                try
                {
                    var minhaString = Convert.ToString(comboBox4.SelectedItem);
                    char ID = minhaString[7];
                    var conta = Conta.BuscarContaPorID((int)char.GetNumericValue(ID));
                    var consumo = Conta.GetConsumo(conta);
                    var consumoReais = Conta.GetConsumoReais(conta);
                    MessageBox.Show("Variação de consumo: " + consumo + "\nVariação em reais: R$" + consumoReais);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem == null)
                MessageBox.Show("Selecione uma conta");
            else
            {
                try
                {
                    var minhaString = Convert.ToString(comboBox4.SelectedItem);
                    char ID = minhaString[7];
                    var conta = Conta.BuscarContaPorID((int)char.GetNumericValue(ID));
                    var consumo = Conta.GetConsumoMedioReais(conta);
                    MessageBox.Show("Média de consumo em reais: R$" + consumo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
            }
        }
    }
}
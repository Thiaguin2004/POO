using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO.Models
{
    public class Conta
    {
        public int ID { get; set; }
        public bool TipoConta { get;set; } //True = Energia, False = Agua
        public double LeituraMesAnterior { get; set; }
        public double LeituraMesAtual { get; set; }
        public int IDImovel { get; set; }
        public Imovel Imovel {  get; set; }

        double GetConsumo(Conta conta)
        {
            return conta.LeituraMesAtual - conta.LeituraMesAnterior;
        }
        double GetValorTotal(Conta conta)
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
                    valorConsumo += 13.25;
                    valorConsumo = valorConsumo + (valorConsumo * 0.2195);
                    return valorConsumo;
                }
            }
            else //Agua
            {
                if (conta.Imovel.TipoImovel.Equals(true)) //Residencial
                {
                    var consumo = GetConsumo(conta);
                    var valorConsumoAgua = 0.0;
                    var valorConsumoEsgoto = 0.0;
                    if (consumo < 6)
                    {
                        valorConsumoAgua = TarifaResidencial.zeroAseisAgua;
                        valorConsumoEsgoto = TarifaResidencial.zeroAseisEsgoto;
                    }

                    var valorTotal = valorConsumoAgua + valorConsumoEsgoto;
                    return valorTotal + (valorTotal * 0.03);
                }
                else //Comercial
                {
                    var consumo = GetConsumo(conta);
                    var valorConsumoAgua = 0.0;
                    var valorConsumoEsgoto = 0.0;
                    if (consumo < 6)
                    {
                        valorConsumoAgua = TarifaComercial.zeroAseisAgua;
                        valorConsumoEsgoto = TarifaComercial.zeroAseisEsgoto;
                    }

                    var valorTotal = valorConsumoAgua + valorConsumoEsgoto;
                    return valorTotal + (valorTotal * 0.03);
                }
            }
        }
    }
}

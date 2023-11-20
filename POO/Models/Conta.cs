using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO.Models
{
    public class Conta
    {
        int ID { get; set; }
        bool TipoConta { get;set; }
        double LeituraMesAnterior { get; set; }
        double LeituraMesAtual { get; set; }
        int IDImovel { get; set; }
        Imovel Imovel {  get; set; }
    }
}

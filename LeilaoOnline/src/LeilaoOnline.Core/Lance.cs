using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeilaoOnline.Core
{
    public class Lance
    {
        public Interessada Cliente { get; }
        public double Valor { get; }

        public Lance(Interessada cliente, double valor)
        {
            if (valor < 0) throw new ArgumentException("O valor do lance deve ser positivo");
            
            Cliente = cliente;
            Valor = valor;
        }
    }
}

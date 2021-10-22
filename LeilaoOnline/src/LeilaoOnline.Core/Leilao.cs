using System;
using System.Collections.Generic;

namespace LeilaoOnline.Core
{
    public enum EstadoLeilao
    {
        LeilaoEstabelecido,
        LeilaoEmAndamento,
        LeilaoFinalizado,
    }
    
    public class Leilao
    {
        public IList<Lance> Lances { get; private set; }
        public string Peca { get; }
        public Lance Ganhador { get; private set; }
        public EstadoLeilao Status { get; private set; }
        public Interessada UltimoCliente { get; private set; }
        public IModalidadeAvaliacao Modalidade { get; private set; }

        public Leilao(string peca, IModalidadeAvaliacao modalidade)
        {
            Peca = peca;
            Lances = new List<Lance>();
            Status = EstadoLeilao.LeilaoEstabelecido;
            Modalidade = modalidade;
        }

        public void RecebeLance(Interessada cliente, double valor)
        {
            if (AceitarNovoLance(cliente))
            {
                Lances.Add(new Lance(cliente, valor));
                UltimoCliente = cliente;
            }
        }

        public virtual void TerminaPregao() 
        {
            if (Status == EstadoLeilao.LeilaoEstabelecido) throw new InvalidOperationException("Não é possível terminal o pregão sem que ele tenha começado. Para isso utilize o método IniciaPregao");

            Ganhador = Modalidade.Avalia(Lances);

            Status = EstadoLeilao.LeilaoFinalizado;
        }

        public void IniciaPregao()
        {
            Status = EstadoLeilao.LeilaoEmAndamento;
        }

        public bool AceitarNovoLance(Interessada cliente)
        {
            return (Status == EstadoLeilao.LeilaoEmAndamento && cliente != UltimoCliente);
        }
    }
}

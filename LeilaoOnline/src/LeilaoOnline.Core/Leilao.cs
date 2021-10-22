using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private IList<Lance> _lances;
        public IEnumerable<Lance> Lances => _lances;
        public string Peca { get; }
        public Lance Ganhador { get; private set; }
        public EstadoLeilao Status { get; private set; }
        public Interessada _ultimoCliente { get; private set; }

        public Leilao(string peca)
        {
            Peca = peca;
            _lances = new List<Lance>();
            Status = EstadoLeilao.LeilaoEstabelecido;
        }

        public void RecebeLance(Interessada cliente, double valor)
        {
            if (AceitarNovoLance(cliente))
            {
                _lances.Add(new Lance(cliente, valor));
                _ultimoCliente = cliente;
            }
        }

        public void TerminaPregao() 
        {
            if (Status == EstadoLeilao.LeilaoEstabelecido) throw new InvalidOperationException("Não é possível terminal o pregão sem que ele tenha começado. Para isso utilize o método IniciaPregao");
            
            Ganhador = _lances
                .DefaultIfEmpty(new Lance(null, 0))
                .OrderByDescending(x => x.Valor)
                .FirstOrDefault();

            Status = EstadoLeilao.LeilaoFinalizado;
        }

        public void IniciaPregao()
        {
            Status = EstadoLeilao.LeilaoEmAndamento;
        }

        private bool AceitarNovoLance(Interessada cliente)
        {
            return (Status == EstadoLeilao.LeilaoEmAndamento && cliente != _ultimoCliente);
        }
    }
}

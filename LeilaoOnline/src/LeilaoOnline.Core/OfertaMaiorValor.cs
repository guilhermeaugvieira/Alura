using System.Collections.Generic;
using System.Linq;

namespace LeilaoOnline.Core
{
    public class OfertaMaiorValor : IModalidadeAvaliacao
    {
        public Lance Avalia(IEnumerable<Lance> lances)
        {
            return lances
                .DefaultIfEmpty(new Lance(null, 0))
                .OrderByDescending(x => x.Valor)
                .FirstOrDefault();
        }
    }
}

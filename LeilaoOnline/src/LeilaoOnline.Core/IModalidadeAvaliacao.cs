using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeilaoOnline.Core
{
    public interface IModalidadeAvaliacao
    {
        public Lance Avalia(IEnumerable<Lance> lances);
    }
}

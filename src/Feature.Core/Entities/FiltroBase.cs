using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Core.Entities
{
    public class FiltroBase
    {
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }
        public string? PalabrasABuscar { get; set; }
        public string? ColumnaAOrdenar { get; set; }
        public FiltroBase()
        {
            PageIndex = 1;
            PageSize = Int32.MaxValue;
        }
    }
}

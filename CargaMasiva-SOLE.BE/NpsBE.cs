using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasiva_SOLE.BE
{
  public class NPSBE
    {
        public  string NPSId { get; set; }
        public int Sole_numeroencuesta { get; set; }
        public DateTime Sole_fechaencuesta { get; set; }
        public string Sole_ordenservicio { get; set; }
        public int Sole_puntuacion { get; set; }
        public string Sole_comentarios { get; set; }
        public int Sole_tipoencuesta { get; set; }
    }
}

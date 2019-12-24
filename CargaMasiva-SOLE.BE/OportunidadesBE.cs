using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasiva_SOLE.BE
{
    public class OportunidadesBE
    {
      
       public string IdOportunidadCargaInicial { get; set; }
        public string Sole_tema { get; set; }
        public string Sole_ordenservicio { get; set; }
        public DateTime Sole_fechasolicitud { get; set; }
        public string Sole_serieproducto { get; set; }
        public string Sole_codigoproducto { get; set; }
        public int Sole_unidades { get; set; }
        public decimal Sole_precioventapublico { get; set; }
        public decimal Sole_ventaneta { get; set; }
        public decimal Sole_costoventa { get; set; }
        public decimal Sole_utilidadbruta { get; set; }
        public int Sole_margenbruto { get; set; }
        public decimal Sole_importetotal { get; set; }
        public string Sole_tiposervicio { get; set; }
        public DateTime Sole_fechaservicio { get; set; }
        public string Sole_empresaqueinstalo { get; set; }
        public string Sole_nombretecnico { get; set; }
        public string Sole_canal { get; set; }
        public string Sole_tienda { get; set; }
        public ContactoBE cliente { get; set; }
        public DateTime Sole_fechacompra { get; set; }
        public string Sole_tipocomprobante { get; set; }
        public string Sole_numerocomprobante { get; set; }
        public string Sole_cuponcampana { get; set; }
        public string Sole_metodoventa { get; set; }
        public string Sole_estadocarga { get; set; }
        public string Nombre_Producto { get; set; }

    }
}

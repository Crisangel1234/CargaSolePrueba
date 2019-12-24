using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasiva_SOLE.BE
{

    public  class BitacoraErroresBE
    {

        // Constructor que se ejecuta cuando se inicializa un objeto de esta clase
        public BitacoraErroresBE() {

            this.init();
        }

        // Inicializacion 
        private void init()
        {
           
        }

        // Encapsulamiento
        // Tenemos campos y propiedades de la Clase BitacoraErroresBE
        private String _Codigo;
        public String Codigo
        {
            get
            {
                return _Codigo;
            }
            set
            {
                _Codigo = value;
            }
        }

        private String _Proceso;
        public String Proceso
        {
            get
            {
                return _Proceso;
            }
            set
            {
                _Proceso = value;
            }
        }

        private String _Error;
        public String Error
        {
            get
            {
                return _Error;
            }
            set
            {
                _Error = value;
            }
        }

        private String _Descripcion;
        public String Descripcion
        {
            get
            {
                return _Descripcion;
            }
            set
            {
                _Descripcion = value;
            }
        }

        private String _IdRegistro;
        public String IdRegistro
        {
            get
            {
                return _IdRegistro;
            }
            set
            {
                _IdRegistro = value;
            }
        }

        private String _Entidad;
        public String Entidad
        {
            get
            {
                return _Entidad;
            }
            set
            {
                _Entidad = value;
            }
        }

        private String _EstadoCarga;
        public String EstadoCarga
        {
            get
            {
                return _EstadoCarga;
            }
            set
            {
                _EstadoCarga = value;
            }
        }
    }
}

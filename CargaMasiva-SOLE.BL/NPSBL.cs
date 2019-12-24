using CargaMasiva_SOLE.BE;
using CargaMasiva_SOLE.DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasiva_SOLE.BL
{
    public  class NPSBL
    {
        #region HISTORIA
        //Autor: Carlos C.
        //Fecha: 07/08/2019
        //Notas: Clase Entidad de los NPS
        #endregion

        #region Variables
        private NPSDA oNPSDA = new NPSDA();
        #endregion

        #region MÉTODOS

        /// <summary>
        /// Lista las NPS A Cargar
        /// </summary>
        /// <returns>Lista las NPS A Cargar</returns>
        public DataTable ListadoNPSACargar()
        {
            try
            {
                return oNPSDA.ListadoNPSACargar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Crea el NPS
        /// </summary>
        /// <param name="ClienteId">Id del Cliente al que se le asociará la NPS</param>
        /// <param name="oNPSBE">Datos de la NPS</param>
        /// <returns>Crea el NPS</returns>
        public Guid CrearNPS(Guid ClienteId, NPSBE oNPSBE)
        {
            try
            {
                return oNPSDA.CrearNPS(ClienteId, oNPSBE);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

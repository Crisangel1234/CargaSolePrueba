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
    public  class OportunidadesBL
    {
        #region HISTORIA
        //Autor: Carlos C.
        //Fecha: 17/07/2019
        //Notas: Clase Entidad de los Oportunidades
        #endregion

        #region Variables
        private OportunidadesDA oOportunidadesDA = new OportunidadesDA();
        #endregion

        #region MÉTODOS

        /// <summary>
        /// Lista las Oportunidades a Cargar
        /// </summary>
        /// <returns>Lista las Oportunidades a Cargar</returns>
        public DataTable ListadoOportunidadesACargar()
        {
            try
            {
                return oOportunidadesDA.ListadoOportunidadesACargar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Crea La Oportunidad
        /// </summary>
        /// <param name="ClienteId">Cliente Id al que le Asignamos la Oportunidad</param>
        /// <param name="oOportunidadBE">Datos de la Oportunidad</param>
        /// <returns> Crea La Oportunidad</returns>
        public Guid CrearOportunidad(Guid ClienteId, OportunidadesBE oOportunidadBE)
        {
            try
            {
                return oOportunidadesDA.CrearOportunidad(ClienteId, oOportunidadBE);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Registramos el Producto de la Oportunidad
        /// </summary>
        /// <param name="IdOportunidadCargaInicial">Id de la Tabla Oportunidad</param>
        /// <param name="OportunidadId">Guid de la Oportunidad</param>
        /// <param name="ProductoId">Guid del Producto</param>
        /// <returns>Registramos el Producto de la Oportunidad</returns>
        public object CrearProductoDelaOportunidad(OportunidadesBE oOportunidadBE, 
                                                   string IdOportunidadCargaInicial, 
                                                   Guid OportunidadId, 
                                                   Guid ProductoId)
        {
            try
            {
                return oOportunidadesDA.CrearProductoDelaOportunidad(oOportunidadBE, IdOportunidadCargaInicial, OportunidadId, ProductoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


    }
}

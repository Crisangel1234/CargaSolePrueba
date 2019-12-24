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
    public  class ContactoBL
    {
        #region HISTORIA
        //Autor: Carlos C.
        //Fecha: 16/07/2019
        //Notas: Clase Entidad de los Contactos
        #endregion

        #region Variables
        private ContactoDA oContactoDA = new ContactoDA();
        #endregion

        #region MÉTODOS

        /// <summary>
        /// Listado de Contactos a Cargar
        /// </summary>
        /// <returns>Listado de Contactos a Cargar</returns>
        public DataTable ListadoContactosACargar()
        {

          
            try
            {
                return oContactoDA.ListadoContactosACargar();
                // retorna el metodo listado de contactos a cargar que proviene de la capa DA
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validamos si Existe el Contacto Mediante su DNI
        /// </summary>
        /// <param name="DNI">DNI a Validar</param>
        /// <returns>Validamos si Existe el Contacto Mediante su DNI</returns>
        public DataTable ValidaExisteContacto(string TipoDeDocumento, string NumeroDocumento)
        {
            try
            {
                return oContactoDA.ValidaExisteContacto(TipoDeDocumento, NumeroDocumento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza los Datos del Contacto
        /// </summary>
        /// <param name="guidContacto">Guid del Contacto a Actualizar</param>
        /// <param name="oContactoBE">Datos del Contacto a Actualizar</param>
        /// <returns>Actualiza los Datos del Contacto</returns>
        public Boolean ActualizaContacto(string guidContacto, ContactoBE oContactoBE)
        {
            try
            {
                return oContactoDA.ActualizaContacto(guidContacto, oContactoBE);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Crea el Nuevo Contacto
        /// </summary>
        /// <param name="oContactoBE">Datos del Contacto a Actualizar</param>
        /// <returns>Crea el Nuevo Contacto</returns>
        public Guid CrearContacto(ContactoBE oContactoBE)
        {
            try
            {
                return oContactoDA.CrearContacto(oContactoBE);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

using CargaMasiva_SOLE.BE;
using CargaMasiva_SOLE.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasiva_SOLE.BL
{
    public  class FuncionesBL
    {
        #region HISTORIA
        //Autor: Carlos C.
        //Fecha: 16/07/2019
        //Notas: Clase para las Funciones Varias
        #endregion

        #region Variables
        private FuncionesDA oFuncionesDA = new FuncionesDA();
        #endregion

        #region MÉTODOS

        /// <summary>
        /// Registra los Errores que se Produjeron Durante el Proceso
        /// </summary>
        /// <param name="oBitacoraErroresBE">Datos del Error Producido</param>
        /// <returns>Registra los Errores que se Produjeron Durante el Proceso</returns>
        public bool RegistraBitacoraErrores(BitacoraErroresBE oBitacoraErroresBE)
        {
            try
            {
                return oFuncionesDA.RegistraBitacoraErrores(oBitacoraErroresBE);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza el Estado de la Tabla ContactoCargaInicial
        /// </summary>
        /// <param name="IdContactoCargaInicial">Id a Actualizar</param>
        /// <returns>Actualiza el Estado de la Tabla ContactoCargaInicial</returns>
        public bool ActualizarEstadoTablaContacto(string IdContactoCargaInicial)
        {
            try
            {
                return oFuncionesDA.ActualizarEstadoTablaContacto(IdContactoCargaInicial);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el Guid del Departamento
        /// </summary>
        /// <param name="DepartamentoUbigeo">Código Ubigueo del Departamento</param>
        /// <returns>Obtiene el Guid del Departamento</returns>
        public string ObtieneDepartamento(string DepartamentoUbigeo)
        {
            try
            {
                return oFuncionesDA.ObtieneDepartamento(DepartamentoUbigeo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el Guid de la Provincia
        /// </summary>
        /// <param name="ProvinciaUbigeo">Código Ubigueo de la Provincia</param>
        /// <returns>Obtiene el Guid de la Provincia</returns>
        public string ObtieneProvincia(string ProvinciaUbigeo)
        {
            try
            {
                return oFuncionesDA.ObtieneProvincia(ProvinciaUbigeo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el Guid del Distrito
        /// </summary>
        /// <param name="DistritoUbigeo">Guid del Distrito</param>
        /// <returns>Obtiene el Guid del Distrito</returns>
        public string ObtieneDistrito(string DistritoUbigeo)
        {
            try
            {
                return oFuncionesDA.ObtieneDistrito(DistritoUbigeo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el Guid de la Tienda
        /// </summary>
        /// <param name="CodigoSucursal">Código de Sucursal de la Tienda</param>
        /// <returns>Obtiene el Guid de la Tienda</returns>
        public string ObtieneTienda(string CodigoSucursal)
        {
            try
            {
                return oFuncionesDA.ObtieneTienda(CodigoSucursal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtenemos el Id del Cliente mediante el DNI
        /// </summary>
        /// <param name="DNI"></param>
        /// <returns>Obtenemos el Id del Cliente mediante el DNI</returns>
        public string ObtieneClientexDNI(string TipoDocumento, string DNI)
        {
            try
            {
                return oFuncionesDA.ObtieneClientexDNI(TipoDocumento, DNI);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza el Estado de la Tabla OportunidadCargaInicial
        /// </summary>
        /// <param name="IdOportunidadCargaInicial">Id a Actualizar</param>
        /// <returns>Actualiza el Estado de la Tabla OportunidadCargaInicial</returns>
        public bool ActualizarEstadoTablaOportunidad(string IdOportunidadCargaInicial)
        {
            try
            {
                return oFuncionesDA.ActualizarEstadoTablaOportunidad(IdOportunidadCargaInicial);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtenemos el Guid del Producto
        /// </summary>
        /// <param name="CodigoProducto">Códgo del Producto</param>
        /// <returns>Obtenemos el Guid del Producto</returns>
        public string ObtieneProductoId(string CodigoProducto)
        {
            try
            {
                return oFuncionesDA.ObtieneProductoId(CodigoProducto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el Nombre del Producto
        /// </summary>
        /// <param name="CodigoProducto">Busca el Nombre del Producto mediante el Código del Producto</param>
        /// <returns>Devuelve el Nombre del Producto</returns>
        public string ObtieneNombreProducto(string CodigoProducto)
        {
            try
            {
                return oFuncionesDA.ObtieneNombreProducto(CodigoProducto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ActualizarEstadoTablaNPS(string IdNPSCargaInicial)
        {
            try
            {
                return oFuncionesDA.ActualizarEstadoTablaNPS(IdNPSCargaInicial);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

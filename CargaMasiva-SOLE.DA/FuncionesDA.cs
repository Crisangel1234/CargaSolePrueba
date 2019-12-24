using CargaMasiva_SOLE.BE;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasiva_SOLE.DA
{
    public  class FuncionesDA
    {
        #region HISTORIA
        //Autor: Carlos C.
        //Fecha: 16/07/2019
        //Notas: Clase para las Funciones Varias
        #endregion

        #region MÉTODOS

        string Ruta = ConfigurationManager.AppSettings["PathLogServicio"].ToString();

        /// <summary>
        /// Registra los Errores que se Produjeron Durante el Proceso
        /// </summary>
        /// <param name="oBitacoraErroresBE">Datos del Error Producido</param>
        /// <returns>Registra los Errores que se Produjeron Durante el Proceso</returns>
        public bool RegistraBitacoraErrores(BitacoraErroresBE oBitacoraErroresBE)
        {
            using (SqlConnection oSqlConnection = new SqlConnection(ConexionSQLDA.ObtenerConexion()))
                // Instancia la conexión, using para desechar
            {
                using (SqlCommand oSqlCommand = new SqlCommand("CRM_isp_Bitacora_Errores", oSqlConnection))
                {
                    // Procedimiento almacenado, Conexión SQL
                    try
                    {
                        {
                            var withBlock = oSqlCommand;
                            // Manejamos el procedimiento almacenado con withBlock
                            withBlock.CommandType = CommandType.StoredProcedure;
                            // Tipo de comando SQL, se define como procedimiento almacenado : Nota existen 3 tipos de command type
                            withBlock.CommandTimeout = 10;
                            // Intervalo de tiempo
                            withBlock.Parameters.Add("@v_codigo", SqlDbType.VarChar, 500).Value = oBitacoraErroresBE.Codigo;
                            withBlock.Parameters.Add("@v_proceso", SqlDbType.VarChar, 500).Value = oBitacoraErroresBE.Proceso;
                            withBlock.Parameters.Add("@v_error", SqlDbType.VarChar, 500).Value = oBitacoraErroresBE.Error;
                            withBlock.Parameters.Add("@v_descripcion", SqlDbType.VarChar, 4000).Value = oBitacoraErroresBE.Descripcion;
                            withBlock.Parameters.Add("@v_idregistro", SqlDbType.VarChar, 500).Value = oBitacoraErroresBE.IdRegistro;
                            withBlock.Parameters.Add("@v_entidad", SqlDbType.VarChar, 500).Value = oBitacoraErroresBE.Entidad;
                            withBlock.Parameters.Add("@v_estadocarga", SqlDbType.VarChar, 500).Value = oBitacoraErroresBE.EstadoCarga;


                            oSqlConnection.Open();
                            // abre la conexión
                            withBlock.ExecuteNonQuery();
                            // Pedimos a sql que ejecute el comando especificado, no retorna data
                            // ExcuteNonQuery inserta, elimina, actualiza
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        string Mensaje = "Se ha producido el siguiente error: " + ex.Message;
                        ZthMetodosVarios.Metodos.GuardarLog(Ruta, Mensaje);

                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Actualiza el Estado de la Tabla ContactoCargaInicial
        /// </summary>
        /// <param name="IdContactoCargaInicial">Id a Actualizar</param>
        /// <returns>Actualiza el Estado de la Tabla ContactoCargaInicial</returns>
        public bool ActualizarEstadoTablaContacto(string IdContactoCargaInicial)
        {
            using (SqlConnection oSqlConnection = new SqlConnection(ConexionSQLDA.ObtenerConexion()))
            {
                using (SqlCommand oSqlCommand = new SqlCommand("Zth_ActualizaEstadoContacto", oSqlConnection))
                {
                    try
                    {
                        {
                            var withBlock = oSqlCommand;
                            withBlock.CommandType = CommandType.StoredProcedure;
                            withBlock.CommandTimeout = 10;
                            withBlock.Parameters.Add("@IdContactoCargaInicial", SqlDbType.Int).Value = IdContactoCargaInicial;


                            // no retorna pero hace algo en la base de datos internamente
                            oSqlConnection.Open();
                            withBlock.ExecuteNonQuery();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        string Mensaje = "Se ha producido el siguiente error: " + ex.Message;
                        ZthMetodosVarios.Metodos.GuardarLog(Ruta, Mensaje);
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene el Guid del Departamento
        /// </summary>
        /// <param name="DepartamentoUbigeo">Código Ubigueo del Departamento</param>
        /// <returns>Obtiene el Guid del Departamento</returns>
        public string ObtieneDepartamento(string DepartamentoUbigeo)
        {
            IOrganizationService servicio;
            servicio = ConexionCRMDA.ObtenerConexion();

            string guidComuna = string.Empty;

            ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("sole_departamento", ref servicio);

            fetch.AgregarCampoRetorno("sole_departamento", "sole_departamentoid", ZthFetchXml365.zthFetch.TipoRetorno.Key);

            fetch.AgregarFiltroPlano("sole_departamento", ZthFetchXml365.zthFetch.TipoFiltro.and, "sole_codigoubigeo", ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, DepartamentoUbigeo);

            DataTable Dato = new DataTable();
            Dato = fetch.GeneraTblconFetchResult(false);

            if (Dato.Rows.Count > 0)
                return guidComuna = Dato.Rows[0]["sole_departamentoid"].ToString();
            else
                return guidComuna = null;
        }

        /// <summary>
        /// Obtiene el Guid de la Provincia
        /// </summary>
        /// <param name="ProvinciaUbigeo">Código Ubigueo de la Provincia</param>
        /// <returns>Obtiene el Guid de la Provincia</returns>
        public string ObtieneProvincia(string ProvinciaUbigeo)
        {
            IOrganizationService servicio;
            servicio = ConexionCRMDA.ObtenerConexion();

            string guidComuna = string.Empty;

            ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("sole_provincia", ref servicio);

            fetch.AgregarCampoRetorno("sole_provincia", "sole_provinciaid", ZthFetchXml365.zthFetch.TipoRetorno.Key);

            fetch.AgregarFiltroPlano("sole_provincia", ZthFetchXml365.zthFetch.TipoFiltro.and, "sole_codigoubigeo", ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, ProvinciaUbigeo);

            DataTable Dato = new DataTable();
            Dato = fetch.GeneraTblconFetchResult(false);

            if (Dato.Rows.Count > 0)
                return guidComuna = Dato.Rows[0]["sole_provinciaid"].ToString();
            else
                return guidComuna = null;
        }

        /// <summary>
        /// Obtiene el Guid del Distrito
        /// </summary>
        /// <param name="DistritoUbigeo">Guid del Distrito</param>
        /// <returns>Obtiene el Guid del Distrito</returns>
        public string ObtieneDistrito(string DistritoUbigeo)
        {
            IOrganizationService servicio;
            servicio = ConexionCRMDA.ObtenerConexion();

            string guidComuna = string.Empty;

            ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("sole_distrito", ref servicio);

            fetch.AgregarCampoRetorno("sole_distrito", "sole_distritoid", ZthFetchXml365.zthFetch.TipoRetorno.Key);

            fetch.AgregarFiltroPlano("sole_distrito", ZthFetchXml365.zthFetch.TipoFiltro.and, "sole_codigoubigeo", ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, DistritoUbigeo);

            DataTable Dato = new DataTable();
            Dato = fetch.GeneraTblconFetchResult(false);

            if (Dato.Rows.Count > 0)
                return guidComuna = Dato.Rows[0]["sole_distritoid"].ToString();
            else
                return guidComuna = null;
        }

        /// <summary>
        /// Obtiene el Guid de la Tienda
        /// </summary>
        /// <param name="CodigoSucursal">Código de Sucursal de la Tienda</param>
        /// <returns>Obtiene el Guid de la Tienda</returns>
        public string ObtieneTienda(string CodigoSucursal)
        {
            IOrganizationService servicio;
            servicio = ConexionCRMDA.ObtenerConexion();

            string guidTienda = string.Empty;

            ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("sole_tienda", ref servicio);

            fetch.AgregarCampoRetorno("sole_tienda", "sole_tiendaid", ZthFetchXml365.zthFetch.TipoRetorno.Key);

            fetch.AgregarFiltroPlano("sole_tienda", ZthFetchXml365.zthFetch.TipoFiltro.and, "sole_codigosucursal", ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, CodigoSucursal);

            DataTable Dato = new DataTable();
            Dato = fetch.GeneraTblconFetchResult(false);

            if (Dato.Rows.Count > 0)
                return guidTienda = Dato.Rows[0]["sole_tiendaid"].ToString();
            else
                return guidTienda = null;
        }

        /// <summary>
        /// Obtenemos el Id del Cliente mediante el DNI
        /// </summary>
        /// <param name="DNI"></param>
        /// <returns>Obtenemos el Id del Cliente mediante el DNI</returns>
        public string ObtieneClientexDNI(string TipoDocumento, string DNI)
        {
            IOrganizationService servicio;
            servicio = ConexionCRMDA.ObtenerConexion();

            string guidContacto = string.Empty;

            ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("contact", ref servicio);

            fetch.AgregarCampoRetorno("contact", "contactid", ZthFetchXml365.zthFetch.TipoRetorno.Key);

            fetch.AgregarFiltroPlano("contact", ZthFetchXml365.zthFetch.TipoFiltro.and, "sole_tipodocumento", ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, TipoDocumento);
            fetch.AgregarFiltroPlano("contact", ZthFetchXml365.zthFetch.TipoFiltro.and, "sole_numerodocumento", ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, DNI);

            DataTable Dato = new DataTable();
            Dato = fetch.GeneraTblconFetchResult(false);

            if (Dato.Rows.Count > 0)
                return guidContacto = Dato.Rows[0]["contactid"].ToString();
            else
                return guidContacto = null;
        }

        /// <summary>
        /// Actualiza el Estado de la Tabla OportunidadCargaInicial
        /// </summary>
        /// <param name="IdOportunidadCargaInicial">Id a Actualizar</param>
        /// <returns>Actualiza el Estado de la Tabla OportunidadCargaInicial</returns>
        public bool ActualizarEstadoTablaOportunidad(string IdOportunidadCargaInicial)
        {
            using (SqlConnection oSqlConnection = new SqlConnection(ConexionSQLDA.ObtenerConexion()))
            {
                using (SqlCommand oSqlCommand = new SqlCommand("Zth_ActualizaEstadoOportunidad", oSqlConnection))
                {
                    try
                    {
                        {
                            var withBlock = oSqlCommand;
                            withBlock.CommandType = CommandType.StoredProcedure;
                            withBlock.CommandTimeout = 10;
                            withBlock.Parameters.Add("@IdOportunidadCargaInicial", SqlDbType.Int).Value = IdOportunidadCargaInicial;
                            // Explicitamente define el tipo de valor que insertaremos
                            oSqlConnection.Open();
                            withBlock.ExecuteNonQuery();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Obtenemos el Guid del Producto
        /// </summary>
        /// <param name="CodigoProducto">Códgo del Producto</param>
        /// <returns>Obtenemos el Guid del Producto</returns>
        public string ObtieneProductoId(string CodigoProducto)
        {
            IOrganizationService servicio;
            servicio = ConexionCRMDA.ObtenerConexion();

            string guidProducto = string.Empty;

            ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("product", ref servicio);

            fetch.AgregarCampoRetorno("product", "productid", ZthFetchXml365.zthFetch.TipoRetorno.Key);

            fetch.AgregarFiltroPlano("product", ZthFetchXml365.zthFetch.TipoFiltro.and, "productnumber", ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, CodigoProducto);
            // Producto y Numero del producto es igual a Codigo del Producto
            DataTable Dato = new DataTable();
            Dato = fetch.GeneraTblconFetchResult(false);

            if (Dato.Rows.Count > 0)
                return guidProducto = Dato.Rows[0]["productid"].ToString();
            else
                return guidProducto = null;
        }

        /// <summary>
        /// Obtiene el Guid del Usuario
        /// </summary>
        /// <param name="CodigoTienda">Código de la Tienda por Donde Buscaremos al Usuario</param>
        /// <returns>Obtiene el Guid del Usuario</returns>
        public string ObtieneUsuarioIdxTiendaAsignada(string CodigoTienda)
        {
            IOrganizationService servicio;
            servicio = ConexionCRMDA.ObtenerConexion();

            string guidUsuario = string.Empty;

            ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("systemuser", ref servicio);
            // entidad systemuser

            fetch.AgregarCampoRetorno("systemuser", "systemuserid", ZthFetchXml365.zthFetch.TipoRetorno.Key);
            // public void AgregarCampoRetorno(string NombreEntidad, string NombreCampo, TipoRetorno Tipo);

            fetch.AgregarFiltroPlano("systemuser", ZthFetchXml365.zthFetch.TipoFiltro.and, "new_codigotiendaasignada", ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, CodigoTienda);

            DataTable Dato = new DataTable();
            // Instancia tabla en memoria
            Dato = fetch.GeneraTblconFetchResult(false);
            // ??

            if (Dato.Rows.Count > 0)
                return guidUsuario = Dato.Rows[0]["systemuserid"].ToString();
            else
                return guidUsuario = null;
        }

        /// <summary>
        /// Obtiene el Nombre del Producto
        /// </summary>
        /// <param name="CodigoProducto">Busca el Nombre del Producto mediante el Código del Producto</param>
        /// <returns>Devuelve el Nombre del Producto</returns>
        public string ObtieneNombreProducto(string CodigoProducto)
        {
            IOrganizationService servicio;
            servicio = ConexionCRMDA.ObtenerConexion();

            string guidProducto = string.Empty;

            ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("product", ref servicio);

            fetch.AgregarCampoRetorno("product", "name", ZthFetchXml365.zthFetch.TipoRetorno.String);

            fetch.AgregarFiltroPlano("product", ZthFetchXml365.zthFetch.TipoFiltro.and, "productnumber", ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, CodigoProducto);

            DataTable Dato = new DataTable();
            Dato = fetch.GeneraTblconFetchResult(false);

            if (Dato.Rows.Count > 0)
                return guidProducto = Dato.Rows[0]["name"].ToString();
            else
                return guidProducto = null;
        }

        public bool ActualizarEstadoTablaNPS(string IdNPSCargaInicial)
        {
            using (SqlConnection oSqlConnection = new SqlConnection(ConexionSQLDA.ObtenerConexion()))
            {
                using (SqlCommand oSqlCommand = new SqlCommand("Zth_ActualizaEstadoNPS", oSqlConnection))
                {
                    try
                    {
                        {
                            var withBlock = oSqlCommand;
                            withBlock.CommandType = CommandType.StoredProcedure;
                            withBlock.CommandTimeout = 10;
                            withBlock.Parameters.Add("@IdNPSCargaInicial", SqlDbType.Int).Value = IdNPSCargaInicial;

                            oSqlConnection.Open();
                            withBlock.ExecuteNonQuery();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        #endregion
    }
}

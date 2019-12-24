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
    public   class OportunidadesDA
    {
        #region HISTORIA
        //Autor: Carlos C.
        //Fecha: 17/07/2019
        //Notas: Clase Entidad de los Oportunidades
        #endregion

        #region MÉTODOS

        BitacoraErroresBE oBitacoraErroresBE = new BitacoraErroresBE();
        FuncionesDA oFuncionesDA = new FuncionesDA();

        string Ruta = ConfigurationManager.AppSettings["PathLogServicio"].ToString();
        string CorreoSoporte = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["CorreoSoporte"].ToString());
        string CorreoCliente = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["CorreoCliente"].ToString());
        string Aplicativo = "Carga Masiva - SOLE";
        string CorreoClave = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["CorreoClave"].ToString());
        string Host = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["Host"].ToString());
        string Port = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["Port"].ToString());

        /// <summary>
        /// Lista las Oportunidades a Cargar
        /// </summary>
        /// <returns>Lista las Oportunidades a Cargar</returns>
        public DataTable ListadoOportunidadesACargar()
        {

            using (SqlConnection oSqlConnection = new SqlConnection(ConexionSQLDA.ObtenerConexion()))
            {

                using (SqlCommand oSqlCommand = new SqlCommand("Zth_Oportunidad_Carga", oSqlConnection))
                {

                    try
                    {
                        oSqlCommand.CommandType = CommandType.StoredProcedure;
                        oSqlCommand.CommandTimeout = 0;

                        DataTable oDataTable = new DataTable();
                        oSqlConnection.Open();
                        oDataTable.Load(oSqlCommand.ExecuteReader());

                        return oDataTable;
                    }
                    catch (Exception ex)
                    {
                        string Mensaje = "Se ha producido el siguiente error: " + ex.Message;
                        ZthMetodosVarios.Metodos.GuardarLog(Ruta, Mensaje);

                        return null;
                    }
                }
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

            IOrganizationService servicio;
            servicio = ConexionCRMDA.ObtenerConexion();
            SOLE.Opportunity Oportunidad = new SOLE.Opportunity();
            Guid guidOportunidad;

            try
            {
                // con los datos de sql almacenados en BE

                if (oOportunidadBE.Sole_fechasolicitud.ToString() != "01-01-0001 0:00:00" && oOportunidadBE.Sole_fechasolicitud.ToString() != "01-01-1900 0:00:00")
                {
                    Oportunidad.sole_fechasolicitud = Convert.ToDateTime(oOportunidadBE.Sole_fechasolicitud);
                }

                if (oOportunidadBE.Sole_canal != "")
                {
                    Oportunidad.sole_canal = new OptionSetValue(int.Parse(oOportunidadBE.Sole_canal));
                }
                // Referencias a entidades  ue yacen en la entidad principal Oportunidad

                Oportunidad.sole_tiendaid = new EntityReference(SOLE.sole_tienda.EntityLogicalName, Guid.Parse(oOportunidadBE.Sole_tienda));
                Oportunidad.ParentContactId = new EntityReference(SOLE.Contact.EntityLogicalName, ClienteId);
                Oportunidad.sole_ordensolicitud = oOportunidadBE.Sole_ordenservicio;
           
                if (oOportunidadBE.Sole_fechaservicio.ToString() != "01-01-0001 0:00:00" && oOportunidadBE.Sole_fechaservicio.ToString() != "01-01-1900 0:00:00")
                {
                    Oportunidad.sole_fechaservicio = Convert.ToDateTime(oOportunidadBE.Sole_fechaservicio);
                }
                
                if (oOportunidadBE.Sole_tiposervicio != "")
                {
                    Oportunidad.sole_tiposervicio = new OptionSetValue(int.Parse(oOportunidadBE.Sole_tiposervicio));
                }

                if (oOportunidadBE.Sole_empresaqueinstalo != "")
                {
                    Oportunidad.sole_empresainstalo = new OptionSetValue(int.Parse(oOportunidadBE.Sole_empresaqueinstalo));
                }

                Oportunidad.sole_nombretecnico = oOportunidadBE.Sole_nombretecnico;

                if (oOportunidadBE.Sole_fechacompra.ToString() != "01-01-0001 0:00:00" && oOportunidadBE.Sole_fechacompra.ToString() != "01-01-1900 0:00:00")
                {
                    Oportunidad.ActualCloseDate = Convert.ToDateTime(oOportunidadBE.Sole_fechacompra);
                }

                if (oOportunidadBE.Sole_tipocomprobante != null)
                {
                    Oportunidad.sole_tipocomprobante = new OptionSetValue(int.Parse(oOportunidadBE.Sole_tipocomprobante));
                }

                Oportunidad.sole_numerocomprobante = oOportunidadBE.Sole_numerocomprobante;

                if (oOportunidadBE.Sole_metodoventa != null)
                {
                    Oportunidad.sole_metodoventa = new OptionSetValue(int.Parse(oOportunidadBE.Sole_metodoventa));
                }

                Money costoVenta = new Money(oOportunidadBE.Sole_costoventa);
                Oportunidad.sole_costoventa = costoVenta;

                Money utilidadBruta = new Money(oOportunidadBE.Sole_utilidadbruta);
                Oportunidad.sole_utilidadbruta = utilidadBruta;

                Money precioVentaPublico = new Money(oOportunidadBE.Sole_precioventapublico);
                Oportunidad.TotalAmountLessFreight = precioVentaPublico;
                            
                Money ventaNeta = new Money(oOportunidadBE.Sole_ventaneta);
                Oportunidad.TotalAmount = ventaNeta;

                //Como solo hay una Lista de Precio la Traemos de Duro del App.Config
                string listaDePrecios = "";
                listaDePrecios = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["ListaPrecios"].ToString());
                Oportunidad.PriceLevelId = new EntityReference(SOLE.Contact.EntityLogicalName, Guid.Parse(listaDePrecios));

                guidOportunidad = servicio.Create(Oportunidad);

                return guidOportunidad;
            }
            catch (Exception ex)
            {
                oBitacoraErroresBE.Codigo = ex.HResult.ToString();
                oBitacoraErroresBE.Proceso = "Crear Oportunidad";
                oBitacoraErroresBE.Error = ex.Source;
                oBitacoraErroresBE.Descripcion = ex.Message;
                oBitacoraErroresBE.IdRegistro = oOportunidadBE.IdOportunidadCargaInicial;
                oBitacoraErroresBE.Entidad = "Oportunidad";
                oBitacoraErroresBE.EstadoCarga = "0";

                oFuncionesDA.RegistraBitacoraErrores(oBitacoraErroresBE);
                //oFuncionesDA.ActualizarEstadoTablaOportunidad(oOportunidadBE.IdOportunidadCargaInicial);

                string Mensaje = "Error al Crear la Oportunidad. Se ha producido el siguiente error: " + ex.Message;
                ZthMetodosVarios.Metodos.GuardarLog(Ruta, Mensaje);

                //ZthEnvioCorreos_CRM365.Metodos.EnviarCorreoError(CorreoSoporte, CorreoCliente, "Integración MACAL", Ruta, Mensaje, CorreoClave, Port, Host);

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
            IOrganizationService servicio;
            servicio = ConexionCRMDA.ObtenerConexion();
            SOLE.OpportunityProduct ProductoDeLaOportunidad = new SOLE.OpportunityProduct();

            try
            {
                ProductoDeLaOportunidad.PricePerUnit =  new Money(oOportunidadBE.Sole_precioventapublico);
                ProductoDeLaOportunidad.IsProductOverridden = true;
                ProductoDeLaOportunidad.Quantity = new decimal(1);
                ProductoDeLaOportunidad.BaseAmount = new Money(oOportunidadBE.Sole_precioventapublico);  
                ProductoDeLaOportunidad.OpportunityId = new EntityReference(SOLE.Opportunity.EntityLogicalName, OportunidadId);
                ProductoDeLaOportunidad.ProductDescription = oOportunidadBE.Nombre_Producto.ToString();
                //ProductoDeLaOportunidad.ProductId = new EntityReference(SOLE.OpportunityProduct.EntityLogicalName, ProductoId);

                servicio.Create(ProductoDeLaOportunidad);

                return 1;
            }
            catch (Exception ex)
            {
                oBitacoraErroresBE.Codigo = ex.HResult.ToString();
                oBitacoraErroresBE.Proceso = "Crear Producto de la Oportunidad";
                oBitacoraErroresBE.Error = ex.Source;
                oBitacoraErroresBE.Descripcion = ex.Message;
                oBitacoraErroresBE.IdRegistro = IdOportunidadCargaInicial;
                oBitacoraErroresBE.Entidad = "Oportunidad";
                oBitacoraErroresBE.EstadoCarga = "0";

                oFuncionesDA.RegistraBitacoraErrores(oBitacoraErroresBE);
                //oFuncionesDA.ActualizarEstadoTablaOportunidad(IdOportunidadCargaInicial);

                string Mensaje = "Error al Crear el Producto de la Oportunidad. Se ha producido el siguiente error: " + ex.Message;
                ZthMetodosVarios.Metodos.GuardarLog(Ruta, Mensaje);

                //ZthEnvioCorreos_CRM365.Metodos.EnviarCorreoError(CorreoSoporte, CorreoCliente, "Integración MACAL", Ruta, Mensaje, CorreoClave, Port, Host);

                return 0;
            }           
           
        }
                     
        #endregion
    }
}

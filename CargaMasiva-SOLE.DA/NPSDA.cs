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
    public  class NPSDA
    {
        #region HISTORIA
        //Autor: Carlos C.
        //Fecha: 07/08/2019
        //Notas: Clase Entidad de los NPS
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
        /// Lista las NPS A Cargar
        /// </summary>
        /// <returns>Lista las NPS A Cargar</returns>
        public DataTable ListadoNPSACargar()
        {

            using (SqlConnection oSqlConnection = new SqlConnection(ConexionSQLDA.ObtenerConexion()))
            {

                using (SqlCommand oSqlCommand = new SqlCommand("Zth_NPS_Carga", oSqlConnection))
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
        /// Crea el NPS
        /// </summary>
        /// <param name="ClienteId">Id del Cliente al que se le asociará la NPS</param>
        /// <param name="oNPSBE">Datos de la NPS</param>
        /// <returns>Crea el NPS</returns>
        public Guid CrearNPS(Guid ClienteId, NPSBE oNPSBE)
        {

            IOrganizationService servicio;
            servicio = ConexionCRMDA.ObtenerConexion();
            SOLE.sole_nps NPS = new SOLE.sole_nps();
            Guid guidNPS;

            try
            {

                NPS.sole_name = oNPSBE.Sole_numeroencuesta.ToString();
                NPS.sole_codigoencuesta = int.Parse(oNPSBE.Sole_numeroencuesta.ToString());
                NPS.sole_fechaencuesta = oNPSBE.Sole_fechaencuesta;
                NPS.sole_ordenservicio = oNPSBE.Sole_ordenservicio;
                NPS.sole_clienteid = new EntityReference(SOLE.Contact.EntityLogicalName, ClienteId);
                NPS.sole_tipoencuesta = new OptionSetValue(int.Parse(oNPSBE.Sole_tipoencuesta.ToString()));
                NPS.sole_puntuacion = oNPSBE.Sole_puntuacion;
                NPS.sole_comentarios = oNPSBE.Sole_comentarios;

                guidNPS = servicio.Create(NPS);

                return guidNPS;
            }
            catch (Exception ex)
            {
                oBitacoraErroresBE.Codigo = ex.HResult.ToString();
                oBitacoraErroresBE.Proceso = "Crear NPS";
                oBitacoraErroresBE.Error = ex.Source;
                oBitacoraErroresBE.Descripcion = ex.Message;
                oBitacoraErroresBE.IdRegistro = oNPSBE.NPSId;
                oBitacoraErroresBE.Entidad = "NPS";
                oBitacoraErroresBE.EstadoCarga = "0";

                oFuncionesDA.RegistraBitacoraErrores(oBitacoraErroresBE);
                //oFuncionesDA.ActualizarEstadoTablaOportunidad(oOportunidadBE.IdOportunidadCargaInicial);

                string Mensaje = "Error al Crear la Oportunidad. Se ha producido el siguiente error: " + ex.Message;
                ZthMetodosVarios.Metodos.GuardarLog(Ruta, Mensaje);

                //ZthEnvioCorreos_CRM365.Metodos.EnviarCorreoError(CorreoSoporte, CorreoCliente, "Integración MACAL", Ruta, Mensaje, CorreoClave, Port, Host);

                throw ex;
            }
        }

        #endregion
    }
}

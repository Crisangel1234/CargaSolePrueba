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
using ZthFetchXml365;

namespace CargaMasiva_SOLE.DA
{
    public  class ContactoDA
    {
        #region HISTORIA
        //Autor: Carlos C.
        //Fecha: 16/07/2019
        //Notas: Clase Entidad de los Contactos
        #endregion

        #region MÉTODOS

        // Instancia de BitacoraErroesBE
        BitacoraErroresBE oBitacoraErroresBE = new BitacoraErroresBE();
        // Instancia de FuncionesDA
        FuncionesDA oFuncionesDA = new FuncionesDA();

        /**
         * 
         * Área de desencriptación y Definición de Variables
         * 
         * */



        #region Definición de Variables
        string Ruta = ConfigurationManager.AppSettings["PathLogServicio"].ToString();
        // Asignamos la ruta del log a la variable Ruta, en string
        string CorreoSoporte = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["CorreoSoporte"].ToString());
        // Asignamos el campo desencriptado a la variable CorreoSoporte, en string
        string CorreoCliente = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["CorreoCliente"].ToString());
        // Asignamos el campo desencriptado a la variable CorreoCliente, en string
        string Aplicativo = "Carga Masiva - SOLE";
        // Nombre de la aplicación
        string CorreoClave = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["CorreoClave"].ToString());
        // Asignamos el campo desencriptado a la variable CorreoClave, en string
        string Host = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["Host"].ToString());
        // Asignamos el campo desencriptado a la variable Host, en string
        string Port = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["Port"].ToString());
        // Asignamos el campo desencriptado a la variable Port, en string
        #endregion


        /// <summary>
        /// Listado de Contactos a Cargar
        /// </summary>
        /// <returns>Listado de Contactos a Cargar</returns>
        public DataTable ListadoContactosACargar() 
        {

            using (SqlConnection oSqlConnection = new SqlConnection(ConexionSQLDA.ObtenerConexion())) 
                // Establecemos la conexión a SQL server, entregamos por parámetro a la cadena de conexión (dConexion)
            {

                using (SqlCommand oSqlCommand = new SqlCommand("Zth_Contacto_Carga", oSqlConnection))
                {     // entrega como parametro la instancia de conexión de la BD , parámetro de crm, 
                     //Zth_Contacto_Carga es un procedimiento almacenado, una consulta para cargar los contactos
                   

                    try
                    {    // Implementaciones de SqlCommand, Asignamos al nombre del procedimiento almacenado
                        
                        oSqlCommand.CommandType = CommandType.StoredProcedure; 
                        // CommandType especifica que tipo de comando sql es 
                        oSqlCommand.CommandTimeout = 0;
                        // define el tiempo de espera antes de ejecutar un error, por ejemplo: Si definieramos 30 segundos, esperaria 30 segundos antes de saltar alguna excepción 
                        // Un valor de 0 indica que no hay límite (un intento de ejecutar un comando esperará indefinidamente)

                        DataTable oDataTable = new DataTable();
                          // abre la conexión,  Open() Before Load();
                        oSqlConnection.Open();
                         // Ejecuta el Procedimiento almacenado 
                        oDataTable.Load(oSqlCommand.ExecuteReader());
                        // ExcecuterReader utilizado para obtener resultados de una consulta 

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
        /// Validamos si Existe el Contacto Mediante su DNI 
        /// </summary>
        /// <param name="DNI">DNI a Validar</param>
        /// <returns>Validamos si Existe el Contacto Mediante su DNI</returns>
        public DataTable ValidaExisteContacto(string TipoDeDocumento, string NumeroDocumento)
        {
            try

            {
                // Resumen:
                //         Cadena de conexión  ----> CrmServiceClient ( Set up de las Credenciales) ---->  IOrganizationServices (Web Service Intermedio) ----> CRM online 2016

                IOrganizationService servicio; // Instancia de IOrgnizationService
                servicio = ConexionCRMDA.ObtenerConexion(); // Obtenemos la Conexión de CRM 

                /**
                 * 
                 * FetchXML, se utiliza para una busqueda avanzada 
                 *  
                 * **/

                // Ocupamos el Espacio de Nombre "ZthFetchXml365"
                // Instancia de fetch, se ocupa para manipular los datos del CRM :
                //Le Entregamos el nombre de la entidad principal a buscar(Contacto, y la referencia al servicio web de CRM)
                zthFetch fetch = new zthFetch("contact", ref servicio);


                // Fetch contiene métodos parámetrizados, cuando me meta mas al CRM lo voy a entender mejor

                
                fetch.AgregarCampoRetorno("contact", "contactid", ZthFetchXml365.zthFetch.TipoRetorno.Key);
                //    public void AgregarCampoRetorno(string NombreEntidad, string NombreCampo, TipoRetorno Tipo);
                fetch.AgregarFiltroPlano("contact", ZthFetchXml365.zthFetch.TipoFiltro.and, "sole_tipodocumento", ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, TipoDeDocumento);
                //     public void AgregarFiltroPlano(string NombreEntidad, TipoFiltro TipoFiltro, string NombreAtributo, TipoComparacionFiltro TipoComparacion, string ValorAtributo);
                //     Tipo Documento
                fetch.AgregarFiltroPlano("contact", ZthFetchXml365.zthFetch.TipoFiltro.and, "sole_numerodocumento", ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual, NumeroDocumento);
                //     Numero Documento
                fetch.AgregarCantidadRegistrosDevolver_puedesermenorque5000(1);

                //  Crea un objeto de tipo  datatable
                DataTable Dato = new DataTable();

                // Genera la tabla con el resultado del Fetch y lo llena en la tabla de datos en memoria
                
                Dato = fetch.GeneraTblconFetchResult(false); // ¿Por que false?

                return Dato;
            }
            catch (Exception ex)

            {
                // Mensaje de la excepción 
                string Mensaje = "Se ha producido el siguiente error: " + ex.Message;
                // DLL guarda el mensaje de excepción en un registro
                ZthMetodosVarios.Metodos.GuardarLog(Ruta, Mensaje);
                // Lo mandan a un correo compartido en outlook, correo soporte 
                ZthEnvioCorreos_CRM365.Metodos.EnviarCorreoError(CorreoSoporte, CorreoCliente, Aplicativo, Ruta, Mensaje, CorreoClave, Port, Host);

                throw ex;
            }
        }

        /// <summary>
        /// Actualiza los Datos del Contactogithub
        /// </summary>
        /// <param name="guidContacto">Guid del Contacto a Actualizar</param>
        /// <param name="oContactoBE">Datos del Contacto a Actualizar</param>
        /// <returns>Actualiza los Datos del Contacto</returns>
        public Boolean ActualizaContacto(string guidContacto, ContactoBE oContactoBE)
        {

            IOrganizationService servicio;
            // Instancia del servicio 
            servicio = ConexionCRMDA.ObtenerConexion();
            // asginamos la cadena de conexión
            SOLE.Contact Contacto = new SOLE.Contact();
            // Instancia de la entidad Contacto

            try
            {
                // guid esta dentro del crm
                Contacto.ContactId = Guid.Parse(guidContacto);
                /*
                  Resumen:
                          Asignamos campos de BE a Campos de la entidad contacto de CRM (Campos de BE contienen los datos de SQL Server)
                          Actualizamos el contacto del CRM
                    */
                Contacto.FirstName = oContactoBE.Sole_nombres;
                Contacto.LastName = oContactoBE.Sole_apellidos;
                Contacto.Telephone1 = oContactoBE.Sole_telefono1;
                Contacto.EMailAddress1 = oContactoBE.Sole_correoelectronico1;
                //if (oContactoBE.Sole_fechanacimiento.ToString() != "01-01-0001 0:00:00")
                //{
                //    Contacto.BirthDate = oContactoBE.Sole_fechanacimiento;
                //}
                //Contacto.GenderCode = new OptionSetValue((int)oContactoBE.Sole_sexo);
                Contacto.Telephone2 = oContactoBE.Sole_telefono2;
                Contacto.Telephone3 = oContactoBE.Sole_telefono3;
                Contacto.EMailAddress2 = oContactoBE.Sole_correoelectronico2;
                Contacto.EMailAddress3 = oContactoBE.Sole_correoelectronico3;
                //Contacto.sole_estilovida = new OptionSetValue((int)oContactoBE.Sole_estilovida);
                //Contacto.sole_ocupacion = new OptionSetValue((int)oContactoBE.Sole_ocupacion);
                //Contacto.sole_perfilcliente = new OptionSetValue((int)oContactoBE.Sole_perfilcliente);
                //Contacto.sole_tipoclientepotencial = new OptionSetValue((int)oContactoBE.Sole_tipoclientepotencial);
                //Contacto.sole_fuenteorigen = new OptionSetValue((int)oContactoBE.Sole_fuenteorigen);
                //Contacto.sole_tipofuenteorigen = new OptionSetValue((int)oContactoBE.Sole_fuenteorigen);
                //if (oContactoBE.Sole_fechaconversion.ToString() != "01-01-0001 0:00:00")
                //{
                //    Contacto.sole_fechaconversion = oContactoBE.Sole_fechaconversion;
                //}
                Contacto.sole_interesbano = oContactoBE.Sole_interesbano;
                Contacto.sole_interescocina = oContactoBE.Sole_interescocina;
                Contacto.sole_interesdescanso = oContactoBE.Sole_interesdescanso;
                if (oContactoBE.Sole_estadoSole != 0)
                {
                    Contacto.sole_estadosole = new OptionSetValue((int)oContactoBE.Sole_estadoSole);
                }
                //if (oContactoBE.Sole_fechanuevo.ToString() != "01-01-0001 0:00:00")
                //{
                //    Contacto.sole_fechanuevo = oContactoBE.Sole_fechanuevo;
                //}
                //if (oContactoBE.Sole_fechaactivo.ToString() != "01-01-0001 0:00:00")
                //{
                //    Contacto.sole_fechaactivo = oContactoBE.Sole_fechaactivo;
                //}
                //if (oContactoBE.Sole_fechaincativo.ToString() != "01-01-0001 0:00:00")
                //{
                //    Contacto.sole_fechainactivo = oContactoBE.Sole_fechaincativo;
                //}
                Contacto.DoNotEMail = oContactoBE.Sole_permissionmarketingcorreo;
                Contacto.DoNotPhone = oContactoBE.Sole_permissionmarketingtelefono;
                Contacto.sole_departamentoid = new EntityReference(SOLE.sole_departamento.EntityLogicalName, Guid.Parse(oContactoBE.Sole_departamento));
                Contacto.sole_provinciaid = new EntityReference(SOLE.sole_provincia.EntityLogicalName, Guid.Parse(oContactoBE.Sole_provincia));
                 
                Contacto.sole_distritoid = new EntityReference(SOLE.sole_distrito.EntityLogicalName, Guid.Parse(oContactoBE.Sole_distrito));
                Contacto.Address1_Name = oContactoBE.Sole_direccion;
                Contacto.sole_zona = new OptionSetValue((int)oContactoBE.Sole_zona);

                if (oContactoBE.Sole_nse != 0)
                {
                    Contacto.sole_nse = new OptionSetValue((int)oContactoBE.Sole_nse);
                }
                return true;
            }
            catch (Exception ex)


            {

                /*
                 

                 Guardamos los datos en la variables de BE de la clase bitácora errores

                 */
                oBitacoraErroresBE.Codigo = ex.HResult.ToString(); 
                // La asignacion de un identificador 
                oBitacoraErroresBE.Proceso = "Actualizar Contacto";
                // Nombre del proceso
                oBitacoraErroresBE.Error = ex.Source;
                // Fuente de error
                oBitacoraErroresBE.Descripcion = ex.Message;
                // Mensaje descriptivo de la excepción
                oBitacoraErroresBE.IdRegistro = oContactoBE.ContactoId;
                // ID donde se haya el error
                oBitacoraErroresBE.Entidad = "Contacto";
                // entidad del error
                oBitacoraErroresBE.EstadoCarga = "0";
                // Carga cero

                oFuncionesDA.RegistraBitacoraErrores(oBitacoraErroresBE);
                //oFuncionesDA.ActualizarEstadoTablaContacto(oContactoBE.ContactoId);

                string Mensaje = "Error al Actualizar el Contacto. Se ha producido el siguiente error: " + ex.Message;
                ZthMetodosVarios.Metodos.GuardarLog(Ruta, Mensaje);

                //ZthEnvioCorreos_CRM365.Metodos.EnviarCorreoError(CorreoSoporte, CorreoCliente, "Integración MACAL", Ruta, Mensaje, CorreoClave, Port, Host);

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

            IOrganizationService servicio;
            servicio = ConexionCRMDA.ObtenerConexion();
            SOLE.Contact Contacto = new SOLE.Contact();
            Guid guidContacto; // ??? del tipo Guid

            try
            {
                Contacto.FirstName = oContactoBE.Sole_nombres;
                Contacto.LastName = oContactoBE.Sole_apellidos;
                Contacto.sole_tipodocumento = new OptionSetValue((int)oContactoBE.Sole_tipodocumento);
                Contacto.sole_numerodocumento = oContactoBE.Sole_numerodocumento;

                if (oContactoBE.Sole_tipoContacto == 365230000)
                {
                    Contacto.sole_estadosole = new OptionSetValue((int)oContactoBE.Sole_estadoSole);
                }

                Contacto.sole_tipocontacto = new OptionSetValue((int)oContactoBE.Sole_tipoContacto);


                Contacto.Telephone1 = oContactoBE.Sole_telefono1;
                Contacto.EMailAddress1 = oContactoBE.Sole_correoelectronico1;
                //if (oContactoBE.Sole_fechanacimiento.ToString() != "01-01-1900 0:00:00" && oContactoBE.Sole_fechanacimiento.ToString() != "01-01-0001 0:00:00")
                //{
                //    Contacto.BirthDate = oContactoBE.Sole_fechanacimiento;
                //}
                Contacto.Telephone2 = oContactoBE.Sole_telefono2;
                Contacto.Telephone3 = oContactoBE.Sole_telefono3;
                Contacto.EMailAddress2 = oContactoBE.Sole_correoelectronico2;
                Contacto.EMailAddress3 = oContactoBE.Sole_correoelectronico3;
                //Contacto.GenderCode = new OptionSetValue((int)oContactoBE.Sole_sexo);
                //Contacto.sole_estilovida = new OptionSetValue((int)oContactoBE.Sole_estilovida);
                //Contacto.sole_ocupacion = new OptionSetValue((int)oContactoBE.Sole_ocupacion);
                //Contacto.sole_perfilcliente = new OptionSetValue((int)oContactoBE.Sole_perfilcliente);
                //Contacto.sole_fuenteorigen = new OptionSetValue((int)oContactoBE.Sole_fuenteorigen);
                //Contacto.sole_tipofuenteorigen = new OptionSetValue((int)oContactoBE.Sole_fuenteorigen);
                if (oContactoBE.Sole_tipoclientepotencial != 0)
                {
                    Contacto.sole_tipoclientepotencial = new OptionSetValue((int)oContactoBE.Sole_tipoclientepotencial);
                }
                //if (oContactoBE.Sole_fechaconversion.ToString() != "01-01-1900 0:00:00" && oContactoBE.Sole_fechaconversion.ToString() != "01-01-0001 0:00:00")
                //{
                //    Contacto.sole_fechaconversion = oContactoBE.Sole_fechaconversion;
                //}
                Contacto.sole_interesbano = oContactoBE.Sole_interesbano;
                Contacto.sole_interescocina = oContactoBE.Sole_interescocina;
                Contacto.sole_interesdescanso = oContactoBE.Sole_interesdescanso;
                //if (oContactoBE.Sole_estadoSole != 0)
                //{
                //    Contacto.sole_estadosole = new OptionSetValue((int)oContactoBE.Sole_estadoSole);
                //}
                //if (oContactoBE.Sole_fechanuevo.ToString() != "01-01-1900 0:00:00" && oContactoBE.Sole_fechanuevo.ToString() != "01-01-0001 0:00:00")
                //{
                //    Contacto.sole_fechanuevo = oContactoBE.Sole_fechanuevo;
                //}
                //if (oContactoBE.Sole_fechaactivo.ToString() != "01-01-1900 0:00:00" && oContactoBE.Sole_fechaactivo.ToString() != "01-01-0001 0:00:00")
                //{
                //    Contacto.sole_fechaactivo = oContactoBE.Sole_fechaactivo;
                //}
                //if (oContactoBE.Sole_fechaincativo.ToString() != "01-01-1900 0:00:00" && oContactoBE.Sole_fechaincativo.ToString() != "01-01-0001 0:00:00")
                //{
                //    Contacto.sole_fechainactivo = oContactoBE.Sole_fechaincativo;
                //}
                                           
                Contacto.DoNotEMail = oContactoBE.Sole_permissionmarketingcorreo;
                Contacto.DoNotPhone = oContactoBE.Sole_permissionmarketingtelefono;
                Contacto.sole_departamentoid = new EntityReference(SOLE.sole_departamento.EntityLogicalName, Guid.Parse(oContactoBE.Sole_departamento));
                Contacto.sole_provinciaid = new EntityReference(SOLE.sole_provincia.EntityLogicalName, Guid.Parse(oContactoBE.Sole_provincia));
                Contacto.sole_distritoid = new EntityReference(SOLE.sole_distrito.EntityLogicalName, Guid.Parse(oContactoBE.Sole_distrito));
                Contacto.Address1_Name = oContactoBE.Sole_direccion;

                //Validamos si el Campo Sole Tienda Asignada tiene Datos
                if (oContactoBE.Sole_tienda != "" )
                {
                    //Como Tiene Datos, buscamos el Guid del Usuario de la Tienda Asignada
                    string guidUsuario;
                    guidUsuario = oFuncionesDA.ObtieneUsuarioIdxTiendaAsignada(oContactoBE.Sole_tienda);
                    // Busca el id del usuario por la tienda asignada

                    if (guidUsuario != "")
                    {
                        Contacto.OwnerId = new EntityReference("systemuser", Guid.Parse(guidUsuario)); 
                        // Referncia de una entidad que esta relacionada con Contacto

                        guidContacto = servicio.Create(Contacto);
                        // ocupamos uno de los metodos de IOrganizationServices

                        return guidContacto;
                    }
                    else
                    {
                        guidContacto = servicio.Create(Contacto);

                        return guidContacto;
                    }
                }
                else
                {
                    guidContacto = servicio.Create(Contacto);

                    return guidContacto;
                }


            }
            catch (Exception ex)
            {
                oBitacoraErroresBE.Codigo = ex.HResult.ToString();
                oBitacoraErroresBE.Proceso = "Crear Contacto";
                oBitacoraErroresBE.Error = ex.Source;
                oBitacoraErroresBE.Descripcion = ex.Message;
                oBitacoraErroresBE.IdRegistro = oContactoBE.ContactoId;
                oBitacoraErroresBE.Entidad = "Contacto";
                oBitacoraErroresBE.EstadoCarga = "0";

                oFuncionesDA.RegistraBitacoraErrores(oBitacoraErroresBE);
                //oFuncionesDA.ActualizarEstadoTablaContacto(oContactoBE.ContactoId);

                string Mensaje = "Error al Crear el Contacto. Se ha producido el siguiente error: " + ex.Message;
                ZthMetodosVarios.Metodos.GuardarLog(Ruta, Mensaje);

                //ZthEnvioCorreos_CRM365.Metodos.EnviarCorreoError(CorreoSoporte, CorreoCliente, "Integración MACAL", Ruta, Mensaje, CorreoClave, Port, Host);

                throw ex;
            }
        }

        #endregion
    }
}

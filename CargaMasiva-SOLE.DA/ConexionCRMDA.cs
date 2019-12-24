using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZthSeguridad;

namespace CargaMasiva_SOLE.DA
{

    /*
     * 
     *  Creamos la clase con el Nombre ConexiónCRMDA  
     * 
     */

    public  class ConexionCRMDA
    {
        // SDK  =  SOFTWARE DEVELOPMENT KIT
        static IOrganizationService _orgService; // Interface Proveniente del DLL Microsoft.Xrm.Sdk 

        /*
         * La interface IOrganizationService tiene una coleccion de Acciones 
         * En Dynamics 365 for Customer Engagement, el servicio web principal que tiene acceso a los datos
         * y metadatos de la organización es IOrganizationService.
         */

        /// <summary>
        /// Static (Miembro de una clase) = La definición formal de los elementos estáticos (o miembros de clase) nos dice que son aquellos que pertenecen a la clase, 
        /// en lugar de pertenecer a un objeto en particular 
        /// 1. Son miembros que existen dentro de la propia clase
        /// 2. En vez de acceder a traves de un objeto, accedemos a travez del nombre de la clase 
        /// 
        /// </summary>

        static public string connection = ""; // Declara variable vacía de una conexión
        static public string usuarioCRM = Metodos.Desencriptar(ConfigurationManager.AppSettings["Usuario_CRM"]); 
        // accede al dato duro o key para poder desencriptar el Usuario CRM ( Halladas en App.config) y lo asigna a la variable
        static public string claveCRM = Metodos.Desencriptar(ConfigurationManager.AppSettings["Clave_CRM"]);
        // accede al dato duro o key de claveCRM para poder desencriptar la Clave CRM y lo asigna a la variable
        static public string deviceId = ""; // ??
        static public string deviceClave = ""; //??
        static public string urlCRM = Metodos.Desencriptar(ConfigurationManager.AppSettings["URL_CRM"]);
        //accede al dato duro o key para desencriptar la URL del CRM  y lo asigna a la variable
        /*
        
             * Instanciamos los parametros como la contraseña en app.config y los invocamos en la clase para su posterior uso   * 
         
         */



        
        static CrmServiceClient conn = null; // Invocamos la Conexión y creamos el campo conn*, la variable conn es de tipo crmServiceClient, solo hay una copia de cada campo estático 
        
        public static IOrganizationService ObtenerConexion() // Método estático, del tipo organization services 
        {
            
            try
            {
                 

                // Utilizamos el metodo string.format() , convierte el valor de los objetos a string en el formato deseado
                connection = string.Format(ConfigurationManager.AppSettings["Connection_CRM"], urlCRM, usuarioCRM, claveCRM);

                // Output: "Url={0}; Username={1};Password={2};AuthType=Office365;RequireNewInstance=True;"

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 
                // Definicion del protocolo de seguridad de la capa de transporte 

                // Credenciales para conectarse a la instancia especifica de CRM online 
                conn = new CrmServiceClient(connection); // con connection entregamos los parametros de conexión formateadas anteriormente

                // Entregamos la conexión formateada

                //Validamos si se pudo realizar la Conexión
                if (conn.IsReady)
                {
                    
                    _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService) // respondio datos o fue null
                    conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy; // set up de la variable
                    conn.Dispose(); // Definida en la Interface Disposable ( desechar), su funcion es Liberar Recursos 
                    conn = null; // reasignamos la variable a null
                    return _orgService; // retornamos la conexión 
                }
                else
                {
                    conn.Dispose();
                    conn = null;
                    return _orgService;
                }
            }
            catch (Exception ex)
            {

                // Implementación del DLL
                // Guarda en la variable ruta, La ruta de registro del servicio
                string ruta = ConfigurationManager.AppSettings["PathLogServicio"];
                // Utiliza el .dll ZthMetodosVarios 
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                // Guarda el registro en ruta; Se ha producido el siguiente error + el mensaje que arroja la excepción
                throw new Exception(ex.Message.ToString());
            }

        }

    }
}

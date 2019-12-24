using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaMasiva_SOLE.DA
{
    public  class ConexionSQLDA
    {
       
        public static string ObtenerConexion()
        {
           
            try


            {

                /*Variables Vacías*/

                string cadenaEncriptada = "";   
                // Variable que recibira la cadena encriptada
                string cadenaDesencriptada = "";
                // Variable que recibira la cadena desencriptada

                cadenaEncriptada = ConfigurationManager.ConnectionStrings["Activa"].ConnectionString; 
                // Invocamos y asignamos, establecemos la cadena de conexión 
                cadenaDesencriptada = ZthSeguridad.Metodos.Desencriptar(cadenaEncriptada);
                // Entregamos la cadena de conexión encriptada y aplicamos el metodo de la dll ZthSeguridad desencriptar, asginamos el valor desencriptada a la variable

                string dConexion = cadenaDesencriptada; // El connection string la asignamos a una variable mas descriptiva


                /*
                 
                 El If else está comparando la conexión con una entrada vacía, por ende si entra empty retorna null
                 Por otro lado, si dConexion regresa con la cadena de conexión, la retornar sin problemas 
                 
                 Este método retonar true si el obj1 es igual a la instancia del obj2, si ambos son nulos retonar false 
                
                 */
                 

                if (object.ReferenceEquals(dConexion /*Objeto N°1*/, string.Empty /*Objeto N°2 */ ))
                {
                    return null;
                }
                else
                {
                    return dConexion;
                }
            }
            catch (Exception ex)
            {
                string RutaLog = ConfigurationManager.AppSettings["PathLogServicio"].ToString();
                string Mensaje = "Error en la conexión con el servidor de Base de Datos: " + ex.Message;
                ZthMetodosVarios.Metodos.GuardarLog(RutaLog, Mensaje);

                return null;
            }
        }
    }
}

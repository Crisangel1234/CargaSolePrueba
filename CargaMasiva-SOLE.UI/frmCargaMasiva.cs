using CargaMasiva_SOLE.BE;
using CargaMasiva_SOLE.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CargaMasiva_SOLE.UI
{
    public partial class frmCargaMasiva : Form
    {

        BitacoraErroresBE oBitacoraErroresBE = new BitacoraErroresBE(); // Business Entities

        ContactoBE oContactoBE = new ContactoBE(); // Business Entities
        OportunidadesBE oOportunidadesBE = new OportunidadesBE(); //  Business Entities
        NPSBE oNPSBE = new NPSBE(); // Business Entities


        FuncionesBL oFuncionesBL = new FuncionesBL(); // Business Logic

        ContactoBL oContactoBL = new ContactoBL(); // Business Logic
        OportunidadesBL oOportunidadesBL = new OportunidadesBL(); // Business Logic
        NPSBL oNPSBL = new NPSBL(); //Business Logic
        string Mensaje = "";

        /*
         
            Declaración de variables y desencriptación 

             */

        string ruta = ConfigurationManager.AppSettings["PathLogServicio"];
        string CorreoSoporte = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["CorreoSoporte"].ToString());
        string CorreoCliente = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["CorreoCliente"].ToString());
        string Aplicativo = "Carga Masiva - SOLE";
        string CorreoClave = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["CorreoClave"].ToString());
        string Host = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["Host"].ToString());
        string Port = ZthSeguridad.Metodos.Desencriptar(ConfigurationManager.AppSettings["Port"].ToString());
        
        public frmCargaMasiva()
        {
            InitializeComponent();
        }

        private void frmCargaMasiva_Load(object sender, EventArgs e)
        {
            IniciarCargaMasiva();
            this.Close();
        }

        private void btnIniciarCarga_Click(object sender, EventArgs e)
        {
            //IniciarCargaMasiva();
            //this.Close();
        }

        private void IniciarCargaMasiva()
        {
            //Cargamos los Contactos
            CargarContactos();   // esta listo 

            //Cargamos las Oportunidades
            CargarOportunidades();

            ///Cargamos los NPS
            CargarNPS();
        }

        private void CargarContactos()
        {
            ZthMetodosVarios.Metodos.GuardarLog(ruta, "--- Iniciamos el proceso de Carga Masiva de los Contactos " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "---");
            // Guardar Registro

            DataTable contactosxCargar; // variable local que representa las tablas de las bases de datos 

            contactosxCargar = oContactoBL.ListadoContactosACargar();// Instancia de contactoBL, accedemos a un método 
            // a contactosxCargar Asignamos 

            if (contactosxCargar.Rows.Count > 0)
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Contactos por cargar: " + contactosxCargar.Rows.Count);
            
                // Tipo , de que tipo, en el objeto contactosxCargar.Rows
                // DataRow representa una fila de datos, dtrow del tipo DataRow, en el objeto que recorremos
                foreach (DataRow dtRow in contactosxCargar.Rows)
                {
                    try
                    {
                        // iNSERTAMOS LA DATA DE LAS TABLAS DE SQL SERVER EN LAS PROPIEDADES ( ATRIBUTOS O VARIABLES ) DE LA CAPA DE BE

                        oContactoBE.ContactoId = dtRow["IdContactoCargaInicial"].ToString(); 
                        oContactoBE.Sole_nombres = dtRow["sole_nombres"].ToString();
                        oContactoBE.Sole_apellidos = dtRow["sole_apellidos"].ToString();
                        oContactoBE.Sole_telefono1 = dtRow["sole_telefono1"].ToString();
                        oContactoBE.Sole_telefono2 = dtRow["sole_telefono2"].ToString();
                        oContactoBE.Sole_telefono3 = dtRow["sole_telefono3"].ToString();
                        oContactoBE.Sole_correoelectronico1 = dtRow["sole_correoelectronico1"].ToString();
                        oContactoBE.Sole_correoelectronico2 = dtRow["sole_correoelectronico2"].ToString();
                        oContactoBE.Sole_correoelectronico3 = dtRow["sole_correoelectronico3"].ToString();
                        oContactoBE.Sole_tipodocumento = (int)dtRow["crm_tipodocumento"];
                        oContactoBE.Sole_numerodocumento = dtRow["sole_numerodocumento"].ToString();
                        oContactoBE.Sole_direccion = dtRow["sole_direccion"].ToString();
                        oContactoBE.Sole_departamento = oFuncionesBL.ObtieneDepartamento(dtRow["sole_departamento"].ToString());
                        oContactoBE.Sole_provincia = oFuncionesBL.ObtieneProvincia(dtRow["sole_provincia"].ToString());
                        oContactoBE.Sole_distrito = oFuncionesBL.ObtieneDistrito(dtRow["sole_distrito"].ToString());
                        oContactoBE.Sole_zona = (int)dtRow["crm_zona"];
                        //oContactoBE.Sole_sexo = (int)dtRow["crm_sexo"];
                        //oContactoBE.Sole_fechanacimiento = (DateTime)dtRow["sole_fechanacimiento"];
                        //oContactoBE.Sole_ocupacion = (int)dtRow["crm_ocupacion"];
                        oContactoBE.Sole_tipoContacto = (int)dtRow["crm_tipocontacto"];
                        oContactoBE.Sole_tipoclientepotencial = (int)dtRow["crm_tipoclientepotencial"];
                        oContactoBE.Sole_estadoSole = (int)dtRow["crm_estadocliente"];
                        //oContactoBE.Sole_perfilcliente = (int)dtRow["crm_perfilcliente"];
                        oContactoBE.Sole_categorizacioncliente = (int)dtRow["crm_categorizacioncliente"];
                        //oContactoBE.Sole_estilovida = (int)dtRow["crm_estilovida"];
                         oContactoBE.Sole_nse = (int)dtRow["crm_nse"];
                        oContactoBE.Sole_permissionmarketingcorreo = (bool)dtRow["sole_permissionmarketingcorreo"];
                        oContactoBE.Sole_permissionmarketingtelefono = (bool)dtRow["sole_permissionmarketingtelefono"];
                        oContactoBE.Sole_tienda = dtRow["sole_tienda_asignada"].ToString();

                        //Preguntamos si Existe el DNI
                        DataTable existeContacto = new DataTable();
                        existeContacto = oContactoBL.ValidaExisteContacto(oContactoBE.Sole_tipodocumento.ToString(), oContactoBE.Sole_numerodocumento);

                        if (existeContacto.Rows.Count > 0) // cuenta las filas, si es mayor a cero, entonces hay datos en una fila,  por ende, solo actualizamos
                        {
                            //Como el Contacto Existe, Actualizamos sus Datos
                            DataRow row = existeContacto.Rows[0];

                            string guidContacto;

                            guidContacto = row["contactid"].ToString();

                            oContactoBL.ActualizaContacto(guidContacto, oContactoBE);
                            oFuncionesBL.ActualizarEstadoTablaContacto(oContactoBE.ContactoId);

                            ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se actualizo en el CRM el Contacto con DNI: " + oContactoBE.Sole_numerodocumento + " y el GUID es: " + guidContacto);
                        }
                        else
                        {
                            //Como el Contacto No Existe, lo Creamos
                            Guid guidContacto;

                            // Pasamos por business Logic
                            guidContacto = oContactoBL.CrearContacto(oContactoBE);
                            oFuncionesBL.ActualizarEstadoTablaContacto(oContactoBE.ContactoId);

                            ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se creo en el CRM el Contacto con DNI: " + oContactoBE.Sole_numerodocumento + " y el GUID es: " + guidContacto);
                        }
                    }
                    catch (Exception ex)
                    {
                        string Mensaje = "Se ha producido el siguiente error: " + ex.Message;
                        ZthMetodosVarios.Metodos.GuardarLog(ruta, Mensaje);

                        //ZthEnvioCorreos_CRM365.Metodos.EnviarCorreoError(CorreoSoporte, CorreoCliente, Aplicativo, ruta, Mensaje, CorreoClave, Port, Host);

                        continue;
                    }

                }
            }
            else
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "No hay Contactos por cargar.");
            }

        }

        private void CargarOportunidades()
        {
            ZthMetodosVarios.Metodos.GuardarLog(ruta, "--- Iniciamos el proceso de Carga Masiva de las Oportunidades" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "---");

            DataTable oportunidadesxCargar;

            oportunidadesxCargar = oOportunidadesBL.ListadoOportunidadesACargar(); // lee y consulta

            if (oportunidadesxCargar.Rows.Count > 0)
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Oportunidades por cargar: " + oportunidadesxCargar.Rows.Count);

                foreach (DataRow dtRow in oportunidadesxCargar.Rows) // asigna
                {
                    try
                    {
                        // Sacamos los datos de SQL ---> BE
                        oOportunidadesBE.IdOportunidadCargaInicial = dtRow["IdOportunidadCargaInicial"].ToString();
                        oOportunidadesBE.Sole_codigoproducto = dtRow["sole_codigoproducto"].ToString();

                        string validaCodigoProducto;
                        // variable local para validar codigo de producto 
                        validaCodigoProducto = oFuncionesBL.ObtieneProductoId(oOportunidadesBE.Sole_codigoproducto);
                        // Ingresamos al metodo de paso de Business Logic, asignamos el resultado a la variable

                        if (validaCodigoProducto == null)
                        {
                            oBitacoraErroresBE.Codigo = dtRow["sole_codigoproducto"].ToString(); //     Fila del problema
                            oBitacoraErroresBE.Proceso = "Crear Oportunidad";
                            oBitacoraErroresBE.Error = "Error es el código de Producto";
                            oBitacoraErroresBE.Descripcion = "No se encontro el código de Producto en el CRM. El código del Producto que se busco es: " + dtRow["sole_codigoproducto"].ToString();
                            oBitacoraErroresBE.IdRegistro = oOportunidadesBE.IdOportunidadCargaInicial;
                            oBitacoraErroresBE.Entidad = "Oportunidad";
                            oBitacoraErroresBE.EstadoCarga = "0";

                            oFuncionesBL.RegistraBitacoraErrores(oBitacoraErroresBE);
                            oFuncionesBL.ActualizarEstadoTablaOportunidad(oOportunidadesBE.IdOportunidadCargaInicial);

                            string Mensaje = "No se encontro el código de Producto en el CRM. El código del Producto que se busco es: " + dtRow["sole_codigoproducto"].ToString();
                            ZthMetodosVarios.Metodos.GuardarLog(ruta, Mensaje);

                            continue;
                        }

                        oOportunidadesBE.Sole_ordenservicio = dtRow["sole_ordenservicio"].ToString();

                        if (dtRow["sole_fechasolicitud"] != DBNull.Value)
                        {
                            oOportunidadesBE.Sole_fechasolicitud = Convert.ToDateTime(dtRow["sole_fechasolicitud"].ToString()); //zth_fecharespuestainicial
                        }

                        oOportunidadesBE.Sole_serieproducto = dtRow["sole_serieproducto"].ToString();

                        if (oOportunidadesBE.Sole_codigoproducto == "")
                        {
                            ZthMetodosVarios.Metodos.GuardarLog(ruta, "No Existe el Código del Producto. El Producto Carga Masiva Id es: " + oOportunidadesBE.IdOportunidadCargaInicial);
                            continue;
                        }

                        oOportunidadesBE.Nombre_Producto = oFuncionesBL.ObtieneNombreProducto(oOportunidadesBE.Sole_codigoproducto);
                        oOportunidadesBE.Sole_unidades = (int)dtRow["sole_unidades"];
                        oOportunidadesBE.Sole_precioventapublico = (Decimal)dtRow["sole_precioventapublico"];
                        oOportunidadesBE.Sole_ventaneta = (Decimal)dtRow["sole_ventaneta"];
                        oOportunidadesBE.Sole_costoventa = (Decimal)dtRow["sole_costoventa"];
                        oOportunidadesBE.Sole_utilidadbruta = (Decimal)dtRow["sole_utilidadbruta"];
                        oOportunidadesBE.Sole_margenbruto = (int)dtRow["sole_margenbruto"];
                        oOportunidadesBE.Sole_importetotal = (Decimal)dtRow["sole_importetotal"];
                        oOportunidadesBE.Sole_tiposervicio = dtRow["sole_tiposervicio"].ToString();
                        oOportunidadesBE.Sole_fechaservicio = (DateTime)dtRow["sole_fechaservicio"];
                        oOportunidadesBE.Sole_empresaqueinstalo = dtRow["sole_empresaqueinstalo"].ToString();
                        oOportunidadesBE.Sole_nombretecnico = dtRow["sole_nombretecnico"].ToString();
                        oOportunidadesBE.Sole_canal = dtRow["sole_canal"].ToString();

                        oOportunidadesBE.Sole_tienda = oFuncionesBL.ObtieneTienda(dtRow["sole_tienda"].ToString());

                        if (oOportunidadesBE.Sole_tienda == null)
                        {
                            oBitacoraErroresBE.Codigo = dtRow["sole_tienda"].ToString();
                            oBitacoraErroresBE.Proceso = "Crear Oportunidad";
                            oBitacoraErroresBE.Error = "Error al buscar la tienda donde se realizo la venta";
                            oBitacoraErroresBE.Descripcion = "No se encontro la tienda en el CRM. El código de Sucursal que se busco es: " + dtRow["sole_tienda"].ToString();
                            oBitacoraErroresBE.IdRegistro = oOportunidadesBE.IdOportunidadCargaInicial;
                            oBitacoraErroresBE.Entidad = "Oportunidad";
                            oBitacoraErroresBE.EstadoCarga = "0";

                            oFuncionesBL.RegistraBitacoraErrores(oBitacoraErroresBE);
                            oFuncionesBL.ActualizarEstadoTablaOportunidad(oOportunidadesBE.IdOportunidadCargaInicial);

                            string Mensaje = "No se encontro la tienda en el CRM. El código de Sucursal que busco es: " + dtRow["sole_tienda"].ToString();
                            ZthMetodosVarios.Metodos.GuardarLog(ruta, Mensaje);

                            continue;
                        }

                        string ClienteId = oFuncionesBL.ObtieneClientexDNI(dtRow["sole_tipodocumento"].ToString(), dtRow["sole_numerodocumento"].ToString());

                        if (ClienteId == null)
                        {
                            oBitacoraErroresBE.Codigo = dtRow["sole_tienda"].ToString();
                            oBitacoraErroresBE.Proceso = "Crear Oportunidad";
                            oBitacoraErroresBE.Error = "Error al buscar al cliente que realizo la compra";
                            oBitacoraErroresBE.Descripcion = "No se encontro al cliente en el CRM. El tipo de documento es: " + dtRow["sole_tipodocumento"].ToString() + " y su número de documento es: " + dtRow["sole_numerodocumento"].ToString();
                            oBitacoraErroresBE.IdRegistro = oOportunidadesBE.IdOportunidadCargaInicial;
                            oBitacoraErroresBE.Entidad = "Oportunidad";
                            oBitacoraErroresBE.EstadoCarga = "0";

                            oFuncionesBL.RegistraBitacoraErrores(oBitacoraErroresBE);
                            oFuncionesBL.ActualizarEstadoTablaOportunidad(oOportunidadesBE.IdOportunidadCargaInicial);

                            string Mensaje = "No se encontro la tienda en el CRM. El código de Sucursal que busco es: " + dtRow["sole_tienda"].ToString();
                            ZthMetodosVarios.Metodos.GuardarLog(ruta, Mensaje);

                            continue;
                        }


                        // Los valida porque no pueden venir vacíos 
                        if (dtRow["sole_fechacompra"] != DBNull.Value)
                        {
                            oOportunidadesBE.Sole_fechacompra = Convert.ToDateTime(dtRow["sole_fechacompra"].ToString()); //zth_fecharespuestainicial
                        }

                        if (dtRow["sole_tipocomprobante"].ToString() != "")
                        {
                            oOportunidadesBE.Sole_tipocomprobante = dtRow["sole_tipocomprobante"].ToString();
                        }

                        if (dtRow["sole_numerocomprobante"].ToString() != "")
                        {
                            oOportunidadesBE.Sole_numerocomprobante = dtRow["sole_numerocomprobante"].ToString();
                        }

                        oOportunidadesBE.Sole_cuponcampana = dtRow["sole_cuponcampana"].ToString();

                        if (dtRow["sole_metodoventa"].ToString() != "")
                        {
                            oOportunidadesBE.Sole_metodoventa = dtRow["sole_metodoventa"].ToString();
                        }
                        
                        //Creamos la Oportunidad
                        Guid guidOportunidad;
                        guidOportunidad = oOportunidadesBL.CrearOportunidad(Guid.Parse(ClienteId), oOportunidadesBE);

                        if (guidOportunidad != Guid.Empty)
                        {
                            //Capturamos el Guid de la Oportunidad y Creamos el Producto de la Oportunidad
                            //Primero Obtenemos el Guid del Código del Producto
                            string guidCodigoProducto;
                            guidCodigoProducto = oFuncionesBL.ObtieneProductoId(oOportunidadesBE.Sole_codigoproducto);

                            ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se creo en el CRM la Oportunidad asociada al Contacto con DNI: " 
                                                                + dtRow["sole_numerodocumento"].ToString() 
                                                                + " y el GUID es: " + guidCodigoProducto);

                            oOportunidadesBL.CrearProductoDelaOportunidad(oOportunidadesBE,
                                                                          oOportunidadesBE.IdOportunidadCargaInicial, 
                                                                          guidOportunidad, 
                                                                          Guid.Parse(guidCodigoProducto));

                            oFuncionesBL.ActualizarEstadoTablaOportunidad(oOportunidadesBE.IdOportunidadCargaInicial);

                            ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se creo el producto de la Oportunidad, la oportunidad es: " + guidCodigoProducto);
                        }
                        else
                        {
                            ZthMetodosVarios.Metodos.GuardarLog(ruta, "No se pudo crear la Oportunidad del la IdOportunidadCargaInicial: " + oOportunidadesBE.IdOportunidadCargaInicial);
                        }
                    }
                    catch (Exception ex)
                    {
                        string Mensaje = "Se ha producido el siguiente error: " + ex.Message;
                        ZthMetodosVarios.Metodos.GuardarLog(ruta, Mensaje);

                        //ZthEnvioCorreos_CRM365.Metodos.EnviarCorreoError(CorreoSoporte, CorreoCliente, Aplicativo, ruta, Mensaje, CorreoClave, Port, Host);

                        continue;
                    }
                }

            }
            else
            {

            }


        }

        private void CargarNPS()
        {
            ZthMetodosVarios.Metodos.GuardarLog(ruta, "--- Iniciamos el proceso de Carga Masiva de los NPS " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "---");

            DataTable NPSxCargar;

            NPSxCargar = oNPSBL.ListadoNPSACargar();

            if (NPSxCargar.Rows.Count > 0)
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "NPS por cargar: " + NPSxCargar.Rows.Count);

                foreach (DataRow dtRow in NPSxCargar.Rows)
                {
                    try
                    {
                        oNPSBE.NPSId = dtRow["IdNPSCargaInicial"].ToString();
                        oNPSBE.Sole_numeroencuesta = (int)dtRow["sole_numeroencuesta"];
                        oNPSBE.Sole_fechaencuesta = (DateTime)dtRow["sole_fechaencuesta"];
                        oNPSBE.Sole_ordenservicio = dtRow["sole_ordenservicio"].ToString();
                        oNPSBE.Sole_tipoencuesta = (int)dtRow["crm_tipoencuesta"];
                        oNPSBE.Sole_puntuacion = (int)dtRow["sole_puntuacion"];
                        oNPSBE.Sole_comentarios = dtRow["sole_comentarios"].ToString();

                        string ClienteId = oFuncionesBL.ObtieneClientexDNI(dtRow["crm_tipodocumento"].ToString(), dtRow["sole_numerodocumento"].ToString());

                        if (ClienteId == null)
                        {
                            oBitacoraErroresBE.Codigo = dtRow["IdNPSCargaInicial"].ToString();
                            oBitacoraErroresBE.Proceso = "Crear NPS";
                            oBitacoraErroresBE.Error = "Error al buscar el cliente";
                            oBitacoraErroresBE.Descripcion = "No se encontro el cliente en el CTM";
                            oBitacoraErroresBE.IdRegistro = oNPSBE.NPSId;
                            oBitacoraErroresBE.Entidad = "NPS";
                            oBitacoraErroresBE.EstadoCarga = "0";

                            oFuncionesBL.RegistraBitacoraErrores(oBitacoraErroresBE);
                            oFuncionesBL.ActualizarEstadoTablaNPS(oNPSBE.NPSId);

                            string Mensaje = "No se encontro el cliente en el CTM. El Id del NPS Carga Inicial es: " + dtRow["IdNPSCargaInicial"].ToString();
                            ZthMetodosVarios.Metodos.GuardarLog(ruta, Mensaje);

                            continue;
                        }
  
                        oNPSBE.Sole_puntuacion = (int)dtRow["sole_puntuacion"];
                        oNPSBE.Sole_comentarios = dtRow["sole_comentarios"].ToString();
                        oNPSBE.Sole_tipoencuesta = (int)dtRow["crm_tipoencuesta"];

                        //Creamos la NPS
                        Guid guidNPS;
                        guidNPS = oNPSBL.CrearNPS(Guid.Parse(ClienteId), oNPSBE);
                        oFuncionesBL.ActualizarEstadoTablaNPS(oNPSBE.NPSId);

                    }
                    catch (Exception ex)
                    {
                        string Mensaje = "Se ha producido el siguiente error: " + ex.Message;
                        ZthMetodosVarios.Metodos.GuardarLog(ruta, Mensaje);

                        //ZthEnvioCorreos_CRM365.Metodos.EnviarCorreoError(CorreoSoporte, CorreoCliente, Aplicativo, ruta, Mensaje, CorreoClave, Port, Host);

                        continue;
                    }
                }


            }



        }


    }
}

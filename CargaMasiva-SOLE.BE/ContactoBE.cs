using System;

namespace CargaMasiva_SOLE.BE
{
    public class ContactoBE
    {
        public string ContactoId { get; set; }
        public Guid Sole_id { get; set; } // Representa un identificador unico Global
        public string Sole_nombres { get; set; }
        public string Sole_apellidos { get; set; }
        public string Sole_telefono1 { get; set; }
        public string Sole_telefono2 { get; set; }
        public string Sole_telefono3 { get; set; }
        public string Sole_correoelectronico1 { get; set; }
        public string Sole_correoelectronico2 { get; set; }
        public string Sole_correoelectronico3 { get; set; }
        public int Sole_tipodocumento { get; set; }
        public string Sole_numerodocumento { get; set; }
        public string Sole_direccion { get; set; }
        public string Sole_distrito { get; set; }
        public string Sole_provincia { get; set; }
        public string Sole_departamento { get; set; }
        public int Sole_zona { get; set; }
        public int Sole_sexo { get; set; }
        public DateTime Sole_fechanacimiento { get; set; }
        public int Sole_ocupacion { get; set; }
        public int Sole_tipocliente { get; set; }
        public int Sole_perfilcliente { get; set; }
        public int Sole_tenenciaproducto { get; set; }
        public int Sole_categorizacioncliente { get; set; }
        public int Sole_estilovida { get; set; }
        public int Sole_nse { get; set; }
        public bool Sole_permissionmarketingcorreo { get; set; }
        public bool Sole_permissionmarketingtelefono { get; set; }
        public string Sole_tienda { get; set; }
        public bool Sole_estadocarga { get; set; }

        //nuevos campos
        public int Sole_tipoContacto { get; set; }
        public DateTime Sole_fechaalta { get; set; }
        public int Sole_tipoclientepotencial { get; set; }
        public DateTime Sole_fechaconversion { get; set; }
        public int Sole_estadoSole { get; set; }
        public DateTime Sole_fechanuevo { get; set; }
        public DateTime Sole_fechaactivo { get; set; }
        public DateTime Sole_fechaincativo { get; set; }
        public bool Sole_interesbano { get; set; }
        public bool Sole_interescocina { get; set; }
        public bool Sole_interesdescanso { get; set; }
        public DateTime Sole_fechabronce { get; set; }
        public DateTime Sole_fechaplata { get; set; }
        public DateTime Sole_fechaoro { get; set; }
        public bool Sole_clubsolehogar { get; set; }
        public int Sole_fuenteorigen { get; set; }
        public int Sole_tipoalta { get; set; }
    }
}


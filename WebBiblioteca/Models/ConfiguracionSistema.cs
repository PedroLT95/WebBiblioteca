using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebBiblioteca.Models
{
    public class ConfiguracionSistema
    {
        public int Id { get; set; }

        public int DiasPrestamo { get; set; } = 15;
        public int DiasBloqueoPorVencimiento { get; set; } = 7;
    }

}
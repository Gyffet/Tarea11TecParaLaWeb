using System;
using System.Collections.Generic;

namespace Formulario.Model
{
    public partial class ArchivosCedulas
    {
        public int ID { get; set; }
        public string fkNoCed { get; set; }
        public int fkServicio { get; set; }
        public string fcNombreOriginal { get; set; }
        public int length { get; set; }
        public string contentType { get; set; }
        public DateTime FechaCarga { get; set; }
    }
}

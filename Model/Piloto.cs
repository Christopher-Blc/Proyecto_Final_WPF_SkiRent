using System;

namespace APIS_Test.Models
{
    public sealed class Piloto
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string Nacionalidad { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string Numero { get; set; }

        public string Abreviatura { get; set; }

        public string EquipoId { get; set; }

        public override string ToString()
        {
            return Nombre + " (" + Numero + ")";
        }
    }
}

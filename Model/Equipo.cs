namespace APIS_Test.Models
{
    public sealed class Equipo
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string Nacionalidad { get; set; }

        public int? AnioFundacion { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}

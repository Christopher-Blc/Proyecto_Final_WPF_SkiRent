namespace APIS_Test.Models
{
    public sealed class Circuito
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string Ciudad { get; set; }

        public string Pais { get; set; }

        public double? LongitudKm { get; set; }

        public override string ToString()
        {
            return Nombre + " - " + Pais;
        }
    }
}

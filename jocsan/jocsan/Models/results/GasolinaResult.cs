namespace jocsan.Models.results
{
    public class GasolinaResult
    {
        public string? totalgalonpagado { get; set; }
        public string? cantidadgalonpagado { get; set; }
        public string? totalgaloncargado { get; set; }
        public string? cantidadgaloncargado { get; set; }
        public string? totalgalondeuda { get; set; }
        public string? cantidadgalondeuda { get; set; }
        public IEnumerable<GasolinaResulttable>? pagados { get; set; }
        public IEnumerable<GasolinaResulttable>? cargados { get; set; }
    }
    public class GasolinaResulttable
    {
        public string? precio { get; set; }
        public string? cantidad { get; set; }
        public string? total { get; set; }
        public string? fecha { get; set; }
    }
}

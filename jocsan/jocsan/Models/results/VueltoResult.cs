namespace jocsan.Models.results
{
    public class VueltoResult
    {
        public List<VueltoResultData>? Vuelto { get; set; }
        public decimal TotalVuelto { get; set; }
    }
    public class VueltoResultData
    {
        public int IdVuelto { get; set; }
        public string? Descripcion { get; set; }
        public decimal Valor { get; set; }
        public string? Fecha { get; set; }
    }
    public class VueltoResultExtended
    {
        public VueltoResult? VueltoAbono { get; set; }
        public VueltoResult? VueltoCargo { get; set; }
        public decimal TotalVueltoGeneral { get; set; }
    }
}

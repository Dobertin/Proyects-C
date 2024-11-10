namespace jocsan.Models.results
{
    public class AbonoResult
    {
        public int IdAbono { get; set; }
        public string Descripcion { get; set; }
        public decimal ValorAbono { get; set; }
        public string FechaAbono { get; set; }
    }
    public class AbonoResultExtended
    {
        public List<AbonoResult> Abonos { get; set; }
        public decimal TotalValorAbono { get; set; }
    }
}

namespace jocsan.Models.results
{
    public class FacturaResults
    {
        public int IdFactura {  get; set; }
        public string Numproductos { get;  set; } 
        public decimal Total { get; set; }
        public string Fecha { get; set; }
    }
    public class FacturaResultsExtended
    {
        public List<FacturaResults> Facturas { get; set; }
        public decimal TotalValorFactura { get; set; }
    }
}

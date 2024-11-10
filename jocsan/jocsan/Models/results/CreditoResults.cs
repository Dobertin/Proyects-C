namespace jocsan.Models.results
{
    public class CreditoResults
    {
        public int IdCredito { get; set; }
        public string Descripcion { get; set; }
        public decimal ValorCredito { get; set; }
        public string FechaCredito { get; set; }
    }
    public class CreditoResultsExtended
    {
        public List<CreditoResults> Creditos { get; set; }
        public decimal TotalValorCreditos { get; set; }
    }
}

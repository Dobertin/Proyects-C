namespace jocsan.Models.results
{
    public class CuentaResult
    {
        public int IdCuenta { get; set; }
        public string Comentario { get; set; }
        public decimal Monto { get; set; }
        public string FechaCuenta { get; set; }
        public string? NomCliente { get; set; }
    }
    public class CuentaResultExtended
    {
        public List<CuentaResult> Cuentas { get; set; }
        public decimal TotalValorCuenta { get; set; }
    }
    public class CuentaResultTotal
    {
        public CreditoResultsExtended? CreditoTotal { get; set; }
        public AbonoResultExtended? AbonoTotal { get; set; }
        public FacturaResultsExtended? FacturasTotal { get; set; }
        public CuentaResult? UltimaCuenta { get; set; }
    }
}

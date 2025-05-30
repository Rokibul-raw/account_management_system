namespace AMS.Model
{
    public class VoucherLineDTO
    {
        public int AccountID { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Narration { get; set; }
    }
}

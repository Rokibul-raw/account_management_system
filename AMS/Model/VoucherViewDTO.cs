namespace AMS.Model
{
    public class VoucherViewDTO
    {

        public int VoucherID { get; set; }
        public DateTime VoucherDate { get; set; }
        public string ReferenceNo { get; set; }
        public string VoucherType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public int LineID { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Narration { get; set; }
    }
}

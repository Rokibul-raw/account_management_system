namespace AMS.Model
{
    public class VoucherHeaderDTO
    {
        public DateTime VoucherDate { get; set; }
        public string ReferenceNo { get; set; }
        public string VoucherType { get; set; }
        public int CreatedBy { get; set; }
    }
}

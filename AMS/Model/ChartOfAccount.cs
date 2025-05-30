namespace AMS.Model
{
    public class ChartOfAccount
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public int? ParentId { get; set; }
        public string AccountType { get; set; }
        public List<ChartOfAccount> Children { get; set; } = new();
    }
}

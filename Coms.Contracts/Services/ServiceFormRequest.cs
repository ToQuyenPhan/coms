namespace Coms.Contracts.Services
{
    public class ServiceFormRequest
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int ContractCategoryId { get; set; }
    }
}

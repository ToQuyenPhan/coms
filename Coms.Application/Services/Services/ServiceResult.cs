namespace Coms.Application.Services.Services
{
    public class ServiceResult
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Status { get; set; }
        public string StatusString { get; set; }
        public int? ContractCategoryId {  get; set; }
        public string ContractCategoryName { get; set;}
    }
}

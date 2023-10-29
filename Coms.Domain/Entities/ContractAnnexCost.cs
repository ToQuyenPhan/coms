namespace Coms.Domain.Entities
{
    public class ContractAnnexCost
    {
        public int? ContractCostId { get; set; }
        public virtual ContractCost ContractCost { get; set; }

        public int? ContractAnnexId { get; set; }
        public virtual ContractAnnex ContractAnnex { get; set; }
    }
}

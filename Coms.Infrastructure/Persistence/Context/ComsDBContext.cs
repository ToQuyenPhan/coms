using Coms.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coms.Infrastructure.Persistence.Context
{
    public class ComsDBContext : DbContext
    {
        public ComsDBContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractCategory> ContractCategories { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<TemplateField> TemplateFields { get; set; }
        public DbSet<LiquidationRecord> LiquidationRecords { get; set; }
        public DbSet<ContractAnnex> ContractAnnexes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ContractCost> ContractCosts { get; set; }
        public DbSet<ContractAnnexCost> ContractAnnexCosts { get; set; }
        public DbSet<ActionHistory> ActionHistories { get; set; }
        public DbSet<Comment> Comments { get; set; }   
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerReview> PartnerReviews { get; set; }
        public DbSet<PartnerComment> PartnerComments { get; set; }
        public DbSet<PartnerSign> PartnerSigns { get; set; }
        public DbSet<TemplateFile> TemplateFiles { get; set; }
        public DbSet<ContractFile> ContractFiles { get; set; }
        public DbSet<ContractAnnexFile> ContractAnnexFiles { get; set; }
        public DbSet<LiquidationRecordFile> LiquidationRecordFiles { get; set; }
        public DbSet<Flow> Flows { get; set; }
        public DbSet<FlowDetail> FlowDetails { get; set; }
        public DbSet<Contract_FlowDetail> UserFlowDetails { get; set; }
        public DbSet<ContractField> ContractFields { get; set; }
        public DbSet<SystemSettings> SystemSettings { get; set; }
        public DbSet<ContractAnnexAttachment> ContractAnnexAttachments { get; set; }
        public DbSet<LiquidationRecordAttachment> LiquidationRecordAttachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractAnnexCost>().HasKey(cac => new { cac.ContractAnnexId, cac.ContractCostId });
            modelBuilder.Entity<PartnerSign>().HasKey(ps => new { ps.PartnerId, ps.ContractId });
        }
    }
}

using Coms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Infrastructure.Persistence.Context
{
    public class ComsDBContext : DbContext
    {
        public ComsDBContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Permission_Role> PermissionRoles { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractCategory> ContractCategories { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<TemplateField> TemplateFields { get; set; }
        public DbSet<LiquidationRecord> LiquidationRecords { get; set; }
        public DbSet<TemplateType> TemplateTypes { get; set; }
        public DbSet<ContractAnnex> ContractAnnexes { get; set; }
        public DbSet<ContractTerm> ContractTerms { get; set; }
        public DbSet<TemplateTerm> TemplateTerms { get; set; }
        public DbSet<LiquidationRecordTerm> LiquidationRecordTerms { get; set; }
        public DbSet<ContractAnnexTerm> ContractAnnexTerms { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ContractCost> ContractCosts { get; set; }
        public DbSet<ContractAnnexCost> ContractAnnexCosts { get; set; }
        public DbSet<Access> Accesses { get; set; }
        //public DbSet<User_Access> UserAccesses { get; set; }
        public DbSet<ApproveWorkflow> ApproveWorkflows { get; set; }
        public DbSet<ActionHistory> ActionHistories { get; set; }
        public DbSet<Comment> Comments { get; set; }   
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerReview> PartnerReviews { get; set; }
        public DbSet<PartnerComment> PartnerComments { get; set; }
        public DbSet<PartnerSign> PartnerSigns { get; set; }
        public DbSet<TemplateContent> TemplateContents { get; set; }
        public DbSet<TemplateFile> TemplateFiles { get; set; }
        public DbSet<ContractFile> ContractFiles { get; set; }
        public DbSet<ContractAnnexFile> ContractAnnexFiles { get; set; }
        public DbSet<LiquidationRecordFile> LiquidationRecordFiles { get; set; }
        public DbSet<Flow> Flows { get; set; }
        public DbSet<FlowDetail> FlowDetails { get; set; }
        public DbSet<User_FlowDetail> UserFlowDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission_Role>().HasKey(pr => new { pr.RoleId, pr.PermissionId });
            modelBuilder.Entity<ContractTerm>().HasKey(ct => new { ct.ContractId, ct.Number });
            modelBuilder.Entity<TemplateTerm>().HasKey(ctt => new { ctt.TemplateId, ctt.Number });
            modelBuilder.Entity<LiquidationRecordTerm>().HasKey(lrt => new { lrt.LiquidationRecordId, lrt.Number });
            modelBuilder.Entity<ContractAnnexTerm>().HasKey(cat => new { cat.ContractAnnexId, cat.Number });
            modelBuilder.Entity<ContractAnnexCost>().HasKey(cac => new { cac.ContractAnnexId, cac.ContractCostId });
            //modelBuilder.Entity<User_Access>().HasKey(uc => new { uc.UserId, uc.AccessId });
            modelBuilder.Entity<ApproveWorkflow>().HasKey(aw => new { aw.AccessId, aw.Order });
            modelBuilder.Entity<PartnerSign>().HasKey(ps => new { ps.PartnerId, ps.ContractId });
            modelBuilder.Entity<TemplateContent>().HasKey(tc => new { tc.ContractId, tc.TemplateFieldId });
        }
    }
}

namespace Business_Services.Models.DAL.LoancareDBContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MDBService : DbContext
    {
        public MDBService()
             : base("LoancareDB")
        {
        }

        public virtual DbSet<AlertTemplate> AlertTemplates { get; set; }
        public virtual DbSet<FeedbackQuestionMaster> FeedbackQuestionMasters { get; set; }
        public virtual DbSet<LegalPrivacyTerm> LegalPrivacyTerms { get; set; }
        public virtual DbSet<MobileUser> MobileUsers { get; set; }
        public virtual DbSet<PaymentAlert> PaymentAlerts { get; set; }
        public virtual DbSet<PaymentTransactionMapping> PaymentTransactionMappings { get; set; }
        public virtual DbSet<UserAlert> UserAlerts { get; set; }
        public virtual DbSet<UserFeedback> UserFeedbacks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlertTemplate>()
                .Property(e => e.title_template)
                .IsUnicode(false);

            modelBuilder.Entity<AlertTemplate>()
                .Property(e => e.alert_detail_template)
                .IsUnicode(false);

            modelBuilder.Entity<AlertTemplate>()
                .Property(e => e.created_by)
                .IsUnicode(false);

            modelBuilder.Entity<AlertTemplate>()
                .Property(e => e.updated_by)
                .IsUnicode(false);

            modelBuilder.Entity<FeedbackQuestionMaster>()
                .Property(e => e.question_text)
                .IsUnicode(false);

            modelBuilder.Entity<FeedbackQuestionMaster>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FeedbackQuestionMaster>()
                .Property(e => e.created_by)
                .IsUnicode(false);

            modelBuilder.Entity<FeedbackQuestionMaster>()
                .Property(e => e.updated_by)
                .IsUnicode(false);

            modelBuilder.Entity<LegalPrivacyTerm>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<LegalPrivacyTerm>()
                .Property(e => e.Formatted_Text)
                .IsUnicode(false);

            modelBuilder.Entity<MobileUser>()
                .Property(e => e.mae_steps_completed)
                .IsUnicode(false);

            modelBuilder.Entity<MobileUser>()
                .Property(e => e.pin)
                .IsUnicode(false);

            modelBuilder.Entity<MobileUser>()
                .Property(e => e.User_Id)
                .IsUnicode(false);

            modelBuilder.Entity<MobileUser>()
                .Property(e => e.Mobile_Token_Id)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentAlert>()
                .Property(e => e.loan_number)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentAlert>()
                .Property(e => e.alert_title)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentAlert>()
                .Property(e => e.alert_read_status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PaymentTransactionMapping>()
                .Property(e => e.source_transaction_code)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentTransactionMapping>()
                .Property(e => e.source_transaction_descr)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentTransactionMapping>()
                .Property(e => e.target_transaction_descr)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentTransactionMapping>()
                .Property(e => e.created_by)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentTransactionMapping>()
                .Property(e => e.updated_by)
                .IsUnicode(false);

            modelBuilder.Entity<UserAlert>()
                .Property(e => e.loan_number)
                .IsUnicode(false);

            modelBuilder.Entity<UserAlert>()
                .Property(e => e.message_title)
                .IsUnicode(false);

            modelBuilder.Entity<UserAlert>()
                .Property(e => e.message_date)
                .IsUnicode(false);

            modelBuilder.Entity<UserAlert>()
                .Property(e => e.message_body)
                .IsUnicode(false);

            modelBuilder.Entity<UserFeedback>()
                .Property(e => e.created_by)
                .IsUnicode(false);
        }
<<<<<<< HEAD
        //Added by BBSR Team on 5th March 2018 : Defect # 1218
       // public virtual DbSet<TransDescription> TransDescription { get; set; }
=======
>>>>>>> parent of 8b01ef4... CommitPDF
    }
}

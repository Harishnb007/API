namespace Business_Services.Models.DAL.LoanCareContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MDBServices : DbContext
    {
        public MDBServices()
            : base("Data Source=LNCDEV01SFUI01;Initial Catalog=LoanCareMobile;Persist Security Info=True;User ID=LCMobileUser;Password=Dr.P3pp3r;MultipleActiveResultSets=True")
        {
        }

        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<Mobile_User> Mobile_User { get; set; }
        public virtual DbSet<Privacy> Privacies { get; set; }
        public virtual DbSet<SecurityQandA> SecurityQandAs { get; set; }
        public virtual DbSet<SharedContent> SharedContents { get; set; }
        public virtual DbSet<UserAlert> UserAlerts { get; set; }
        public virtual DbSet<UserFeedback> UserFeedbacks { get; set; }
        public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }
        public virtual DbSet<LegalPrivacyTerms> LegalPrivacyTerm { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>()
                .Property(e => e.routing_number)
                .IsUnicode(false);

            modelBuilder.Entity<BankAccount>()
                .Property(e => e.bank_name)
                .IsUnicode(false);

            modelBuilder.Entity<BankAccount>()
                .Property(e => e.account_number)
                .IsUnicode(false);

            modelBuilder.Entity<BankAccount>()
                .Property(e => e.account_type)
                .IsUnicode(false);

            modelBuilder.Entity<BankAccount>()
                .Property(e => e.legal_name)
                .IsUnicode(false);

            modelBuilder.Entity<BankAccount>()
                .Property(e => e.account_nickname)
                .IsUnicode(false);

            modelBuilder.Entity<Mobile_User>()
                .Property(e => e.mae_steps_completed)
                .IsUnicode(false);

            modelBuilder.Entity<Mobile_User>()
                .Property(e => e.pin)
                .IsUnicode(false);

            modelBuilder.Entity<Mobile_User>()
                .Property(e => e.User_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Mobile_User>()
                .Property(e => e.Mobile_Token_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Privacy>()
                .Property(e => e.heading)
                .IsUnicode(false);

            modelBuilder.Entity<Privacy>()
                .Property(e => e.detail)
                .IsUnicode(false);

            modelBuilder.Entity<Privacy>()
                .Property(e => e.Account_ID)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityQandA>()
                .Property(e => e.question)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityQandA>()
                .Property(e => e.answer)
                .IsUnicode(false);

            modelBuilder.Entity<SharedContent>()
                .Property(e => e.category)
                .IsUnicode(false);

            modelBuilder.Entity<SharedContent>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<SharedContent>()
                .Property(e => e.content)
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
                .Property(e => e.fdbk_question_1)
                .IsUnicode(false);

            modelBuilder.Entity<UserFeedback>()
                .Property(e => e.fdbk_rating_1)
                .HasPrecision(18, 0);

            modelBuilder.Entity<UserFeedback>()
                .Property(e => e.fdbk_question_2)
                .IsUnicode(false);

            modelBuilder.Entity<UserFeedback>()
                .Property(e => e.fdbk_rating_2)
                .HasPrecision(18, 0);

            modelBuilder.Entity<UserFeedback>()
                .Property(e => e.fdbk_question_3)
                .IsUnicode(false);

            modelBuilder.Entity<UserFeedback>()
                .Property(e => e.fdbk_rating_3)
                .HasPrecision(18, 0);

            modelBuilder.Entity<UserFeedback>()
                .Property(e => e.fdbk_question_4)
                .IsUnicode(false);

            modelBuilder.Entity<UserFeedback>()
                .Property(e => e.fdbk_rating_4)
                .HasPrecision(18, 0);

            modelBuilder.Entity<UserFeedback>()
                .Property(e => e.fdbk_question_5)
                .IsUnicode(false);

            modelBuilder.Entity<UserFeedback>()
                .Property(e => e.fdbk_rating_5)
                .HasPrecision(18, 0);

            modelBuilder.Entity<UserFeedback>()
                .Property(e => e.fdbk_question_6)
                .IsUnicode(false);

            modelBuilder.Entity<UserFeedback>()
                .Property(e => e.fdbk_answer_6)
                .HasPrecision(18, 0);

            modelBuilder.Entity<LegalPrivacyTerms>()
               .Property(e => e.Type)
               .IsUnicode(false);
            modelBuilder.Entity<LegalPrivacyTerms>()
               .Property(e => e.Formatted_Text)
               .IsUnicode(false);
           
        }
    }
}

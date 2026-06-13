using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AssociationRule> AssociationRules { get; set; }

    public virtual DbSet<AssociationRuleAntecedent> AssociationRuleAntecedents { get; set; }

    public virtual DbSet<AssociationRuleConsequent> AssociationRuleConsequents { get; set; }

    public virtual DbSet<BitTableItem> BitTableItems { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<FrequentItemset> FrequentItemsets { get; set; }

    public virtual DbSet<FrequentItemsetItem> FrequentItemsetItems { get; set; }

    public virtual DbSet<MiningTransaction> MiningTransactions { get; set; }

    public virtual DbSet<MiningTransactionItem> MiningTransactionItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<RecommendationLog> RecommendationLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-O8L8O12;Database=ProductRecommendationDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__accounts__3213E83F295FBE6C");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.Username, "UQ__accounts__F3DBC57270E6167C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Customer).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_accounts_customers");
        });

        modelBuilder.Entity<AssociationRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__associat__3213E83FCB6B845A");

            entity.ToTable("association_rules");

            entity.HasIndex(e => e.RuleCode, "UQ__associat__5DA4F7EFCB36E6D0").IsUnique();

            entity.HasIndex(e => e.ConfidenceValue, "idx_association_rules_confidence").IsDescending();

            entity.HasIndex(e => e.LiftValue, "idx_association_rules_lift").IsDescending();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AntecedentText).HasColumnName("antecedent_text");
            entity.Property(e => e.ConfidenceValue)
                .HasColumnType("decimal(8, 4)")
                .HasColumnName("confidence_value");
            entity.Property(e => e.ConsequentText).HasColumnName("consequent_text");
            entity.Property(e => e.GeneratedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("generated_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LiftValue)
                .HasColumnType("decimal(8, 4)")
                .HasColumnName("lift_value");
            entity.Property(e => e.MinConfidence)
                .HasColumnType("decimal(8, 4)")
                .HasColumnName("min_confidence");
            entity.Property(e => e.MinSupport)
                .HasColumnType("decimal(8, 4)")
                .HasColumnName("min_support");
            entity.Property(e => e.RuleCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rule_code");
            entity.Property(e => e.SupportValue)
                .HasColumnType("decimal(8, 4)")
                .HasColumnName("support_value");
        });

        modelBuilder.Entity<AssociationRuleAntecedent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__associat__3213E83F84DB3281");

            entity.ToTable("association_rule_antecedents");

            entity.HasIndex(e => e.ProductId, "idx_rule_antecedents_product_id");

            entity.HasIndex(e => new { e.RuleId, e.ProductId }, "uq_rule_antecedent_product").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.RuleId).HasColumnName("rule_id");

            entity.HasOne(d => d.Product).WithMany(p => p.AssociationRuleAntecedents)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rule_antecedents_product");

            entity.HasOne(d => d.Rule).WithMany(p => p.AssociationRuleAntecedents)
                .HasForeignKey(d => d.RuleId)
                .HasConstraintName("fk_rule_antecedents_rule");
        });

        modelBuilder.Entity<AssociationRuleConsequent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__associat__3213E83FDEC2D1FB");

            entity.ToTable("association_rule_consequents");

            entity.HasIndex(e => e.ProductId, "idx_rule_consequents_product_id");

            entity.HasIndex(e => new { e.RuleId, e.ProductId }, "uq_rule_consequent_product").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.RuleId).HasColumnName("rule_id");

            entity.HasOne(d => d.Product).WithMany(p => p.AssociationRuleConsequents)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rule_consequents_product");

            entity.HasOne(d => d.Rule).WithMany(p => p.AssociationRuleConsequents)
                .HasForeignKey(d => d.RuleId)
                .HasConstraintName("fk_rule_consequents_rule");
        });

        modelBuilder.Entity<BitTableItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__bit_tabl__3213E83FCE4D562F");

            entity.ToTable("bit_table_items");

            entity.HasIndex(e => e.ProductId, "idx_bit_table_items_product_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BitVector).HasColumnName("bit_vector");
            entity.Property(e => e.GeneratedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("generated_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.SupportCount).HasColumnName("support_count");
            entity.Property(e => e.SupportValue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(8, 4)")
                .HasColumnName("support_value");

            entity.HasOne(d => d.Product).WithMany(p => p.BitTableItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bit_table_items_product");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__brands__3213E83FB38AE2A4");

            entity.ToTable("brands");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__categori__3213E83FC3801FD1");

            entity.ToTable("categories");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.NameEn)
                .HasMaxLength(255)
                .HasColumnName("name_en");
            entity.Property(e => e.NameVi)
                .HasMaxLength(255)
                .HasColumnName("name_vi");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("fk_categories_parent");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__customer__3213E83F6792CB61");

            entity.ToTable("customers");

            entity.HasIndex(e => e.CustomerCode, "UQ__customer__6A9E4CB7EC1E96EC").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("customer_code");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<FrequentItemset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__frequent__3213E83F550F0B63");

            entity.ToTable("frequent_itemsets");

            entity.HasIndex(e => e.ItemsetCode, "UQ__frequent__E29FF5B6BF549038").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GeneratedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("generated_at");
            entity.Property(e => e.ItemsetCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("itemset_code");
            entity.Property(e => e.ItemsetSize).HasColumnName("itemset_size");
            entity.Property(e => e.MinSupport)
                .HasColumnType("decimal(8, 4)")
                .HasColumnName("min_support");
            entity.Property(e => e.SupportCount).HasColumnName("support_count");
            entity.Property(e => e.SupportValue)
                .HasColumnType("decimal(8, 4)")
                .HasColumnName("support_value");
        });

        modelBuilder.Entity<FrequentItemsetItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__frequent__3213E83F625BA04D");

            entity.ToTable("frequent_itemset_items");

            entity.HasIndex(e => e.FrequentItemsetId, "idx_frequent_itemset_items_itemset_id");

            entity.HasIndex(e => e.ProductId, "idx_frequent_itemset_items_product_id");

            entity.HasIndex(e => new { e.FrequentItemsetId, e.ProductId }, "uq_frequent_itemset_product").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FrequentItemsetId).HasColumnName("frequent_itemset_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.FrequentItemset).WithMany(p => p.FrequentItemsetItems)
                .HasForeignKey(d => d.FrequentItemsetId)
                .HasConstraintName("fk_frequent_itemset_items_itemset");

            entity.HasOne(d => d.Product).WithMany(p => p.FrequentItemsetItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_frequent_itemset_items_product");
        });

        modelBuilder.Entity<MiningTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mining_t__3213E83F3F87EA3B");

            entity.ToTable("mining_transactions");

            entity.HasIndex(e => e.TransactionCode, "UQ__mining_t__DD5740BECFDECE9D").IsUnique();

            entity.HasIndex(e => e.OrderId, "idx_mining_transactions_order_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.IsUsedForTraining)
                .HasDefaultValue(true)
                .HasColumnName("is_used_for_training");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.TotalItems)
                .HasDefaultValue(0)
                .HasColumnName("total_items");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("transaction_code");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");

            entity.HasOne(d => d.Order).WithMany(p => p.MiningTransactions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_mining_transactions_order");
        });

        modelBuilder.Entity<MiningTransactionItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mining_t__3213E83FEE2C8AFA");

            entity.ToTable("mining_transaction_items");

            entity.HasIndex(e => e.ProductId, "idx_mining_transaction_items_product_id");

            entity.HasIndex(e => e.TransactionId, "idx_mining_transaction_items_transaction_id");

            entity.HasIndex(e => new { e.TransactionId, e.ProductId }, "uq_transaction_product").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

            entity.HasOne(d => d.Product).WithMany(p => p.MiningTransactionItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_mining_transaction_items_product");

            entity.HasOne(d => d.Transaction).WithMany(p => p.MiningTransactionItems)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("fk_mining_transaction_items_transaction");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orders__3213E83FF1079ACF");

            entity.ToTable("orders");

            entity.HasIndex(e => e.InvoiceCode, "UQ__orders__5ED70A35B94A96BA").IsUnique();

            entity.HasIndex(e => e.CustomerId, "idx_orders_customer_id");

            entity.HasIndex(e => e.OrderDate, "idx_orders_order_date");

            entity.HasIndex(e => e.Status, "idx_orders_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("discount_amount");
            entity.Property(e => e.FinalAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("final_amount");
            entity.Property(e => e.InvoiceCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("invoice_code");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("order_date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("payment_method");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("completed")
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("total_amount");
            entity.Property(e => e.TotalQuantity)
                .HasDefaultValue(0)
                .HasColumnName("total_quantity");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_orders_customer");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_it__3213E83F055FA87E");

            entity.ToTable("order_items");

            entity.HasIndex(e => e.OrderId, "idx_order_items_order_id");

            entity.HasIndex(e => e.ProductId, "idx_order_items_product_id");

            entity.HasIndex(e => new { e.OrderId, e.ProductId }, "uq_order_product").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("discount_amount");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_order_items_order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_items_product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__products__3213E83FE8412AE3");

            entity.ToTable("products");

            entity.HasIndex(e => e.ProductCode, "UQ__products__AE1A8CC46A69EE71").IsUnique();

            entity.HasIndex(e => e.BrandId, "idx_products_brand_id");

            entity.HasIndex(e => e.CategoryId, "idx_products_category_id");

            entity.HasIndex(e => e.Name, "idx_products_name");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BasePrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("base_price");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("product_code");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasDefaultValue("cái")
                .HasColumnName("unit");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("fk_products_brand");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_category");
        });

        modelBuilder.Entity<RecommendationLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__recommen__3213E83FF32E8938");

            entity.ToTable("recommendation_logs");

            entity.HasIndex(e => e.InputProductId, "idx_recommendation_logs_input_product_id");

            entity.HasIndex(e => e.RecommendedProductId, "idx_recommendation_logs_recommended_product_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConfidenceValue)
                .HasColumnType("decimal(8, 4)")
                .HasColumnName("confidence_value");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.InputProductId).HasColumnName("input_product_id");
            entity.Property(e => e.LiftValue)
                .HasColumnType("decimal(8, 4)")
                .HasColumnName("lift_value");
            entity.Property(e => e.RecommendedProductId).HasColumnName("recommended_product_id");
            entity.Property(e => e.RuleId).HasColumnName("rule_id");

            entity.HasOne(d => d.InputProduct).WithMany(p => p.RecommendationLogInputProducts)
                .HasForeignKey(d => d.InputProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_recommendation_logs_input_product");

            entity.HasOne(d => d.RecommendedProduct).WithMany(p => p.RecommendationLogRecommendedProducts)
                .HasForeignKey(d => d.RecommendedProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_recommendation_logs_recommended_product");

            entity.HasOne(d => d.Rule).WithMany(p => p.RecommendationLogs)
                .HasForeignKey(d => d.RuleId)
                .HasConstraintName("fk_recommendation_logs_rule");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

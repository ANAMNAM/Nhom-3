CREATE TABLE dbo.categories (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    name_vi NVARCHAR(255) NOT NULL,
    name_en NVARCHAR(255),
    description NVARCHAR(MAX),
    parent_id BIGINT NULL,
    is_active BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT SYSDATETIME(),
    updated_at DATETIME2 DEFAULT SYSDATETIME(),

    CONSTRAINT fk_categories_parent
        FOREIGN KEY (parent_id) REFERENCES dbo.categories(id)
);
GO

CREATE TABLE dbo.brands (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX),
    is_active BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT SYSDATETIME(),
    updated_at DATETIME2 DEFAULT SYSDATETIME()
);
GO

CREATE TABLE dbo.products (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    product_code VARCHAR(50) UNIQUE NOT NULL,
    name NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX),

    category_id BIGINT NOT NULL,
    brand_id BIGINT NULL,

    unit NVARCHAR(50) DEFAULT N'cái',
    base_price DECIMAL(12,2) DEFAULT 0,
    image_url NVARCHAR(MAX),

    is_active BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT SYSDATETIME(),
    updated_at DATETIME2 DEFAULT SYSDATETIME(),

    CONSTRAINT fk_products_category
        FOREIGN KEY (category_id) REFERENCES dbo.categories(id),

    CONSTRAINT fk_products_brand
        FOREIGN KEY (brand_id) REFERENCES dbo.brands(id)
);
GO

CREATE TABLE dbo.customers (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    customer_code VARCHAR(50) UNIQUE,
    full_name NVARCHAR(255),
    phone VARCHAR(20),
    email VARCHAR(255),
    gender VARCHAR(20),
    address NVARCHAR(MAX),
    created_at DATETIME2 DEFAULT SYSDATETIME(),
    updated_at DATETIME2 DEFAULT SYSDATETIME()
);
GO

CREATE TABLE dbo.orders (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    invoice_code VARCHAR(50) UNIQUE NOT NULL,

    customer_id BIGINT NULL,

    order_date DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    total_quantity INT DEFAULT 0,
    total_amount DECIMAL(12,2) DEFAULT 0,
    discount_amount DECIMAL(12,2) DEFAULT 0,
    final_amount DECIMAL(12,2) DEFAULT 0,

    payment_method VARCHAR(50),
    status VARCHAR(50) DEFAULT 'completed',

    created_at DATETIME2 DEFAULT SYSDATETIME(),
    updated_at DATETIME2 DEFAULT SYSDATETIME(),

    CONSTRAINT fk_orders_customer
        FOREIGN KEY (customer_id) REFERENCES dbo.customers(id)
);
GO

CREATE TABLE dbo.order_items (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,

    order_id BIGINT NOT NULL,
    product_id BIGINT NOT NULL,

    quantity INT NOT NULL DEFAULT 1,
    unit_price DECIMAL(12,2) NOT NULL DEFAULT 0,
    discount_amount DECIMAL(12,2) DEFAULT 0,
    total_price DECIMAL(12,2) NOT NULL DEFAULT 0,

    created_at DATETIME2 DEFAULT SYSDATETIME(),

    CONSTRAINT fk_order_items_order
        FOREIGN KEY (order_id) REFERENCES dbo.orders(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_order_items_product
        FOREIGN KEY (product_id) REFERENCES dbo.products(id),

    CONSTRAINT uq_order_product UNIQUE (order_id, product_id)
);
GO

CREATE TABLE dbo.mining_transactions (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,

    order_id BIGINT NOT NULL,
    transaction_code VARCHAR(50) UNIQUE NOT NULL,

    total_items INT DEFAULT 0,
    transaction_date DATETIME2,

    is_used_for_training BIT DEFAULT 1,

    created_at DATETIME2 DEFAULT SYSDATETIME(),

    CONSTRAINT fk_mining_transactions_order
        FOREIGN KEY (order_id) REFERENCES dbo.orders(id)
        ON DELETE CASCADE
);
GO

CREATE TABLE dbo.mining_transaction_items (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,

    transaction_id BIGINT NOT NULL,
    product_id BIGINT NOT NULL,

    created_at DATETIME2 DEFAULT SYSDATETIME(),

    CONSTRAINT fk_mining_transaction_items_transaction
        FOREIGN KEY (transaction_id) REFERENCES dbo.mining_transactions(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_mining_transaction_items_product
        FOREIGN KEY (product_id) REFERENCES dbo.products(id),

    CONSTRAINT uq_transaction_product UNIQUE (transaction_id, product_id)
);
GO

CREATE TABLE dbo.bit_table_items (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,

    product_id BIGINT NOT NULL,

    bit_vector NVARCHAR(MAX) NOT NULL,
    support_count INT NOT NULL DEFAULT 0,
    support_value DECIMAL(8,4) DEFAULT 0,

    generated_at DATETIME2 DEFAULT SYSDATETIME(),

    CONSTRAINT fk_bit_table_items_product
        FOREIGN KEY (product_id) REFERENCES dbo.products(id)
);
GO

CREATE TABLE dbo.frequent_itemsets (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,

    itemset_code VARCHAR(50) UNIQUE NOT NULL,

    itemset_size INT NOT NULL,

    support_count INT NOT NULL,
    support_value DECIMAL(8,4) NOT NULL,

    min_support DECIMAL(8,4) NOT NULL,

    generated_at DATETIME2 DEFAULT SYSDATETIME()
);
GO

CREATE TABLE dbo.frequent_itemset_items (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,

    frequent_itemset_id BIGINT NOT NULL,
    product_id BIGINT NOT NULL,

    CONSTRAINT fk_frequent_itemset_items_itemset
        FOREIGN KEY (frequent_itemset_id) REFERENCES dbo.frequent_itemsets(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_frequent_itemset_items_product
        FOREIGN KEY (product_id) REFERENCES dbo.products(id),

    CONSTRAINT uq_frequent_itemset_product 
        UNIQUE (frequent_itemset_id, product_id)
);
GO

CREATE TABLE dbo.association_rules (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,

    rule_code VARCHAR(50) UNIQUE NOT NULL,

    antecedent_text NVARCHAR(MAX) NOT NULL,
    consequent_text NVARCHAR(MAX) NOT NULL,

    support_value DECIMAL(8,4) NOT NULL,
    confidence_value DECIMAL(8,4) NOT NULL,
    lift_value DECIMAL(8,4),

    min_support DECIMAL(8,4) NOT NULL,
    min_confidence DECIMAL(8,4) NOT NULL,

    is_active BIT DEFAULT 1,

    generated_at DATETIME2 DEFAULT SYSDATETIME()
);
GO

CREATE TABLE dbo.association_rule_antecedents (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,

    rule_id BIGINT NOT NULL,
    product_id BIGINT NOT NULL,

    CONSTRAINT fk_rule_antecedents_rule
        FOREIGN KEY (rule_id) REFERENCES dbo.association_rules(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_rule_antecedents_product
        FOREIGN KEY (product_id) REFERENCES dbo.products(id),

    CONSTRAINT uq_rule_antecedent_product 
        UNIQUE (rule_id, product_id)
);
GO

CREATE TABLE dbo.association_rule_consequents (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,

    rule_id BIGINT NOT NULL,
    product_id BIGINT NOT NULL,

    CONSTRAINT fk_rule_consequents_rule
        FOREIGN KEY (rule_id) REFERENCES dbo.association_rules(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_rule_consequents_product
        FOREIGN KEY (product_id) REFERENCES dbo.products(id),

    CONSTRAINT uq_rule_consequent_product 
        UNIQUE (rule_id, product_id)
);
GO

CREATE TABLE dbo.recommendation_logs (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,

    input_product_id BIGINT NOT NULL,
    recommended_product_id BIGINT NOT NULL,

    rule_id BIGINT NULL,

    confidence_value DECIMAL(8,4),
    lift_value DECIMAL(8,4),

    created_at DATETIME2 DEFAULT SYSDATETIME(),

    CONSTRAINT fk_recommendation_logs_input_product
        FOREIGN KEY (input_product_id) REFERENCES dbo.products(id),

    CONSTRAINT fk_recommendation_logs_recommended_product
        FOREIGN KEY (recommended_product_id) REFERENCES dbo.products(id),

    CONSTRAINT fk_recommendation_logs_rule
        FOREIGN KEY (rule_id) REFERENCES dbo.association_rules(id)
);
GO

CREATE INDEX idx_products_category_id 
ON dbo.products(category_id);
GO

CREATE INDEX idx_products_brand_id 
ON dbo.products(brand_id);
GO

CREATE INDEX idx_products_name 
ON dbo.products(name);
GO

CREATE INDEX idx_orders_customer_id 
ON dbo.orders(customer_id);
GO

CREATE INDEX idx_orders_order_date 
ON dbo.orders(order_date);
GO

CREATE INDEX idx_orders_status 
ON dbo.orders(status);
GO

CREATE INDEX idx_order_items_order_id 
ON dbo.order_items(order_id);
GO

CREATE INDEX idx_order_items_product_id 
ON dbo.order_items(product_id);
GO

CREATE INDEX idx_mining_transactions_order_id 
ON dbo.mining_transactions(order_id);
GO

CREATE INDEX idx_mining_transaction_items_transaction_id 
ON dbo.mining_transaction_items(transaction_id);
GO

CREATE INDEX idx_mining_transaction_items_product_id 
ON dbo.mining_transaction_items(product_id);
GO

CREATE INDEX idx_bit_table_items_product_id 
ON dbo.bit_table_items(product_id);
GO

CREATE INDEX idx_frequent_itemset_items_itemset_id 
ON dbo.frequent_itemset_items(frequent_itemset_id);
GO

CREATE INDEX idx_frequent_itemset_items_product_id 
ON dbo.frequent_itemset_items(product_id);
GO

CREATE INDEX idx_association_rules_confidence 
ON dbo.association_rules(confidence_value DESC);
GO

CREATE INDEX idx_association_rules_lift 
ON dbo.association_rules(lift_value DESC);
GO

CREATE INDEX idx_rule_antecedents_product_id 
ON dbo.association_rule_antecedents(product_id);
GO

CREATE INDEX idx_rule_consequents_product_id 
ON dbo.association_rule_consequents(product_id);
GO

CREATE INDEX idx_recommendation_logs_input_product_id 
ON dbo.recommendation_logs(input_product_id);
GO

CREATE INDEX idx_recommendation_logs_recommended_product_id 
ON dbo.recommendation_logs(recommended_product_id);
GO
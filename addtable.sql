CREATE DATABASE ProductRecommendationDB;
GO

USE ProductRecommendationDB;
GO

--thêm bảng accounts
CREATE TABLE accounts (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    full_name NVARCHAR(100) NOT NULL,
    phone NVARCHAR(20) NULL,
    address NVARCHAR(255) NULL,
    username NVARCHAR(50) NOT NULL UNIQUE,
    password NVARCHAR(100) NOT NULL,
    created_at DATETIME DEFAULT GETDATE()
);
ALTER TABLE accounts
ADD customer_id BIGINT NULL;

ALTER TABLE accounts
ADD CONSTRAINT FK_accounts_customers
FOREIGN KEY (customer_id) REFERENCES customers(id);


--dùng để chạy console
--Scaffold-DbContext "Server=DESKTOP-O8L8O12;Database=ProductRecommendationDB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context AppDbContext -Force
## XÂY DỰNG HỆ THỐNG GỢI Ý SẢN PHẨM MUA KÈM BẰNG THUẬT TOÁN APRIORI

---

# 1. GIỚI THIỆU ĐỀ TÀI

Đề tài xây dựng website bán hàng sử dụng ASP.NET Core MVC kết hợp thuật toán Apriori để phân tích lịch sử mua hàng và đưa ra gợi ý sản phẩm thường mua cùng.

Mục tiêu của hệ thống:

* Quản lý sản phẩm
* Quản lý khách hàng
* Đăng ký và đăng nhập
* Giỏ hàng
* Thanh toán
* Lịch sử mua hàng
* Gợi ý sản phẩm mua kèm bằng Apriori
* Thống kê doanh thu

---

# 2. CÔNG NGHỆ SỬ DỤNG

## Backend

* ASP.NET Core MVC
* C#
* Entity Framework Core

## Database

* SQL Server

## Frontend

* Bootstrap 5
* HTML
* CSS
* JavaScript

---

# 3. CẤU TRÚC CƠ SỞ DỮ LIỆU

## Nhóm khách hàng

### accounts

Lưu thông tin đăng nhập:

* Username
* Password
* CustomerId

### customers

Lưu thông tin khách hàng:

* CustomerCode
* FullName
* Phone
* Email
* Address

Quan hệ:

Account → Customer

Mỗi tài khoản chỉ thuộc một khách hàng.

---

## Nhóm sản phẩm

### products

Lưu:

* Tên sản phẩm
* Giá
* Ảnh
* Danh mục
* Thương hiệu

### categories

Lưu danh mục sản phẩm.

Ví dụ:

* Sữa
* Bánh kẹo
* Chăm sóc cá nhân

### brands

Lưu thương hiệu sản phẩm.

Ví dụ:

* Vinamilk
* TH True Milk
* Omo
* Sunlight

---

## Nhóm bán hàng

### orders

Lưu:

* Mã hóa đơn
* Khách hàng
* Ngày mua
* Tổng tiền

### order_items

Lưu:

* Sản phẩm
* Số lượng
* Đơn giá
* Thành tiền

Quan hệ:

Customers
↓
Orders
↓
OrderItems
↓
Products

---

## Nhóm thuật toán Apriori

### mining_transactions

Lưu danh sách giao dịch.

### mining_transaction_items

Lưu các sản phẩm trong từng giao dịch.

### bit_table_items

Biểu diễn giao dịch dạng nhị phân.

### frequent_itemsets

Lưu tập phổ biến.

### association_rules

Lưu luật kết hợp.

### association_rule_antecedents

Lưu vế trái của luật.

Ví dụ:

Sunlight
+
Omo

### association_rule_consequents

Lưu vế phải của luật.

Ví dụ:

Khăn giấy

### recommendation_logs

Lưu lịch sử gợi ý.

---

# 4. LUỒNG XỬ LÝ THUẬT TOÁN APRIORI

Dữ liệu được lấy từ:

Orders
+
OrderItems

↓

MiningTransactions
+
MiningTransactionItems

↓

BitTableItems

↓

FrequentItemsets

↓

AssociationRules

↓

Hiển thị gợi ý sản phẩm trên website

---

# 5. THUẬT TOÁN APRIORI

Thuật toán Apriori dùng để tìm các sản phẩm thường được mua cùng nhau.

Ví dụ:

Giao dịch 1:

* Sữa
* Bánh mì

Giao dịch 2:

* Sữa
* Trứng

Giao dịch 3:

* Sữa
* Bánh mì
* Trứng

Thuật toán sẽ tìm ra:

{Sữa}
→
{Bánh mì}

---

# 6. CÁC CHỈ SỐ QUAN TRỌNG

## Support

Support(A→B)

= Số giao dịch chứa A và B

/

Tổng số giao dịch

Ý nghĩa:

Mức độ xuất hiện của luật trong toàn bộ dữ liệu.

---

## Confidence

Confidence(A→B)

= Số giao dịch chứa A và B

/

Số giao dịch chứa A

Ý nghĩa:

Xác suất mua B khi đã mua A.

---

## Lift

Lift(A→B)

= Confidence(A→B)

/

Support(B)

Ý nghĩa:

Độ mạnh liên kết giữa hai sản phẩm.

Đánh giá:

* Lift > 1: Có liên hệ
* Lift = 1: Ngẫu nhiên
* Lift < 1: Liên hệ yếu

---

# 7. VÍ DỤ LUẬT APRIORI TRONG HỆ THỐNG

RULE0001

Nước rửa chén Sunlight
+
Bột giặt Omo 800g

↓

Khăn giấy hộp

Thông số:

Support = 0.0850

Confidence = 0.9444

Lift = 5.9028

Giải thích:

94.44% khách mua Sunlight và Omo cũng mua thêm Khăn giấy hộp.

Khả năng mua Khăn giấy hộp cao gấp 5.9 lần khách hàng bình thường.

---

# 8. CHỨC NĂNG HỆ THỐNG

## Khách hàng

### Trang sản phẩm

* Hiển thị danh sách sản phẩm
* Tìm kiếm
* Gợi ý khi nhập
* Phân trang
* Lọc theo danh mục
* Sắp xếp theo giá

### Chi tiết sản phẩm

* Hiển thị thông tin sản phẩm
* Gợi ý sản phẩm mua cùng

### Giỏ hàng

* Thêm sản phẩm
* Tăng giảm số lượng
* Xóa sản phẩm
* Hiển thị tổng tiền
* Gợi ý mua kèm

### Đăng ký

* Tạo Customer mới
* Tạo Account mới
* Kiểm tra trùng số điện thoại

### Đăng nhập

* Session AccountId
* Session CustomerId

### Hồ sơ khách hàng

* Xem thông tin
* Cập nhật thông tin
* Đổi mật khẩu

### Thanh toán

* Xác nhận bằng Bootstrap Modal
* Tạo hóa đơn

### Lịch sử mua hàng

* Chỉ hiển thị đơn hàng của khách đang đăng nhập

### Chi tiết đơn hàng

* Mã hóa đơn
* Danh sách sản phẩm
* Tổng tiền

---

## Quản trị viên

### Dashboard

Hiển thị:

* Tổng đơn hàng
* Tổng doanh thu
* Sản phẩm bán chạy
* Danh sách đơn hàng mới nhất
* Luật Apriori

---

# 9. CÀI ĐẶT HỆ THỐNG

## Bước 1

Mở SQL Server.

Tạo database:

ProductRecommendationDB

Import file SQL.

---

## Bước 2

Mở project bằng Visual Studio.

Mở file:

DoAn.sln

---

## Bước 3

Kiểm tra Connection String.

File:

appsettings.json

Ví dụ:

Server=DESKTOP-O8L8O12;
Database=ProductRecommendationDB;
Trusted_Connection=True;
TrustServerCertificate=True;

---

## Bước 4

Cài đặt Entity Framework Core.

Package Manager Console:

Install-Package Microsoft.EntityFrameworkCore.SqlServer

Install-Package Microsoft.EntityFrameworkCore.Tools

Install-Package Microsoft.EntityFrameworkCore.Design

---

## Bước 5

Kiểm tra Session trong Program.cs

builder.Services.AddSession();

app.UseSession();

---

## Bước 6

Chạy chương trình.

Ctrl + F5


Công nghệ: ASP.NET Core MVC – SQL Server – Entity Framework Core – Bootstrap 5

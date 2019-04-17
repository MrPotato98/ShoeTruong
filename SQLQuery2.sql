Use HuyQuanShop
GO


CREATE TABLE KHACHHANG
(
	MaKH INT IDENTITY(1,1),
	HoTen nVarchar(50) NOT NULL,
	Taikhoan Varchar(50) UNIQUE,
	Matkhau Varchar(50) NOT NULL,
	Email Varchar(100) UNIQUE,
	DiachiKH nVarchar(200),
	DienthoaiKH Varchar(50),	
	Ngaysinh DATETIME
	CONSTRAINT PK_Khachhang PRIMARY KEY(MaKH)
)

Create Table LOAI
(
	MALOAI int Identity(1,1),
	TenLOAI nvarchar(50) NOT NULL,
	CONSTRAINT PK_LOAI PRIMARY KEY(MALOAI)
)

CREATE TABLE GIAY
(
	MaGIAY INT IDENTITY(1,1),
	TenGIAY NVARCHAR(100) NOT NULL,
	Giaban Decimal(18,0) CHECK (Giaban>=0),
	Mota NVarchar(Max),
	Anhbia VARCHAR(50),
	Ngaycapnhat DATETIME,
	Soluongton INT,
	MaLOAI INT,
	
	Constraint PK_Sach Primary Key(MaGIAY),
	Constraint FK_LOAI Foreign Key(MALOAI) References LOAI(MALOAI),

)

CREATE TABLE DONDATHANG
(
	MaDonHang INT IDENTITY(1,1),
	Dathanhtoan bit,
	Tinhtranggiaohang  bit,
	Ngaydat Datetime,
	Ngaygiao Datetime,	
	MaKH INT,
	Constraint FK_Khachhang Foreign Key(MaKH) References KHACHHANG(MAKH),
	Constraint PK_Dondathang Primary Key(MaDonhang)
)


CREATE TABLE CHITIETDONTHANG
(
	MaDonHang INT,
	Magiay INT,
	Soluong Int Check(Soluong>0),
	Dongia Decimal(18,0) Check(Dongia>=0),	
	CONSTRAINT PK_CTDatHang PRIMARY KEY(MaDonHang,Magiay),
	Constraint FK_Donhang Foreign Key(MaDonHang) References DONDATHANG(MaDonHang),
	CONSTRAINT FK_Sach FOREIGN KEY (Magiay) REFERENCES giay(magiay)
)

Create table Admin(
	UserAdmin varchar(30) primary key,
	PassAdmin varchar(30) not null,
	Hoten nvarchar(50)
)


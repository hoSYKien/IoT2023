USE [master]
GO
/****** Object:  Database [iot2024]    Script Date: 11/8/2024 11:11:04 AM ******/
CREATE DATABASE [iot2024]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'iot2024', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\iot2024.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'iot2024_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\iot2024_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [iot2024] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [iot2024].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [iot2024] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [iot2024] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [iot2024] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [iot2024] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [iot2024] SET ARITHABORT OFF 
GO
ALTER DATABASE [iot2024] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [iot2024] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [iot2024] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [iot2024] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [iot2024] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [iot2024] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [iot2024] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [iot2024] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [iot2024] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [iot2024] SET  ENABLE_BROKER 
GO
ALTER DATABASE [iot2024] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [iot2024] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [iot2024] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [iot2024] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [iot2024] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [iot2024] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [iot2024] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [iot2024] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [iot2024] SET  MULTI_USER 
GO
ALTER DATABASE [iot2024] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [iot2024] SET DB_CHAINING OFF 
GO
ALTER DATABASE [iot2024] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [iot2024] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [iot2024] SET DELAYED_DURABILITY = DISABLED 
GO
USE [iot2024]
GO
/****** Object:  UserDefinedFunction [dbo].[getID]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[getID]
(
    @tenBang nvarchar(100),
    @tenCot nvarchar(50)
)
RETURNS nvarchar(50)
AS
begin
	DECLARE @id nvarchar(50)

	DECLARE @sql nvarchar(max)
	SET @sql = 'SELECT TOP 1 ' + QUOTENAME(@tenCot) + ' AS ID FROM ' + QUOTENAME(@tenBang) + ' ORDER BY ID DESC'

	EXEC sp_executesql @sql, N'@id nvarchar(50) OUTPUT', @id OUTPUT
	return @id
end

GO
/****** Object:  UserDefinedFunction [dbo].[getSoBacSi]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[getSoBacSi](@maKhoa nvarchar(50))
RETURNS int
AS
BEGIN
    DECLARE @soBacSi int;
    
    SELECT @soBacSi = COUNT(*) 
    FROM dbo.bacsi 
    WHERE bacsi.maKhoa = @maKhoa;

    RETURN @soBacSi;
END;




GO
/****** Object:  UserDefinedFunction [dbo].[getSoPhong]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[getSoPhong] (@maKhoa nvarchar(50))
returns int
as
begin 
	declare @soPhong int;
	select @soPhong = count(*)
	from Phong
	where phong.maKhoa = @maKhoa
	return @soPhong
end;


GO
/****** Object:  UserDefinedFunction [dbo].[getTenKhoa]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[getTenKhoa]
(
    @maKhoa NVARCHAR(50)
)
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @tenKhoa NVARCHAR(50);

    SELECT @tenKhoa = tenKhoa 
    FROM khoa 
    WHERE maKhoa = @maKhoa;

    RETURN @tenKhoa;
END;


GO
/****** Object:  UserDefinedFunction [dbo].[location]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create Function [dbo].[location]
(
@maBS nvarchar(50)
)
returns nvarchar(50)
as
	begin
	declare @maKhoa nvarchar(50)
	select @maKhoa =  maKhoa from bacsi where maBS = @maBS
	declare @loca nvarchar(50)
	select @loca = vitri from khoa where maKhoa = @maKhoa
	return @loca
	end

GO
/****** Object:  UserDefinedFunction [dbo].[locationPCD]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[locationPCD]
(
    @maPCD nvarchar(50)
)
RETURNS nvarchar(50)
AS
BEGIN
    DECLARE @maPhong nvarchar(50), @stt nvarchar(50), @loca nvarchar(50)
    
    -- Lấy maPhong và stt từ bảng Phieukham
    SELECT @maPhong = maPhong, @stt = CONVERT(nvarchar(50), stt)
    FROM Phieukham
    WHERE maPCD = @maPCD AND trangThai = 'false'

    -- Lấy vitri từ bảng phong dựa trên maPhong
    SELECT @loca = vitri
    FROM phong
    WHERE maPhong = @maPhong

    RETURN @loca + ' ' + @stt
END


GO
/****** Object:  UserDefinedFunction [dbo].[maDH]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[maDH]
(
@maPK nvarchar(50)
)
returns nvarchar(50)
as
begin
	declare @maPCD nvarchar(50), @maBN nvarchar(50), @maDH nvarchar(50)
	select @maPCD = maPCD from phieuKham where maPK = @maPK
	select @maBN = maBN from phieuChuanDoan where maPCD = @maPCD
	select @maDH = maDH from dongho where maBN = @maBN
	return @maDH
end

GO
/****** Object:  UserDefinedFunction [dbo].[maDH2]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[maDH2]
(
@maPCD nvarchar(50)
)
returns nvarchar(50)
as
begin
	declare @maBN nvarchar(50), @maDH nvarchar(50)
	select @maBN = maBN from phieuChuanDoan where maPCD = @maPCD
	select @maDH = maDH from dongho where maBN = @maBN
	return @maDH
end

GO
/****** Object:  UserDefinedFunction [dbo].[sltPCD]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[sltPCD]
(
    @maDH nvarchar(50)
)
RETURNS int
AS
BEGIN
    DECLARE @maBN nvarchar(50);
    DECLARE @soLuong int = 0; -- Khởi tạo giá trị mặc định cho biến @soLuong

    SELECT @maBN = maBN FROM dongho WHERE maDH = @maDH;
	
    IF @maBN IS NOT NULL
    BEGIN
		select @soluong = count(*) from phieuChuanDoan where maBN = @maBN
		if @soLuong = 0
		begin
			return -1;
		end
        SELECT @soLuong = COUNT(*) FROM phieuChuanDoan WHERE maBN = @maBN AND trangThai = 'false';
		
    END
    ELSE
    BEGIN
        -- Trường hợp không tìm thấy maDH tương ứng trong bảng 'dongho'
        SET @soLuong = -1; -- hoặc giá trị khác thích hợp để chỉ ra rằng không có kết quả
    END

    RETURN @soLuong; -- Câu lệnh return cuối cùng để trả về giá trị của hàm
END;


GO
/****** Object:  UserDefinedFunction [dbo].[tenBenhNhan]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[tenBenhNhan]
( 
    -- parameters
    @maDH nvarchar(50)
)
RETURNS nvarchar(50)
AS
BEGIN
    -- function body
    DECLARE @result nvarchar(50)
    DECLARE @tenBN nvarchar(50)
	DECLARE @traVe nvarchar(50)
    SELECT @result = maBN FROM dongho WHERE maDH = @maDH;

    IF @result IS NULL 
        Set @traVe = N'Khong Tim Thay MaDH';
    ELSE 
    BEGIN
        SELECT @tenBN = tenBN FROM benhnhan WHERE maBN = @result;
        Set @traVe = @tenBN;
    END
	return @traVe
END


GO
/****** Object:  Table [dbo].[bacsi]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bacsi](
	[maBS] [nvarchar](50) NOT NULL,
	[tenBS] [nvarchar](50) NULL,
	[gioiTinh] [nvarchar](10) NULL,
	[chucVu] [nvarchar](20) NULL,
	[maKhoa] [nvarchar](50) NULL,
	[diaChi] [nvarchar](50) NULL,
	[soDienThoai] [nvarchar](20) NULL,
	[email] [nvarchar](50) NULL,
	[capBac] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[maBS] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[benh]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[benh](
	[maBenh] [nvarchar](50) NOT NULL,
	[maNhomBenh] [nvarchar](50) NULL,
	[tenBenh] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[maBenh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[benhnhan]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[benhnhan](
	[maBN] [nvarchar](50) NOT NULL,
	[tenBN] [nvarchar](50) NULL,
	[gioiTinh] [nvarchar](10) NULL,
	[ngaySinh] [date] NULL,
	[diaChi] [nvarchar](50) NULL,
	[soDienThoai] [nvarchar](50) NOT NULL,
	[maBHYT] [nvarchar](50) NULL,
	[maPhong] [nvarchar](50) NULL CONSTRAINT [DF_benhnhan_maPhong]  DEFAULT (''),
 CONSTRAINT [PK__benhnhan__7A3F664CB277E7B3] PRIMARY KEY CLUSTERED 
(
	[maBN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cambien]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cambien](
	[maCB] [nvarchar](50) NOT NULL,
	[cbNhipTim] [float] NULL,
	[cbSPO2] [float] NULL,
	[cbGPS] [nvarchar](20) NULL,
	[maBN] [nvarchar](50) NULL,
	[maDH] [nvarchar](50) NULL,
 CONSTRAINT [PK__cambien__7A3E0CE48E6ABF94] PRIMARY KEY CLUSTERED 
(
	[maCB] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CamBienBenhVien]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CamBienBenhVien](
	[maCBBV] [nvarchar](50) NOT NULL,
	[tenCB] [nvarchar](50) NULL,
	[trangThai] [int] NULL,
	[maPhong] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[maCBBV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[dongho]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dongho](
	[maDH] [nvarchar](50) NOT NULL,
	[maBN] [nvarchar](50) NOT NULL,
	[pass] [nchar](20) NULL,
 CONSTRAINT [PK_dongho] PRIMARY KEY CLUSTERED 
(
	[maDH] ASC,
	[maBN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[donthuoc]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[donthuoc](
	[maDT] [nvarchar](50) NOT NULL,
	[maThuoc] [nvarchar](50) NULL,
	[soLuong] [int] NULL,
	[tienThuoc] [int] NULL,
	[maPCD] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[maDT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[khoa]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[khoa](
	[maKhoa] [nvarchar](50) NOT NULL,
	[tenKhoa] [nvarchar](50) NULL,
	[viTri] [nvarchar](50) NULL,
	[pass] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[maKhoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lichhen]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[lichhen](
	[maBS] [nvarchar](50) NOT NULL,
	[maBN] [nvarchar](50) NOT NULL,
	[ngay] [date] NOT NULL,
	[gio] [varchar](50) NOT NULL,
	[trangThai] [nvarchar](20) NULL,
 CONSTRAINT [PK_lichhen] PRIMARY KEY CLUSTERED 
(
	[maBS] ASC,
	[ngay] ASC,
	[gio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[lichsubenh]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lichsubenh](
	[maBenh] [nvarchar](50) NOT NULL,
	[maPCD] [nvarchar](50) NOT NULL,
	[Note] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[maBenh] ASC,
	[maPCD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[nhombenh]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[nhombenh](
	[maNhomBenh] [nvarchar](50) NOT NULL,
	[tenNhomBenh] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[maNhomBenh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[phieuchuandoan]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[phieuchuandoan](
	[maPCD] [nvarchar](50) NOT NULL,
	[maBS] [nvarchar](50) NULL,
	[maBN] [nvarchar](50) NULL,
	[ngayCD] [datetime] NULL,
	[tongTien] [float] NULL,
	[trangThai] [nvarchar](50) NULL,
	[stt] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[maPCD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[phieukham]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[phieukham](
	[maPK] [nvarchar](50) NOT NULL,
	[ngayKham] [date] NULL,
	[stt] [int] NULL CONSTRAINT [DF_phieukham_stt]  DEFAULT ((2)),
	[tienKham] [float] NULL,
	[maPhong] [nvarchar](50) NULL,
	[maPCD] [nvarchar](50) NULL,
	[trangThai] [nvarchar](50) NULL CONSTRAINT [DF_phieukham_trangThai]  DEFAULT (N'false'),
PRIMARY KEY CLUSTERED 
(
	[maPK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[phong]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[phong](
	[maPhong] [nvarchar](50) NOT NULL,
	[maKhoa] [nvarchar](50) NULL,
	[tenPhong] [nvarchar](50) NULL,
	[viTri] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[maPhong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[thuoc]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[thuoc](
	[maThuoc] [nvarchar](50) NOT NULL,
	[tenThuoc] [nvarchar](50) NULL,
	[donVi] [nvarchar](20) NULL,
	[giaTien] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[maThuoc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  UserDefinedFunction [dbo].[bangPCD]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[bangPCD]()
returns table
as
return
(
	select maPCD, bacsi.tenBS, benhnhan.tenBN, ngayCD, tongTien, trangThai
	from phieuChuanDoan
	Inner Join bacsi on phieuChuanDoan.maBS = bacSi.maBS
	inner join benhnhan on phieuChuanDoan.maBN = benhNhan.maBN
	where trangThai = 'false'
)

GO
ALTER TABLE [dbo].[bacsi]  WITH CHECK ADD  CONSTRAINT [FK_maKhoa] FOREIGN KEY([maKhoa])
REFERENCES [dbo].[khoa] ([maKhoa])
GO
ALTER TABLE [dbo].[bacsi] CHECK CONSTRAINT [FK_maKhoa]
GO
ALTER TABLE [dbo].[benh]  WITH CHECK ADD  CONSTRAINT [FK_maNhomBenh] FOREIGN KEY([maNhomBenh])
REFERENCES [dbo].[nhombenh] ([maNhomBenh])
GO
ALTER TABLE [dbo].[benh] CHECK CONSTRAINT [FK_maNhomBenh]
GO
ALTER TABLE [dbo].[benhnhan]  WITH CHECK ADD  CONSTRAINT [FK_maPhong] FOREIGN KEY([maPhong])
REFERENCES [dbo].[phong] ([maPhong])
GO
ALTER TABLE [dbo].[benhnhan] CHECK CONSTRAINT [FK_maPhong]
GO
ALTER TABLE [dbo].[cambien]  WITH CHECK ADD  CONSTRAINT [FK_cambien_dongho] FOREIGN KEY([maDH], [maBN])
REFERENCES [dbo].[dongho] ([maDH], [maBN])
GO
ALTER TABLE [dbo].[cambien] CHECK CONSTRAINT [FK_cambien_dongho]
GO
ALTER TABLE [dbo].[CamBienBenhVien]  WITH CHECK ADD  CONSTRAINT [FK_maPhong8] FOREIGN KEY([maPhong])
REFERENCES [dbo].[phong] ([maPhong])
GO
ALTER TABLE [dbo].[CamBienBenhVien] CHECK CONSTRAINT [FK_maPhong8]
GO
ALTER TABLE [dbo].[dongho]  WITH CHECK ADD  CONSTRAINT [FK_maBN] FOREIGN KEY([maBN])
REFERENCES [dbo].[benhnhan] ([maBN])
GO
ALTER TABLE [dbo].[dongho] CHECK CONSTRAINT [FK_maBN]
GO
ALTER TABLE [dbo].[donthuoc]  WITH CHECK ADD  CONSTRAINT [FK_donthuoc_phieuchuandoan] FOREIGN KEY([maPCD])
REFERENCES [dbo].[phieuchuandoan] ([maPCD])
GO
ALTER TABLE [dbo].[donthuoc] CHECK CONSTRAINT [FK_donthuoc_phieuchuandoan]
GO
ALTER TABLE [dbo].[donthuoc]  WITH CHECK ADD  CONSTRAINT [FK_maThuoc2] FOREIGN KEY([maThuoc])
REFERENCES [dbo].[thuoc] ([maThuoc])
GO
ALTER TABLE [dbo].[donthuoc] CHECK CONSTRAINT [FK_maThuoc2]
GO
ALTER TABLE [dbo].[lichhen]  WITH CHECK ADD  CONSTRAINT [FK_maBN2] FOREIGN KEY([maBN])
REFERENCES [dbo].[benhnhan] ([maBN])
GO
ALTER TABLE [dbo].[lichhen] CHECK CONSTRAINT [FK_maBN2]
GO
ALTER TABLE [dbo].[lichhen]  WITH CHECK ADD  CONSTRAINT [FK_maBS] FOREIGN KEY([maBS])
REFERENCES [dbo].[bacsi] ([maBS])
GO
ALTER TABLE [dbo].[lichhen] CHECK CONSTRAINT [FK_maBS]
GO
ALTER TABLE [dbo].[lichhen]  WITH CHECK ADD  CONSTRAINT [FK_maBS2] FOREIGN KEY([maBS])
REFERENCES [dbo].[bacsi] ([maBS])
GO
ALTER TABLE [dbo].[lichhen] CHECK CONSTRAINT [FK_maBS2]
GO
ALTER TABLE [dbo].[lichsubenh]  WITH CHECK ADD  CONSTRAINT [FK_maBenh1] FOREIGN KEY([maBenh])
REFERENCES [dbo].[benh] ([maBenh])
GO
ALTER TABLE [dbo].[lichsubenh] CHECK CONSTRAINT [FK_maBenh1]
GO
ALTER TABLE [dbo].[lichsubenh]  WITH CHECK ADD  CONSTRAINT [FK_maPCD1] FOREIGN KEY([maPCD])
REFERENCES [dbo].[phieuchuandoan] ([maPCD])
GO
ALTER TABLE [dbo].[lichsubenh] CHECK CONSTRAINT [FK_maPCD1]
GO
ALTER TABLE [dbo].[phieuchuandoan]  WITH CHECK ADD  CONSTRAINT [FK_maBN4] FOREIGN KEY([maBN])
REFERENCES [dbo].[benhnhan] ([maBN])
GO
ALTER TABLE [dbo].[phieuchuandoan] CHECK CONSTRAINT [FK_maBN4]
GO
ALTER TABLE [dbo].[phieuchuandoan]  WITH CHECK ADD  CONSTRAINT [FK_maBS4] FOREIGN KEY([maBS])
REFERENCES [dbo].[bacsi] ([maBS])
GO
ALTER TABLE [dbo].[phieuchuandoan] CHECK CONSTRAINT [FK_maBS4]
GO
ALTER TABLE [dbo].[phieukham]  WITH CHECK ADD  CONSTRAINT [FK_maPCD5] FOREIGN KEY([maPCD])
REFERENCES [dbo].[phieuchuandoan] ([maPCD])
GO
ALTER TABLE [dbo].[phieukham] CHECK CONSTRAINT [FK_maPCD5]
GO
ALTER TABLE [dbo].[phieukham]  WITH CHECK ADD  CONSTRAINT [FK_maPhong5] FOREIGN KEY([maPhong])
REFERENCES [dbo].[phong] ([maPhong])
GO
ALTER TABLE [dbo].[phieukham] CHECK CONSTRAINT [FK_maPhong5]
GO
ALTER TABLE [dbo].[phong]  WITH CHECK ADD  CONSTRAINT [FK_maKhoa6] FOREIGN KEY([maKhoa])
REFERENCES [dbo].[khoa] ([maKhoa])
GO
ALTER TABLE [dbo].[phong] CHECK CONSTRAINT [FK_maKhoa6]
GO
/****** Object:  StoredProcedure [dbo].[checkInfo]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[checkInfo]
@madh varchar(25),
@pass varchar(25)
as
begin
	set nocount on;
	declare @mabn varchar(25);

	select @mabn = mabn from dongho where madh = @madh and pass = @pass;

	select tenbn from benhnhan where mabn = @mabn;
end;

GO
/****** Object:  StoredProcedure [dbo].[getID2]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getID2]
    @tenBang nvarchar(100),
    @tenCot nvarchar(50),
    @id nvarchar(50) OUTPUT
AS
BEGIN
    DECLARE @sql nvarchar(max)
    SET @sql = 'SELECT TOP 1 @id = ' + QUOTENAME(@tenCot) + ' FROM ' + QUOTENAME(@tenBang) + ' ORDER BY ID DESC'

    EXEC sp_executesql @sql, N'@id nvarchar(50) OUTPUT', @id OUTPUT
END

GO
/****** Object:  StoredProcedure [dbo].[GetPhieuChuanDoan]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPhieuChuanDoan]
    @mabn NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT bs.tenBS AS 'Tên bác sĩ', pcd.stt AS 'Số thứ tự', pcd.ngaycd AS 'Ngày chuẩn đoán', k.vitri AS 'Vị trí khoa'
    FROM phieuchuandoan AS pcd
    INNER JOIN bacsi AS bs ON bs.mabs = pcd.mabs
    INNER JOIN khoa AS k ON bs.makhoa = k.makhoa
    WHERE pcd.mabn = @mabn;
END;

GO
/****** Object:  StoredProcedure [dbo].[GetPhieuChuanDoanByDate]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPhieuChuanDoanByDate]
    @NgayCD DATE,
    @mabn NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT bs.tenBS as 'Tên bác sĩ' , pcd.stt as 'Số thứ tự', pcd.ngaycd as 'Ngày chuẩn đoán', k.vitri as 'Vị trí khoa'
    FROM phieuchuandoan AS pcd
    INNER JOIN bacsi AS bs ON bs.mabs = pcd.mabs
    INNER JOIN khoa AS k ON bs.makhoa = k.makhoa
    WHERE CAST(pcd.ngaycd AS DATE) = @NgayCD AND pcd.mabn = @mabn;
END;

GO
/****** Object:  StoredProcedure [dbo].[insDT]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[insDT]
@maDT nvarchar(50),
@tenThuoc nvarchar(50),
@soLuong int,
@maPCD nvarchar(50)
as
begin
	begin
	declare @maThuoc nvarchar(50)
	declare @tien int
	select @maThuoc = maThuoc, @tien = giaTien from thuoc where tenThuoc = @tenThuoc
	end
	insert into donThuoc values (@maDT, @maThuoc, @soLuong, @soLuong * @tien, @maPCD)
	select thuoc.TenThuoc as 'Tên Bệnh', soluong as 'Số Lượng', tienThuoc as 'Tiền Thuốc'
	from donThuoc
	inner join thuoc on thuoc.mathuoc = donThuoc.maThuoc
	where maPCD = @maPCD
end

GO
/****** Object:  StoredProcedure [dbo].[insLSB]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[insLSB]
@tenBenh nvarchar(50),
@maPCD nvarchar(50),
@note nvarchar(max)
as
begin
	begin
	declare @maBenh nvarchar(50)
	select @maBenh = maBenh from benh where tenBenh = @tenBenh
	end
	insert into lichsubenh values (@maBenh, @maPCD, @note)
	select benh.TenBenh as 'Tên Bệnh', note as 'Ghi Chú' 
	from lichSuBenh
	inner join benh on lichSuBenh.mabenh = benh.mabenh
	where maPCD = @maPCD
end

GO
/****** Object:  StoredProcedure [dbo].[khoiTaoBenhNhan]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[khoiTaoBenhNhan]
@maPCD nvarchar(50),
@maBS nvarchar(50),
@maDH nvarchar(50)
AS
	begin
	DECLARE @maBN nvarchar(50)
	select @maBN = maBN from dongho where maDH = @maDH
	end
	DECLARE @ngay datetime
	select @ngay = GetDATE()
	insert into phieuchuandoan (maPCD, maBS, maBN, ngayCD, trangThai) values (@maPCD, @maBS, @maBN, @ngay, 'false')
	select * from phieuchuandoan where maPCD = @maPCD

GO
/****** Object:  StoredProcedure [dbo].[SearchBacSi]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchBacSi]
    @keyword NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT maBS as 'Mã Bác Sĩ', tenBS as 'Tên', gioiTinh as 'Giới Tính', chucVu as 'Chức Vụ', diaChi as 'Địa Chỉ', soDienThoai as 'SĐT', email
    FROM bacsi 
    WHERE maBS LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI 
        OR tenBS LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI 
        OR chucVu LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI
		OR maKhoa LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI
		OR soDienThoai LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI
		OR email LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI
END;

GO
/****** Object:  StoredProcedure [dbo].[SearchBenhNhan]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchBenhNhan]
    @keyword NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT maBN as'Mã Bệnh Nhân', tenBN as 'Tên', gioiTinh as 'Giới Tính', ngaySinh as 'Ngày Sinh', diaChi as 'Địa Chỉ', soDienThoai as 'SĐT', maBHYT as 'BHYT'
    FROM benhnhan
    WHERE maBN LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI 
        OR tenBN LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI 
        OR diaChi LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI
		OR soDienThoai LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI
		OR maPhong LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI
		OR maBHYT LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI
END;

GO
/****** Object:  StoredProcedure [dbo].[SearchPCD]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchPCD]
    @keyword NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT phieuChuanDoan.maPCD as 'ID', phieuChuanDoan.maBS as 'Mã BS', bacsi.tenBS as 'Tên BS', phieuChuanDoan.maBN as 'Mã BN', benhnhan.tenBN as 'Tên BN', ngayCD as 'Ngày Chuẩn Đoán', tongTien as 'Thành Tiền', TrangThai as 'Trạng Thái', stt
    FROM phieuChuanDoan
	INNER JOIN bacsi ON phieuChuanDoan.maBS = bacsi.maBS
	INNER JOIN benhnhan ON phieuChuanDoan.maBN = benhnhan.maBN
    WHERE maPCD LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI 
        OR phieuChuanDoan.maBS LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI 
        OR phieuChuanDoan.maBN LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI
		OR bacSi.tenBS LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI
		OR benhnhan.tenBN LIKE '%' + @keyword + '%' COLLATE Latin1_General_CI_AI

END;

GO
/****** Object:  StoredProcedure [dbo].[SearchThuoc]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchThuoc]
    @keyword NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT thuoc.tenThuoc, thuoc.maThuoc
    FROM thuoc
	--INNER JOIN thuoc ON donThuoc.maThuoc = thuoc.maThuoc
    WHERE Thuoc.maThuoc LIKE  @keyword + '%' COLLATE Latin1_General_CI_AI 
        OR thuoc.tenThuoc LIKE @keyword + '%' COLLATE Latin1_General_CI_AI 

END;

GO
/****** Object:  StoredProcedure [dbo].[selLSB]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[selLSB]
@maPCD nvarchar(50)
as
begin
select benh.TenBenh as 'Tên Bệnh', note as 'Ghi Chú' 
	from lichSuBenh
	inner join benh on lichSuBenh.mabenh = benh.mabenh
	where maPCD = @maPCD
end


GO
/****** Object:  StoredProcedure [dbo].[slBS]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[slBS]
@tenBS nvarchar(50)
as
begin
	select * from bacsi where tenBS = @tenBS
end

GO
/****** Object:  StoredProcedure [dbo].[slDT]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[slDT]
@maPCD nvarchar(50)
as
begin
	select thuoc.TenThuoc as 'Tên Bệnh', soluong as 'Số Lượng', tienThuoc as 'Tiền Thuốc'
	from donThuoc
	inner join thuoc on thuoc.mathuoc = donThuoc.maThuoc
	where maPCD = @maPCD
end

GO
/****** Object:  StoredProcedure [dbo].[slLSB]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[slLSB]
@maPCD nvarchar(50)
as
begin
	select thuoc.TenThuoc as 'Tên Bệnh', soluong as 'Số Lượng', tienThuoc as 'Tiền Thuốc'
	from donThuoc
	inner join thuoc on thuoc.mathuoc = donThuoc.maThuoc
	where maPCD = @maPCD
end

GO
/****** Object:  StoredProcedure [dbo].[slPK]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[slPK]
@maPCD nvarchar(50)
As
begin
	select  tenBN as 'Tên Bệnh Nhân', ngayKham as 'Ngày Khám', phieuKham.stt as 'Số Thứ Tự', tienKham as 'Tiền Khám', phong.tenPhong as 'Tên Phòng'
	from phieuKham 
	inner join phong ON phong.maPhong = phieuKham.maPhong
	inner join phieuChuanDoan ON phieuChuanDoan.maPCD = phieuKham.maPCD
	inner join benhNhan ON phieuChuanDoan.maBN = benhnhan.maBN
	where phieuKham.maPCD = @maPCD
end

GO
/****** Object:  StoredProcedure [dbo].[slPKP]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[slPKP]
@maPhong nvarchar(50)
As
begin
	select  tenBN as 'Tên Bệnh Nhân', ngayKham as 'Ngày Khám', phieuKham.stt as 'Số Thứ Tự', tienKham as 'Tiền Khám'
	from phieuKham 
	inner join phong ON phong.maPhong = phieuKham.maPhong
	inner join phieuChuanDoan ON phieuChuanDoan.maPCD = phieuKham.maPCD
	inner join benhNhan ON phieuChuanDoan.maBN = benhnhan.maBN
	where phong.maPhong = @maPhong
end

GO
/****** Object:  StoredProcedure [dbo].[themPK]    Script Date: 11/8/2024 11:11:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[themPK]
@maPK nvarchar(50),
@tienKham float,
@tenPhong nvarchar(50),
@maPCD nvarchar(50)
AS
Begin
	declare @maPhong nvarchar(50)
	select @maPhong = maPhong from phong where tenPhong = @tenPhong
	declare @stt int
	IF (SELECT max(stt) FROM phieuKham WHERE maPhong = @maPhong) is null
	SELECT @stt = 1
	ELSE
    SELECT @stt = max(stt) + 1 FROM phieuKham WHERE maPhong = @maPhong
	declare @ngayKham datetime
	select @ngayKham = GetDate()
	insert into phieuKham (maPK, ngayKham, stt, tienKham, maPhong, maPCD)
	values (@maPK, @ngayKham, @stt, @tienKham, @maPhong, @maPCD)
	select  tenBN as 'Tên Bệnh Nhân', ngayKham as 'Ngày Khám', phieuKham.stt as 'Số Thứ Tự', tienKham as 'Tiền Khám', phong.tenPhong as 'Tên Phòng'
	from phieuKham 
	inner join phong ON phong.maPhong = phieuKham.maPhong
	inner join phieuChuanDoan ON phieuChuanDoan.maPCD = phieuKham.maPCD
	inner join benhNhan ON phieuChuanDoan.maBN = benhnhan.maBN
	where phieuKham.maPCD = @maPCD
end

GO
USE [master]
GO
ALTER DATABASE [iot2024] SET  READ_WRITE 
GO

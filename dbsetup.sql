IF DB_ID('Group10_iPERMITDB') IS NOT NULL
BEGIN
    ALTER DATABASE [Group10_iPERMITDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [Group10_iPERMITDB];
END
GO

CREATE DATABASE [Group10_iPERMITDB];
GO

USE [Group10_iPERMITDB];
GO
/****** Object:  Table [dbo].[Decision]    Script Date: 4/8/2026 1:11:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Decision](
	[ID] [varchar](50) NOT NULL,
	[dateOfDecision] [date] NULL,
	[finalDecision] [varchar](50) NULL,
	[description] [varchar](max) NULL,
	[EOID] [varchar](50) NOT NULL,
	[permitRequestID] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailArchive]    Script Date: 4/8/2026 1:11:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailArchive](
	[emailID] [varchar](50) NOT NULL,
	[emailDate] [date] NULL,
	[reason] [varchar](50) NULL,
	[REID] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[emailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnvironmentalPermits]    Script Date: 4/8/2026 1:11:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnvironmentalPermits](
	[permitID] [varchar](50) NOT NULL,
	[permitName] [varchar](50) NULL,
	[permitFee] [float] NULL,
	[description] [varchar](max) NULL,
	[paymentCtrlBy] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[permitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EO]    Script Date: 4/8/2026 1:11:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EO](
	[ID] [varchar](50) NOT NULL,
	[Name] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OPS_CPP]    Script Date: 4/8/2026 1:11:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OPS_CPP](
	[ID] [varchar](50) NOT NULL,
	[Name] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 4/8/2026 1:11:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[paymentID] [varchar](50) NOT NULL,
	[paymentDate] [date] NULL,
	[paymentMethod] [varchar](50) NULL,
	[last4CardDigit] [float] NULL,
	[cardHolderName] [varchar](50) NULL,
	[paymentApproved] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[paymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permit]    Script Date: 4/8/2026 1:11:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permit](
	[permitID] [varchar](50) NOT NULL,
	[dateOfIssue] [date] NULL,
	[duration] [varchar](50) NULL,
	[description] [varchar](max) NULL,
	[issuedBy] [varchar](50) NULL,
	[issuedTo] [varchar](50) NULL,
	[relatedTo] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[permitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermitRequest]    Script Date: 4/8/2026 1:11:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermitRequest](
	[requestNo] [varchar](50) NOT NULL,
	[dateOfRequest] [date] NULL,
	[activityDescription] [varchar](max) NULL,
	[activityStartDate] [date] NULL,
	[activityDuration] [date] NULL,
	[permitFee] [float] NULL,
	[permitTypeID] [varchar](50) NOT NULL,
	[permitREID] [varchar](50) NOT NULL,
	[permitPayment] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[requestNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RE]    Script Date: 4/8/2026 1:11:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RE](
	[ID] [varchar](50) NOT NULL,
	[contactPersonName] [varchar](50) NULL,
	[password] [varchar](50) NOT NULL,
	[createdDate] [date] NULL,
	[email] [varchar](50) NULL,
	[organizationName] [varchar](50) NULL,
	[organizationAddress] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestStatus]    Script Date: 4/8/2026 1:11:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestStatus](
	[permitRequestStatus] [varchar](50) NOT NULL,
	[date] [date] NOT NULL,
	[description] [varchar](max) NOT NULL,
	[requestID] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[permitRequestStatus] ASC,
	[date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RESite]    Script Date: 4/8/2026 1:11:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RESite](
	[ID] [varchar](50) NOT NULL,
	[siteAddress] [varchar](50) NULL,
	[siteContactPerson] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Decision]  WITH CHECK ADD  CONSTRAINT [fk_EO] FOREIGN KEY([EOID])
REFERENCES [dbo].[EO] ([ID])
GO
ALTER TABLE [dbo].[Decision] CHECK CONSTRAINT [fk_EO]
GO
ALTER TABLE [dbo].[Decision]  WITH CHECK ADD  CONSTRAINT [fk_PR] FOREIGN KEY([permitRequestID])
REFERENCES [dbo].[PermitRequest] ([requestNo])
GO
ALTER TABLE [dbo].[Decision] CHECK CONSTRAINT [fk_PR]
GO
ALTER TABLE [dbo].[EmailArchive]  WITH CHECK ADD  CONSTRAINT [fk_emailRE] FOREIGN KEY([REID])
REFERENCES [dbo].[RE] ([ID])
GO
ALTER TABLE [dbo].[EmailArchive] CHECK CONSTRAINT [fk_emailRE]
GO
ALTER TABLE [dbo].[EnvironmentalPermits]  WITH CHECK ADD  CONSTRAINT [(payment_fk)] FOREIGN KEY([paymentCtrlBy])
REFERENCES [dbo].[OPS_CPP] ([ID])
GO
ALTER TABLE [dbo].[EnvironmentalPermits] CHECK CONSTRAINT [(payment_fk)]
GO
ALTER TABLE [dbo].[Permit]  WITH CHECK ADD  CONSTRAINT [issued_by] FOREIGN KEY([issuedBy])
REFERENCES [dbo].[EO] ([ID])
GO
ALTER TABLE [dbo].[Permit] CHECK CONSTRAINT [issued_by]
GO
ALTER TABLE [dbo].[Permit]  WITH CHECK ADD  CONSTRAINT [issued_to] FOREIGN KEY([issuedTo])
REFERENCES [dbo].[RE] ([ID])
GO
ALTER TABLE [dbo].[Permit] CHECK CONSTRAINT [issued_to]
GO
ALTER TABLE [dbo].[Permit]  WITH CHECK ADD  CONSTRAINT [related_to] FOREIGN KEY([relatedTo])
REFERENCES [dbo].[PermitRequest] ([requestNo])
GO
ALTER TABLE [dbo].[Permit] CHECK CONSTRAINT [related_to]
GO
ALTER TABLE [dbo].[PermitRequest]  WITH CHECK ADD  CONSTRAINT [fk_payment] FOREIGN KEY([permitPayment])
REFERENCES [dbo].[Payment] ([paymentID])
GO
ALTER TABLE [dbo].[PermitRequest] CHECK CONSTRAINT [fk_payment]
GO
ALTER TABLE [dbo].[PermitRequest]  WITH CHECK ADD  CONSTRAINT [fk_permit_type] FOREIGN KEY([permitTypeID])
REFERENCES [dbo].[EnvironmentalPermits] ([permitID])
GO
ALTER TABLE [dbo].[PermitRequest] CHECK CONSTRAINT [fk_permit_type]
GO
ALTER TABLE [dbo].[PermitRequest]  WITH CHECK ADD  CONSTRAINT [fk_REID] FOREIGN KEY([permitREID])
REFERENCES [dbo].[RE] ([ID])
GO
ALTER TABLE [dbo].[PermitRequest] CHECK CONSTRAINT [fk_REID]
GO
ALTER TABLE [dbo].[RequestStatus]  WITH CHECK ADD  CONSTRAINT [req_fk] FOREIGN KEY([requestID])
REFERENCES [dbo].[PermitRequest] ([requestNo])
GO
ALTER TABLE [dbo].[RequestStatus] CHECK CONSTRAINT [req_fk]
GO
ALTER TABLE [dbo].[RESite]  WITH CHECK ADD  CONSTRAINT [fk_RE] FOREIGN KEY([ID])
REFERENCES [dbo].[RE] ([ID])
GO
ALTER TABLE [dbo].[RESite] CHECK CONSTRAINT [fk_RE]
GO

INSERT INTO EO 
VALUES ('eo001','John Smith');

INSERT INTO OPS_CPP
VALUES ('1','Payment Controller');

INSERT INTO EnvironmentalPermits
VALUES
('1','Air Permit','17.50','For activities that pollute the air.','1'),
('2','Land Permit','67.50','For activities that pollute the ground.','1'),
('3','Water Permit','37.50','For activities that pollute the water.','1');

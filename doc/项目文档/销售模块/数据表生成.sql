--开票申请
CREATE TABLE [dbo].[T_BillingApplication](
	[Code] [nvarchar](50)   NOT NULL,
	[BillMainCode] [nvarchar](50)   NULL,
	[TruckCode] [nvarchar](50)   NULL,
	[AppPersionCode] [nvarchar](50)   NULL,
	[SaleDeptCode] [nvarchar](50)   NULL,
	[AppDate] [nvarchar](50)   NULL,
	[SysPersionCode] [nvarchar](50)   NULL,
	[SysDateTime] [nvarchar](50)   NULL,
	[Explain] [nvarchar](500)   NULL,
	[Note1] [nvarchar](50)   NULL,
	[Note2] [nvarchar](50)   NULL,
	[Note3] [nvarchar](50)   NULL,
	[Note4] [nvarchar](50)   NULL,
	[Note5] [nvarchar](50)   NULL,
	[Note6] [nvarchar](50)   NULL,
	[Note7] [nvarchar](50)   NULL,
	[Note8] [nvarchar](50)   NULL,
	[Note9] [nvarchar](50)   NULL,
	[Note10] [nvarchar](50)   NULL,
 CONSTRAINT [PK_T_BillingApplication] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
--配置项
CREATE TABLE [dbo].[T_ControlItem](
	[Code] [nvarchar](50)   NOT NULL,
	[CName] [nvarchar](50)   NULL,
	[ControlCodeFirst] [nvarchar](50)   NULL,
	[ControlNameFirst] [nvarchar](50)   NULL,
	[ControlCodeSecond] [nvarchar](50)   NULL,
	[ControlNameSecond] [nvarchar](50)   NULL,
	[Remark] [nvarchar](100)   NULL,
	[Note1] [nvarchar](50)   NULL,
	[Note2] [nvarchar](50)   NULL,
	[Note3] [nvarchar](50)   NULL,
	[Note4] [nvarchar](50)   NULL,
	[Note5] [nvarchar](50)   NULL,
 CONSTRAINT [PK_T_ControlItem] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
--返利标准
CREATE TABLE [dbo].[T_RebatesStandard](
	[EffectiveYear] [char](4)   NULL,
	[TruckTypeCode] [nvarchar](50)   NULL,
	[DeptCode] [nvarchar](50)   NULL,
	[SaleFeeTypeCode] [nvarchar](50)   NULL,
	[DeptBegainAllocation] [decimal](18, 2) NULL,
	[ControlItemCode] [nvarchar](50)   NULL,
	[Fee] [decimal](18, 2) NULL,
	[Status] [char](1)   NULL,
	[Remark] [nvarchar](50)   NULL,
	[Note1] [nvarchar](50)   NULL,
	[Note2] [nvarchar](50)   NULL,
	[Note3] [nvarchar](50)   NULL,
	[Note4] [nvarchar](50)   NULL,
	[Note5] [nvarchar](50)   NULL,
	[Note6] [nvarchar](50)   NULL,
	[Note7] [nvarchar](50)   NULL,
	[Note8] [nvarchar](50)   NULL,
	[Note9] [nvarchar](50)   NULL,
	[Note10] [nvarchar](50)   NULL
) ON [PRIMARY]
--销售过程表
CREATE TABLE [dbo].[T_SaleProcess](
	[Code] [nvarchar](50)   NOT NULL,
	[PName] [nvarchar](50)   NULL,
	[Status] [char](1)   NULL,
	[Note1] [nvarchar](50)   NULL,
	[Note2] [nvarchar](50)   NULL,
	[Note3] [nvarchar](50)   NULL,
	[Note4] [nvarchar](50)   NULL,
	[Note5] [nvarchar](50)   NULL,
 CONSTRAINT [PK_T_SaleProcess] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
--特殊返利申请
CREATE TABLE [dbo].[T_SpecialRebatesApp](
	[Code] [nvarchar](50)   NOT NULL,
	[BillMainCode] [nvarchar](50)   NULL,
	[TruckTypeCode] [nvarchar](50)   NULL,
	[TruckCode] [nvarchar](50)   NULL,
	[AppPersionCode] [nvarchar](50)   NULL,
	[SaleDeptCode] [nvarchar](50)   NULL,
	[StandardSaleAmount] [decimal](18, 0) NULL,
	[FactSaleAmount] [decimal](18, 0) NULL,
	[AppDate] [char](10)   NULL,
	[SysPersionCode] [nvarchar](50)   NULL,
	[SysDateTime] [varchar](20)   NULL,
	[Explain] [nvarchar](500)   NULL,
	[Note1] [nvarchar](50)   NULL,
	[Note2] [nvarchar](50)   NULL,
	[Note3] [nvarchar](50)   NULL,
	[Note4] [nvarchar](50)   NULL,
	[Note5] [nvarchar](50)   NULL,
	[Note6] [nvarchar](50)   NULL,
	[Note7] [nvarchar](50)   NULL,
	[Note8] [nvarchar](50)   NULL,
	[Note9] [nvarchar](50)   NULL,
	[Note10] [nvarchar](50)   NULL,
 CONSTRAINT [PK_T_SpecialRebatesApp] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
--特殊返利批复
CREATE TABLE [dbo].[T_SpecialRebatesStandard](
	[AppBillCode] [nvarchar](50)   NULL,
	[TruckCode] [nvarchar](50)   NULL,
	[TruckTypeCode] [nvarchar](50)   NULL,
	[DeptCode] [nvarchar](50)   NULL,
	[SaleFeeTypeCode] [nvarchar](50)   NULL,
	[ControlItemCode] [nvarchar](50)   NULL,
	[Fee] [decimal](18, 2) NULL,
	[MarkerCode] [nvarchar](50)   NULL,
	[SysUserCode] [nvarchar](50)   NULL,
	[SysTime] [nvarchar](20)   NULL,
	[Note1] [nvarchar](50)   NULL,
	[Note2] [nvarchar](50)   NULL,
	[Note3] [nvarchar](50)   NULL,
	[Note4] [nvarchar](50)   NULL,
	[Note5] [nvarchar](50)   NULL,
	[Note6] [nvarchar](50)   NULL,
	[Note7] [nvarchar](50)   NULL,
	[Note8] [nvarchar](50)   NULL,
	[Note9] [nvarchar](50)   NULL,
	[Note10] [nvarchar](50)   NULL
) ON [PRIMARY]
--车辆类型表
CREATE TABLE [dbo].[T_truckType](
	[typeCode] [nvarchar](50)   NOT NULL,
	[typeName] [nvarchar](50)   NULL,
	[parentCode] [nvarchar](50)   NULL,
	[status] [char](1)   NULL CONSTRAINT [DF_T_truckType_status]  DEFAULT ((1)),
	[IsLastNode] [char](1)   NULL CONSTRAINT [DF_T_truckType_IsLastNode]  DEFAULT ((1)),
	[NOTE1] [nvarchar](50)   NULL,
	[NOTE2] [nvarchar](50)   NULL,
	[NOTE3] [nvarchar](50)   NULL,
	[NOTE4] [nvarchar](50)   NULL,
	[NOTE5] [nvarchar](50)   NULL,
 CONSTRAINT [PK_T_truckType] PRIMARY KEY CLUSTERED 
(
	[typeCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
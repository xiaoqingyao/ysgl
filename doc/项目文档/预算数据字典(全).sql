
--******************************基础信息beg************************************
--1、基础资料
--用户表 bill_users
CREATE TABLE [dbo].[bill_users](
	[userCode] [varchar](20) NULL,--用户编号
	[userName] [varchar](50) NULL,--用户名
	[userGroup] [varchar](20) NULL,--角色组编号
	[userStatus] [varchar](1) NULL,--状态 0-删除 1正常
	[userDept] [varchar](50) NULL,--部门编号
	[userPwd] [varchar](32) NULL,--密码
	[isSystem] [varchar](1) NULL,--是否系统用户
	[userPosition] [varchar](50) NULL--职位编号  对应bill_datadic中datatype为05下的datacode
)

--部门表 bill_departments
CREATE TABLE [dbo].[bill_departments](
	[deptCode] [varchar](20) NULL,--部门编号
	[deptName] [varchar](50) NULL,--部门名称
	[sjDeptCode] [varchar](20) NULL,--上级部门编号
	[deptStatus] [varchar](1) NULL,--状态 D-删除 1正常
	[IsSell] [char](1) NULL,
	[deptJianma] [varchar](50) NULL,
	[foru8id] [varchar](50) NULL,
	[forTianJian] [varchar](50) NULL,	
	[Isgk] [char](1) null, --是否是归口部门 Y-是 N-不是
	iskzys varchar(50) null,--

	
)

--- 部门归口设置
create table [bill_dept_gksz](
	gkdeptcode varchar(50) null,--归口部门编号
	gkdeptname varchar(50) null,--归口部门名称
	deptCode varchar(50) null,--被归口部门编号
	deptName varchar(50) null,--被归口部门名称
	note0 varchar(50) null,
	note1 varchar(50) null,
	note2 varchar(50) null,
	note3 varchar(50) null,
	note4 varchar(50) null,
	note5 varchar(50) null
)


--部门主管表 bill_dept_ywzg
CREATE TABLE [dbo].[bill_dept_ywzg](
	[deptCode] [varchar](20) NULL,--部门编号
	[userCode] [varchar](20) NULL--人员编号
	
)


--部门分管领导表 bill_dept_fgld
CREATE TABLE [dbo].[bill_dept_fgld](
	[deptCode] [varchar](20) NULL,--部门编号
	[userCode] [varchar](20) NULL--人员编号
)

--角色
CREATE TABLE [dbo].[bill_userGroup](
	[groupID] [varchar](20) NOT NULL,
	[groupName] [varchar](50) NULL,
	[gType] [varchar](1) NULL
) ON [PRIMARY]

--数据字典表 bill_dataDic
CREATE TABLE [dbo].[bill_dataDic](
	[dicType] [varchar](20) NULL,--类型（00，一级，其余2级，），
	[dicCode] [varchar](50) NULL,--编号
	[dicName] [varchar](50) NULL,--名称
	
	[cjys] [varchar](10) NULL,--是否冲减预算（帐套：存储帐套的接受者）
	[cys] [varchar](1) NULL,--控制预算
	cdj	varchar(2) null,--是否必须附单据（报销类型，0，否，1，报告单，2，申请单，）（申请类别，0，否，1，必须附报告单）（帐套号1 默认 0 不是默认）
	[note1] [nvarchar](50) NULL, --若dictype=18 存储预算flowID 若07(生成凭证对应类型) 存储对应的科目预算类型
	[note2] [nvarchar](50) NULL,--若dictype=18 存储决算 flowID
	[note3] [nvarchar](50) NULL,--已使用
	[note4] [nvarchar](50) NULL,
	[note5] [nvarchar](50) NULL
) ON [PRIMARY]

 --消息表 桌面友情提示表bill_msg
 CREATE TABLE [dbo].[bill_msg](
	[id] [int] IDENTITY(1,1) NOT NULL,--记录id 自增列
	[title] [varchar](200) NULL,--消息主题
	[contents] [text] NULL,--消息内容
	[writer] [varchar](50) NULL,--发布人
	[date] [datetime] NULL,--发布时间
	[readTimes] [int] NULL,--阅读次数
	[mstype] [nvarchar](50) NULL,--消息类型
	[notifierid] [nvarchar](50) NULL,
	[notifiername] [nvarchar](50) NULL,--通知人
	[endtime] [nvarchar](50) NULL,--通知有效截止日期
	[Accessories] [nvarchar](200) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

--通知人员
create table MessageReader(
	code varchar(30) null,--主表编号
	usercode varchar(30) null,--用户编号
	isRead int null --是否已读
)

--日记表bill_notePad 
CREATE TABLE [dbo].[bill_notePad](
	[listid] [decimal](18, 0) IDENTITY(1,1) NOT NULL,--记录id 自增列
	[noteDate] [datetime] NULL,--日期
	[userCode] [varchar](50) NULL,--用户编号
	[context] [varchar](300) NULL,--内容
	[noteType] [varchar](5) NULL,--类型
 CONSTRAINT [PK_bill_notePad] PRIMARY KEY CLUSTERED 
(
	[listid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

--预算项目表
CREATE TABLE [dbo].[bill_xm](
	[xmCode] [varchar](20) NULL,--编号
	[xmName] [varchar](200) NULL,--名称
	[sjXm] [varchar](50) NULL,--上级项目
	[xmDept] [varchar](50) NULL,--项目所属部门
	[xmStatus] [varchar](1) NULL--项目状态（0，停用，1，正常）
) ON [PRIMARY]


--项目部门年度控制金额表 作用：设置各部门下的各个项目的年度控制金额，并通过设置isCtrl字段来设置是否开启预算
  create table bill_xm_dept_nd
	  (
	    xmCode varchar(20),--项目编号
	    xmDept varchar(50),--项目部门  其中'000001'为公司项目 其他为部门下的项目
	    je float,--金额
	    isCtrl varchar(2),--项目金额是否受控
	    nd varchar(4),-- 年度
	    status varchar(2),--是否可用 0不可用 1可用 默认为1
	    note0 varchar(50),
	    note1 varchar(50),
	    note2 varchar(50),
	    note3 varchar(50),
	    note4 varchar(50),
	    note5 varchar(50),
	    note6 varchar(50),
	    note7 varchar(50),
	   )
	   
	--项目支付单表 2012-04-25增加
 CREATE TABLE [dbo].[bill_xmzfd](
	[billcode] [varchar](20) COLLATE Chinese_PRC_CI_AS NULL,--单号
	[sj] [datetime] NULL,--制单日期
	[zfDept] [varchar](500) COLLATE Chinese_PRC_CI_AS NULL,--支付部门
	[zfxm] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,--支付项目
	[zynr] [text] COLLATE Chinese_PRC_CI_AS NULL,--内容
	[sm] [text] COLLATE Chinese_PRC_CI_AS NULL,--说明
	[cbr] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,--承办人
	[ms] [text] COLLATE Chinese_PRC_CI_AS NULL,--附件
	[zfje] [float] NULL--金额
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

--生成编号表
CREATE TABLE [dbo].[cardNum](
	[list_id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[card] [varchar](30) NULL,
	[seed] [varchar](30) NULL, 
	[lsh] [int] NULL,
	[lshws] [int] NULL,
 CONSTRAINT [PK_cardNum] PRIMARY KEY CLUSTERED 
(
	[list_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


--******************************基础信息end************************************


--******************************系统设置beg************************************

--标记录tab_Message
CREATE TABLE [dbo].[tab_Message](
	[tbName] [nvarchar](50) NULL,--表名
	[tbMeaning] [nvarchar](100) NULL,--表备注
	[tbType] [char](1) NOT NULL,--表类型  2系统表 1业务表 0基础数据表
	[tbStatus] [char](1) NOT NULL,--状态1 正常 0废除
	[note1] [nvarchar](50) NULL,
	[note2] [nvarchar](50) NULL,
	[note3] [nvarchar](50) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表类型1=业务数据0=基础数据 2=不可删除数据' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tab_Message', @level2type=N'COLUMN',@level2name=N'tbType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表状态1 正常D废弃' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tab_Message', @level2type=N'COLUMN',@level2name=N'tbStatus'
GO

ALTER TABLE [dbo].[tab_Message] ADD  CONSTRAINT [DF_tab_Message_tbType]  DEFAULT ((1)) FOR [tbType]
GO

ALTER TABLE [dbo].[tab_Message] ADD  CONSTRAINT [DF_tab_Message_tbStatus]  DEFAULT ((1)) FOR [tbStatus]
GO


--系统参数设置
create table t_Config(
	akey varchar(30) primary key,--编号
	meaning varchar(50) not null,--名称
	avalue varchar(30) not null,--标识（0，否，1，是）
	classify varchar(30)  null,--分类（）
	remark  varchar(300) null,--备注
	szsm varchar(2000) null--设置说明
)


--用户权限表（菜单权限） bill_userRight
CREATE TABLE [dbo].[bill_userRight](
	[userCode] [varchar](50) NULL,
	[objectID] [varchar](20) NULL,
	[rightType] [varchar](1) NULL --1=>菜单操作权限 2=>人员管理部门权限 3==角色管理权限
)

--角色表 bill_userGroup
CREATE TABLE [dbo].[bill_userGroup](
	[groupID] [varchar](20) NOT NULL,--角色编号
	[groupName] [varchar](50) NULL,--角色名
	[gType] [varchar](1) 
) 


--系统配置
CREATE TABLE [dbo].[bill_SysConfig](
	[ConfigName] [varchar](50) NULL,--配置类型代码
	[ConfigValue] [varchar](50) NULL,--配置值
	[Memo] [varchar](50) NULL,--可选配置值及说明
	[nd] [varchar](10) NULL--年度
) ON [PRIMARY]


--菜单表 bill_sysMenu
CREATE TABLE [dbo].[bill_sysMenu](
	[menuid] [varchar](20) NOT NULL,--菜单id
	[menuName] [varchar](50) NULL,--系统菜单名 不允许用户修改的
	[showName] [varchar](50) NULL,--用于显示的名字 允许用户修改的
	[menuUrl] [varchar](200) NULL,--菜单url
	[menuOrder] [int] NULL,--排序字段
	[menusm] [varchar](200) NULL,
	[menustate] [varchar](10) NULL--菜单状态 状态为D（delete）不可见 逻辑删除

) ON

--菜单帮助
CREATE TABLE [dbo].[bill_sysMenuHelp](
	[list_id] [int] IDENTITY(1,1) NOT NULL,--记录id自增列
	[menuid] [varchar](20) NOT NULL,--菜单编号
	[menusm] [text] NULL,--菜单说明
	[note0] [varchar](50) NULL,
	[status] [varchar](1) NULL,
	[note1] [nvarchar](500) NULL,
	[note2] [nvarchar](500) NULL,
	[note3] [nvarchar](500) NULL,
	[note4] [nvarchar](500) NULL,
	[note5] [nvarchar](500) NULL,
	[note6] [nvarchar](500) NULL,
	[note7] [nvarchar](500) NULL,
	[note8] [nvarchar](500) NULL,
	[note9] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[list_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

--系统参数配置 用于生成预算过程  这个应该是有用的bill_syspar 
CREATE TABLE [dbo].[bill_syspar](
	[parname] [varchar](20) NULL,--配置名
	[parVal] [text] NULL--值
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


--******************************系统设置end************************************

--******************************预算模块beg************************************


--预算科目 bill_yskm
CREATE TABLE [dbo].[bill_yskm](
	[yskmCode] [varchar](20) NULL,--科目代码
	[yskmBm] [varchar](50) NULL,--科目编号
	[yskmMc] [varchar](50) NULL,--科目名称
	[gjfs] [varchar](1) NULL,--
	[tbsm] [text] NULL,--填报说明
	[tblx] [varchar](20) NULL,--填报类型（01，单位填报，02，财务填报，03销售费用）
	[kmStatus] [varchar](1) NULL,--科目状态（0，禁用，1，启用）
	[kmlx] varchar(2) null,--科目类型(是否可控 0：可控，1：不可控)
	gkfy varchar(1) null,--归口费用
	xmhs varchar(1) null,--项目核算
	bmhs varchar(1) null,--部门核算
	ryhs varchar(1),--人员核算
    kmzg nvarchar(100) null, --科目主管
	szlx varchar(50) null,--收支类型  0-减项  1-加项
	allowTz varchar(10) null,--是否允许科目预算调整  0不允许  1允许    默认为允许
    --新加字段
    dydj varchar(50) null,--对应单据
	zjhs varchar(50) null,--资金核算
	kmType varchar(50) null,--科目类型
	iszyys nvarchar(50)null,--是否占用预算 0 不占用
	note1 nvarchar(50) null,
	note2 nvarchar(50) null,
	note3 nvarchar(50) null,
	note4 nvarchar(50) null,
	note5 nvarchar(50) null
	) 



--预算科目部门对应表 bill_yskm_dept
create TABLE [dbo].[bill_yskm_dept](
	[deptCode] [varchar](50) NULL,--部门
	[yskmCode] [varchar](50) NULL,--预算科目
	[cwkmCode] [varchar](50) NULL,--财务科目--废弃字段
	jfkmcode1 varchar(50) null,--借方科目编号1
    dfkmcode1 varchar(50) null,--贷方科目编号1
    jfkmcode2 varchar(50) null,--借方科目编号2
    dfkmcode2 varchar(50) null--贷方科目编号2
	--新加字段
	djlx varchar(50) null,--单据类型（01 费用报销单（不含发票）、02费用报销单（补录发票）、03费用报销单（含发票）、04收入类、05往来付款）
) 


--预算科目 与财务科目的对照表 bill_yskm_dzb
CREATE TABLE [dbo].[bill_yskm_dzb](
	[yskmCode] [varchar](20) NULL,--预算科目编号
	[cwkmCode] [varchar](20) NULL,--财务科目编号
	[gjdw] [varchar](50) NULL,
	[xjdw] [varchar](50) NULL
) ON [PRIMARY]

 --预算科目归口部门对应表 bill_yskm_dept
create table bill_yskm_gkdept(
	deptcode varchar(50) not null,--部门编号
	yskmcode varchar(50) not null,--预算科目编号
	note1 varchar(50) null,
	note2 varchar(50) null,
	note3 varchar(50) null,
	note4 varchar(50) null,
	note5 varchar(50) null,
)


--预算明细表 bill_ysmxb
CREATE TABLE [dbo].[bill_ysmxb](
	[gcbh] [varchar](50) NULL,--预算过程编号
	[billCode] [varchar](50) NULL,--单据编号
	[yskm] [varchar](50) NULL,--预算科目
	[ysje] [float] NULL,--预算金额
	[ysDept] [varchar](50) NULL,--预算部门
	[ysType] [varchar](1) NULL,--预算类型（1，一般预算，2，追加，3预算调整，4科目之间调整 5预算内追加（从科目未分配金额中分配给部门） 6收入 7预算内追加（从部门预留金额中追加） 8 新开分校填报）
    [list_id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	 ---新加字段
	[fujian] [varchar](50) NULL,
	 sm nvarchar(50) null--说明
	
	) 

--预算明细说明附件 
CREATE TABLE [dbo].[bill_ysmxb_smfj](
	[billCode] [varchar](50) NULL,--编号
	[yskm] [varchar](20) NULL,--预算科目
	[sm] [text] NULL,--说明
	[fj] [varchar](2000) NULL--附件
) 

--预算过程 bill_ysgc
CREATE TABLE [dbo].[bill_ysgc](
	[gcbh] [varchar](50) NULL,--编号
	[xmmc] [varchar](50) NULL,--名称
	[kssj] [datetime] NULL,--开始时间
	[jzsj] [datetime] NULL,--截止时间
	[status] [varchar](1) NULL,--状态（0，未开始，1，已开始，2，已结束）
	[fqr] [varchar](50) NULL,--发起人
	[fqsj] [varchar](50) NULL,--发起时间
	[nian] [varchar](4) NULL,--年度
	[yue] [varchar](2) NULL,--月
	[ysType] [varchar](6) NULL--预算类型（0，年度预算，1，季度预算，2，月度预算）
) 
--预算比例 bill_ysbl 
CREATE TABLE [dbo].[bill_ysbl](
	[yf] [varchar](50) NULL,--月份
	[type1] [varchar](50) NULL,
	[type2] [varchar](50) NULL,
	[listid] [decimal](18, 0) IDENTITY(1,1) NOT NULL,--编号 自增列
	[type3] [varchar](50) NULL,
	[bl] [decimal](18, 6) NULL,--比例
 CONSTRAINT [PK_bill_ysbl] PRIMARY KEY CLUSTERED 
(
	[listid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


--项目分解利润表
CREATE TABLE [dbo].[bill_ys_xmfjlrb](
	[procode] [varchar](20)  NOT NULL,   --项目编号
	[kmcode] [varchar](20)  NULL,   --科目编号
	[budgetmoney] [money] NULL,  --金额
	[annual] [varchar](4)  NOT NULL,  --年度
	[by1] [varchar](30)  NULL,  --备用1
	[by2] [varchar](30)  NULL,   --备用2
	[by3] [varchar](30)  NULL --备用3
)
--部门科目分解
CREATE TABLE [dbo].[bill_ys_xmfjbm](
	[procode] [varchar](30) NULL,    --项目编号（由于后来变动  现在表示年度）
	[deptcode] [varchar](30)  NOT NULL,  --（部门编号）
	[kmcode] [varchar](30)  NULL,  --科目编号
	[je] [money] NULL,  --金额
	[by1] [money] NULL,  --（回执意见金额， 表示部门对分配金额不满所回执的金额）
	[by2] [varchar](30)  NULL,--（回执意见，表示部门对分配金额不满所回执的意见）
	[by3] [varchar](30) NULL) --（确认标记状态 1预算确认 2 部门确认 3 部门异议


--效益项目对应表
create table bill_ys_benefits_yskm(
procode varchar(20) not null,--效益项目编号
yskmCode varchar(20) not null,--科目编号
deptcode varchar(20) null, --部门编号
--新加字段
yslb varchar(50) null --预算类别
)

--年度效益预算表
create table bill_ys_benefits_budget(
procode varchar(20) not null,--效益项目编号
annual varchar(4) not null,--年度
kmcode varchar(20) null,--科目编号
budgetmoney money null,--预算金额
adduser varchar(10) null,--预算人
adddate datetime null,--预算时间
modifyuser varchar(10) null,--修改人
modifydate datetime null--修改时间
)

--效益项目档案表
create table bill_ys_benefitpro(
annual varchar(4) not null,--年度
procode varchar(20) not null,--效益项目编号
proname varchar(60) not null,--效益项目名称
calculatype varchar(20) null,--计算方式（加、减、不计算）
fillintype varchar(20) null,--填写方式（直接录入、明细汇总）
sortcode varchar(4) null,--排序号
status varchar(2) null,--是否禁用
adduser varchar(10) null,--新增人
adddate datetime null,--新增时间
modifyuser varchar(10) null,--修改人
modifydate datetime null--修改时间
--新加字段
yslb varchar(50) null --预算类别

)


--预算科目 bill_yskm
CREATE TABLE [dbo].[bill_yskm](
	[yskmCode] [varchar](20) NULL,--科目代码
	[yskmBm] [varchar](50) NULL,--科目编号
	[yskmMc] [varchar](50) NULL,--科目名称
	[gjfs] [varchar](1) NULL,--
	[tbsm] [text] NULL,--填报说明
	[tblx] [varchar](20) NULL,--填报类型（01，单位填报，02，财务填报，03销售费用）
	[kmStatus] [varchar](1) NULL,--科目状态（0，禁用，1，启用）
	[kmlx] varchar(2) null,--科目类型
	gkfy varchar(1) null,--归口费用
	xmhs varchar(1) null,--项目核算
	bmhs varchar(1) null,--部门核算
	ryhs varchar(1),--人员核算
    kmzg nvarchar(100) null, --科目主管
	zjhs varchar(50) null,--加项还是减项 1加项 0减项
	dydj varchar(50) null,--对应单据  对应数据字典里的dictype为18的diccode
) 

--预算调整表
CREATE TABLE [dbo].[bill_ystz](
	[billCode] [varchar](50) NULL,
	[billDept] [varchar](50) NULL,
	[sCode] [varchar](20) NULL,
	[tCode] [varchar](20) NULL
) 

--预算调整前（废弃）
CREATE TABLE [dbo].[bill_ystz_after](
	[billCode] [varchar](50) NULL,
	[billDept] [varchar](50) NULL,
	[ysgc] [varchar](20) NULL,
	[km] [varchar](20) NULL,
	[sJe] [float] NULL,
	[tJe] [float] NULL
) 


--预算调整后（废弃）
CREATE TABLE [dbo].[bill_ystz_before](
	[billCode] [varchar](50) NULL,
	[billDept] [varchar](50) NULL,
	[ysgc] [varchar](20) NULL,
	[km] [varchar](20) NULL,
	[sJe] [float] NULL,
	[tJe] [float] NULL
) 

--预算附件
CREATE TABLE [dbo].[bill_ysfj](
	[deptcode] [varchar](50) NULL,
	[nd] [varchar](50) NULL,
	[fujian] [varchar](500) NULL,
	[filename] [varchar](500) NULL,
	[xmbh] [varchar](500) NULL
)
--******************************预算模块end************************************


--******************************财务管理beg************************************
--财务科目 bill_cwkm
CREATE TABLE [dbo].[bill_cwkm](
	[cwkmCode] [varchar](20) NULL,--科目编号
	[cwkmBm] [varchar](50) NULL,--名称
	[cwkmMc] [varchar](50) NULL,--显示名称
	[hsxm1] [varchar](200) NULL,--核算项目1
	[hsxm2] [varchar](200) NULL,
	[hsxm3] [varchar](200) NULL,
	[hsxm4] [varchar](200) NULL,
	[hsxm5] [varchar](200) NULL,
	XianShiMc varchar(200) null,--显示名称(字段重复 废弃)
	[Type]  varchar(50) null,--类型
	Fangxiang varchar(50) null,--方向
	JiCi varchar(50) null,--级次
	FuZhuHeSuan varchar(200) null,--辅助核算
	ShiFouFengCun varchar(200) null,--是否封存
	Note1 nvarchar(100) null,
	Note2 nvarchar(100) null,
	Note3 nvarchar(100) null,
	Note4 nvarchar(100) null,
	Note5 nvarchar(100) null,
	Note6 nvarchar(100) null,
	Note7 nvarchar(100) null,
	Note8 nvarchar(100) null,
	Note9 nvarchar(100) null,
	Note10 nvarchar(100) null,
	Note11 nvarchar(100) null,
	Note12 nvarchar(100) null,
	Note13 nvarchar(100) null,
	Note14 nvarchar(100) null,
	Note15 nvarchar(100) null
) ON [PRIMARY]

--NC部门和本系统部门对应表bill_pingzheng_bumenduiying
CREATE TABLE [dbo].[bill_pingzheng_bumenduiying](
	[OSDeptCode] [varchar](50) NOT NULL,--本系统部门编号
	[OSDeptName] [nvarchar](100) NULL,--本系统部门名称
	[ParentCode] [varchar](50) NULL,
	[ParentName] [nvarchar](100) NULL,--对应NC系统部门名
	[Note1] [nchar](10) NULL,--本系统部门名
	[Note2] [nchar](10) NULL,
	[Note3] [nchar](10) NULL,
	[Note4] [nchar](10) NULL,
	[Note5] [nchar](10) NULL,
	[Note6] [nchar](10) NULL
) ON [PRIMARY]

--凭证项目bill_pingzheng_xm
CREATE TABLE [dbo].[bill_pingzheng_xm](
	[list_id] [int] IDENTITY(1,1) NOT NULL,--记录id自增列
	[xmcode] [nvarchar](50) NULL,--类型编码
	[xmname] [nvarchar](50) NULL,--类型名称
	[parentcode] [nvarchar](50) NULL,--上级编号
	[parentname] [nvarchar](50) NULL,--上级名称
	[isdefault] [nvarchar](50) NULL,--是否默认
	[status] [nvarchar](50) NULL,--状态
	[note1] [nvarchar](50) NULL,
	[note2] [nvarchar](50) NULL,
	[note3] [nvarchar](50) NULL,
	[note4] [nvarchar](50) NULL,
	[note5] [nvarchar](50) NULL,
	[note6] [nvarchar](50) NULL,
	[note7] [nvarchar](50) NULL,
	[note8] [nvarchar](50) NULL,
	[note9] [nvarchar](50) NULL,
 CONSTRAINT [PK_bill_pingzheng_xm] PRIMARY KEY CLUSTERED 
(
	[list_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
--******************************财务管理end************************************


--******************************报销模块beg************************************
-- 一般报销 费用申请 bill_ybbx_fysq
CREATE TABLE [dbo].[bill_ybbx_fysq](
	[billCode] [varchar](50) NULL,--对应单据编号
	[sqCode] [varchar](50) NULL,--申请编号
	[status] [varchar](1) NULL--状态
) ON [PRIMARY]


--一般报销明细表
CREATE TABLE [dbo].[bill_ybbxmxb](
	[billCode] [varchar](50) NOT NULL,--单据编号
	[bxr] [varchar](50) NULL,--经办人
	[bxzy] [varchar](2000) NULL,--报销摘要
	[bxsm] [varchar](2000) NULL,--报销说明
	[sfdk] [varchar](1) NULL,--是否抵扣
	[ytje] [float] NULL,--应退金额
	[ybje] [float] NULL,---应补金额
	[sfgf] [varchar](1) NULL,--是否给付
	[bxmxlx] [varchar](50) NULL,--报销明细类型
	[gfr] [varchar](50) NULL,--给付人
	[gfsj] [datetime] NULL,--给付时间
	[cxsj] [datetime] NULL,--撤销时间
	[cxr] [varchar](50) NULL,--撤销人
	[cxyy] [varchar](4000) NULL,--撤销原因
	[se] decimal(18,4) null,--税额
	bxdjs int null,--报销单据数
	pzcode varchar(50) null,--凭证编号
	pzdate datetime null,--凭证日期
	pzbldate datetime null,--凭证补录日期
	guazhang varchar(50) null,--是否挂账
	zhangtao varchar(50) null,--帐套
	bxr2 varchar(200) null,--归口报销单的时候  用来存报销人/报销单位
	bxrzh varchar(200) null,--报销人账号 格式为 开户行|&|卡号
	bxrphone varchar(200) null--报销人电话
	pzbldate datetime null--凭证补录日期
	fujian varchar(2000) null,--附件  附件名|附件地址
	ykfs varchar(80) null---用款方式
) ON [PRIMARY]
--一般报销明细表 报销单据 bill_ybbxmxb_bxdj 
CREATE TABLE [dbo].[bill_ybbxmxb_bxdj](
	[billCode] [varchar](50) NULL,--对应单据编号
	[djUrl] [varchar](200) NULL,--单据url
	[djName] [varchar](50) NULL,--单据名称
	[djGuid] [varchar](50) NULL,--单据编号
	[djStatus] [varchar](1) NULL--状态
) ON [PRIMARY]

--一般报销明细表 抵扣 bill_ybbxmxb_fydk 
CREATE TABLE [dbo].[bill_ybbxmxb_fydk](
	[billCode] [varchar](50) NULL,--对应单据编号
	[jkmxCode] [varchar](50) NULL,--借款明细编号
	[status] [varchar](2) NULL,--状态
) ON [PRIMARY]

--一般报明细表销科目 bill_ybbxmxb_fykm
CREATE TABLE [dbo].[bill_ybbxmxb_fykm](
	[billCode] [varchar](50) NULL,--单据编号
	[fykm] [varchar](50) NULL,--费用科目
	[je] [float] NULL,--金额
	[mxGuid] [varchar](50) NULL,
	[status] [varchar](2) NULL,--状态 0 默认 1未知  2已经冲减过借款单的记录
	[ms] [varchar](4000) NULL
) ON [PRIMARY]


-- 一般报销明细表 部门 bill_ybbxmxb_fykm_dept qq
CREATE TABLE [dbo].[bill_ybbxmxb_fykm_dept](
	[kmmxGuid] [varchar](50) NULL,--一般报销科目明细编号
	[mxGuid] [varchar](50) NULL,--一般报销明细部门编号
	[deptCode] [varchar](50) NULL,--部门编号
	[je] [float] NULL,--金额
	[status] [varchar](1) NULL--状态
) ON [PRIMARY]

--一般报销明细表 科目 费用分摊 bill_ybbxmxb_fykm_ft  qq
CREATE TABLE [dbo].[bill_ybbxmxb_fykm_ft](
	[billCode] [varchar](50) NULL,--对应单据编号
	[kmmxGuid] [varchar](50) NULL,--一般报销科目明细编号
	[cbzx] [varchar](50) NULL,--对应bill_cbzx的[zxCode]
	[je] [float] NULL,--金额
	[ftmxGuid] [varchar](50) NULL,--分摊明细编号
	[status] [varchar](2) NULL--状态
) ON [PRIMARY]


--一般报销明细表 核算项目 bill_ybbxmxb_hsxm qq
CREATE TABLE [dbo].[bill_ybbxmxb_hsxm](
	[kmmxGuid] [varchar](50) NULL,--一般报销科目明细编号
	[mxGuid] [varchar](50) NULL,--核算项目编号
	[xmCode] [varchar](50) NULL,--项目编号
	[je] [float] NULL--金额
) ON [PRIMARY]


--出差报告单
CREATE TABLE [dbo].[bill_travelReport](
	[MainCode] [varchar](50) NOT NULL,--对应单据编号
	[TravelProcess] [text] NULL,--出差说明
	[WorkProcess] [text] NULL,--工作过程
	[Result] [text] NULL,--结果
	[Note1] [nchar](10) NULL,
	[Note2] [nchar](10) NULL,
	[Note3] [nchar](10) NULL,
	[Note4] [nchar](10) NULL,
	[Note5] [nchar](10) NULL,
	[Attachment] [nvarchar](100) NULL,
	[AttachmentName] [nvarchar](100) NULL,
 CONSTRAINT [PK_bill_travelReport] PRIMARY KEY CLUSTERED 
(
	[MainCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
--出差管理单
CREATE TABLE [dbo].[bill_travelApplication](
	[maincode] [varchar](50) NOT NULL,--对应单据编号
	[typecode] [varchar](50) NULL,
	[travelPersionCode] [varchar](50) NULL,--制单人
	[arrdess] [varchar](500) NULL,--出差地点
	[travelDate] [varchar](50) NULL,--预计出差时间
	[reasion] [varchar](max) NULL,--出差事由
	[travelplan] [varchar](max) NULL,--出差日程及工作安排
	[needAmount] [int] NULL,--花费总金额
	[Transport] [varchar](50) NULL,--申请交通工具
	[MoreThanStandard] [int] NULL,--是否超过规定标准
	[ReportCode] [varchar](50) NULL,--编号
	[jiaotongfei] [int] NULL,--交通费
	[zhusufei] [int] NULL,--住宿费
	[yewuzhaodaifei] [int] NULL,--业务招待费
	[huiyifei] [int] NULL,--会议费
	[yinshuafei] [int] NULL,--印刷费
	[qitafei] [int] NULL,--其他费用
	[sendDept] [varchar](50) NULL--部门
) ON [PRIMARY]



--费用申请附件表 bill_fysq_fjb
CREATE TABLE [dbo].[bill_fysq_fjb](
	[billCode] [varchar](50) NULL,--对应单据编号
	[fjGuid] [varchar](50) NULL,--附件唯一编号
	[fjUrl] [varchar](200) NULL,--附件地址
	[fjName] [varchar](50) NULL,--附件名称
	[djStatus] [varchar](1) NULL--状态
) ON [PRIMARY]
 
--费用申请明细表 bill_fysq_mxb 
CREATE TABLE [dbo].[bill_fysq_mxb](
	[billCode] [varchar](50) NULL,--对应单据编号
	[mxname] [varchar](50) NULL,--名称
	[je] [float] NULL,--金额
	[mxGuid] [varchar](50) NULL
) ON [PRIMARY]


--单据主表
CREATE TABLE [dbo].[bill_main](
	[billCode] [varchar](50) NULL,--唯一号编号
	[billName] [varchar](50) NULL,--单据编号
	[flowID] [varchar](50) NULL,--单据类型(ys,预算，yszj，预算追加，xmys，项目预算。。。)
	[stepID] [varchar](50) NULL,--审核步骤
	[billUser] [varchar](50) NULL,--人员
	[billDate] [datetime] NULL,--用款日期
	[billDept] [varchar](50) NULL,--部门
	[billJe] [float] NULL,--金额  
	[loopTimes] [int] NULL,--循环次数
	[billType] [varchar](1) NULL,--单据类型 一般来讲 预算、预算调整、预算追加的单子都与bill_ybbxmxb的billtype相对应  为了将手工做的预算追加，做预算时 记录单据类型 
	[billName2] [varchar](50) NULL,---追加说明    如果是年初填报预算 用来存储附件
	[isgk] [varchar](1) NULL,--是否归口
	[gkdept] [varchar](50) NULL,--归口部门
	
	--新加字段
	[dydj] [varchar](50) NULL,--对应单据 预算类型  （对应数据字典中dictype=18 中的值）
	[note1] text NULL,--单据报销日期
	[note2] text NULL,--财年年月
	[note3] [varchar](50) NULL,--如果是flowid='xmys’ 项目预算填报 存项目编号  如果是项目预算汇总 也是存项目编号
	[note4] [varchar](50) NULL,---报销单是 是否是新财年
	[note5] [varchar](50) NULL，--
) 
--预支单/借款单表  yzsq/jksq
CREATE TABLE [dbo].[T_LoanList](
	[Listid] [nvarchar](50) NOT NULL,--主表编号
	[LoanCode] [nvarchar](50) NULL,--经办人编号
	[LoanDeptCode] [nvarchar](50) NULL,--经办部门
	[LoanDate] [nvarchar](50) NULL,--借款日期
	[LoanSystime] [nvarchar](50) NULL,--系统时间
	[LoanMoney] [decimal](18, 3) NULL,--金额
	[LoanExplain] [nvarchar](500) NULL,--说明
	[Status] [char](1) NULL,--冲减状态 1冲减中   2冲减完毕
	[SettleType] [char](1) NULL,
	[CJCode] [nvarchar](500) NULL,
	[ResponsibleCode] [nvarchar](50) NULL,--借款人
	[ResponsibleDate] [nvarchar](50) NULL,
	[ResponsibleSysTime] [nvarchar](50) NULL,
	[NOTE1] [nvarchar](50) NULL,
	[NOTE2] [nvarchar](500) NULL,
	[NOTE3] [nvarchar](50) NULL,--已还款金额
	[NOTE4] [nvarchar](50) NULL,
	[NOTE5] [nvarchar](500) NULL,
	[NOTE6] [nvarchar](50) NULL,
	[NOTE7] [nvarchar](50) NULL,--附加单据
	[NOTE8] [nvarchar](50) NULL,
	[NOTE9] [nvarchar](50) NULL,
	[NOTE10] [nvarchar](50) NULL,
	[NOTE11] [nvarchar](50) NULL,
	[NOTE12] [nvarchar](50) NULL,
	[NOTE13] [nvarchar](50) NULL,
	[NOTE14] [nvarchar](50) NULL,
	[NOTE15] [nvarchar](50) NULL,
	[NOTE16] [nvarchar](50) NULL,
	[NOTE17] [nvarchar](50) NULL,
	[NOTE18] [nvarchar](50) NULL,
	[NOTE19] [nvarchar](50) NULL,
	[NOTE20] [nvarchar](50) NULL,
 CONSTRAINT [PK_T_LoanList] PRIMARY KEY CLUSTERED 
(
	[Listid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

--还款记录表
create table T_ReturnNote(
	listid int identity(1,1) not null,
	ltype int not null,--1  报销单冲减   2还款
	loancode varchar(50) not null,--借款单号或预支单号
	billcode varchar(50) null,--报销单号/或还款单号
	ldate varchar(50) not null,
	je decimal(18,3) not null,
	usercode varchar(50) not null,--操作人
	
	note1 varchar(50) null,--是否审批通过 0 未审核 1 已审核
	note2 varchar(50) null,--备注
	note3 varchar(50) null,--凭证号
	note4 varchar(50) null,--billcode 对应的冲减的报销单的编号  如果是从凭证冲借款过来的 则记录对应的报销单的主键
	note5 varchar(50) null,
)



--******************************报销模块end************************************

--******************************采购模块beg************************************

--采购审批单主表
CREATE TABLE [dbo].[bill_cgsp](
	[cgbh] [varchar](50) NULL,--编号
	[sj] [datetime] NULL,--时间
	[cgdept] [varchar](50) NULL,--部门
	[cglb] [varchar](50) NULL,--类别（01，一般，02，紧急）
	[cbr] [varchar](50) NULL,--承办人
	[cgze] [float] NULL,--采购总额
	[sm] [text] NULL,--说明
	[ms] [text] NULL,
	[spyj01] [text] NULL,
	[spyj02] [text] NULL,
	[spyj03] [text] NULL,
	[spyj04] [text] NULL,
	[spyj05] [text] NULL,
	[spyj06] [text] NULL,
	[spyj07] [text] NULL,
	[spyj08] [text] NULL
) 


--采购审批明细表
CREATE TABLE [dbo].[bill_cgsp_mxb](
	[cgbh] [varchar](50) NULL,--采购编号
	[mc] [text] NULL,--名称
	[gg] [text] NULL,--规格
	[sl] [float] NULL,--数量
	[dj] [float] NULL,--单价
	[zj] [float] NULL,--总价
	[bz] [text] NULL,--备注
	[cgbhGuid] [varchar](50) NULL,----采购编号
	[cgIndex] [int] NOT NULL--标识编号
) 




--采购审批单--附件表
CREATE TABLE [dbo].[bill_cgsp_fjb](
	[billCode] [varchar](50) NULL,--对应单据编号
	[djUrl] [varchar](200) NULL,--路径
	[djName] [varchar](50) NULL,--名称
	[djGuid] [varchar](50) NULL,
	[djStatus] [varchar](1) NULL--状态
)

--采购审批单--询价表
CREATE TABLE [dbo].[bill_cgsp_xjb](
	[cgbh] [varchar](50) NULL,--对应单据号
	[xjdw] [text] NULL,--单位
	[xjqk] [text] NULL--情况
) 

--附单表 采购审批 对应报告申请表
CREATE TABLE [dbo].[bill_cgsp_lscg](
	[billCode] [varchar](50) NULL,--主单号
	[lscgCode] [varchar](50) NULL,--附单号
	[status] [varchar](1) NULL--状态
) ON [PRIMARY]




--临时采购申请单
CREATE TABLE [dbo].[bill_lscg](
	[cgbh] [varchar](20) NULL,--编号
	[sj] [datetime] NULL,--时间
	[cgDept] [varchar](500) NULL,--部门
	[cglb] [varchar](50) NULL,--类别（01，一般，02，紧急）
	[zynr] [text] NULL,--内容
	[sm] [text] NULL,--说明
	[cbr] [varchar](50) NULL,--承办人
	[ms] [text] NULL,--
	[spyj01] [text] NULL,
	[spyj02] [text] NULL,
	[spyj03] [text] NULL,
	[spyj04] [text] NULL,
	[spyj05] [text] NULL,
	[spyj06] [text] NULL,
	[spyj07] [text] NULL,
	[spyj08] [text] NULL,
	[yjfy] [float] NULL,--金额
	fjName varchar(200) null,
	fjUrl varchar(300) null
) 

-- 采购资金付款
CREATE TABLE [dbo].[bill_cgzjfk](
	[billcode] [varchar](50)  NULL, --对应主表bill_main的code
	[gysbh] [varchar](50)  NULL,--供应商编号
	[gysmc] [varchar](100)  NULL,--供应商名称
	[jhje] [float] NULL,--计划金额
	[fkje] [float] NULL,--付款金额
	[bz] [varchar](100)  NULL,--备注
	[jhindex] [decimal](18, 0) IDENTITY(1,1) NOT NULL,--序号
	[cdefine1] [varchar](20)  NULL,
	[cdefine2] [varchar](50)  NULL,
	[idefine3] [float] NULL,
	[idefine4] [float] NULL,
	[ddefine5] [datetime] NULL
) ON [PRIMARY]

-- 采购资金计划
CREATE TABLE [dbo].[bill_cgzjjh](
	[cgbh] [varchar](50) NULL,--采购编号
	[gysbh] [varchar](50) NULL,--供应商编号
	[gysmc] [varchar](100) NULL,--供应商名称
	[syrkje] [float] NULL,
	[byjhje] [float] NULL,
	[byfkje] [float] NULL,
	[fkje] [float] NULL,--付款金额
	[bz] [varchar](100) NULL,--备注
	[jhindex] [decimal](18, 0) IDENTITY(1,1) NOT NULL,--需要
	[cdefine1] [varchar](20) NULL,
	[cdefine2] [varchar](50) NULL,
	[idefine3] [float] NULL,
	[idefine4] [float] NULL,
	[ddefine5] [datetime] NULL
) ON [PRIMARY]

--归口分解比例
create table bill_gkfjbili(
	nian nvarchar(4) not null,--年度
	gkdeptcode nvarchar(50) not null,--归口部门编号
	yskmcode nvarchar(50) not null,--预算科目编号
	fjdeptcode nvarchar(50) not null,--分解部门编号
	fjbl decimal(18,6) not null,
	note1 nvarchar(50) null,
	note2 nvarchar(50) null,
	note3 nvarchar(50) null,
	note4 nvarchar(50) null,
	note5 nvarchar(50) null
)

--临时采购申请单
CREATE TABLE [dbo].[bill_lscg](
	[cgbh] [varchar](20) NULL,--编号
	[sj] [datetime] NULL,--时间
	[cgDept] [varchar](500) NULL,--部门
	[cglb] [varchar](50) NULL,--类别（01，一般，02，紧急）
	[zynr] [text] NULL,--内容
	[sm] [text] NULL,--说明
	[cbr] [varchar](50) NULL,--承办人
	[ms] [text] NULL,--
	[spyj01] [text] NULL,
	[spyj02] [text] NULL,
	[spyj03] [text] NULL,
	[spyj04] [text] NULL,
	[spyj05] [text] NULL,
	[spyj06] [text] NULL,
	[spyj07] [text] NULL,
	[spyj08] [text] NULL,
	[yjfy] [float] NULL--金额
) 
select 1467-575

--用款申请单和报销单对照表
create table dz_yksq_bxd 
(
	yksq_code nvarchar(50) not null,
	bxd_code nvarchar(50) not null,
	note1 nvarchar(50) null,--单据类型 yszj，预算追加，xmys，项目预算。。。
	note2 nvarchar(50) null,--回冲预算信息
	note3 nvarchar(50) null,
	note4 nvarchar(50) null,
	note5 nvarchar(50) null
)

--其它报销 费用申请 bill_qtbx_fysq 
CREATE TABLE [dbo].[bill_qtbx_fysq](
	[billCode] [varchar](50) NULL,--对应单据编号
	[sqCode] [varchar](50) NULL,--申请编号
	[status] [varchar](1) NULL--状态
) ON [PRIMARY]

--其它报销明细表 bill_qtbxmxb 
CREATE TABLE [dbo].[bill_qtbxmxb](
	[billCode] [varchar](50) NOT NULL,--对应单据编号
	[bxr] [varchar](50) NULL,--报销人
	[bxzy] [varchar](2000) NULL,--报销摘要
	[bxsm] [varchar](2000) NULL,--报销说明
	[sfdk] [varchar](1) NULL,--是否使用借款抵付
	[ytje] [float] NULL,--应退金额
	[ybje] [float] NULL,---应补金额
	[sfgf] [varchar](1) NULL,--是否给付
	[bxmxlx] [varchar](50) NULL,--报销明细类型
	[gfr] [varchar](50) NULL,--给付人
	[gfsj] [datetime] NULL,--给付时间
	[cxsj] [datetime] NULL,--撤销时间
	[cxr] [varchar](50) NULL,--撤销人
	[cxyy] [text] NULL--撤销原因
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

--其它报销明细表 报销单价 bill_qtbxmxb_bxdj 
CREATE TABLE [dbo].[bill_qtbxmxb_bxdj](
	[billCode] [varchar](50) NULL,--对应单据编号
	[djUrl] [varchar](200) NULL,--单据地址
	[djName] [varchar](50) NULL,--单据名
	[djGuid] [varchar](50) NULL,--报价单编号
	[djStatus] [varchar](1) NULL--状态
) ON [PRIMARY]

--其它报销明细表 费用单据 bill_qtbxmxb_fydk 
CREATE TABLE [dbo].[bill_qtbxmxb_fydk](
	[billCode] [varchar](50) NULL,--对应单据编号
	[jkmxCode] [varchar](50) NULL,--借款明细编号
	[status] [varchar](2) NULL,--状态
	[dkGuid] [varchar](50) NULL --贷款编号
) ON [PRIMARY]

--其它报销明细表 费用科目 bill_qtbxmxb_fykm 
CREATE TABLE [dbo].[bill_qtbxmxb_fykm](
	[billCode] [varchar](50) NULL,--对应单据编号
	[fykm] [varchar](50) NULL,--费用科目
	[je] [float] NULL,--金额
	[mxGuid] [varchar](50) NULL,--单据编号
	[status] [varchar](2) NULL,--状态
	[ms] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

--其它报销明细表 费用科目 单位bill_qtbxmxb_fykm_dept 
CREATE TABLE [dbo].[bill_qtbxmxb_fykm_dept](
	[kmmxGuid] [varchar](50) NULL,--科目明细编号
	[mxGuid] [varchar](50) NULL,--明细编号
	[deptCode] [varchar](50) NULL,--部门编号
	[je] [float] NULL,--金额
	[status] [varchar](1) NULL--状态
) ON [PRIMARY]

--其它报销明细表 费用科目 分摊 bill_qtbxmxb_fykm_ft
CREATE TABLE [dbo].[bill_ybbxmxb_fykm_ft](
	[billCode] [varchar](50) NULL,--对应单据编号
	[kmmxGuid] [varchar](50) NULL,--报销明细费用科目明细编号
	[cbzx] [varchar](50) NULL,--对应bill_cbzx的[zxCode]
	[je] [float] NULL,--金额
	[ftmxGuid] [varchar](50) NULL,--分摊明细编号
	[status] [varchar](2) NULL--状态
) ON [PRIMARY]

--其它报销明细表 核算项目bill_qtbxmxb_hsxm 
CREATE TABLE [dbo].[bill_qtbxmxb_hsxm](
	[kmmxGuid] [varchar](50) NULL,--报销明细费用科目明细编号
	[mxGuid] [varchar](50) NULL,--核算编号
	[xmCode] [varchar](50) NULL,--核算项目编号
	[je] [float] NULL--金额
) ON [PRIMARY]

--******************************采购模块end************************************



--******************************审批模块beg************************************
--审批记录主表
create table workflowrecord(
	recordid decimal(18,0) IDENTITY(1,1),--记录编号(被审批的单据持有此号)
	billCode varchar(50) not null,--单据编号
	billType varchar(30) not null,--单据类型(是什么单子,类名)
	flowId varchar(30) not null,--工作流编号
	isEdit int not null,--是否经过修改,0没有,1有
	rdState int not null--状态(0,等待;1,正在执行;2,通过;3,废弃未通过)
)

--审批记录子表
create table workflowrecords(
	recordid decimal(18,0) not null,--主表对应编号
	flowid varchar(30) null,--流程编号
	stepid varchar(30) null,--步骤编号
	steptext varchar(30) null,--步骤名称
	recordtype varchar(30) null,--类型(一个部门的人都能审，还是只能是个人))
	checkuser varchar(30) null,--审批人(应当审批的人,或部门)
	finaluser varchar(30) null,--单据审批人(真正审批的人)
	rdstate int null,--状态(0,等待;1,正在执行;2,通过;3,废弃)
	mind varchar(30) null,--审批意见
	checkdate datetime null--审批时间
	checktype varchar(30) null --审批类型 1=单签 2=会签
)
--审批步骤表workflowstep
CREATE TABLE [dbo].[workflowstep](
	[stepid] [int] NOT NULL,--编号
	[flowid] [varchar](30) NOT NULL,--流程类型
	[steptype] [varchar](30) NULL,
	[steptext] [varchar](50) NULL,
	[checkcode] [varchar](100) NULL,
	[minmoney] [decimal](18, 4) NULL,
	[maxmoney] [decimal](18, 4) NULL,
	[mindate] [datetime] NULL,
	[maxdate] [datetime] NULL,
	[memo] [varchar](50) NULL,--部门编号
	[checktype] [varchar](30) NULL,--审批类型 1=单签 2=会签
	[filterkemuManager] [int] NULL,--是否是科目主管
	kmType varchar(50) null,--科目
 CONSTRAINT [PK_workflowstep] PRIMARY KEY CLUSTERED 
(
	[stepid] ASC,
	[flowid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
--审批类型表
create table mainworkflow(
	flowid varchar(30)  primary key,--编号（即billmain里的单据类型字段）
	flowName varchar(50) null--名称
)

--审批流对应表（目前是一对一）
create table billtoworkflow(
	billtype varchar(30) primary key,	--编号
	billname varchar(30) null,--名称
	flowid varchar(30) null--flowid（即billmain里的单据类型字段）
)

--流程表 bill_workFlow 
CREATE TABLE [dbo].[bill_workFlow](
	[flowId] [varchar](50) NULL,编号
	[flowText] [varchar](50) NULL,--审批类型名称
	[stepTextColor] [varchar](50) NULL,--文字颜色
	[stepStrokeColor] [varchar](50) NULL,
	[stepShadowColor] [varchar](50) NULL,
	[stepFocusedStrokeColor] [varchar](50) NULL,
	[isStepShadow] [varchar](50) NULL,
	[actionStrokeColor] [varchar](50) NULL,
	[actionTextColor] [varchar](50) NULL,
	[actionFocusedStrokeColor] [varchar](50) NULL,
	[sStepTextColor] [varchar](50) NULL,
	[sStepStrokeColor] [varchar](50) NULL,
	[stepColor1] [varchar](50) NULL,
	[stepColor2] [varchar](50) NULL,
	[isStep3D] [varchar](50) NULL,
	[step3DDepth] [varchar](50) NULL,
	[orderBy] [varchar](50) NULL
) ON [PRIMARY]


--流程动作表  bill_workFlowAction qq
CREATE TABLE [dbo].[bill_workFlowAction](
	[flowID] [varchar](50) NULL,--审批类型
	[actionID] [varchar](50) NULL,--审批编号
	[actionText] [varchar](50) NULL,--动作名称
	[actionType] [varchar](50) NULL,--动作类型
	[actionFrom] [varchar](50) NULL,
	[actionTo] [varchar](50) NULL,
	[startArrow] [varchar](50) NULL,
	[endArrow] [varchar](50) NULL,
	[strokeWeight] [varchar](50) NULL,
	[isFocused] [varchar](50) NULL,
	[zIndex] [varchar](50) NULL
) ON [PRIMARY]

--bill_workFlowGroup 
CREATE TABLE [dbo].[bill_workFlowGroup](
	[flowID] [varchar](50) NULL,--审批类型
	[stepID] [varchar](50) NULL,--步骤id
	[wkGroup] [varchar](50) NULL,--审批组
	[flowMode] [varchar](50) NULL,--审批模式
	[wkUser] [varchar](50) NULL,--审批人
	[wkModel] [varchar](50) NULL--审批流步骤类型
) ON [PRIMARY]

--审核记录表  bill_workFlowRecord  qq
CREATE TABLE [dbo].[bill_workFlowRecord](
	[billCode] [varchar](50) NULL,--对应bill_main单据编号
	[flowID] [varchar](50) NULL,--审批类型
	[beginStep] [varchar](50) NULL,
	[endStep] [varchar](50) NULL,
	[checkUser] [varchar](50) NULL,--审核用户
	[checkDate] [datetime] NULL,--审核时间
	[checkBz] [text] NULL,--审核备注
	[loopTimes] [int] NULL,--循环次数
	[checkGroup] [varchar](50) NULL,
	[result] [varchar](50) NULL,--当前步骤
	[stepUser] [varchar](50) NULL,
	[wkModel] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]



--流程步骤  bill_workFlowStep  qq
CREATE TABLE [dbo].[bill_workFlowStep](
	[flowID] [varchar](50) NOT NULL,
	[stepID] [varchar](50) NOT NULL,
	[stepText] [varchar](50) NULL,
	[stepType] [varchar](50) NULL,
	[v_width] [varchar](50) NULL,
	[v_height] [varchar](50) NULL,
	[v_x] [varchar](50) NULL,
	[v_y] [varchar](50) NULL,
	[textWeight] [varchar](50) NULL,
	[strokeWeight] [varchar](50) NULL,
	[isFocused] [varchar](50) NULL,
	[zIndex] [varchar](50) NULL
) ON [PRIMARY]

--单据类型表，如一般报销单、预算单、审批报告单、……
create table bill_djlx(
djbh varchar(20) null,--单据类型编号
djmc varchar(50) null,--单据名称
djbm varchar(50) null,--单据别名
splx varchar(20) null,--审批类型
bhbj varchar(20) null,--单据编号表头标记
bhlslx varchar(2) null,--单据编号的类型（n为按年流水、m为按月流水、d为按日流水）
bhlscd int null,--编号流水长度
djtype varchar(2) null --是否启用（0 启用，1 禁用）
)


--******************************审批模块end************************************

--******************************统计查询beg************************************
--月结bill_yj
CREATE TABLE [dbo].[bill_yj](
	[yf] [varchar](10) NULL,
	[userCode] [varchar](30) NULL,
	[cguid] [varchar](50) NULL,
	[yjsj] [datetime] NULL,
	[yjbj] [varchar](1) NULL
) ON [PRIMARY]
--收入明细表
CREATE TABLE [dbo].[bill_srmxb](
	[gcbh] [varchar](50) NOT NULL,--归口编号
	[billCode] [varchar](50) NOT NULL,--对应单据编号
	[yskm] [varchar](50)  NOT NULL,--预算科目
	[ysje] [float] NULL,--金额
	[ysDept] [varchar](50)  NOT NULL,--部门
	[ysType] [varchar](1)  NULL,
	[list_id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,--记录id 自增列
 CONSTRAINT [PK_bill_srmxb_1] PRIMARY KEY CLUSTERED 
(
	[list_id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

--预算导入
create table bill_ys_import(
  id nvarchar(50) null, --编号
  deptcode varchar(50) null,--部门编号
  deptname varchar(50) null,--部门名称
  yskmcode varchar(50) null,--科目编号
  yskmmc varchar(50) null,--科目名称
  yi float null, --1月
  er float null,--2月
  san float null,--3月
  si float null,--4月
  wu float null,--5月
  liu float null,--6月
  qi float null,--7月
  ba float null,--8月
  jiu float null,--9月
  shi float null,--10月
  shiyi float null,--11月
  shier float null,--12月
  nian float null
)
--导入报销单临时表
create table lsbxd_main(
	billcode varchar(50) null,--单据唯一编码
	flowid nvarchar(10) null,--单据类型  ybbx/gkbx/qtbx
	billUser varchar(50) null,--制单人
	billDate varchar(50) null,--单据日期
	billDept varchar(50) null,--制单部门
	je decimal(18,4) null,--科目金额 
	se decimal(18,4) null,--科目税额
	isgk	varchar(1) null,--是否归口
	gkdept varchar(50) null,--归口部门
	bxzy varchar(50) null,--报销摘要
	bxsm varchar(50) null,--报销说明
	fykmcode varchar(50) null,--费用科目编号
	sydept varchar(50) null,--使用部门
	bxlx varchar(50) null,--报销类型
	note1 varchar(50) null,
	note2 varchar(50) null,
	note3 varchar(50) null,
	note4 varchar(50) null,
	note5 varchar(50) null,
	note6 varchar(50) null,
	note7 varchar(50) null,
	note8 varchar(50) null,
	note9 varchar(50) null,
	note0 varchar(50) null,
)
--市立医院 卫材药品出库单导入报销单 对应表
CREATE TABLE [dbo].[gdzc_yskm_yongyou](
	[deptCode] [varchar](50) NOT NULL,
	[deptName] [nvarchar](100) NOT NULL,
	[ckCode] [varchar](50) NOT NULL,
	[ckName] [nvarchar](100) NOT NULL,
	[yskmCode] [varchar](50) NULL,
	[yskmName] [nvarchar](100) NULL,
	[nType] [varchar](10) NULL,
	[note1] [nvarchar](50) NULL,
	[note2] [nvarchar](50) NULL,
	[note3] [nvarchar](50) NULL,
	[note4] [nvarchar](50) NULL,
	[note5] [nvarchar](50) NULL,
	[note6] [nvarchar](50) NULL,
	[note7] [nvarchar](50) NULL,
	[note8] [nvarchar](50) NULL,
	[note9] [nvarchar](50) NULL,
	[note0] [nvarchar](50) NULL
) ON [PRIMARY]
--******************************统计查询end************************************


--******************************销售模块beg************************************

---开票申请单
CREATE TABLE [dbo].[T_BillingApplication](
	[Code] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[BillMainCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[TruckCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SaleDeptCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[AppDate] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[DealersName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SysPersionCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SysDateTime] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Explain] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NULL,
	[InvoiceCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[BillingDate] [char](10) COLLATE Chinese_PRC_CI_AS NULL,
	[BIllingSysTime] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[IsJC] [char](1) COLLATE Chinese_PRC_CI_AS NULL,
	[IsSpApp] [char](1) COLLATE Chinese_PRC_CI_AS NULL,
	[AttachmentUrl] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[Note1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note6] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note7] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note8] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note9] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note10] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL
) ON [PRIMARY]
----配置项

CREATE TABLE [dbo].[T_ControlItem](
	[Code] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[CName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ControlCodeFirst] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ControlNameFirst] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ControlCodeSecond] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Status] [char](1) COLLATE Chinese_PRC_CI_AS NULL,
	[ControlNameSecond] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Months] [varchar](5) COLLATE Chinese_PRC_CI_AS NULL,
	[Remark] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[Note1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_T_ControlItem] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

----标准返利设置
CREATE TABLE [dbo].[T_RebatesStandard](
	[NID] [bigint] IDENTITY(1,1) NOT NULL,
	[EffectiveDateFrm] [char](10) COLLATE Chinese_PRC_CI_AS NULL,
	[EffectiveDateTo] [char](10) COLLATE Chinese_PRC_CI_AS NULL,
	[SaleCountFrm] [int] NULL,
	[SaleCountTo] [int] NULL,
	[TruckTypeCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[DeptCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SaleFeeTypeCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ControlItemCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Fee] [decimal](18, 2) NULL,
	[Status] [char](1) COLLATE Chinese_PRC_CI_AS NULL,
	[Type] [char](1) COLLATE Chinese_PRC_CI_AS NULL,
	[SaleProcessCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Remark] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[AuditUserCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note6] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note7] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note8] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note9] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note10] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_T_RebatesStandard] PRIMARY KEY CLUSTERED 
(
	[NID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
------车款上缴报告
CREATE TABLE [dbo].[T_Remittance](
	[NID] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[PaymentDeptName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[PaymentDeptCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[OrderCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[TruckCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[RemittanceNumber] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[RemittanceDate] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[RemittanceType] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[RemittanceMoney] [decimal](18, 2) NULL,
	[RemittanceUse] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SystemDate] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SystemuserCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[OrderCodeDate] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Accessories] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE6] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE7] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE8] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE9] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE0] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL
) ON [PRIMARY]

-----------报告申请单
CREATE TABLE [dbo].[T_ReportApplication](
	[ReportAppCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ReportName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ReportNameCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ReportDeptCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ReportDeptName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ReportDate] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ReportExplain] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ReportRemark] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE6] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE7] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE8] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE9] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE0] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_ReportApplication] PRIMARY KEY CLUSTERED 
(
	[ReportAppCode] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
-------销售返利明细
CREATE TABLE [dbo].[T_SaleFeeAllocationNote](
	[Nid] [bigint] IDENTITY(1,1) NOT NULL,
	[ActionDate] [char](10) COLLATE Chinese_PRC_CI_AS NULL,
	[ActionTimes] [varchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[BillCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[TruckCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[TruckTypeCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[DeptCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ControlItemCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SaleFeeTypeCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Fee] [decimal](18, 2) NULL,
	[Status] [char](1) COLLATE Chinese_PRC_CI_AS NULL,
	[AuditUserCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ActionNote] [nvarchar](max) COLLATE Chinese_PRC_CI_AS NULL,
	[RebatesType] [char](1) COLLATE Chinese_PRC_CI_AS NULL,
	[Remark] [nvarchar](100) COLLATE Chinese_PRC_CI_AS NULL,
	[Note1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note6] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note7] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note8] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note9] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note10] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_T_SaleFeeAllocationNote] PRIMARY KEY CLUSTERED 
(
	[Nid] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
-------销售返利报销明细


CREATE TABLE [dbo].[T_SaleFeeSpendNote](
	[Listnid] [int] IDENTITY(1,1) NOT NULL,
	[YsgcCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Billcode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Deptcode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Yskmcode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Status] [char](1) COLLATE Chinese_PRC_CI_AS NULL,
	[Fee] [decimal](18, 2) NULL,
	[Sysdatetime] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Sysusercode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Remark] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note6] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note7] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note8] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note9] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note0] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_T_SaleFeesSendNote] PRIMARY KEY CLUSTERED 
(
	[Listnid] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

---销售过程
CREATE TABLE [dbo].[T_SaleProcess](
	[Code] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[PName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Status] [char](1) COLLATE Chinese_PRC_CI_AS NULL,
	[Note1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_T_SaleProcess] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

-----特殊返利申请单

CREATE TABLE [dbo].[T_SpecialRebatesApp](
	[Code] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[BillMainCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[TruckCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[TruckCount] [int] NULL,
	[SaleDeptCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[StandardSaleAmount] [decimal](18, 3) NULL,
	[ExceedStandardPoint] [decimal](18, 2) NULL,
	[AppDate] [char](10) COLLATE Chinese_PRC_CI_AS NULL,
	[SysPersionCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SysDateTime] [varchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[EffectiveDateFrm] [char](10) COLLATE Chinese_PRC_CI_AS NULL,
	[EffectiveDateTo] [char](10) COLLATE Chinese_PRC_CI_AS NULL,
	[Attachment] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[Explain] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NULL,
	[CheckAttachment] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[Note1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note6] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note7] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note8] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note9] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note10] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL
) ON [PRIMARY]
-----特殊返利设置

CREATE TABLE [dbo].[T_SpecialRebatesStandard](
	[NID] [bigint] IDENTITY(1,1) NOT NULL,
	[AppBillCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[TruckCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[TruckTypeCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[DeptCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SaleFeeTypeCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SaleProcessCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ControlItemCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Fee] [decimal](18, 2) NULL,
	[MarkerCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Status] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Type] [char](1) COLLATE Chinese_PRC_CI_AS NULL,
	[SysUserCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[SysTime] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[Note1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note6] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note7] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note8] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note9] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note10] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_T_SpecialRebatesStandard] PRIMARY KEY CLUSTERED 
(
	[NID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
-----车辆类型
CREATE TABLE [dbo].[T_truckType](
	[typeCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[typeName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[parentCode] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[status] [char](1) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_T_truckType_status]  DEFAULT ((1)),
	[IsLastNode] [char](1) COLLATE Chinese_PRC_CI_AS NULL CONSTRAINT [DF_T_truckType_IsLastNode]  DEFAULT ((1)),
	[HigherPerPoint] [decimal](18, 3) NULL,
	[RebatePoint] [decimal](18, 3) NULL,
	[DeductionPoint] [decimal](18, 3) NULL,
	[NOTE1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[NOTE5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL
) ON [PRIMARY]


-----车辆类型对应

CREATE TABLE [dbo].[T_TruckTypeCorrespond](
	[list_id] [bigint] IDENTITY(1,1) NOT NULL,
	[truckTypeCode] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[factTruckType] [nvarchar](200) COLLATE Chinese_PRC_CI_AS NULL,
	[Note1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note5] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note6] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note7] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note8] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note9] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Note10] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_T_TruckCorrespond] PRIMARY KEY CLUSTERED 
(
	[list_id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


--******************************销售模块end************************************



--******************************未知beg************************************
CREATE TABLE [dbo].[管理部室$](
	[部门编号] [nvarchar](255) NULL,
	[部门名称] [nvarchar](255) NULL,
	[预算科目编号] [float] NULL,
	[预算科目名称] [nvarchar](255) NULL,
	[借方科目编号] [nvarchar](255) NULL,
	[借方科目] [nvarchar](255) NULL,
	[贷方科目编号] [nvarchar](255) NULL,
	[贷方科目] [nvarchar](255) NULL,
	[解放科目2] [nvarchar](255) NULL,
	[贷方科目2] [nvarchar](255) NULL
) ON [PRIMARY]


CREATE TABLE [dbo].[sheet](
	[序号] [nvarchar](50) NULL,
	[分类] [nvarchar](50) NULL,
	[二级序号] [nvarchar](50) NULL,
	[项目] [nvarchar](50) NULL,
	[下级设置] [nvarchar](50) NULL,
	[制造费用] [nvarchar](50) NULL,
	[列 6] [nvarchar](50) NULL,
	[列 7] [nvarchar](50) NULL,
	[列 8] [nvarchar](50) NULL,
	[列 9] [nvarchar](50) NULL,
	[列 10] [nvarchar](50) NULL
) ON [PRIMARY]



CREATE TABLE [dbo].[bill_cbzx](
	[zxCode] [varchar](50) NOT NULL,
	[zxName] [varchar](50) NULL,
 CONSTRAINT [PK_bill_cbzx] PRIMARY KEY CLUSTERED 
(
	[zxCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[bill_searchRight](
	[userCode] [varchar](50) NULL,
	[deptCode] [varchar](50) NULL
) ON [PRIMARY]


--费用申请 bill_fysq 
CREATE TABLE [dbo].[bill_fysq](
	[billCode] [varchar](50) NULL,--对应单据编号
	[jbr] [varchar](50) NULL,
	[jkdjlx] [varchar](50) NULL,
	[sqzy] [text] NULL,
	[sqbz] [text] NULL,--申请备注
	[dwmc] [varchar](100) NULL,
	[khh] [varchar](100) NULL,
	[yhzh] [varchar](20) NULL,
	[sfjk] [varchar](1) NULL,
	[sfgf] [varchar](1) NULL,
	[sfth] [varchar](1) NULL,
	[hjje] [float] NULL--合计金额
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


--temp_ysfpbl
CREATE TABLE [dbo].[temp_ysfpbl](
	[ysgc] [varchar](8) NULL,
	[bl] [decimal](18, 4) NULL
) ON [PRIMARY]

--titleMessage
CREATE TABLE [dbo].[titleMessage](
	[code] [varchar](30) NULL,
	[title] [varchar](50) NULL,
	[context] [varchar](300) NULL,
	[memo] [varchar](50) NULL,
	[userCode] [varchar](30) NULL,
	[messageDate] [datetime] NULL,
	[msgType] [varchar](2) NULL,
	[upfile] [varchar](50) NULL
) ON [PRIMARY]



CREATE TABLE [dbo].[xs_srb](
	[ny] [varchar](10) NULL,
	[srys] [money] NULL,
	[srsj] [money] NULL,
	[scys] [money] NULL,
	[scsj] [money] NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[xs_ygzmx](
	[ny] [varchar](8) NULL,
	[km] [varchar](50) NULL,
	[s02] [float] NULL,
	[s05] [float] NULL,
	[s23] [float] NULL,
	[s18] [float] NULL,
	[s19] [float] NULL,
	[s21] [float] NULL,
	[s20] [float] NULL,
	[s28] [float] NULL,
	[s24] [float] NULL,
	[s26] [float] NULL,
	[s25] [float] NULL,
	[s59] [float] NULL,
	[s17] [float] NULL,
	[s15] [float] NULL,
	[s14] [float] NULL,
	[s13] [float] NULL,
	[zt] [varchar](1) NULL
) ON [PRIMARY]

--预算结转表
CREATE TABLE [dbo].[ysjz_temptb](
	[deptcode] [varchar](50) NULL,--部门编号
	[yskmcode] [varchar](50) NULL,--科目编号
	[je] [decimal](18, 2) NULL,--金额
	[frmysgc] [varchar](50) NULL,--调整预算过程出
	[toysgc] [varchar](50) NULL,--调整预算过程入
	[usercode] [varchar](50) NULL,--用户编号
	[guidid] [varchar](50) NULL,--唯一码
	[hsdo] [varchar](1) NULL--是否做结转
) ON [PRIMARY]


CREATE TABLE [dbo].[yssj](
	[bmbh] [nvarchar](255) NULL,
	[bmmc] [nvarchar](255) NULL,
	[fybh] [nvarchar](255) NULL,
	[fymc] [nvarchar](255) NULL,
	[je] [float] NULL
) ON [PRIMARY]


--工资项目与预算科目对应
CREATE TABLE [dbo].[bill_gzxmdy](
	[yskmCode] [varchar](20) NOT NULL,
	[dyName] [varchar](50) NOT NULL,
	[status] [varchar](2) NULL,
	[note1] [nchar](10) NULL,
	[note2] [nchar](10) NULL
) ON [PRIMARY]


--领用单主表 2015-05-07
create table bill_lyd(
[guid] varchar(50) primary key  not null,--主表主键
lyDate  varchar(30) null,--领用时间
lyr varchar(50) null,  --领用人
zdr varchar(50) null,--制单人
lyDept varchar(50) null, --领用部门
je decimal(18,2)null,	--总金额（子表汇总）
sm varchar(1000) null,   --说明
bz varchar(1000) null,	 --备注
zt varchar(10) null,  --状态默认为未确认
note1 varchar(50) null,
note2 varchar(50) null,
note3 varchar(50) null,
note4 varchar(50) null,
note5 varchar(50) null,
note6 varchar(50) null,
note7 varchar(50) null,
note8 varchar(50) null,
note9 varchar(50) null,
note0 varchar(50) null
)

--领用单子表 2015-05-07
create table bill_lyds(
guid varchar(50) not null,--主表主键
myGuid varchar(50) primary key not null,--主表主键
fykm varchar(50) null,--费用科目
je decimal(18,2) null,--金额
note1 varchar(50) null,
note2 varchar(50) null,
note3 varchar(50) null,
note4 varchar(50) null,
note5 varchar(50) null,
note6 varchar(50) null,
note7 varchar(50) null,
note8 varchar(50) null,
note9 varchar(50) null,
note0 varchar(50) null
)


 insert into dbo.bill_sysMenu (menuid,menuName,showName,menuUrl,menuOrder)values ('0425','采购支付','采购支付','../bxgl/bxglFrame.aspx?dydj=cgzf','625')	
 insert into dbo.bill_sysMenu (menuid,menuName,showName,menuUrl,menuOrder)values ('0426','采购支付审核','采购支付审核','../MyWorkFlow/BillMainToApprove.aspx?flowid=cgzf','626')	
  insert into dbo.bill_sysMenu (menuid,menuName,showName,menuUrl,menuOrder)values ('0427','领用单','领用单','../bxgl/LingYongDanList.aspx','627')	
 
 
 insert into  bill_djlx (djbh,djmc,djbm,splx,bhbj,bhlslx,bhlscd,djtype) values ('cgzf','采购支付','采购支付',	'cgzf','cgzf','d','6','0')
insert into mainworkflow(flowid,flowName) values ('cgzf','采购支付')
insert into bill_workFlow (flowId,flowText,stepTextColor,stepStrokeColor,stepShadowColor,stepFocusedStrokeColor,isStepShadow,actionStrokeColor,actionTextColor,actionFocusedStrokeColor,sStepTextColor,sStepStrokeColor,stepColor1,stepColor2,isStep3D,step3DDepth,orderBy)values
('cgzf','采购支付','green','green','#b3b3b3','yellow','T','green','','yellow','red','	green','green','white','true','	20','2015-02-06 11:36:29')
insert into billtoworkflow (billtype,billname,flowid) values('cgzf','采购支付','cgzf')

--******************************未知end************************************


--******************************手机端begin************************************
--手机端报销单临时表
create table ph_main(
	[billCode] [varchar](50) NULL,--唯一号编号
	[billName] [varchar](50) NULL,--单据编号
	[flowID] [varchar](50) NULL,--单据类型(ys,预算，yszj，预算追加，xmys，项目预算。。。)
	[stepID] [varchar](50) NULL,--审核步骤
	[billUser] [varchar](50) NULL,--制单人
	[bxr] varchar(50)null,--报销人
	[billDate] varchar(10) NULL,--制单日期
	[billDept] [varchar](50) NULL,--部门
	[loopTimes] [int] NULL,--循环次数
	[isgk] varchar(2) null,--是否归口
	[gkdept] varchar(50) null,--归口部门
	[bxmxlx] [varchar](50) NULL,--报销明细类型
	[bxzy] varchar(200) null,--报销摘要
	[bxsm] varchar(200) null--报销说明	
)

--手机端菜单表
create table  ph_sysmenu(
menuId varchar(20) not null, --菜单编号
menuName varchar(50) null,--菜单名称
showName varchar(50) null,--显示名称
menuUrl varchar(200) null,--菜单页面
menuIcon varchar(200) null,--菜单图标
menuOrder varchar(200) null,--排序号
IsCount varchar(2) null,--是否显示数目提示 1显示 0不显示  默认不显示，即为0
getCountSql varchar(1000) null,--获取提示数字的sql语句 参数用@userCode 表示 
sqlsm varchar(500) null,--sql说明 及sql参数说明 @userCode 当前用户编号  
menuSm varchar(200) null,--菜单说明
menuState varchar(10) null,--菜单状态 1正常 0禁用  默认正常，即为1
note0 varchar(50) null,
note1 varchar(50)null,
note2 varchar(50) null,
note3 varchar(50)null,
note4 varchar(50) null,
note5 varchar(50) null,
note6 varchar(50)null,
note7 varchar(50) null,
note8 varchar(50) null,
note9 varchar(50)null
)

--手机端菜单权限表
create table ph_menuRight
(
menuid varchar(20) not null, --菜单编号
objecId varchar(20) not null, --人员编号/角色编号
rightType varchar(10) not null, --角色类型 人员编号1 角色编号3 
note0 varchar(50) null,
note1 varchar(50)null,
note2 varchar(50) null,
note3 varchar(50)null,
note4 varchar(50) null,
note5 varchar(50) null,
note6 varchar(50)null,
note7 varchar(50) null,
note8 varchar(50) null,
note9 varchar(50)null
)



--==================================商用车新增表===============================
---新加菜单  bill_sysMenu 表
0815	电子发票单据	电子发票单据	../fysq/Dzfplist.aspx	814	NULL	NULL
--新加配置项 t_config表
HasFP	是否有电子发票单	1	0	没有此记录则默认值为1  1是 0 否	NULL

---凭证配置sql
update t_Config set avalue='http://192.168.100.64:80/service/XChangeServlet?account=0001&receiver=' where akey='ToNcURL'
select * from dbo.bill_dataDic where dicType='10'
insert into bill_dataDic(dictype,diccode        ,dicname,cjys,cys,cdj) 
values ('10','CNHTC0209010102','济南商用车公司销售部','1108','1','0');

--电子发票附件
create table bill_fpfj(
	fprq  varchar(50)  null,--单据日期
	billCode varchar(50) null,--单据编号
	deptCode varchar(50) NULL,--部门编号
	deptName varchar(50) null,--部门名称
	fpusercode varchar(50) null,--发票人员编号
	fpusername varchar(50) null,--发票人员名称
	bz varchar(100)null,--备注
	note0 varchar(50) null,
	note1 varchar(50)null,
	note2 varchar(50) null,
	note3 varchar(50)null,
	note4 varchar(50) null,
	note5 varchar(50) null,
	note6 varchar(50)null,
	note7 varchar(50) null,
	note8 varchar(50) null,
	note9 varchar(50)null
)

create table bill_fpfjs(
	billCode varchar(50) null,--单据编号
	fph varchar(50) null,--发票号
	fpdw varchar(50) null,--发票单位
	fpje decimal(18,2) null,--发票金额
	bz nvarchar(100) null,--备注
	note0 varchar(50) null,
	note1 varchar(50)null,
	note2 varchar(50) null,
	note3 varchar(50)null,
	note4 varchar(50) null,
	note5 varchar(50) null,
	note6 varchar(50)null,
	note7 varchar(50) null,
	note8 varchar(50) null,
	note9 varchar(50)null

)

--******************************手机端end************************************



--******************************市立医院收入类费用***************************
--外部系统收入科目编号与预算系统编号对应关系
create table sr_kmdy(
	dytype varchar(10) not null,--对应类型  502预交金 503出院结算  504 住院收入  505门诊收入
	outcode varchar(50) not null,--外部系统科目编号
	outname nvarchar(50) not null,--外部系统科目名称
	yskmcode varchar(50)  null,--预算科目编号
	yskmname nvarchar(50)  null,--预算科目名称
	note1 nvarchar(50) null,--
	note2 nvarchar(50) null,--
	note3 nvarchar(50) null,--
	note4 nvarchar(50) null,--
	note5 nvarchar(50) null,--
	
)
--从外部数据源获取信息后 临时存储表
create table sr_import_temp(
	id nvarchar(50) not null,--唯一标识符
	imptype varchar(10) not null,--导入类型 502预交金 503出院结算  504 住院收入  505门诊收入
	dostatus  varchar(10) not null,--默认0 表示未处理 1表示已经处理
	operno nvarchar(50) null,--操作人编号  15563361861
	opername nvarchar(50) null,--操作人名称
	deptcode nvarchar(50) null,--部门编号
	deptname nvarchar(50) null,--部门名称
	orderby nvarchar(50) null,--开单科室编号
	orderbyname nvarchar(50) null,--开单科室名称
	classcode nvarchar(50) null,--科目编号
	classname nvarchar(50) null,--科目名称
	impdate nvarchar(50) null,--日期
	costs decimal(18,2) null,--应收
	charges decimal(18,2) null,--实收
	outputid varchar(10) null,--输出id
	payway nvarchar(50) null,--支付方式(502 503有数据)
	amount decimal(18,2) null,--支付金额(502 503有数据)
	[deptcode_ys] [nvarchar](50) NULL,
	[orderby_ys] [nvarchar](50) NULL,
	[classcode_ys] [nvarchar](50) NULL,
	note1 nvarchar(50) null,--支付方式  506的时候用于区分
	note2 nvarchar(50) null,--
	note3 nvarchar(50) null,--
	note4 nvarchar(50) null,--
	note5 nvarchar(50) null,--
	note6 nvarchar(50) null,--
	note7 nvarchar(50) null,--
	note8 nvarchar(50) null,--
	note9 nvarchar(50) null,--
	note0 nvarchar(50) null,--
)
create table sr_kmdy_dz(
	atype varchar(10) not null,--课程 ;物品
	outname nvarchar(50) not null,--外部系统科目名称
	yskmcode varchar(50)  null,--预算科目编号
	yskmname nvarchar(50)  null,--预算科目名称
	note1 nvarchar(50) null,--
	note2 nvarchar(50) null,--
	note3 nvarchar(50) null,--
	note4 nvarchar(50) null,--
	note5 nvarchar(50) null,--
	
)
--大智导入收入数据临时表
create table sr_import_temp_dz(
	id nvarchar(50) not null,--唯一标识符
	atype nvarchar(50) null,--项目类型：1为课程，2为物品
	CampusName nvarchar(50) null,--校区名称
	ReceiptNo nvarchar(50) null,--收据号
	UserName nvarchar(50) null,--操作员姓名
	aDate nvarchar(50) null,--收费日期
	ItemName nvarchar(50) null,--项目名称
	TotalMoney decimal(18,2) null,--金额
	EmployeeNames nvarchar(50)  null,--业绩归属人姓名（如果有多个用逗号分隔）
	ConfirmUserName nvarchar(50)  null,--收费确认的用户名
	ConfirmTime nvarchar(50) null,--收费确认的时间
	dostatus  varchar(10) not null,--默认0 表示未处理 1表示已经处理
	
	dept_ys varchar(50) null,--预算系统编号
	yskmcode varchar(50) null,--预算系统预算科目编号
	note1 nvarchar(50) null,--
	note2 nvarchar(50) null,--
	note3 nvarchar(50) null,--
	note4 nvarchar(50) null,--
	note5 nvarchar(50) null,--
	note6 nvarchar(50) null,--
	note7 nvarchar(50) null,--
	note8 nvarchar(50) null,--
	note9 nvarchar(50) null,--
	note0 nvarchar(50) null,--
)
--制单日志
create table sr_zd_note(
	billtype varchar(10) not null,--导入类型 502预交金 503出院结算  504 住院收入  505门诊收入
	billdate nvarchar(50) not null,--制单日期
	deptcode nvarchar(50) null,--单据部门
	deptname nvarchar(50) null,--单据部门名称
	srdcode nvarchar(50) null,--对应的报销单号
	note1 nvarchar(50) null,--
	note2 nvarchar(50) null,--
	note3 nvarchar(50) null,--
	note4 nvarchar(50) null,--
	note5 nvarchar(50) null,--
	note6 nvarchar(50) null,--
	note7 nvarchar(50) null,--
	note8 nvarchar(50) null,--
	note9 nvarchar(50) null,--
	note0 nvarchar(50) null,--
)
--因为天健中可能多个部门编号对应一个预算系统部门编号 所以设对应表
create table sr_dept_tianjian(
	tianjiancode nvarchar(50) not null,--天健系统中编号
	ysdeptcode nvarchar(50) not null,--预算系统部门编号
	note1 nvarchar(50) null,--
	note2 nvarchar(50) null,--
	note3 nvarchar(50) null,--
	note4 nvarchar(50) null,--
	note5 nvarchar(50) null,--
)
--******************************市立医院收入类费用end***************************
--用款申请单  beg 2015-03-13 edit by zyl
create table bill_yksq
(
billCode varchar(50) primary key not null,   --单据编号
jbr varchar(50) null,	  --经办人
billDept varchar(50) null,	 --用款部门
yt	 nvarchar(500)   null,   --用途
je   decimal(18,2)null,		--申请金额
rkCodes	 varchar(500) null,	--对应外部入库单号 多个单号用逗号隔开

cdecfine0 decimal(18,2) null,	
cdecfine1 decimal(18,2) null,
cdecfine2 decimal(18,2) null,
cdecfine3 decimal(18,2) null,
cdecfine4 decimal(18,2) null,

ddefine0 datetime null,	  --日期
ddefine1 datetime null,
ddefine3 datetime null,
ddefine4 datetime null,
ddefine5 datetime null,

note1 nvarchar(50) null,--备用字段
note2 nvarchar(50) null,--备用字段
note3 nvarchar(50) null,--备用字段
note4 nvarchar(50) null,--备用字段
note5 nvarchar(50) null,--备用字段
note6 nvarchar(50) null,--备用字段
note7 nvarchar(50) null,--备用字段
note8 nvarchar(50) null,--备用字段
note9 nvarchar(50) null,--备用字段
note0 nvarchar(50) null,--备用字段
)
--用款申请单  end
 ---==============================大智增表==============================----
 -- 费用部门比例设置 
 create table bill_deptFyblDy(
     list_id int IDENTITY (1, 1) NOT NULL ,--自增列
     deptCode varchar(50) null,--部门编号
	 deptName varchar(50) null,--部门名称
	 fjbl decimal(18,4) null,   --分解比例
	 cdefine1 varchar(10) null,--年度
	 cdefine2 varchar(30) null,--
	 cdefine3 varchar(30) null,--
	 cdefine4 varchar(100) null,--
	 cdefine5 varchar(100) null,--
	 cdefine6 varchar(300) null,--
	 ddefine7 decimal(18,2) null,-- 分解金额
	 ddefine8 decimal(18,2) null,--
	 ddefine9 datetime null,--
	 ddefine10 datetime null--
 )
 --财年配置表
 create table bill_Cnpz( 
  list_id int IDENTITY (1, 1) NOT NULL ,--自增列
  beg_time varchar(50) null,--自然年开始日期
  end_time varchar(50) null,--自然年结束日期
  year_moth varchar(50) null,--财年年月
  year_CN varchar(50) null,--财年月
  note0 varchar(50) null,
  note1 varchar(50) null,
  note2 varchar(50)  null,
  note3 varchar(50) null,
  note4 varchar(50) null,
  note5 varchar(50) null
 )
 --收入目标
 create table bill_Srmb(
   nd varchar(50) null,--预算财年
   zje decimal(18,4) null,--总金额
   yfjje decimal(18,4) null,--已分解金额
   syje decimal(18,4) null,--剩余金额
   note0 varchar(50) null,--费用类型 sr:收入预算  fy: 费用预算
   note1 varchar(50) null,
   note2 varchar(50)  null,
   note3 varchar(50) null,
   note4 varchar(50) null,
   note5 varchar(50) null 
 )
 --预算部门分解表（已废弃）
 create table bill_deptfj(
	nd varchar(50) null,--预算年度
	deptcode varchar(50) null,--部门编号
	deptname varchar(50) null,--部门名称
	bl decimal(18,4) null,--比例
	xjje decimal(18,4) null,--分解金额
	note0 varchar(50) null,
	note1 varchar(50) null,
	note2 varchar(50)  null,
	note3 varchar(50) null,
	note4 varchar(50) null,
	note5 varchar(50) null
 )
 -----资产购置申请单
create table dz_zncgzsqd(
    swbh varchar(50) null,--事物编号（zcgzyyyyMMdd001）
	zydj varchar(50) null,--重要等级
	sqsy varchar(500) null,--申请事由
	tsbz varchar(500) null,--特殊备注
	
	bh varchar(50) null,--编号(手动录入)
	sqsj varchar(50) null,--申请时间
	wpmc  varchar(50) null,--物品名称
	ggsl varchar(50) null,--规格数量
	yt varchar(50) null,--用途
	sybm varchar(50) null,--使用部门
	xyrq varchar(50) null,--需用日期
	gjjz varchar(50) null,--估计价值
	gzbz varchar(50) null,--购置备注
	sqjs decimal(18,4) null,--共申请办公用品件数
	zje decimal(18,4) null,--总金额
	sgbmfzr varchar(50) null,--申购部门负责人
	sgbmrq varchar(50) null,--申购部门日期
	nqbyj varchar(50) null,--内勤部意见
	nqbrq varchar(50) null,--内勤部日期
	cwbyj varchar(50) null,--财务部意见
	cwbrq varchar(50) null,--财务部日期
	rzxzyj varchar(50) null,--人资行政意见
	rzxzrq varchar(50) null,--人资行政日期
	xzbyj varchar(50) null,--行政部意见
	xzbrq varchar(50) null,--行政部日期
	[fj] [varchar](2000) NULL,--附件
	note0 varchar(50) null,
	note1 varchar(50) null,
	note2 varchar(50) null,
	note3 varchar(50) null,
	note4 varchar(50) null,
	note5 varchar(50) null
	
)
--用款申请单
create table dz_yksqd(
 sqbh varchar(50) null,--申请单号
 sqsj varchar(50) null,--申请时间
 sqlx varchar(50)null,--申请类型
 ykrq varchar(50) null,--用款日前
 sqr varchar(50) null,--申请人
 kxyt varchar(50) null,--款项用途
 ykfs varchar(50) null,--用款方式
 kxje_dx varchar(50) null,--款项金额（大写）
 kxje_xx decimal(18,4) null,--款项金额(小写)
 skdw varchar(50) null,--收款单位
 khh varchar(50) null,--开户行
 zh varchar(50) null,--账号
 bmfzr_yj varchar(50)null,--部门负责人意见
 bmfzr_qz varchar(50)null,--部门负责人签字
 bmfzr_rq varchar(50)null,--部门负责人日期
 yfzkzy_yj varchar(50)NULL,--应付账款专员意见
 yfzkzy_qz varchar(50)NULL,--应付账款专员签字
 yfzkzy_rq varchar(50)NULL,--应付账款专员日期
 cwbfzr_yj varchar(50)null,--财务部负责人意见
 cwbfzr_qz varchar(50)null,--财务部负责人签字
 cwbfzr_rq varchar(50)null,--财务部负责人日期
 cwxz_yj varchar(50)null,--财务分管校长/区域总校长意见
 cwxz_qz varchar(50)null,--财务分管校长/区域总校长签字
 cwxz_rq varchar(50)null,--财务分管校长/区域总校长日期
 dsz_yj varchar(50)null,--董事长意见
 dsz_qz varchar(50) null,--董事长签字
 dsz_rq varchar(50) null,--懂事长日期
 [fj] [varchar](2000) NULL,--附件
 note0 varchar(50) null,--用款部门
 note1 varchar(50) null,
 note2 varchar(50) null,
 note3 varchar(50) null,
 note4 varchar(50) null,
 note5 varchar(50) null
 
)
--预算科目与存货对应表

create table [dbo].[bill_yskmchdy]
(
   chcode varchar(50) null,--存货编号
   chname varchar(50) null,--存货名称
   kmcode varchar(50) null,--科目编号
   kmname varchar(50) null,--科目名称
   yslx varchar(50) null, --预算类型
   ufdata varchar(50) null,--对应帐套
   note0 varchar(50) NULL,--
   note1 varchar(50) NULL,--
   note2 varchar(50) NULL,--
   note3 varchar(50) NULL,--
   note4 varchar(50) NULL,--
   note5 varchar(50) NULL --

)
--从用友导入本系统生成存货领用单时，记录已经生成了的单子
create table bill_chly_mark(
	billCode nvarchar(50) null,--
	billuser nvarchar(50) null,--制单人
	billdept nvarchar(50) null,--制单部门
	billdate nvarchar(50) null,--制单时间
    [pocode] [nvarchar](50) NULL,--单据编号
	[mark] [nvarchar](100) NULL,  --标记内容 
	[chcode] [nvarchar](50) NULL,--存货code
	ufdata  [nvarchar](50) NULL,--装套数据库
	[note1] [nvarchar](50) NULL,--
	[note2] [nvarchar](50) NULL,
	[note3] [nvarchar](50) NULL,
	[note4] [nvarchar](50) NULL,
	[note5] [nvarchar](50) NULL
)
CREATE TABLE [dbo].[bill_Mark](
	[billcode] [nvarchar](50) NULL,--单据编号
	[mark] [nvarchar](100) NULL,--标记内容
	[usercode] [nvarchar](50) NULL,--用户编号
	[note1] [nvarchar](50) NULL,
	[note2] [nvarchar](50) NULL,
	[note3] [nvarchar](50) NULL,
	[note4] [nvarchar](50) NULL,
	[note5] [nvarchar](50) NULL
) ON [PRIMARY]
--校管家
CREATE TABLE [dbo].[bill_xg](
	yedjh nvarchar(50) null,--业务单据号
	zdrcode nvarchar(50) null,--制单人编号
	zdrname nvarchar(50) null,--制单人姓名
	fsrq nvarchar(50) null,--发生日期
	fxcode nvarchar(50) null,--分校编号
	fxname nvarchar(50) null,--分校名称
	srxmcode nvarchar(50) null,--收入项目编号
	srxmname nvarchar(50) null,--收入项目名称
	xmsrje decimal(18,4) null,--业绩归属部门
	yjgsbmcode nvarchar(50) null,--业绩归属部门编号
	yjgsbmname nvarchar(50) null,--业绩归属部门名称
	yjgsrycode nvarchar(50) null,--业绩归属人员编号
	yjgsryname nvarchar(50) null,--业绩归属人员名称
	shr nvarchar(50) null,--审核人
	shrq nvarchar(50) null--审核日期
) 

--大智学校跨区域转费转校申请单
CREATE TABLE [dbo].[bill_zfzxsqd_dz](
    billCode nvarchar(50) null,--billCode
	sqrq nvarchar(50) null,--申请日期
	zcfx nvarchar(50) null,--转出区域分校
	zrfx nvarchar(50) null,--转入分校
	xyxm nvarchar(50) null,--学员姓名
	nianji nvarchar(50) null,--年级
	yxyfdfy decimal(18,4) null,--原协议辅导费用
	ybmkc nvarchar(50) null,--原报名课程
	ykcxsyh nvarchar(50) null,--原课程享受优惠
	yxfks decimal(18,4) null,--已消费课时/次
	dyksdj decimal(18,4) null,--对应课时单价
	yxffy decimal(18,4) null,--已消费费用
	ykqtfy decimal(18,4) null,--应扣其他费用
	syjexx decimal(18,4)null,--剩余金额
	syjedx nvarchar(50) null,--剩余金额大写
	zfyy nvarchar(50) null,--转费原因
	xbxs nvarchar(50) null,--新报小时/课程
	xbjje decimal(18,4) null,--须补交金额
	xbjjedx nvarchar(50)null,--须补交金额大写
	[note1] [nvarchar](50) NULL,
	[note2] [nvarchar](50) NULL,
	[note3] [nvarchar](50) NULL,
	[note4] [nvarchar](50) NULL,
	[note5] [nvarchar](50) NULL
) 
---关系学员特惠信息
create table bill_gxxythxx_dz(
   billCode nvarchar(50) null,
   fenxiao nvarchar(50) null,--分校
   xyxm nvarchar(50) null,--学员姓名
   nianji nvarchar(50) null,--年级
   bmkc nvarchar(50) null,--报名课程
   ysf nvarchar(50) null,--应收费
   xhyh nvarchar(50)null,--现行优惠
   youhui decimal(18,4) null,--优惠
   zengsong1 nvarchar(50) null,--赠送
   zengsong2 nvarchar(50) null,--赠送（备注）
   beizhu nvarchar(50) null,--备注
   [note1] [nvarchar](50) NULL,
	[note2] [nvarchar](50) NULL,
	[note3] [nvarchar](50) NULL,
	[note4] [nvarchar](50) NULL,
	[note5] [nvarchar](50) NULL
   
)











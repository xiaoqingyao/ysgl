
--******************************������Ϣbeg************************************
--1����������
--�û��� bill_users
CREATE TABLE [dbo].[bill_users](
	[userCode] [varchar](20) NULL,--�û����
	[userName] [varchar](50) NULL,--�û���
	[userGroup] [varchar](20) NULL,--��ɫ����
	[userStatus] [varchar](1) NULL,--״̬ 0-ɾ�� 1����
	[userDept] [varchar](50) NULL,--���ű��
	[userPwd] [varchar](32) NULL,--����
	[isSystem] [varchar](1) NULL,--�Ƿ�ϵͳ�û�
	[userPosition] [varchar](50) NULL--ְλ���  ��Ӧbill_datadic��datatypeΪ05�µ�datacode
)

--���ű� bill_departments
CREATE TABLE [dbo].[bill_departments](
	[deptCode] [varchar](20) NULL,--���ű��
	[deptName] [varchar](50) NULL,--��������
	[sjDeptCode] [varchar](20) NULL,--�ϼ����ű��
	[deptStatus] [varchar](1) NULL,--״̬ D-ɾ�� 1����
	[IsSell] [char](1) NULL,
	[deptJianma] [varchar](50) NULL,
	[foru8id] [varchar](50) NULL,
	[forTianJian] [varchar](50) NULL,	
	[Isgk] [char](1) null, --�Ƿ��ǹ�ڲ��� Y-�� N-����
	iskzys varchar(50) null,--

	
)

--- ���Ź������
create table [bill_dept_gksz](
	gkdeptcode varchar(50) null,--��ڲ��ű��
	gkdeptname varchar(50) null,--��ڲ�������
	deptCode varchar(50) null,--����ڲ��ű��
	deptName varchar(50) null,--����ڲ�������
	note0 varchar(50) null,
	note1 varchar(50) null,
	note2 varchar(50) null,
	note3 varchar(50) null,
	note4 varchar(50) null,
	note5 varchar(50) null
)


--�������ܱ� bill_dept_ywzg
CREATE TABLE [dbo].[bill_dept_ywzg](
	[deptCode] [varchar](20) NULL,--���ű��
	[userCode] [varchar](20) NULL--��Ա���
	
)


--���ŷֹ��쵼�� bill_dept_fgld
CREATE TABLE [dbo].[bill_dept_fgld](
	[deptCode] [varchar](20) NULL,--���ű��
	[userCode] [varchar](20) NULL--��Ա���
)

--��ɫ
CREATE TABLE [dbo].[bill_userGroup](
	[groupID] [varchar](20) NOT NULL,
	[groupName] [varchar](50) NULL,
	[gType] [varchar](1) NULL
) ON [PRIMARY]

--�����ֵ�� bill_dataDic
CREATE TABLE [dbo].[bill_dataDic](
	[dicType] [varchar](20) NULL,--���ͣ�00��һ��������2��������
	[dicCode] [varchar](50) NULL,--���
	[dicName] [varchar](50) NULL,--����
	
	[cjys] [varchar](10) NULL,--�Ƿ���Ԥ�㣨���ף��洢���׵Ľ����ߣ�
	[cys] [varchar](1) NULL,--����Ԥ��
	cdj	varchar(2) null,--�Ƿ���븽���ݣ��������ͣ�0����1�����浥��2�����뵥�������������0����1�����븽���浥�������׺�1 Ĭ�� 0 ����Ĭ�ϣ�
	[note1] [nvarchar](50) NULL, --��dictype=18 �洢Ԥ��flowID ��07(����ƾ֤��Ӧ����) �洢��Ӧ�Ŀ�ĿԤ������
	[note2] [nvarchar](50) NULL,--��dictype=18 �洢���� flowID
	[note3] [nvarchar](50) NULL,--��ʹ��
	[note4] [nvarchar](50) NULL,
	[note5] [nvarchar](50) NULL
) ON [PRIMARY]

 --��Ϣ�� ����������ʾ��bill_msg
 CREATE TABLE [dbo].[bill_msg](
	[id] [int] IDENTITY(1,1) NOT NULL,--��¼id ������
	[title] [varchar](200) NULL,--��Ϣ����
	[contents] [text] NULL,--��Ϣ����
	[writer] [varchar](50) NULL,--������
	[date] [datetime] NULL,--����ʱ��
	[readTimes] [int] NULL,--�Ķ�����
	[mstype] [nvarchar](50) NULL,--��Ϣ����
	[notifierid] [nvarchar](50) NULL,
	[notifiername] [nvarchar](50) NULL,--֪ͨ��
	[endtime] [nvarchar](50) NULL,--֪ͨ��Ч��ֹ����
	[Accessories] [nvarchar](200) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

--֪ͨ��Ա
create table MessageReader(
	code varchar(30) null,--������
	usercode varchar(30) null,--�û����
	isRead int null --�Ƿ��Ѷ�
)

--�ռǱ�bill_notePad 
CREATE TABLE [dbo].[bill_notePad](
	[listid] [decimal](18, 0) IDENTITY(1,1) NOT NULL,--��¼id ������
	[noteDate] [datetime] NULL,--����
	[userCode] [varchar](50) NULL,--�û����
	[context] [varchar](300) NULL,--����
	[noteType] [varchar](5) NULL,--����
 CONSTRAINT [PK_bill_notePad] PRIMARY KEY CLUSTERED 
(
	[listid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

--Ԥ����Ŀ��
CREATE TABLE [dbo].[bill_xm](
	[xmCode] [varchar](20) NULL,--���
	[xmName] [varchar](200) NULL,--����
	[sjXm] [varchar](50) NULL,--�ϼ���Ŀ
	[xmDept] [varchar](50) NULL,--��Ŀ��������
	[xmStatus] [varchar](1) NULL--��Ŀ״̬��0��ͣ�ã�1��������
) ON [PRIMARY]


--��Ŀ������ȿ��ƽ��� ���ã����ø������µĸ�����Ŀ����ȿ��ƽ���ͨ������isCtrl�ֶ��������Ƿ���Ԥ��
  create table bill_xm_dept_nd
	  (
	    xmCode varchar(20),--��Ŀ���
	    xmDept varchar(50),--��Ŀ����  ����'000001'Ϊ��˾��Ŀ ����Ϊ�����µ���Ŀ
	    je float,--���
	    isCtrl varchar(2),--��Ŀ����Ƿ��ܿ�
	    nd varchar(4),-- ���
	    status varchar(2),--�Ƿ���� 0������ 1���� Ĭ��Ϊ1
	    note0 varchar(50),
	    note1 varchar(50),
	    note2 varchar(50),
	    note3 varchar(50),
	    note4 varchar(50),
	    note5 varchar(50),
	    note6 varchar(50),
	    note7 varchar(50),
	   )
	   
	--��Ŀ֧������ 2012-04-25����
 CREATE TABLE [dbo].[bill_xmzfd](
	[billcode] [varchar](20) COLLATE Chinese_PRC_CI_AS NULL,--����
	[sj] [datetime] NULL,--�Ƶ�����
	[zfDept] [varchar](500) COLLATE Chinese_PRC_CI_AS NULL,--֧������
	[zfxm] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,--֧����Ŀ
	[zynr] [text] COLLATE Chinese_PRC_CI_AS NULL,--����
	[sm] [text] COLLATE Chinese_PRC_CI_AS NULL,--˵��
	[cbr] [varchar](50) COLLATE Chinese_PRC_CI_AS NULL,--�а���
	[ms] [text] COLLATE Chinese_PRC_CI_AS NULL,--����
	[zfje] [float] NULL--���
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

--���ɱ�ű�
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


--******************************������Ϣend************************************


--******************************ϵͳ����beg************************************

--���¼tab_Message
CREATE TABLE [dbo].[tab_Message](
	[tbName] [nvarchar](50) NULL,--����
	[tbMeaning] [nvarchar](100) NULL,--��ע
	[tbType] [char](1) NOT NULL,--������  2ϵͳ�� 1ҵ��� 0�������ݱ�
	[tbStatus] [char](1) NOT NULL,--״̬1 ���� 0�ϳ�
	[note1] [nvarchar](50) NULL,
	[note2] [nvarchar](50) NULL,
	[note3] [nvarchar](50) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������1=ҵ������0=�������� 2=����ɾ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tab_Message', @level2type=N'COLUMN',@level2name=N'tbType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��״̬1 ����D����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tab_Message', @level2type=N'COLUMN',@level2name=N'tbStatus'
GO

ALTER TABLE [dbo].[tab_Message] ADD  CONSTRAINT [DF_tab_Message_tbType]  DEFAULT ((1)) FOR [tbType]
GO

ALTER TABLE [dbo].[tab_Message] ADD  CONSTRAINT [DF_tab_Message_tbStatus]  DEFAULT ((1)) FOR [tbStatus]
GO


--ϵͳ��������
create table t_Config(
	akey varchar(30) primary key,--���
	meaning varchar(50) not null,--����
	avalue varchar(30) not null,--��ʶ��0����1���ǣ�
	classify varchar(30)  null,--���ࣨ��
	remark  varchar(300) null,--��ע
	szsm varchar(2000) null--����˵��
)


--�û�Ȩ�ޱ��˵�Ȩ�ޣ� bill_userRight
CREATE TABLE [dbo].[bill_userRight](
	[userCode] [varchar](50) NULL,
	[objectID] [varchar](20) NULL,
	[rightType] [varchar](1) NULL --1=>�˵�����Ȩ�� 2=>��Ա������Ȩ�� 3==��ɫ����Ȩ��
)

--��ɫ�� bill_userGroup
CREATE TABLE [dbo].[bill_userGroup](
	[groupID] [varchar](20) NOT NULL,--��ɫ���
	[groupName] [varchar](50) NULL,--��ɫ��
	[gType] [varchar](1) 
) 


--ϵͳ����
CREATE TABLE [dbo].[bill_SysConfig](
	[ConfigName] [varchar](50) NULL,--�������ʹ���
	[ConfigValue] [varchar](50) NULL,--����ֵ
	[Memo] [varchar](50) NULL,--��ѡ����ֵ��˵��
	[nd] [varchar](10) NULL--���
) ON [PRIMARY]


--�˵��� bill_sysMenu
CREATE TABLE [dbo].[bill_sysMenu](
	[menuid] [varchar](20) NOT NULL,--�˵�id
	[menuName] [varchar](50) NULL,--ϵͳ�˵��� �������û��޸ĵ�
	[showName] [varchar](50) NULL,--������ʾ������ �����û��޸ĵ�
	[menuUrl] [varchar](200) NULL,--�˵�url
	[menuOrder] [int] NULL,--�����ֶ�
	[menusm] [varchar](200) NULL,
	[menustate] [varchar](10) NULL--�˵�״̬ ״̬ΪD��delete�����ɼ� �߼�ɾ��

) ON

--�˵�����
CREATE TABLE [dbo].[bill_sysMenuHelp](
	[list_id] [int] IDENTITY(1,1) NOT NULL,--��¼id������
	[menuid] [varchar](20) NOT NULL,--�˵����
	[menusm] [text] NULL,--�˵�˵��
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

--ϵͳ�������� ��������Ԥ�����  ���Ӧ�������õ�bill_syspar 
CREATE TABLE [dbo].[bill_syspar](
	[parname] [varchar](20) NULL,--������
	[parVal] [text] NULL--ֵ
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


--******************************ϵͳ����end************************************

--******************************Ԥ��ģ��beg************************************


--Ԥ���Ŀ bill_yskm
CREATE TABLE [dbo].[bill_yskm](
	[yskmCode] [varchar](20) NULL,--��Ŀ����
	[yskmBm] [varchar](50) NULL,--��Ŀ���
	[yskmMc] [varchar](50) NULL,--��Ŀ����
	[gjfs] [varchar](1) NULL,--
	[tbsm] [text] NULL,--�˵��
	[tblx] [varchar](20) NULL,--����ͣ�01����λ���02���������03���۷��ã�
	[kmStatus] [varchar](1) NULL,--��Ŀ״̬��0�����ã�1�����ã�
	[kmlx] varchar(2) null,--��Ŀ����(�Ƿ�ɿ� 0���ɿأ�1�����ɿ�)
	gkfy varchar(1) null,--��ڷ���
	xmhs varchar(1) null,--��Ŀ����
	bmhs varchar(1) null,--���ź���
	ryhs varchar(1),--��Ա����
    kmzg nvarchar(100) null, --��Ŀ����
	szlx varchar(50) null,--��֧����  0-����  1-����
	allowTz varchar(10) null,--�Ƿ������ĿԤ�����  0������  1����    Ĭ��Ϊ����
    --�¼��ֶ�
    dydj varchar(50) null,--��Ӧ����
	zjhs varchar(50) null,--�ʽ����
	kmType varchar(50) null,--��Ŀ����
	iszyys nvarchar(50)null,--�Ƿ�ռ��Ԥ�� 0 ��ռ��
	note1 nvarchar(50) null,
	note2 nvarchar(50) null,
	note3 nvarchar(50) null,
	note4 nvarchar(50) null,
	note5 nvarchar(50) null
	) 



--Ԥ���Ŀ���Ŷ�Ӧ�� bill_yskm_dept
create TABLE [dbo].[bill_yskm_dept](
	[deptCode] [varchar](50) NULL,--����
	[yskmCode] [varchar](50) NULL,--Ԥ���Ŀ
	[cwkmCode] [varchar](50) NULL,--�����Ŀ--�����ֶ�
	jfkmcode1 varchar(50) null,--�跽��Ŀ���1
    dfkmcode1 varchar(50) null,--������Ŀ���1
    jfkmcode2 varchar(50) null,--�跽��Ŀ���2
    dfkmcode2 varchar(50) null--������Ŀ���2
	--�¼��ֶ�
	djlx varchar(50) null,--�������ͣ�01 ���ñ�������������Ʊ����02���ñ���������¼��Ʊ����03���ñ�����������Ʊ����04�����ࡢ05�������
) 


--Ԥ���Ŀ ������Ŀ�Ķ��ձ� bill_yskm_dzb
CREATE TABLE [dbo].[bill_yskm_dzb](
	[yskmCode] [varchar](20) NULL,--Ԥ���Ŀ���
	[cwkmCode] [varchar](20) NULL,--�����Ŀ���
	[gjdw] [varchar](50) NULL,
	[xjdw] [varchar](50) NULL
) ON [PRIMARY]

 --Ԥ���Ŀ��ڲ��Ŷ�Ӧ�� bill_yskm_dept
create table bill_yskm_gkdept(
	deptcode varchar(50) not null,--���ű��
	yskmcode varchar(50) not null,--Ԥ���Ŀ���
	note1 varchar(50) null,
	note2 varchar(50) null,
	note3 varchar(50) null,
	note4 varchar(50) null,
	note5 varchar(50) null,
)


--Ԥ����ϸ�� bill_ysmxb
CREATE TABLE [dbo].[bill_ysmxb](
	[gcbh] [varchar](50) NULL,--Ԥ����̱��
	[billCode] [varchar](50) NULL,--���ݱ��
	[yskm] [varchar](50) NULL,--Ԥ���Ŀ
	[ysje] [float] NULL,--Ԥ����
	[ysDept] [varchar](50) NULL,--Ԥ�㲿��
	[ysType] [varchar](1) NULL,--Ԥ�����ͣ�1��һ��Ԥ�㣬2��׷�ӣ�3Ԥ�������4��Ŀ֮����� 5Ԥ����׷�ӣ��ӿ�Ŀδ�������з�������ţ� 6���� 7Ԥ����׷�ӣ��Ӳ���Ԥ�������׷�ӣ� 8 �¿���У���
    [list_id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	 ---�¼��ֶ�
	[fujian] [varchar](50) NULL,
	 sm nvarchar(50) null--˵��
	
	) 

--Ԥ����ϸ˵������ 
CREATE TABLE [dbo].[bill_ysmxb_smfj](
	[billCode] [varchar](50) NULL,--���
	[yskm] [varchar](20) NULL,--Ԥ���Ŀ
	[sm] [text] NULL,--˵��
	[fj] [varchar](2000) NULL--����
) 

--Ԥ����� bill_ysgc
CREATE TABLE [dbo].[bill_ysgc](
	[gcbh] [varchar](50) NULL,--���
	[xmmc] [varchar](50) NULL,--����
	[kssj] [datetime] NULL,--��ʼʱ��
	[jzsj] [datetime] NULL,--��ֹʱ��
	[status] [varchar](1) NULL,--״̬��0��δ��ʼ��1���ѿ�ʼ��2���ѽ�����
	[fqr] [varchar](50) NULL,--������
	[fqsj] [varchar](50) NULL,--����ʱ��
	[nian] [varchar](4) NULL,--���
	[yue] [varchar](2) NULL,--��
	[ysType] [varchar](6) NULL--Ԥ�����ͣ�0�����Ԥ�㣬1������Ԥ�㣬2���¶�Ԥ�㣩
) 
--Ԥ����� bill_ysbl 
CREATE TABLE [dbo].[bill_ysbl](
	[yf] [varchar](50) NULL,--�·�
	[type1] [varchar](50) NULL,
	[type2] [varchar](50) NULL,
	[listid] [decimal](18, 0) IDENTITY(1,1) NOT NULL,--��� ������
	[type3] [varchar](50) NULL,
	[bl] [decimal](18, 6) NULL,--����
 CONSTRAINT [PK_bill_ysbl] PRIMARY KEY CLUSTERED 
(
	[listid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


--��Ŀ�ֽ������
CREATE TABLE [dbo].[bill_ys_xmfjlrb](
	[procode] [varchar](20)  NOT NULL,   --��Ŀ���
	[kmcode] [varchar](20)  NULL,   --��Ŀ���
	[budgetmoney] [money] NULL,  --���
	[annual] [varchar](4)  NOT NULL,  --���
	[by1] [varchar](30)  NULL,  --����1
	[by2] [varchar](30)  NULL,   --����2
	[by3] [varchar](30)  NULL --����3
)
--���ſ�Ŀ�ֽ�
CREATE TABLE [dbo].[bill_ys_xmfjbm](
	[procode] [varchar](30) NULL,    --��Ŀ��ţ����ں����䶯  ���ڱ�ʾ��ȣ�
	[deptcode] [varchar](30)  NOT NULL,  --�����ű�ţ�
	[kmcode] [varchar](30)  NULL,  --��Ŀ���
	[je] [money] NULL,  --���
	[by1] [money] NULL,  --����ִ����� ��ʾ���ŶԷ����������ִ�Ľ�
	[by2] [varchar](30)  NULL,--����ִ�������ʾ���ŶԷ����������ִ�������
	[by3] [varchar](30) NULL) --��ȷ�ϱ��״̬ 1Ԥ��ȷ�� 2 ����ȷ�� 3 ��������


--Ч����Ŀ��Ӧ��
create table bill_ys_benefits_yskm(
procode varchar(20) not null,--Ч����Ŀ���
yskmCode varchar(20) not null,--��Ŀ���
deptcode varchar(20) null, --���ű��
--�¼��ֶ�
yslb varchar(50) null --Ԥ�����
)

--���Ч��Ԥ���
create table bill_ys_benefits_budget(
procode varchar(20) not null,--Ч����Ŀ���
annual varchar(4) not null,--���
kmcode varchar(20) null,--��Ŀ���
budgetmoney money null,--Ԥ����
adduser varchar(10) null,--Ԥ����
adddate datetime null,--Ԥ��ʱ��
modifyuser varchar(10) null,--�޸���
modifydate datetime null--�޸�ʱ��
)

--Ч����Ŀ������
create table bill_ys_benefitpro(
annual varchar(4) not null,--���
procode varchar(20) not null,--Ч����Ŀ���
proname varchar(60) not null,--Ч����Ŀ����
calculatype varchar(20) null,--���㷽ʽ���ӡ����������㣩
fillintype varchar(20) null,--��д��ʽ��ֱ��¼�롢��ϸ���ܣ�
sortcode varchar(4) null,--�����
status varchar(2) null,--�Ƿ����
adduser varchar(10) null,--������
adddate datetime null,--����ʱ��
modifyuser varchar(10) null,--�޸���
modifydate datetime null--�޸�ʱ��
--�¼��ֶ�
yslb varchar(50) null --Ԥ�����

)


--Ԥ���Ŀ bill_yskm
CREATE TABLE [dbo].[bill_yskm](
	[yskmCode] [varchar](20) NULL,--��Ŀ����
	[yskmBm] [varchar](50) NULL,--��Ŀ���
	[yskmMc] [varchar](50) NULL,--��Ŀ����
	[gjfs] [varchar](1) NULL,--
	[tbsm] [text] NULL,--�˵��
	[tblx] [varchar](20) NULL,--����ͣ�01����λ���02���������03���۷��ã�
	[kmStatus] [varchar](1) NULL,--��Ŀ״̬��0�����ã�1�����ã�
	[kmlx] varchar(2) null,--��Ŀ����
	gkfy varchar(1) null,--��ڷ���
	xmhs varchar(1) null,--��Ŀ����
	bmhs varchar(1) null,--���ź���
	ryhs varchar(1),--��Ա����
    kmzg nvarchar(100) null, --��Ŀ����
	zjhs varchar(50) null,--����Ǽ��� 1���� 0����
	dydj varchar(50) null,--��Ӧ����  ��Ӧ�����ֵ����dictypeΪ18��diccode
) 

--Ԥ�������
CREATE TABLE [dbo].[bill_ystz](
	[billCode] [varchar](50) NULL,
	[billDept] [varchar](50) NULL,
	[sCode] [varchar](20) NULL,
	[tCode] [varchar](20) NULL
) 

--Ԥ�����ǰ��������
CREATE TABLE [dbo].[bill_ystz_after](
	[billCode] [varchar](50) NULL,
	[billDept] [varchar](50) NULL,
	[ysgc] [varchar](20) NULL,
	[km] [varchar](20) NULL,
	[sJe] [float] NULL,
	[tJe] [float] NULL
) 


--Ԥ������󣨷�����
CREATE TABLE [dbo].[bill_ystz_before](
	[billCode] [varchar](50) NULL,
	[billDept] [varchar](50) NULL,
	[ysgc] [varchar](20) NULL,
	[km] [varchar](20) NULL,
	[sJe] [float] NULL,
	[tJe] [float] NULL
) 

--Ԥ�㸽��
CREATE TABLE [dbo].[bill_ysfj](
	[deptcode] [varchar](50) NULL,
	[nd] [varchar](50) NULL,
	[fujian] [varchar](500) NULL,
	[filename] [varchar](500) NULL,
	[xmbh] [varchar](500) NULL
)
--******************************Ԥ��ģ��end************************************


--******************************�������beg************************************
--�����Ŀ bill_cwkm
CREATE TABLE [dbo].[bill_cwkm](
	[cwkmCode] [varchar](20) NULL,--��Ŀ���
	[cwkmBm] [varchar](50) NULL,--����
	[cwkmMc] [varchar](50) NULL,--��ʾ����
	[hsxm1] [varchar](200) NULL,--������Ŀ1
	[hsxm2] [varchar](200) NULL,
	[hsxm3] [varchar](200) NULL,
	[hsxm4] [varchar](200) NULL,
	[hsxm5] [varchar](200) NULL,
	XianShiMc varchar(200) null,--��ʾ����(�ֶ��ظ� ����)
	[Type]  varchar(50) null,--����
	Fangxiang varchar(50) null,--����
	JiCi varchar(50) null,--����
	FuZhuHeSuan varchar(200) null,--��������
	ShiFouFengCun varchar(200) null,--�Ƿ���
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

--NC���źͱ�ϵͳ���Ŷ�Ӧ��bill_pingzheng_bumenduiying
CREATE TABLE [dbo].[bill_pingzheng_bumenduiying](
	[OSDeptCode] [varchar](50) NOT NULL,--��ϵͳ���ű��
	[OSDeptName] [nvarchar](100) NULL,--��ϵͳ��������
	[ParentCode] [varchar](50) NULL,
	[ParentName] [nvarchar](100) NULL,--��ӦNCϵͳ������
	[Note1] [nchar](10) NULL,--��ϵͳ������
	[Note2] [nchar](10) NULL,
	[Note3] [nchar](10) NULL,
	[Note4] [nchar](10) NULL,
	[Note5] [nchar](10) NULL,
	[Note6] [nchar](10) NULL
) ON [PRIMARY]

--ƾ֤��Ŀbill_pingzheng_xm
CREATE TABLE [dbo].[bill_pingzheng_xm](
	[list_id] [int] IDENTITY(1,1) NOT NULL,--��¼id������
	[xmcode] [nvarchar](50) NULL,--���ͱ���
	[xmname] [nvarchar](50) NULL,--��������
	[parentcode] [nvarchar](50) NULL,--�ϼ����
	[parentname] [nvarchar](50) NULL,--�ϼ�����
	[isdefault] [nvarchar](50) NULL,--�Ƿ�Ĭ��
	[status] [nvarchar](50) NULL,--״̬
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
--******************************�������end************************************


--******************************����ģ��beg************************************
-- һ�㱨�� �������� bill_ybbx_fysq
CREATE TABLE [dbo].[bill_ybbx_fysq](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[sqCode] [varchar](50) NULL,--������
	[status] [varchar](1) NULL--״̬
) ON [PRIMARY]


--һ�㱨����ϸ��
CREATE TABLE [dbo].[bill_ybbxmxb](
	[billCode] [varchar](50) NOT NULL,--���ݱ��
	[bxr] [varchar](50) NULL,--������
	[bxzy] [varchar](2000) NULL,--����ժҪ
	[bxsm] [varchar](2000) NULL,--����˵��
	[sfdk] [varchar](1) NULL,--�Ƿ�ֿ�
	[ytje] [float] NULL,--Ӧ�˽��
	[ybje] [float] NULL,---Ӧ�����
	[sfgf] [varchar](1) NULL,--�Ƿ����
	[bxmxlx] [varchar](50) NULL,--������ϸ����
	[gfr] [varchar](50) NULL,--������
	[gfsj] [datetime] NULL,--����ʱ��
	[cxsj] [datetime] NULL,--����ʱ��
	[cxr] [varchar](50) NULL,--������
	[cxyy] [varchar](4000) NULL,--����ԭ��
	[se] decimal(18,4) null,--˰��
	bxdjs int null,--����������
	pzcode varchar(50) null,--ƾ֤���
	pzdate datetime null,--ƾ֤����
	pzbldate datetime null,--ƾ֤��¼����
	guazhang varchar(50) null,--�Ƿ����
	zhangtao varchar(50) null,--����
	bxr2 varchar(200) null,--��ڱ�������ʱ��  �����汨����/������λ
	bxrzh varchar(200) null,--�������˺� ��ʽΪ ������|&|����
	bxrphone varchar(200) null--�����˵绰
	pzbldate datetime null--ƾ֤��¼����
	fujian varchar(2000) null,--����  ������|������ַ
	ykfs varchar(80) null---�ÿʽ
) ON [PRIMARY]
--һ�㱨����ϸ�� �������� bill_ybbxmxb_bxdj 
CREATE TABLE [dbo].[bill_ybbxmxb_bxdj](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[djUrl] [varchar](200) NULL,--����url
	[djName] [varchar](50) NULL,--��������
	[djGuid] [varchar](50) NULL,--���ݱ��
	[djStatus] [varchar](1) NULL--״̬
) ON [PRIMARY]

--һ�㱨����ϸ�� �ֿ� bill_ybbxmxb_fydk 
CREATE TABLE [dbo].[bill_ybbxmxb_fydk](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[jkmxCode] [varchar](50) NULL,--�����ϸ���
	[status] [varchar](2) NULL,--״̬
) ON [PRIMARY]

--һ�㱨��ϸ������Ŀ bill_ybbxmxb_fykm
CREATE TABLE [dbo].[bill_ybbxmxb_fykm](
	[billCode] [varchar](50) NULL,--���ݱ��
	[fykm] [varchar](50) NULL,--���ÿ�Ŀ
	[je] [float] NULL,--���
	[mxGuid] [varchar](50) NULL,
	[status] [varchar](2) NULL,--״̬ 0 Ĭ�� 1δ֪  2�Ѿ���������ļ�¼
	[ms] [varchar](4000) NULL
) ON [PRIMARY]


-- һ�㱨����ϸ�� ���� bill_ybbxmxb_fykm_dept qq
CREATE TABLE [dbo].[bill_ybbxmxb_fykm_dept](
	[kmmxGuid] [varchar](50) NULL,--һ�㱨����Ŀ��ϸ���
	[mxGuid] [varchar](50) NULL,--һ�㱨����ϸ���ű��
	[deptCode] [varchar](50) NULL,--���ű��
	[je] [float] NULL,--���
	[status] [varchar](1) NULL--״̬
) ON [PRIMARY]

--һ�㱨����ϸ�� ��Ŀ ���÷�̯ bill_ybbxmxb_fykm_ft  qq
CREATE TABLE [dbo].[bill_ybbxmxb_fykm_ft](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[kmmxGuid] [varchar](50) NULL,--һ�㱨����Ŀ��ϸ���
	[cbzx] [varchar](50) NULL,--��Ӧbill_cbzx��[zxCode]
	[je] [float] NULL,--���
	[ftmxGuid] [varchar](50) NULL,--��̯��ϸ���
	[status] [varchar](2) NULL--״̬
) ON [PRIMARY]


--һ�㱨����ϸ�� ������Ŀ bill_ybbxmxb_hsxm qq
CREATE TABLE [dbo].[bill_ybbxmxb_hsxm](
	[kmmxGuid] [varchar](50) NULL,--һ�㱨����Ŀ��ϸ���
	[mxGuid] [varchar](50) NULL,--������Ŀ���
	[xmCode] [varchar](50) NULL,--��Ŀ���
	[je] [float] NULL--���
) ON [PRIMARY]


--����浥
CREATE TABLE [dbo].[bill_travelReport](
	[MainCode] [varchar](50) NOT NULL,--��Ӧ���ݱ��
	[TravelProcess] [text] NULL,--����˵��
	[WorkProcess] [text] NULL,--��������
	[Result] [text] NULL,--���
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
--�������
CREATE TABLE [dbo].[bill_travelApplication](
	[maincode] [varchar](50) NOT NULL,--��Ӧ���ݱ��
	[typecode] [varchar](50) NULL,
	[travelPersionCode] [varchar](50) NULL,--�Ƶ���
	[arrdess] [varchar](500) NULL,--����ص�
	[travelDate] [varchar](50) NULL,--Ԥ�Ƴ���ʱ��
	[reasion] [varchar](max) NULL,--��������
	[travelplan] [varchar](max) NULL,--�����ճ̼���������
	[needAmount] [int] NULL,--�����ܽ��
	[Transport] [varchar](50) NULL,--���뽻ͨ����
	[MoreThanStandard] [int] NULL,--�Ƿ񳬹��涨��׼
	[ReportCode] [varchar](50) NULL,--���
	[jiaotongfei] [int] NULL,--��ͨ��
	[zhusufei] [int] NULL,--ס�޷�
	[yewuzhaodaifei] [int] NULL,--ҵ���д���
	[huiyifei] [int] NULL,--�����
	[yinshuafei] [int] NULL,--ӡˢ��
	[qitafei] [int] NULL,--��������
	[sendDept] [varchar](50) NULL--����
) ON [PRIMARY]



--�������븽���� bill_fysq_fjb
CREATE TABLE [dbo].[bill_fysq_fjb](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[fjGuid] [varchar](50) NULL,--����Ψһ���
	[fjUrl] [varchar](200) NULL,--������ַ
	[fjName] [varchar](50) NULL,--��������
	[djStatus] [varchar](1) NULL--״̬
) ON [PRIMARY]
 
--����������ϸ�� bill_fysq_mxb 
CREATE TABLE [dbo].[bill_fysq_mxb](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[mxname] [varchar](50) NULL,--����
	[je] [float] NULL,--���
	[mxGuid] [varchar](50) NULL
) ON [PRIMARY]


--��������
CREATE TABLE [dbo].[bill_main](
	[billCode] [varchar](50) NULL,--Ψһ�ű��
	[billName] [varchar](50) NULL,--���ݱ��
	[flowID] [varchar](50) NULL,--��������(ys,Ԥ�㣬yszj��Ԥ��׷�ӣ�xmys����ĿԤ�㡣����)
	[stepID] [varchar](50) NULL,--��˲���
	[billUser] [varchar](50) NULL,--��Ա
	[billDate] [datetime] NULL,--�ÿ�����
	[billDept] [varchar](50) NULL,--����
	[billJe] [float] NULL,--���  
	[loopTimes] [int] NULL,--ѭ������
	[billType] [varchar](1) NULL,--�������� һ������ Ԥ�㡢Ԥ�������Ԥ��׷�ӵĵ��Ӷ���bill_ybbxmxb��billtype���Ӧ  Ϊ�˽��ֹ�����Ԥ��׷�ӣ���Ԥ��ʱ ��¼�������� 
	[billName2] [varchar](50) NULL,---׷��˵��    ���������Ԥ�� �����洢����
	[isgk] [varchar](1) NULL,--�Ƿ���
	[gkdept] [varchar](50) NULL,--��ڲ���
	
	--�¼��ֶ�
	[dydj] [varchar](50) NULL,--��Ӧ���� Ԥ������  ����Ӧ�����ֵ���dictype=18 �е�ֵ��
	[note1] text NULL,--���ݱ�������
	[note2] text NULL,--��������
	[note3] [varchar](50) NULL,--�����flowid='xmys�� ��ĿԤ��� ����Ŀ���  �������ĿԤ����� Ҳ�Ǵ���Ŀ���
	[note4] [varchar](50) NULL,---�������� �Ƿ����²���
	[note5] [varchar](50) NULL��--
) 
--Ԥ֧��/����  yzsq/jksq
CREATE TABLE [dbo].[T_LoanList](
	[Listid] [nvarchar](50) NOT NULL,--������
	[LoanCode] [nvarchar](50) NULL,--�����˱��
	[LoanDeptCode] [nvarchar](50) NULL,--���첿��
	[LoanDate] [nvarchar](50) NULL,--�������
	[LoanSystime] [nvarchar](50) NULL,--ϵͳʱ��
	[LoanMoney] [decimal](18, 3) NULL,--���
	[LoanExplain] [nvarchar](500) NULL,--˵��
	[Status] [char](1) NULL,--���״̬ 1�����   2������
	[SettleType] [char](1) NULL,
	[CJCode] [nvarchar](500) NULL,
	[ResponsibleCode] [nvarchar](50) NULL,--�����
	[ResponsibleDate] [nvarchar](50) NULL,
	[ResponsibleSysTime] [nvarchar](50) NULL,
	[NOTE1] [nvarchar](50) NULL,
	[NOTE2] [nvarchar](500) NULL,
	[NOTE3] [nvarchar](50) NULL,--�ѻ�����
	[NOTE4] [nvarchar](50) NULL,
	[NOTE5] [nvarchar](500) NULL,
	[NOTE6] [nvarchar](50) NULL,
	[NOTE7] [nvarchar](50) NULL,--���ӵ���
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

--�����¼��
create table T_ReturnNote(
	listid int identity(1,1) not null,
	ltype int not null,--1  ���������   2����
	loancode varchar(50) not null,--���Ż�Ԥ֧����
	billcode varchar(50) null,--��������/�򻹿��
	ldate varchar(50) not null,
	je decimal(18,3) not null,
	usercode varchar(50) not null,--������
	
	note1 varchar(50) null,--�Ƿ�����ͨ�� 0 δ��� 1 �����
	note2 varchar(50) null,--��ע
	note3 varchar(50) null,--ƾ֤��
	note4 varchar(50) null,--billcode ��Ӧ�ĳ���ı������ı��  ����Ǵ�ƾ֤��������� ���¼��Ӧ�ı�����������
	note5 varchar(50) null,
)



--******************************����ģ��end************************************

--******************************�ɹ�ģ��beg************************************

--�ɹ�����������
CREATE TABLE [dbo].[bill_cgsp](
	[cgbh] [varchar](50) NULL,--���
	[sj] [datetime] NULL,--ʱ��
	[cgdept] [varchar](50) NULL,--����
	[cglb] [varchar](50) NULL,--���01��һ�㣬02��������
	[cbr] [varchar](50) NULL,--�а���
	[cgze] [float] NULL,--�ɹ��ܶ�
	[sm] [text] NULL,--˵��
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


--�ɹ�������ϸ��
CREATE TABLE [dbo].[bill_cgsp_mxb](
	[cgbh] [varchar](50) NULL,--�ɹ����
	[mc] [text] NULL,--����
	[gg] [text] NULL,--���
	[sl] [float] NULL,--����
	[dj] [float] NULL,--����
	[zj] [float] NULL,--�ܼ�
	[bz] [text] NULL,--��ע
	[cgbhGuid] [varchar](50) NULL,----�ɹ����
	[cgIndex] [int] NOT NULL--��ʶ���
) 




--�ɹ�������--������
CREATE TABLE [dbo].[bill_cgsp_fjb](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[djUrl] [varchar](200) NULL,--·��
	[djName] [varchar](50) NULL,--����
	[djGuid] [varchar](50) NULL,
	[djStatus] [varchar](1) NULL--״̬
)

--�ɹ�������--ѯ�۱�
CREATE TABLE [dbo].[bill_cgsp_xjb](
	[cgbh] [varchar](50) NULL,--��Ӧ���ݺ�
	[xjdw] [text] NULL,--��λ
	[xjqk] [text] NULL--���
) 

--������ �ɹ����� ��Ӧ���������
CREATE TABLE [dbo].[bill_cgsp_lscg](
	[billCode] [varchar](50) NULL,--������
	[lscgCode] [varchar](50) NULL,--������
	[status] [varchar](1) NULL--״̬
) ON [PRIMARY]




--��ʱ�ɹ����뵥
CREATE TABLE [dbo].[bill_lscg](
	[cgbh] [varchar](20) NULL,--���
	[sj] [datetime] NULL,--ʱ��
	[cgDept] [varchar](500) NULL,--����
	[cglb] [varchar](50) NULL,--���01��һ�㣬02��������
	[zynr] [text] NULL,--����
	[sm] [text] NULL,--˵��
	[cbr] [varchar](50) NULL,--�а���
	[ms] [text] NULL,--
	[spyj01] [text] NULL,
	[spyj02] [text] NULL,
	[spyj03] [text] NULL,
	[spyj04] [text] NULL,
	[spyj05] [text] NULL,
	[spyj06] [text] NULL,
	[spyj07] [text] NULL,
	[spyj08] [text] NULL,
	[yjfy] [float] NULL,--���
	fjName varchar(200) null,
	fjUrl varchar(300) null
) 

-- �ɹ��ʽ𸶿�
CREATE TABLE [dbo].[bill_cgzjfk](
	[billcode] [varchar](50)  NULL, --��Ӧ����bill_main��code
	[gysbh] [varchar](50)  NULL,--��Ӧ�̱��
	[gysmc] [varchar](100)  NULL,--��Ӧ������
	[jhje] [float] NULL,--�ƻ����
	[fkje] [float] NULL,--������
	[bz] [varchar](100)  NULL,--��ע
	[jhindex] [decimal](18, 0) IDENTITY(1,1) NOT NULL,--���
	[cdefine1] [varchar](20)  NULL,
	[cdefine2] [varchar](50)  NULL,
	[idefine3] [float] NULL,
	[idefine4] [float] NULL,
	[ddefine5] [datetime] NULL
) ON [PRIMARY]

-- �ɹ��ʽ�ƻ�
CREATE TABLE [dbo].[bill_cgzjjh](
	[cgbh] [varchar](50) NULL,--�ɹ����
	[gysbh] [varchar](50) NULL,--��Ӧ�̱��
	[gysmc] [varchar](100) NULL,--��Ӧ������
	[syrkje] [float] NULL,
	[byjhje] [float] NULL,
	[byfkje] [float] NULL,
	[fkje] [float] NULL,--������
	[bz] [varchar](100) NULL,--��ע
	[jhindex] [decimal](18, 0) IDENTITY(1,1) NOT NULL,--��Ҫ
	[cdefine1] [varchar](20) NULL,
	[cdefine2] [varchar](50) NULL,
	[idefine3] [float] NULL,
	[idefine4] [float] NULL,
	[ddefine5] [datetime] NULL
) ON [PRIMARY]

--��ڷֽ����
create table bill_gkfjbili(
	nian nvarchar(4) not null,--���
	gkdeptcode nvarchar(50) not null,--��ڲ��ű��
	yskmcode nvarchar(50) not null,--Ԥ���Ŀ���
	fjdeptcode nvarchar(50) not null,--�ֽⲿ�ű��
	fjbl decimal(18,6) not null,
	note1 nvarchar(50) null,
	note2 nvarchar(50) null,
	note3 nvarchar(50) null,
	note4 nvarchar(50) null,
	note5 nvarchar(50) null
)

--��ʱ�ɹ����뵥
CREATE TABLE [dbo].[bill_lscg](
	[cgbh] [varchar](20) NULL,--���
	[sj] [datetime] NULL,--ʱ��
	[cgDept] [varchar](500) NULL,--����
	[cglb] [varchar](50) NULL,--���01��һ�㣬02��������
	[zynr] [text] NULL,--����
	[sm] [text] NULL,--˵��
	[cbr] [varchar](50) NULL,--�а���
	[ms] [text] NULL,--
	[spyj01] [text] NULL,
	[spyj02] [text] NULL,
	[spyj03] [text] NULL,
	[spyj04] [text] NULL,
	[spyj05] [text] NULL,
	[spyj06] [text] NULL,
	[spyj07] [text] NULL,
	[spyj08] [text] NULL,
	[yjfy] [float] NULL--���
) 
select 1467-575

--�ÿ����뵥�ͱ��������ձ�
create table dz_yksq_bxd 
(
	yksq_code nvarchar(50) not null,
	bxd_code nvarchar(50) not null,
	note1 nvarchar(50) null,--�������� yszj��Ԥ��׷�ӣ�xmys����ĿԤ�㡣����
	note2 nvarchar(50) null,--�س�Ԥ����Ϣ
	note3 nvarchar(50) null,
	note4 nvarchar(50) null,
	note5 nvarchar(50) null
)

--�������� �������� bill_qtbx_fysq 
CREATE TABLE [dbo].[bill_qtbx_fysq](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[sqCode] [varchar](50) NULL,--������
	[status] [varchar](1) NULL--״̬
) ON [PRIMARY]

--����������ϸ�� bill_qtbxmxb 
CREATE TABLE [dbo].[bill_qtbxmxb](
	[billCode] [varchar](50) NOT NULL,--��Ӧ���ݱ��
	[bxr] [varchar](50) NULL,--������
	[bxzy] [varchar](2000) NULL,--����ժҪ
	[bxsm] [varchar](2000) NULL,--����˵��
	[sfdk] [varchar](1) NULL,--�Ƿ�ʹ�ý��ָ�
	[ytje] [float] NULL,--Ӧ�˽��
	[ybje] [float] NULL,---Ӧ�����
	[sfgf] [varchar](1) NULL,--�Ƿ����
	[bxmxlx] [varchar](50) NULL,--������ϸ����
	[gfr] [varchar](50) NULL,--������
	[gfsj] [datetime] NULL,--����ʱ��
	[cxsj] [datetime] NULL,--����ʱ��
	[cxr] [varchar](50) NULL,--������
	[cxyy] [text] NULL--����ԭ��
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

--����������ϸ�� �������� bill_qtbxmxb_bxdj 
CREATE TABLE [dbo].[bill_qtbxmxb_bxdj](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[djUrl] [varchar](200) NULL,--���ݵ�ַ
	[djName] [varchar](50) NULL,--������
	[djGuid] [varchar](50) NULL,--���۵����
	[djStatus] [varchar](1) NULL--״̬
) ON [PRIMARY]

--����������ϸ�� ���õ��� bill_qtbxmxb_fydk 
CREATE TABLE [dbo].[bill_qtbxmxb_fydk](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[jkmxCode] [varchar](50) NULL,--�����ϸ���
	[status] [varchar](2) NULL,--״̬
	[dkGuid] [varchar](50) NULL --������
) ON [PRIMARY]

--����������ϸ�� ���ÿ�Ŀ bill_qtbxmxb_fykm 
CREATE TABLE [dbo].[bill_qtbxmxb_fykm](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[fykm] [varchar](50) NULL,--���ÿ�Ŀ
	[je] [float] NULL,--���
	[mxGuid] [varchar](50) NULL,--���ݱ��
	[status] [varchar](2) NULL,--״̬
	[ms] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

--����������ϸ�� ���ÿ�Ŀ ��λbill_qtbxmxb_fykm_dept 
CREATE TABLE [dbo].[bill_qtbxmxb_fykm_dept](
	[kmmxGuid] [varchar](50) NULL,--��Ŀ��ϸ���
	[mxGuid] [varchar](50) NULL,--��ϸ���
	[deptCode] [varchar](50) NULL,--���ű��
	[je] [float] NULL,--���
	[status] [varchar](1) NULL--״̬
) ON [PRIMARY]

--����������ϸ�� ���ÿ�Ŀ ��̯ bill_qtbxmxb_fykm_ft
CREATE TABLE [dbo].[bill_ybbxmxb_fykm_ft](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[kmmxGuid] [varchar](50) NULL,--������ϸ���ÿ�Ŀ��ϸ���
	[cbzx] [varchar](50) NULL,--��Ӧbill_cbzx��[zxCode]
	[je] [float] NULL,--���
	[ftmxGuid] [varchar](50) NULL,--��̯��ϸ���
	[status] [varchar](2) NULL--״̬
) ON [PRIMARY]

--����������ϸ�� ������Ŀbill_qtbxmxb_hsxm 
CREATE TABLE [dbo].[bill_qtbxmxb_hsxm](
	[kmmxGuid] [varchar](50) NULL,--������ϸ���ÿ�Ŀ��ϸ���
	[mxGuid] [varchar](50) NULL,--������
	[xmCode] [varchar](50) NULL,--������Ŀ���
	[je] [float] NULL--���
) ON [PRIMARY]

--******************************�ɹ�ģ��end************************************



--******************************����ģ��beg************************************
--������¼����
create table workflowrecord(
	recordid decimal(18,0) IDENTITY(1,1),--��¼���(�������ĵ��ݳ��д˺�)
	billCode varchar(50) not null,--���ݱ��
	billType varchar(30) not null,--��������(��ʲô����,����)
	flowId varchar(30) not null,--���������
	isEdit int not null,--�Ƿ񾭹��޸�,0û��,1��
	rdState int not null--״̬(0,�ȴ�;1,����ִ��;2,ͨ��;3,����δͨ��)
)

--������¼�ӱ�
create table workflowrecords(
	recordid decimal(18,0) not null,--�����Ӧ���
	flowid varchar(30) null,--���̱��
	stepid varchar(30) null,--������
	steptext varchar(30) null,--��������
	recordtype varchar(30) null,--����(һ�����ŵ��˶����󣬻���ֻ���Ǹ���))
	checkuser varchar(30) null,--������(Ӧ����������,����)
	finaluser varchar(30) null,--����������(������������)
	rdstate int null,--״̬(0,�ȴ�;1,����ִ��;2,ͨ��;3,����)
	mind varchar(30) null,--�������
	checkdate datetime null--����ʱ��
	checktype varchar(30) null --�������� 1=��ǩ 2=��ǩ
)
--���������workflowstep
CREATE TABLE [dbo].[workflowstep](
	[stepid] [int] NOT NULL,--���
	[flowid] [varchar](30) NOT NULL,--��������
	[steptype] [varchar](30) NULL,
	[steptext] [varchar](50) NULL,
	[checkcode] [varchar](100) NULL,
	[minmoney] [decimal](18, 4) NULL,
	[maxmoney] [decimal](18, 4) NULL,
	[mindate] [datetime] NULL,
	[maxdate] [datetime] NULL,
	[memo] [varchar](50) NULL,--���ű��
	[checktype] [varchar](30) NULL,--�������� 1=��ǩ 2=��ǩ
	[filterkemuManager] [int] NULL,--�Ƿ��ǿ�Ŀ����
	kmType varchar(50) null,--��Ŀ
 CONSTRAINT [PK_workflowstep] PRIMARY KEY CLUSTERED 
(
	[stepid] ASC,
	[flowid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
--�������ͱ�
create table mainworkflow(
	flowid varchar(30)  primary key,--��ţ���billmain��ĵ��������ֶΣ�
	flowName varchar(50) null--����
)

--��������Ӧ��Ŀǰ��һ��һ��
create table billtoworkflow(
	billtype varchar(30) primary key,	--���
	billname varchar(30) null,--����
	flowid varchar(30) null--flowid����billmain��ĵ��������ֶΣ�
)

--���̱� bill_workFlow 
CREATE TABLE [dbo].[bill_workFlow](
	[flowId] [varchar](50) NULL,���
	[flowText] [varchar](50) NULL,--������������
	[stepTextColor] [varchar](50) NULL,--������ɫ
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


--���̶�����  bill_workFlowAction qq
CREATE TABLE [dbo].[bill_workFlowAction](
	[flowID] [varchar](50) NULL,--��������
	[actionID] [varchar](50) NULL,--�������
	[actionText] [varchar](50) NULL,--��������
	[actionType] [varchar](50) NULL,--��������
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
	[flowID] [varchar](50) NULL,--��������
	[stepID] [varchar](50) NULL,--����id
	[wkGroup] [varchar](50) NULL,--������
	[flowMode] [varchar](50) NULL,--����ģʽ
	[wkUser] [varchar](50) NULL,--������
	[wkModel] [varchar](50) NULL--��������������
) ON [PRIMARY]

--��˼�¼��  bill_workFlowRecord  qq
CREATE TABLE [dbo].[bill_workFlowRecord](
	[billCode] [varchar](50) NULL,--��Ӧbill_main���ݱ��
	[flowID] [varchar](50) NULL,--��������
	[beginStep] [varchar](50) NULL,
	[endStep] [varchar](50) NULL,
	[checkUser] [varchar](50) NULL,--����û�
	[checkDate] [datetime] NULL,--���ʱ��
	[checkBz] [text] NULL,--��˱�ע
	[loopTimes] [int] NULL,--ѭ������
	[checkGroup] [varchar](50) NULL,
	[result] [varchar](50) NULL,--��ǰ����
	[stepUser] [varchar](50) NULL,
	[wkModel] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]



--���̲���  bill_workFlowStep  qq
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

--�������ͱ���һ�㱨������Ԥ�㵥���������浥������
create table bill_djlx(
djbh varchar(20) null,--�������ͱ��
djmc varchar(50) null,--��������
djbm varchar(50) null,--���ݱ���
splx varchar(20) null,--��������
bhbj varchar(20) null,--���ݱ�ű�ͷ���
bhlslx varchar(2) null,--���ݱ�ŵ����ͣ�nΪ������ˮ��mΪ������ˮ��dΪ������ˮ��
bhlscd int null,--�����ˮ����
djtype varchar(2) null --�Ƿ����ã�0 ���ã�1 ���ã�
)


--******************************����ģ��end************************************

--******************************ͳ�Ʋ�ѯbeg************************************
--�½�bill_yj
CREATE TABLE [dbo].[bill_yj](
	[yf] [varchar](10) NULL,
	[userCode] [varchar](30) NULL,
	[cguid] [varchar](50) NULL,
	[yjsj] [datetime] NULL,
	[yjbj] [varchar](1) NULL
) ON [PRIMARY]
--������ϸ��
CREATE TABLE [dbo].[bill_srmxb](
	[gcbh] [varchar](50) NOT NULL,--��ڱ��
	[billCode] [varchar](50) NOT NULL,--��Ӧ���ݱ��
	[yskm] [varchar](50)  NOT NULL,--Ԥ���Ŀ
	[ysje] [float] NULL,--���
	[ysDept] [varchar](50)  NOT NULL,--����
	[ysType] [varchar](1)  NULL,
	[list_id] [decimal](18, 0) IDENTITY(1,1) NOT NULL,--��¼id ������
 CONSTRAINT [PK_bill_srmxb_1] PRIMARY KEY CLUSTERED 
(
	[list_id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

--Ԥ�㵼��
create table bill_ys_import(
  id nvarchar(50) null, --���
  deptcode varchar(50) null,--���ű��
  deptname varchar(50) null,--��������
  yskmcode varchar(50) null,--��Ŀ���
  yskmmc varchar(50) null,--��Ŀ����
  yi float null, --1��
  er float null,--2��
  san float null,--3��
  si float null,--4��
  wu float null,--5��
  liu float null,--6��
  qi float null,--7��
  ba float null,--8��
  jiu float null,--9��
  shi float null,--10��
  shiyi float null,--11��
  shier float null,--12��
  nian float null
)
--���뱨������ʱ��
create table lsbxd_main(
	billcode varchar(50) null,--����Ψһ����
	flowid nvarchar(10) null,--��������  ybbx/gkbx/qtbx
	billUser varchar(50) null,--�Ƶ���
	billDate varchar(50) null,--��������
	billDept varchar(50) null,--�Ƶ�����
	je decimal(18,4) null,--��Ŀ��� 
	se decimal(18,4) null,--��Ŀ˰��
	isgk	varchar(1) null,--�Ƿ���
	gkdept varchar(50) null,--��ڲ���
	bxzy varchar(50) null,--����ժҪ
	bxsm varchar(50) null,--����˵��
	fykmcode varchar(50) null,--���ÿ�Ŀ���
	sydept varchar(50) null,--ʹ�ò���
	bxlx varchar(50) null,--��������
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
--����ҽԺ ����ҩƷ���ⵥ���뱨���� ��Ӧ��
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
--******************************ͳ�Ʋ�ѯend************************************


--******************************����ģ��beg************************************

---��Ʊ���뵥
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
----������

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

----��׼��������
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
------�����Ͻɱ���
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

-----------�������뵥
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
-------���۷�����ϸ
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
-------���۷���������ϸ


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

---���۹���
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

-----���ⷵ�����뵥

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
-----���ⷵ������

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
-----��������
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


-----�������Ͷ�Ӧ

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


--******************************����ģ��end************************************



--******************************δ֪beg************************************
CREATE TABLE [dbo].[������$](
	[���ű��] [nvarchar](255) NULL,
	[��������] [nvarchar](255) NULL,
	[Ԥ���Ŀ���] [float] NULL,
	[Ԥ���Ŀ����] [nvarchar](255) NULL,
	[�跽��Ŀ���] [nvarchar](255) NULL,
	[�跽��Ŀ] [nvarchar](255) NULL,
	[������Ŀ���] [nvarchar](255) NULL,
	[������Ŀ] [nvarchar](255) NULL,
	[��ſ�Ŀ2] [nvarchar](255) NULL,
	[������Ŀ2] [nvarchar](255) NULL
) ON [PRIMARY]


CREATE TABLE [dbo].[sheet](
	[���] [nvarchar](50) NULL,
	[����] [nvarchar](50) NULL,
	[�������] [nvarchar](50) NULL,
	[��Ŀ] [nvarchar](50) NULL,
	[�¼�����] [nvarchar](50) NULL,
	[�������] [nvarchar](50) NULL,
	[�� 6] [nvarchar](50) NULL,
	[�� 7] [nvarchar](50) NULL,
	[�� 8] [nvarchar](50) NULL,
	[�� 9] [nvarchar](50) NULL,
	[�� 10] [nvarchar](50) NULL
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


--�������� bill_fysq 
CREATE TABLE [dbo].[bill_fysq](
	[billCode] [varchar](50) NULL,--��Ӧ���ݱ��
	[jbr] [varchar](50) NULL,
	[jkdjlx] [varchar](50) NULL,
	[sqzy] [text] NULL,
	[sqbz] [text] NULL,--���뱸ע
	[dwmc] [varchar](100) NULL,
	[khh] [varchar](100) NULL,
	[yhzh] [varchar](20) NULL,
	[sfjk] [varchar](1) NULL,
	[sfgf] [varchar](1) NULL,
	[sfth] [varchar](1) NULL,
	[hjje] [float] NULL--�ϼƽ��
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

--Ԥ���ת��
CREATE TABLE [dbo].[ysjz_temptb](
	[deptcode] [varchar](50) NULL,--���ű��
	[yskmcode] [varchar](50) NULL,--��Ŀ���
	[je] [decimal](18, 2) NULL,--���
	[frmysgc] [varchar](50) NULL,--����Ԥ����̳�
	[toysgc] [varchar](50) NULL,--����Ԥ�������
	[usercode] [varchar](50) NULL,--�û����
	[guidid] [varchar](50) NULL,--Ψһ��
	[hsdo] [varchar](1) NULL--�Ƿ�����ת
) ON [PRIMARY]


CREATE TABLE [dbo].[yssj](
	[bmbh] [nvarchar](255) NULL,
	[bmmc] [nvarchar](255) NULL,
	[fybh] [nvarchar](255) NULL,
	[fymc] [nvarchar](255) NULL,
	[je] [float] NULL
) ON [PRIMARY]


--������Ŀ��Ԥ���Ŀ��Ӧ
CREATE TABLE [dbo].[bill_gzxmdy](
	[yskmCode] [varchar](20) NOT NULL,
	[dyName] [varchar](50) NOT NULL,
	[status] [varchar](2) NULL,
	[note1] [nchar](10) NULL,
	[note2] [nchar](10) NULL
) ON [PRIMARY]


--���õ����� 2015-05-07
create table bill_lyd(
[guid] varchar(50) primary key  not null,--��������
lyDate  varchar(30) null,--����ʱ��
lyr varchar(50) null,  --������
zdr varchar(50) null,--�Ƶ���
lyDept varchar(50) null, --���ò���
je decimal(18,2)null,	--�ܽ��ӱ���ܣ�
sm varchar(1000) null,   --˵��
bz varchar(1000) null,	 --��ע
zt varchar(10) null,  --״̬Ĭ��Ϊδȷ��
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

--���õ��ӱ� 2015-05-07
create table bill_lyds(
guid varchar(50) not null,--��������
myGuid varchar(50) primary key not null,--��������
fykm varchar(50) null,--���ÿ�Ŀ
je decimal(18,2) null,--���
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


 insert into dbo.bill_sysMenu (menuid,menuName,showName,menuUrl,menuOrder)values ('0425','�ɹ�֧��','�ɹ�֧��','../bxgl/bxglFrame.aspx?dydj=cgzf','625')	
 insert into dbo.bill_sysMenu (menuid,menuName,showName,menuUrl,menuOrder)values ('0426','�ɹ�֧�����','�ɹ�֧�����','../MyWorkFlow/BillMainToApprove.aspx?flowid=cgzf','626')	
  insert into dbo.bill_sysMenu (menuid,menuName,showName,menuUrl,menuOrder)values ('0427','���õ�','���õ�','../bxgl/LingYongDanList.aspx','627')	
 
 
 insert into  bill_djlx (djbh,djmc,djbm,splx,bhbj,bhlslx,bhlscd,djtype) values ('cgzf','�ɹ�֧��','�ɹ�֧��',	'cgzf','cgzf','d','6','0')
insert into mainworkflow(flowid,flowName) values ('cgzf','�ɹ�֧��')
insert into bill_workFlow (flowId,flowText,stepTextColor,stepStrokeColor,stepShadowColor,stepFocusedStrokeColor,isStepShadow,actionStrokeColor,actionTextColor,actionFocusedStrokeColor,sStepTextColor,sStepStrokeColor,stepColor1,stepColor2,isStep3D,step3DDepth,orderBy)values
('cgzf','�ɹ�֧��','green','green','#b3b3b3','yellow','T','green','','yellow','red','	green','green','white','true','	20','2015-02-06 11:36:29')
insert into billtoworkflow (billtype,billname,flowid) values('cgzf','�ɹ�֧��','cgzf')

--******************************δ֪end************************************


--******************************�ֻ���begin************************************
--�ֻ��˱�������ʱ��
create table ph_main(
	[billCode] [varchar](50) NULL,--Ψһ�ű��
	[billName] [varchar](50) NULL,--���ݱ��
	[flowID] [varchar](50) NULL,--��������(ys,Ԥ�㣬yszj��Ԥ��׷�ӣ�xmys����ĿԤ�㡣����)
	[stepID] [varchar](50) NULL,--��˲���
	[billUser] [varchar](50) NULL,--�Ƶ���
	[bxr] varchar(50)null,--������
	[billDate] varchar(10) NULL,--�Ƶ�����
	[billDept] [varchar](50) NULL,--����
	[loopTimes] [int] NULL,--ѭ������
	[isgk] varchar(2) null,--�Ƿ���
	[gkdept] varchar(50) null,--��ڲ���
	[bxmxlx] [varchar](50) NULL,--������ϸ����
	[bxzy] varchar(200) null,--����ժҪ
	[bxsm] varchar(200) null--����˵��	
)

--�ֻ��˲˵���
create table  ph_sysmenu(
menuId varchar(20) not null, --�˵����
menuName varchar(50) null,--�˵�����
showName varchar(50) null,--��ʾ����
menuUrl varchar(200) null,--�˵�ҳ��
menuIcon varchar(200) null,--�˵�ͼ��
menuOrder varchar(200) null,--�����
IsCount varchar(2) null,--�Ƿ���ʾ��Ŀ��ʾ 1��ʾ 0����ʾ  Ĭ�ϲ���ʾ����Ϊ0
getCountSql varchar(1000) null,--��ȡ��ʾ���ֵ�sql��� ������@userCode ��ʾ 
sqlsm varchar(500) null,--sql˵�� ��sql����˵�� @userCode ��ǰ�û����  
menuSm varchar(200) null,--�˵�˵��
menuState varchar(10) null,--�˵�״̬ 1���� 0����  Ĭ����������Ϊ1
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

--�ֻ��˲˵�Ȩ�ޱ�
create table ph_menuRight
(
menuid varchar(20) not null, --�˵����
objecId varchar(20) not null, --��Ա���/��ɫ���
rightType varchar(10) not null, --��ɫ���� ��Ա���1 ��ɫ���3 
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



--==================================���ó�������===============================
---�¼Ӳ˵�  bill_sysMenu ��
0815	���ӷ�Ʊ����	���ӷ�Ʊ����	../fysq/Dzfplist.aspx	814	NULL	NULL
--�¼������� t_config��
HasFP	�Ƿ��е��ӷ�Ʊ��	1	0	û�д˼�¼��Ĭ��ֵΪ1  1�� 0 ��	NULL

---ƾ֤����sql
update t_Config set avalue='http://192.168.100.64:80/service/XChangeServlet?account=0001&receiver=' where akey='ToNcURL'
select * from dbo.bill_dataDic where dicType='10'
insert into bill_dataDic(dictype,diccode        ,dicname,cjys,cys,cdj) 
values ('10','CNHTC0209010102','�������ó���˾���۲�','1108','1','0');

--���ӷ�Ʊ����
create table bill_fpfj(
	fprq  varchar(50)  null,--��������
	billCode varchar(50) null,--���ݱ��
	deptCode varchar(50) NULL,--���ű��
	deptName varchar(50) null,--��������
	fpusercode varchar(50) null,--��Ʊ��Ա���
	fpusername varchar(50) null,--��Ʊ��Ա����
	bz varchar(100)null,--��ע
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
	billCode varchar(50) null,--���ݱ��
	fph varchar(50) null,--��Ʊ��
	fpdw varchar(50) null,--��Ʊ��λ
	fpje decimal(18,2) null,--��Ʊ���
	bz nvarchar(100) null,--��ע
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

--******************************�ֻ���end************************************



--******************************����ҽԺ���������***************************
--�ⲿϵͳ�����Ŀ�����Ԥ��ϵͳ��Ŷ�Ӧ��ϵ
create table sr_kmdy(
	dytype varchar(10) not null,--��Ӧ����  502Ԥ���� 503��Ժ����  504 סԺ����  505��������
	outcode varchar(50) not null,--�ⲿϵͳ��Ŀ���
	outname nvarchar(50) not null,--�ⲿϵͳ��Ŀ����
	yskmcode varchar(50)  null,--Ԥ���Ŀ���
	yskmname nvarchar(50)  null,--Ԥ���Ŀ����
	note1 nvarchar(50) null,--
	note2 nvarchar(50) null,--
	note3 nvarchar(50) null,--
	note4 nvarchar(50) null,--
	note5 nvarchar(50) null,--
	
)
--���ⲿ����Դ��ȡ��Ϣ�� ��ʱ�洢��
create table sr_import_temp(
	id nvarchar(50) not null,--Ψһ��ʶ��
	imptype varchar(10) not null,--�������� 502Ԥ���� 503��Ժ����  504 סԺ����  505��������
	dostatus  varchar(10) not null,--Ĭ��0 ��ʾδ���� 1��ʾ�Ѿ�����
	operno nvarchar(50) null,--�����˱��  15563361861
	opername nvarchar(50) null,--����������
	deptcode nvarchar(50) null,--���ű��
	deptname nvarchar(50) null,--��������
	orderby nvarchar(50) null,--�������ұ��
	orderbyname nvarchar(50) null,--������������
	classcode nvarchar(50) null,--��Ŀ���
	classname nvarchar(50) null,--��Ŀ����
	impdate nvarchar(50) null,--����
	costs decimal(18,2) null,--Ӧ��
	charges decimal(18,2) null,--ʵ��
	outputid varchar(10) null,--���id
	payway nvarchar(50) null,--֧����ʽ(502 503������)
	amount decimal(18,2) null,--֧�����(502 503������)
	[deptcode_ys] [nvarchar](50) NULL,
	[orderby_ys] [nvarchar](50) NULL,
	[classcode_ys] [nvarchar](50) NULL,
	note1 nvarchar(50) null,--֧����ʽ  506��ʱ����������
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
	atype varchar(10) not null,--�γ� ;��Ʒ
	outname nvarchar(50) not null,--�ⲿϵͳ��Ŀ����
	yskmcode varchar(50)  null,--Ԥ���Ŀ���
	yskmname nvarchar(50)  null,--Ԥ���Ŀ����
	note1 nvarchar(50) null,--
	note2 nvarchar(50) null,--
	note3 nvarchar(50) null,--
	note4 nvarchar(50) null,--
	note5 nvarchar(50) null,--
	
)
--���ǵ�������������ʱ��
create table sr_import_temp_dz(
	id nvarchar(50) not null,--Ψһ��ʶ��
	atype nvarchar(50) null,--��Ŀ���ͣ�1Ϊ�γ̣�2Ϊ��Ʒ
	CampusName nvarchar(50) null,--У������
	ReceiptNo nvarchar(50) null,--�վݺ�
	UserName nvarchar(50) null,--����Ա����
	aDate nvarchar(50) null,--�շ�����
	ItemName nvarchar(50) null,--��Ŀ����
	TotalMoney decimal(18,2) null,--���
	EmployeeNames nvarchar(50)  null,--ҵ������������������ж���ö��ŷָ���
	ConfirmUserName nvarchar(50)  null,--�շ�ȷ�ϵ��û���
	ConfirmTime nvarchar(50) null,--�շ�ȷ�ϵ�ʱ��
	dostatus  varchar(10) not null,--Ĭ��0 ��ʾδ���� 1��ʾ�Ѿ�����
	
	dept_ys varchar(50) null,--Ԥ��ϵͳ���
	yskmcode varchar(50) null,--Ԥ��ϵͳԤ���Ŀ���
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
--�Ƶ���־
create table sr_zd_note(
	billtype varchar(10) not null,--�������� 502Ԥ���� 503��Ժ����  504 סԺ����  505��������
	billdate nvarchar(50) not null,--�Ƶ�����
	deptcode nvarchar(50) null,--���ݲ���
	deptname nvarchar(50) null,--���ݲ�������
	srdcode nvarchar(50) null,--��Ӧ�ı�������
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
--��Ϊ�콡�п��ܶ�����ű�Ŷ�Ӧһ��Ԥ��ϵͳ���ű�� �������Ӧ��
create table sr_dept_tianjian(
	tianjiancode nvarchar(50) not null,--�콡ϵͳ�б��
	ysdeptcode nvarchar(50) not null,--Ԥ��ϵͳ���ű��
	note1 nvarchar(50) null,--
	note2 nvarchar(50) null,--
	note3 nvarchar(50) null,--
	note4 nvarchar(50) null,--
	note5 nvarchar(50) null,--
)
--******************************����ҽԺ���������end***************************
--�ÿ����뵥  beg 2015-03-13 edit by zyl
create table bill_yksq
(
billCode varchar(50) primary key not null,   --���ݱ��
jbr varchar(50) null,	  --������
billDept varchar(50) null,	 --�ÿ��
yt	 nvarchar(500)   null,   --��;
je   decimal(18,2)null,		--������
rkCodes	 varchar(500) null,	--��Ӧ�ⲿ��ⵥ�� ��������ö��Ÿ���

cdecfine0 decimal(18,2) null,	
cdecfine1 decimal(18,2) null,
cdecfine2 decimal(18,2) null,
cdecfine3 decimal(18,2) null,
cdecfine4 decimal(18,2) null,

ddefine0 datetime null,	  --����
ddefine1 datetime null,
ddefine3 datetime null,
ddefine4 datetime null,
ddefine5 datetime null,

note1 nvarchar(50) null,--�����ֶ�
note2 nvarchar(50) null,--�����ֶ�
note3 nvarchar(50) null,--�����ֶ�
note4 nvarchar(50) null,--�����ֶ�
note5 nvarchar(50) null,--�����ֶ�
note6 nvarchar(50) null,--�����ֶ�
note7 nvarchar(50) null,--�����ֶ�
note8 nvarchar(50) null,--�����ֶ�
note9 nvarchar(50) null,--�����ֶ�
note0 nvarchar(50) null,--�����ֶ�
)
--�ÿ����뵥  end
 ---==============================��������==============================----
 -- ���ò��ű������� 
 create table bill_deptFyblDy(
     list_id int IDENTITY (1, 1) NOT NULL ,--������
     deptCode varchar(50) null,--���ű��
	 deptName varchar(50) null,--��������
	 fjbl decimal(18,4) null,   --�ֽ����
	 cdefine1 varchar(10) null,--���
	 cdefine2 varchar(30) null,--
	 cdefine3 varchar(30) null,--
	 cdefine4 varchar(100) null,--
	 cdefine5 varchar(100) null,--
	 cdefine6 varchar(300) null,--
	 ddefine7 decimal(18,2) null,-- �ֽ���
	 ddefine8 decimal(18,2) null,--
	 ddefine9 datetime null,--
	 ddefine10 datetime null--
 )
 --�������ñ�
 create table bill_Cnpz( 
  list_id int IDENTITY (1, 1) NOT NULL ,--������
  beg_time varchar(50) null,--��Ȼ�꿪ʼ����
  end_time varchar(50) null,--��Ȼ���������
  year_moth varchar(50) null,--��������
  year_CN varchar(50) null,--������
  note0 varchar(50) null,
  note1 varchar(50) null,
  note2 varchar(50)  null,
  note3 varchar(50) null,
  note4 varchar(50) null,
  note5 varchar(50) null
 )
 --����Ŀ��
 create table bill_Srmb(
   nd varchar(50) null,--Ԥ�����
   zje decimal(18,4) null,--�ܽ��
   yfjje decimal(18,4) null,--�ѷֽ���
   syje decimal(18,4) null,--ʣ����
   note0 varchar(50) null,--�������� sr:����Ԥ��  fy: ����Ԥ��
   note1 varchar(50) null,
   note2 varchar(50)  null,
   note3 varchar(50) null,
   note4 varchar(50) null,
   note5 varchar(50) null 
 )
 --Ԥ�㲿�ŷֽ���ѷ�����
 create table bill_deptfj(
	nd varchar(50) null,--Ԥ�����
	deptcode varchar(50) null,--���ű��
	deptname varchar(50) null,--��������
	bl decimal(18,4) null,--����
	xjje decimal(18,4) null,--�ֽ���
	note0 varchar(50) null,
	note1 varchar(50) null,
	note2 varchar(50)  null,
	note3 varchar(50) null,
	note4 varchar(50) null,
	note5 varchar(50) null
 )
 -----�ʲ��������뵥
create table dz_zncgzsqd(
    swbh varchar(50) null,--�����ţ�zcgzyyyyMMdd001��
	zydj varchar(50) null,--��Ҫ�ȼ�
	sqsy varchar(500) null,--��������
	tsbz varchar(500) null,--���ⱸע
	
	bh varchar(50) null,--���(�ֶ�¼��)
	sqsj varchar(50) null,--����ʱ��
	wpmc  varchar(50) null,--��Ʒ����
	ggsl varchar(50) null,--�������
	yt varchar(50) null,--��;
	sybm varchar(50) null,--ʹ�ò���
	xyrq varchar(50) null,--��������
	gjjz varchar(50) null,--���Ƽ�ֵ
	gzbz varchar(50) null,--���ñ�ע
	sqjs decimal(18,4) null,--������칫��Ʒ����
	zje decimal(18,4) null,--�ܽ��
	sgbmfzr varchar(50) null,--�깺���Ÿ�����
	sgbmrq varchar(50) null,--�깺��������
	nqbyj varchar(50) null,--���ڲ����
	nqbrq varchar(50) null,--���ڲ�����
	cwbyj varchar(50) null,--�������
	cwbrq varchar(50) null,--��������
	rzxzyj varchar(50) null,--�����������
	rzxzrq varchar(50) null,--������������
	xzbyj varchar(50) null,--���������
	xzbrq varchar(50) null,--����������
	[fj] [varchar](2000) NULL,--����
	note0 varchar(50) null,
	note1 varchar(50) null,
	note2 varchar(50) null,
	note3 varchar(50) null,
	note4 varchar(50) null,
	note5 varchar(50) null
	
)
--�ÿ����뵥
create table dz_yksqd(
 sqbh varchar(50) null,--���뵥��
 sqsj varchar(50) null,--����ʱ��
 sqlx varchar(50)null,--��������
 ykrq varchar(50) null,--�ÿ���ǰ
 sqr varchar(50) null,--������
 kxyt varchar(50) null,--������;
 ykfs varchar(50) null,--�ÿʽ
 kxje_dx varchar(50) null,--�������д��
 kxje_xx decimal(18,4) null,--������(Сд)
 skdw varchar(50) null,--�տλ
 khh varchar(50) null,--������
 zh varchar(50) null,--�˺�
 bmfzr_yj varchar(50)null,--���Ÿ��������
 bmfzr_qz varchar(50)null,--���Ÿ�����ǩ��
 bmfzr_rq varchar(50)null,--���Ÿ���������
 yfzkzy_yj varchar(50)NULL,--Ӧ���˿�רԱ���
 yfzkzy_qz varchar(50)NULL,--Ӧ���˿�רԱǩ��
 yfzkzy_rq varchar(50)NULL,--Ӧ���˿�רԱ����
 cwbfzr_yj varchar(50)null,--���񲿸��������
 cwbfzr_qz varchar(50)null,--���񲿸�����ǩ��
 cwbfzr_rq varchar(50)null,--���񲿸���������
 cwxz_yj varchar(50)null,--����ֹ�У��/������У�����
 cwxz_qz varchar(50)null,--����ֹ�У��/������У��ǩ��
 cwxz_rq varchar(50)null,--����ֹ�У��/������У������
 dsz_yj varchar(50)null,--���³����
 dsz_qz varchar(50) null,--���³�ǩ��
 dsz_rq varchar(50) null,--���³�����
 [fj] [varchar](2000) NULL,--����
 note0 varchar(50) null,--�ÿ��
 note1 varchar(50) null,
 note2 varchar(50) null,
 note3 varchar(50) null,
 note4 varchar(50) null,
 note5 varchar(50) null
 
)
--Ԥ���Ŀ������Ӧ��

create table [dbo].[bill_yskmchdy]
(
   chcode varchar(50) null,--������
   chname varchar(50) null,--�������
   kmcode varchar(50) null,--��Ŀ���
   kmname varchar(50) null,--��Ŀ����
   yslx varchar(50) null, --Ԥ������
   ufdata varchar(50) null,--��Ӧ����
   note0 varchar(50) NULL,--
   note1 varchar(50) NULL,--
   note2 varchar(50) NULL,--
   note3 varchar(50) NULL,--
   note4 varchar(50) NULL,--
   note5 varchar(50) NULL --

)
--�����ѵ��뱾ϵͳ���ɴ�����õ�ʱ����¼�Ѿ������˵ĵ���
create table bill_chly_mark(
	billCode nvarchar(50) null,--
	billuser nvarchar(50) null,--�Ƶ���
	billdept nvarchar(50) null,--�Ƶ�����
	billdate nvarchar(50) null,--�Ƶ�ʱ��
    [pocode] [nvarchar](50) NULL,--���ݱ��
	[mark] [nvarchar](100) NULL,  --������� 
	[chcode] [nvarchar](50) NULL,--���code
	ufdata  [nvarchar](50) NULL,--װ�����ݿ�
	[note1] [nvarchar](50) NULL,--
	[note2] [nvarchar](50) NULL,
	[note3] [nvarchar](50) NULL,
	[note4] [nvarchar](50) NULL,
	[note5] [nvarchar](50) NULL
)
CREATE TABLE [dbo].[bill_Mark](
	[billcode] [nvarchar](50) NULL,--���ݱ��
	[mark] [nvarchar](100) NULL,--�������
	[usercode] [nvarchar](50) NULL,--�û����
	[note1] [nvarchar](50) NULL,
	[note2] [nvarchar](50) NULL,
	[note3] [nvarchar](50) NULL,
	[note4] [nvarchar](50) NULL,
	[note5] [nvarchar](50) NULL
) ON [PRIMARY]
--У�ܼ�
CREATE TABLE [dbo].[bill_xg](
	yedjh nvarchar(50) null,--ҵ�񵥾ݺ�
	zdrcode nvarchar(50) null,--�Ƶ��˱��
	zdrname nvarchar(50) null,--�Ƶ�������
	fsrq nvarchar(50) null,--��������
	fxcode nvarchar(50) null,--��У���
	fxname nvarchar(50) null,--��У����
	srxmcode nvarchar(50) null,--������Ŀ���
	srxmname nvarchar(50) null,--������Ŀ����
	xmsrje decimal(18,4) null,--ҵ����������
	yjgsbmcode nvarchar(50) null,--ҵ���������ű��
	yjgsbmname nvarchar(50) null,--ҵ��������������
	yjgsrycode nvarchar(50) null,--ҵ��������Ա���
	yjgsryname nvarchar(50) null,--ҵ��������Ա����
	shr nvarchar(50) null,--�����
	shrq nvarchar(50) null--�������
) 

--����ѧУ������ת��תУ���뵥
CREATE TABLE [dbo].[bill_zfzxsqd_dz](
    billCode nvarchar(50) null,--billCode
	sqrq nvarchar(50) null,--��������
	zcfx nvarchar(50) null,--ת�������У
	zrfx nvarchar(50) null,--ת���У
	xyxm nvarchar(50) null,--ѧԱ����
	nianji nvarchar(50) null,--�꼶
	yxyfdfy decimal(18,4) null,--ԭЭ�鸨������
	ybmkc nvarchar(50) null,--ԭ�����γ�
	ykcxsyh nvarchar(50) null,--ԭ�γ������Ż�
	yxfks decimal(18,4) null,--�����ѿ�ʱ/��
	dyksdj decimal(18,4) null,--��Ӧ��ʱ����
	yxffy decimal(18,4) null,--�����ѷ���
	ykqtfy decimal(18,4) null,--Ӧ����������
	syjexx decimal(18,4)null,--ʣ����
	syjedx nvarchar(50) null,--ʣ�����д
	zfyy nvarchar(50) null,--ת��ԭ��
	xbxs nvarchar(50) null,--�±�Сʱ/�γ�
	xbjje decimal(18,4) null,--�벹�����
	xbjjedx nvarchar(50)null,--�벹������д
	[note1] [nvarchar](50) NULL,
	[note2] [nvarchar](50) NULL,
	[note3] [nvarchar](50) NULL,
	[note4] [nvarchar](50) NULL,
	[note5] [nvarchar](50) NULL
) 
---��ϵѧԱ�ػ���Ϣ
create table bill_gxxythxx_dz(
   billCode nvarchar(50) null,
   fenxiao nvarchar(50) null,--��У
   xyxm nvarchar(50) null,--ѧԱ����
   nianji nvarchar(50) null,--�꼶
   bmkc nvarchar(50) null,--�����γ�
   ysf nvarchar(50) null,--Ӧ�շ�
   xhyh nvarchar(50)null,--�����Ż�
   youhui decimal(18,4) null,--�Ż�
   zengsong1 nvarchar(50) null,--����
   zengsong2 nvarchar(50) null,--���ͣ���ע��
   beizhu nvarchar(50) null,--��ע
   [note1] [nvarchar](50) NULL,
	[note2] [nvarchar](50) NULL,
	[note3] [nvarchar](50) NULL,
	[note4] [nvarchar](50) NULL,
	[note5] [nvarchar](50) NULL
   
)











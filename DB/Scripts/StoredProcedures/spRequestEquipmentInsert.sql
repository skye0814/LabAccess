USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spRequestEquipmentInsert]    Script Date: 2/22/2022 2:09:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spRequestEquipmentInsert]
	@RequestID int
	,@RequestorID varchar(100) = ''
	,@Quantity int
	,@EquipmentCategoryID int
	,@Date varchar(50) = ''
	,@Time varchar(50) = ''
	,@isApproved bit
	,@ModifiedBy int
	,@Remarks varchar(250) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT into RequestEquipment (RequestorID
	,Quantity
	,EquipmentCategoryID
	,Date
	,Time
	,isApproved
	,ModifiedBy
	,Remarks)
	VALUES (@RequestorID
	,@Quantity
	,@EquipmentCategoryID
	,@Date
	,@Time
	,@isApproved
	,@ModifiedBy
	,@Remarks)

END

-- SET IDENTITY_INSERT RequestEquipment OFF

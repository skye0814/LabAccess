DROP PROCEDURE IF EXISTS [spEquipmentCategoryInsert]
USE [ERS]
GO
/****** Object:  StoredProcedure [dbo].[spEquipmentInsert]    Script Date: 1/17/2022 5:40:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spEquipmentCategoryInsert]
-- Add the parameters for the stored procedure here
	@Category varchar(50)
	,@CategoryCode varchar(50)
	,@CreatedBy bigint
	,@Comments varchar(250) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @ID INT
	DECLARE @i INT = 1
	DECLARE @equipmentItemCode VARCHAR(50)
	-- Insert statements for procedure here
	INSERT into EquipmentCategory(isActive
	,Category
	,CategoryCode
	,QuantityTotal
	,QuantityUsable
	,QuantityDefective
	,QuantityMissing
	,NoOfTimesBorrowed
	,Comments
	,CreatedBy
	,CreatedDate
	,ModifiedBy
	,ModifiedDate)
	VALUES
	(1
	,@Category
	,@CategoryCode
	,0
	,0
	,0
	,0
	,0
	,@Comments
	,@CreatedBy
	,GETDATE()
	,@CreatedBy
	,GETDATE())

	SELECT SCOPE_IDENTITY() 

END

	

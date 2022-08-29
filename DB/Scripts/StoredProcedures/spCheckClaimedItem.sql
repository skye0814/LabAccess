DROP PROCEDURE IF EXISTS spCheckClaimedItem
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE spCheckClaimedItem
    @requestGUID VARCHAR(100)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
	DECLARE @returnValue INT = 0, @requestStatus VARCHAR(100) = ''
	SELECT @requestStatus = Status
		FROM Requests
		WHERE RequestGUID = @RequestGUID
	IF @requestStatus = 'Unclaimed'
	BEGIN
		SELECT @returnValue = COUNT(isClaimed) 
			FROM RequestEquipmentItem 
			WHERE RequestGUID = @RequestGUID
				AND isClaimed = 0
	END
	ELSE
	BEGIN
		SELECT @returnValue = COUNT(Status) 
			FROM RequestEquipmentItem 
			WHERE RequestGUID = @RequestGUID
				AND Status = 'Claimed'
	END
	IF @returnValue = 0
	BEGIN
		SELECT 1
	END
	ELSE 
	BEGIN
		SELECT 0
	END
END
GO

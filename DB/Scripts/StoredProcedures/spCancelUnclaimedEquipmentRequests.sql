USE [ERS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE spCancelUnclaimedEquipmentRequests
	@GetDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRY
		UPDATE Requests
		SET Status = 'Cancelled'
		WHERE (EndTime < @GetDate) AND (Status = 'Unclaimed')

		UPDATE RequestDetails
		SET Status = 'Cancelled'
		WHERE (EndTime < @GetDate) AND (Status = 'Unclaimed')

		SELECT 1
	END TRY
	BEGIN CATCH
		SELECT 0
	END CATCH
END
GO

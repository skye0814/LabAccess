DROP PROCEDURE IF EXISTS spTransactionReport
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
CREATE PROCEDURE spTransactionReport
    @DateFrom varchar(50)
    , @DateTo varchar(50)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
SELECT ClaimedTime = ISNULL(A.ClaimedTime, RF.ClaimedTime)
    ,ReturnedTime = ISNULL(A.ReturnedTime, RF.ReturnedTime)
    ,Remarks = ISNULL(A.Remarks, RF.Remarks)
    ,BorrowedDescription = ISNULL(Category, RoomType)
    ,BorrowedItemOrRoom = ISNULL(EquipmentItemCode, RoomNumber)
    ,StudentNumber = ISNULL(StudentNumber, 'Lab Personnel')
    ,YearSection = ISNULL(CAST(F.YearID AS VARCHAR(10)) + '-' + H.SectionCode, 'Lab Personnel')
    ,Requestor = ISNULL(Requestor, FacilityRequestor)
    ,@DateFrom as DateFrom
    ,@DateTo as DateTo
FROM Requests A
    INNER JOIN RequestEquipmentItem B ON A.RequestGUID = B.RequestGUID
    INNER JOIN EquipmentCategory C ON B.EquipmentCategoryID = C.EquipmentCategoryID
    FULL JOIN RequestFacility RF ON A.RequestorID = RF.FacilityRequestID
    LEFT JOIN Facility Fa ON Fa.FacilityID = RF.FacilityID
    LEFT JOIN EquipmentItem D ON C.EquipmentCategoryID = D.EquipmentCategoryID 
    LEFT JOIN SystemUser E ON ISNULL(A.RequestorID, RF.FacilityRequestorID) = E.ID
    LEFT JOIN Students F ON E.ID = F.SystemUserID
    LEFT JOIN Year G ON F.YearID = G.YearID
    LEFT JOIN Section H ON F.SectionID = H.SectionID
WHERE (@DateFrom = '' OR CAST(A.ClaimedTime as DATE) >= CAST(@DateFrom as DATE) OR CAST(RF.ClaimedTime as DATE) >= CAST(@DateFrom as DATE)) 
    AND (@DateTo = '' OR CAST(A.ClaimedTime as DATE) <= CAST(@DateTo as DATE)OR CAST(RF.ClaimedTime as DATE) <= CAST(@DateTo as DATE))
    AND ((D.EquipmentItemID = B.EquipmentItemID) OR (RF.FacilityID = Fa.FacilityID))
    AND ((A.Status = 'Completed') OR (RF.Status = 'Completed'))
END
GO

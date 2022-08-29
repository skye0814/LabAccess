using ERSEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSEntity
{
    public class FacilityEntity : CommonEntity
    {
        public int FacilityID { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType {get; set; }
        public string RoomDescription { get; set; }
        public bool isAvailable { get; set; }
        public bool isActive { get; set; }
        public string NextSchedule { get; set; }
        public int NoOfTimesBooked { get; set; }
        public string Comments { get; set; }
        public int ModifiedBy { get; set; }
        public string Status { get; set; }

        //FOR SCHEDULE
        public int ScheduleID { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public int ScheduleDay { get; set; } //0 Monday, 1 Tuesday, 2 Wednesday, 3 Thursday, 4 Friday, 5 Saturday
        public string SubjectCode { get; set; }
        public string CourseName { get; set; }
        public bool ReservedStatus { get; set; }

    }
    public class FacilityListEntity : TableDisplayCommonEntity
    {
        public int FacilityID { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public string RoomDescription { get; set; }
        public bool isAvailable { get; set; }
        public bool isActive { get; set; }
        public string NextSchedule { get; set; }
        public int NoOfTimesBooked { get; set; }
        public string Comments { get; set; }
        public int ModifiedBy { get; set; }
        public string Status { get; set; }

        //FOR SCHEDULE
        public int ScheduleID { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public int ScheduleDay { get; set; } //0 Monday, 1 Tuesday, 2 Wednesday, 3 Thursday, 4 Friday, 5 Saturday
        public string SubjectCode { get; set; }
        public string CourseName { get; set; }
        public bool ReservedStatus { get; set; }

        // For timetable JS
        public string DayOfTheWeek { get; set; }
    }
}

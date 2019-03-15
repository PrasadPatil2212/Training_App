using System;
using System.Collections.Generic;
using System.Text;

namespace View_Models
{
   public class MeetingListVM
    {
        public int MeetingId { get; set; }
        public string MeetingName { set; get; }
        public string OrganiserName { set; get; }
        public string Agenda { set; get; }
        public List<AttendeeVM> AttendeeList { set; get; }
        public DateTime StartTime { set; get; }
        public DateTime EndTime { set; get; }
        public string RoomName { set; get; }
    }
}

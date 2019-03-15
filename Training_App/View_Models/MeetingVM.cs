using System;
using System.Collections.Generic;
using System.Text;

namespace View_Models
{
    public class MeetingVM
    {
        public string MeetingName { set; get; }
        public string Agenda { set; get; }
        public int UserId { get; set; }
        public List<int> MeetingAttendeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RoomId { get; set; }
    }
}

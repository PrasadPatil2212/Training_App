using System;
using System.Collections.Generic;
using System.Text;

namespace View_Models
{
    public class TrainingVM
    {
        public int UserId { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RoomId { get; set; }

    }
}

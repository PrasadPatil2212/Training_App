﻿using System;
using System.Collections.Generic;
using System.Text;
using View_Models;
using DataModel;

namespace MainIenterfaces
{
    public interface IMeetingInfo
    {
        List<MeetingListVM> GetMeetingInfo();
        bool InsertMeetingDetails(MeetingVM meeting);
        bool UpdateMeetingDetails(int id, MeetingVM meeting);
        bool DeleteMeetingDetails(int id);
    }
}
    
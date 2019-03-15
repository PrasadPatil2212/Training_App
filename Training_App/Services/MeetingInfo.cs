using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training_App;
using View_Models;
using MainIenterfaces;


namespace Services
{
    public class MeetingInfo : IMeetingInfo
    {
        // to retrive the data to display list

        public List<MeetingListVM> GetMeetingInfo()
        {
            try
            {
                var meetingList = new List<MeetingListVM>();
                using (TrainingAppEntities dc = new TrainingAppEntities())
                {
                    #region // Retriving meeting list
                    var data = dc.Meetings.ToList();
                    foreach (var item in data)
                    {
                        MeetingListVM meeting = new MeetingListVM();
                        if (item.DeletedAt == null)
                        {
                            meeting.MeetingId = item.MeetingId;
                            meeting.EndTime = item.Schedule.EndTime.Value;
                            meeting.StartTime = item.Schedule.StartTime.Value;
                            meeting.MeetingName = item.MeetingName;
                            meeting.Agenda = item.Agenda;
                            meeting.OrganiserName = string.Concat(item.User.FirstName + item.User.LastName);
                            List<AttendeeVM> attendeeList = new List<AttendeeVM>();
                            foreach (var attendee in item.MeetingsAttendees)
                            {
                                AttendeeVM attend = new AttendeeVM();
                                attend.AttendeeID = attendee.User.UserId;
                                attend.AttendeeName = attendee.User.FirstName;
                                attendeeList.Add(attend);
                            }
                            meeting.AttendeeList = attendeeList;
                            meeting.RoomName = item.Schedule.RoomDetail.RoomName;
                            meetingList.Add(meeting);

                        }
                    }
                    #endregion
                }
                return meetingList;
            }
            catch(Exception)
            {
                return null;
            }
        }

        //to  insert the data from create Meeting form
        public bool InsertMeetingDetails(MeetingVM meeting)
        {
            try
            {
                bool status = false;
                using (TrainingAppEntities dc = new TrainingAppEntities())
                {
                    #region//Creating new schedule record
                    Schedule schedule = new Schedule();
                    schedule.StartTime = meeting.StartTime;
                    schedule.EndTime = meeting.EndTime;
                    schedule.RoomId = meeting.RoomId;
                    var obj = dc.Schedules.Add(schedule);
                    dc.SaveChanges();
                    #endregion

                    #region//Creating new meeting record
                    Meeting meetin = new Meeting();
                    meetin.MeetingName = meeting.MeetingName;
                    meetin.Agenda = meeting.Agenda;
                    meetin.ScheduleId = obj.ScheduleId;
                    meetin.UserId = meetin.UserId;
                    meetin.CreatedAt = meetin.CreatedAt;
                    meetin.UpdatedAt = meetin.UpdatedAt;
                    var obj1 = dc.Meetings.Add(meetin);
                    dc.SaveChanges();
                    #endregion

                    #region//inserting attendeesId's(user's) into meetingattendee's table
                    foreach (int userid in meeting.MeetingAttendeeId)
                    {
                        MeetingsAttendee attendee = new MeetingsAttendee();
                        attendee.MeetingId = obj1.MeetingId;
                        attendee.UserId = userid;
                        dc.MeetingsAttendees.Add(attendee);
                        dc.SaveChanges();
                    }
                    #endregion
                    status = true;
                }
                return status;
            }
            catch(Exception)
            {
                return false;
            }
        }

        //to update the data from edit Meeting Form
        public bool UpdateMeetingDetails(int id, MeetingVM meeting)
        {
            try
            {
                bool status = false;
                using (TrainingAppEntities dc = new TrainingAppEntities())
                {
                    var meetingEntity = dc.Meetings.FirstOrDefault(a => a.MeetingId == id);
                    if (meetingEntity == null)
                    {
                        return status;
                    }
                    else
                    {
                        #region//updating schedule for specific record
                        var scheduleEntity = dc.Schedules.FirstOrDefault(a => a.ScheduleId == meetingEntity.ScheduleId);
                        scheduleEntity.StartTime = meeting.StartTime;
                        scheduleEntity.EndTime = meeting.EndTime;
                        scheduleEntity.RoomId = meeting.RoomId;
                        dc.SaveChanges();
                        #endregion

                        #region//updating meeting for specific record
                        meetingEntity.MeetingName = meeting.MeetingName;
                        meetingEntity.Agenda = meeting.Agenda;
                        //for updating meeting schedule Id is not require cause it is already present in database
                        //for updating meeting User Id is not require cause it is already present in database.()
                        meetingEntity.UpdatedAt = DateTime.Now;
                        dc.SaveChanges();
                        #endregion

                        #region//updating meeting attendance for specific record
                        var meetingAttendeeIdObject = dc.MeetingsAttendees.Where(a => a.MeetingId == id).ToList();
                        foreach (var item in meetingAttendeeIdObject)
                        {
                            dc.MeetingsAttendees.Remove(item);
                            dc.SaveChanges();
                        }

                        foreach (int userid in meeting.MeetingAttendeeId)
                        {
                            MeetingsAttendee attendee = new MeetingsAttendee();
                            attendee.MeetingId = id;
                            attendee.UserId = userid;
                            dc.MeetingsAttendees.Add(attendee);
                            dc.SaveChanges();
                        }
                        #endregion
                        status = true;

                    }
                }
                return status;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //to delete the specific meeting

        public bool DeleteMeetingDetails(int id)
        {
            try
            {
                bool status = false;
                using (TrainingAppEntities dc = new TrainingAppEntities())
                {
                    var meetingEntity = dc.Meetings.FirstOrDefault(a => a.MeetingId == id);
                    if (meetingEntity == null)
                    {
                        return status;
                    }
                    else
                    {
                        #region//Deleting the Schedule for specific meeting
                        var scheduleEntity = dc.Schedules.FirstOrDefault(a => a.ScheduleId == meetingEntity.ScheduleId);
                        dc.Schedules.Remove(scheduleEntity);
                        dc.SaveChanges();
                        #endregion

                        #region//Deleting the meeting
                        meetingEntity.DeletedAt = DateTime.Now;
                        dc.SaveChanges();
                        #endregion

                        #region//Deleting the meeting attendees
                        var meetingAttendeeObjects = dc.MeetingsAttendees.Where(a => a.MeetingId == id).ToList();
                        foreach (var item in meetingAttendeeObjects)
                        {
                            dc.MeetingsAttendees.Remove(item);
                            dc.SaveChanges();
                        }
                        #endregion
                        status = true;
                    }
                }

                return status;
            }
            catch(Exception)
            {
                return false;

            }
        }

    }
}

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
    public class RoomInfo : IRoomInfo
    {
        public List<RoomVM> GetRoomsByTime(TimeSlotVM times)
        {
            try
            {
                DateTime startTime = times.StartTime;
                DateTime endTime = times.EndTime;
                var roomList = new List<RoomVM>();
                using (TrainingAppEntities dc = new TrainingAppEntities())
                {
                    var bookedSlotList = dc.Schedules.Where(a => a.StartTime == startTime || a.EndTime == endTime || (a.StartTime < endTime && a.StartTime > startTime) || (startTime > a.StartTime && endTime < a.EndTime) || (endTime > a.EndTime && startTime < a.StartTime));

                    HashSet<int?> bookedIdList = new HashSet<int?>();
                    var uniqueDetails = dc.RoomDetails.ToList();
                    int count = 0;

                    foreach (var item in bookedSlotList)
                    {
                        bookedIdList.Add(item.RoomId);
                        count++;
                    }
                    Console.WriteLine(count);
                    foreach (var item in bookedIdList)
                    {
                        uniqueDetails.RemoveAll(a => a.RoomId == item);
                    }
                    foreach (var item in uniqueDetails)
                    {
                        RoomVM roomVM = new RoomVM();
                        roomVM.RoomId = item.RoomId;
                        roomVM.RoomName = item.RoomName;
                        roomList.Add(roomVM);

                    }
                }
                return roomList;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

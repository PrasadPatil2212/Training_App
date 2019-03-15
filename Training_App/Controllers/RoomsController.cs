using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using View_Models;
using DataModel;
using MainIenterfaces;
using Services;
using Training_App.MyFilter;

namespace Training_App.Controllers
{
    public class RoomsController : ApiController
    {
        IRoomInfo roomInfo = new RoomInfo();
        // GET: api/Rooms
        [HttpPost]
        [JwtTokenAuthentication]

        public HttpResponseMessage Post([FromBody]TimeSlotVM times)
        {
            try
            {
                var roomList = roomInfo.GetRoomsByTime(times);
                if(roomInfo != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, roomList);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "room is not available at this time slot");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        
    }
}

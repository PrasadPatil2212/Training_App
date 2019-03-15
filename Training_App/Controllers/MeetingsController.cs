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
using Training_App.Helper;
using System.Security.Claims;

namespace Training_App.Controllers
{
    public class MeetingsController : ApiController
    {
        IMeetingInfo meetingInfo = new MeetingInfo();
        // GET: api/Meeting
        [HttpGet]
        [JwtTokenAuthentication]
        public HttpResponseMessage Get()
        {
            try
            {
                var data = meetingInfo.GetMeetingInfo();
                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There are no meetings scheduled yet");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
            

        // GET: api/Meeting/5
       [HttpGet]
       [JwtTokenAuthentication]

       public HttpResponseMessage Get(int id)
        {
            try
            {
                var result = meetingInfo.GetMeetingInfo().Where(a => a.MeetingId == id).FirstOrDefault();
                if (result != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no meeting of ID= " + id + "  scheduled yet");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // POST: api/Meeting
        [HttpPost]
        [JwtTokenAuthentication]
        public HttpResponseMessage Post(MeetingVM meeting)
        {
            try
            {
                int userId = int.Parse((GetClaims.GetClaimsType(ActionContext.Request.Headers.Authorization.Parameter)).FindFirst(ClaimTypes.NameIdentifier).Value);
                meeting.UserId = userId;
                bool status = meetingInfo.InsertMeetingDetails(meeting);
                if(status)
                {
                    var message = Request.CreateResponse(HttpStatusCode.Created, "New meeting is created successfully");
                    return message;
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "cannot insert meeting details(check the data)");
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT: api/Meeting/5
        [HttpPut]
        [JwtTokenAuthentication]
        public HttpResponseMessage Put(int id, MeetingVM meeting)
        {
            try
            {
                bool status = meetingInfo.UpdateMeetingDetails(id, meeting);
                if (status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "The meeting is updated successfully");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Meeting's ID  = " + id.ToString() + "  not found to update");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE: api/Meeting/5
        [HttpDelete]
        [JwtTokenAuthentication]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                bool status = meetingInfo.DeleteMeetingDetails(id);
                if(status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Item Deleted Successfully");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Meeting's ID  = " + id.ToString() + "not found to delete");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}

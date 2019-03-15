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
    public class TrainingsController : ApiController
    {
        ITrainingInfo trainingInfo = new TrainingInfo();
        // GET: api/Trainings
        [HttpGet]
        [JwtTokenAuthentication]

        public HttpResponseMessage Get()
        {
            try
            {
                var data = trainingInfo.GetTrainingDetails();
                if(data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There are no trainig scheduled yet");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/Trainings/5
        [HttpGet]
        [JwtTokenAuthentication]

        public HttpResponseMessage Get(int id)
        {
            try
            {
                var result = trainingInfo.GetTrainingDetails().Where(a => a.TrainingId == id).FirstOrDefault();
                if(result != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no training of ID = " + id + "  scheduled yet");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
       

        // POST: api/Trainings
        [HttpPost]
        [JwtTokenAuthentication]

        public HttpResponseMessage Post(TrainingVM trainer)
        {
            try
            {
                int userId = int.Parse((GetClaims.GetClaimsType(ActionContext.Request.Headers.Authorization.Parameter)).FindFirst(ClaimTypes.NameIdentifier).Value);
                trainer.UserId = userId;
                bool status = trainingInfo.InsertTrainingDetails(trainer);
                if (status)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, trainer);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cannot Insert data May be invalid input");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        

        // PUT: api/Trainings/5
        [HttpPut]
        [JwtTokenAuthentication]

        public HttpResponseMessage Put(int id, TrainingVM trainer)
        {
            try
            {
                bool status = trainingInfo.UpdateTrainingDetails(id, trainer);
                if(status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Data updated successfully");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trainer's ID  = " + id.ToString() + "  not found to update or Maybe invalid input");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        

        // DELETE: api/Trainings/5
        [HttpDelete]
        [JwtTokenAuthentication]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                bool status = trainingInfo.DeleteTrainingDetails(id);
                if (status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Training  Deleted Successfully");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trainer's ID  = " + id.ToString() + "  not found to delete");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Newtonsoft.Json;
using Tutoring_Platform.Models;
using Tutoring_Platform.CustomModels;
using Microsoft.AspNetCore.Authorization;

namespace Tutoring_Platform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TutorController : ControllerBase
    {
        private tutoringContext db;
        public TutorController(tutoringContext dbModel)
        {
            db = dbModel;
        }
        
        [Route("GetTutorRequests")]
        [HttpPost]
        public string GetTutorRequests([FromBody] string userId)
        {
            int user = Convert.ToInt32(userId);
            string jsonResults = "";
            IEnumerable<int> studId = from sti in db.StudTutorInfos
                                      where sti.UserId == user
                                      select sti.Id;
            IEnumerable<int> tutorId = from ti in db.TutorInfos
                                       where ti.UserId == studId.First()
                                       select ti.Id;
            AppointRequest[] appointRequests = (from ar in db.AppointRequests
                                                where ar.TutorId == tutorId.First()
                                                select ar).ToArray();
            List<DisplayRequestsReturn> results = new List<DisplayRequestsReturn>();
            for (int i = 0; i < appointRequests.Count(); i++)
            {

                int getRequestStudId = appointRequests[i].StudId;
                int requestId = appointRequests[i].Id;
                Console.WriteLine(getRequestStudId.ToString());
                var getStudData = from sti in db.StudTutorInfos
                                  where sti.Id == getRequestStudId
                                  select new {sti.UserId, sti.Semester, sti.Program, sti.School };
                IEnumerable<string> getStudName = from u in db.Users
                                  where u.Id == getStudData.First().UserId
                                  select u.Name;
                results.Add(new DisplayRequestsReturn
                {
                    Id = requestId,
                    Name = getStudName.First(),
                    Semester = getStudData.First().Semester.ToString(),
                    Program = getStudData.First().Program,
                    School = getStudData.First().School,
                    CourseName = appointRequests[i].Course,
                    Days = new int[] {Convert.ToInt32(appointRequests[i].Sunday), Convert.ToInt32(appointRequests[i].Monday) , Convert.ToInt32(appointRequests[i].Tuesday) , Convert.ToInt32(appointRequests[i].Wednesday) ,
                        Convert.ToInt32(appointRequests[i].Thursday),Convert.ToInt32(appointRequests[i].Friday),Convert.ToInt32(appointRequests[i].Saturday)}
                });
            }
            jsonResults = JsonConvert.SerializeObject(results);

            return jsonResults;
        }

        [Route("SendAppointSlots")]
        [HttpPost]
        public string SendAppointSlots([FromBody] SendAppointSlots sendAppointSlots)
        {
            
            string jsonResults = "";
            if (sendAppointSlots.requestId != null && sendAppointSlots.Slot1 != null && sendAppointSlots.Slot2 != null && sendAppointSlots.Slot3 != null &&
                sendAppointSlots.Slot4 != null && sendAppointSlots.Slot5 != null)
            {
                int requestId = Convert.ToInt32(sendAppointSlots.requestId);
                AppointSlot aSlot = new AppointSlot
                {
                    RequestId = requestId,
                    Slot = sendAppointSlots.Slot1,
                    Selected = false
                };

                db.AppointSlots.Add(aSlot);
                AppointSlot aSlot2 = new AppointSlot
                {
                    RequestId = requestId,
                    Slot = sendAppointSlots.Slot2,
                    Selected = false
                };

                db.AppointSlots.Add(aSlot2);
                AppointSlot aSlot3 = new AppointSlot
                {
                    RequestId = requestId,
                    Slot = sendAppointSlots.Slot3,
                    Selected = false
                };

                db.AppointSlots.Add(aSlot3);
                AppointSlot aSlot4 = new AppointSlot
                {
                    RequestId = requestId,
                    Slot = sendAppointSlots.Slot4,
                    Selected = false
                };

                db.AppointSlots.Add(aSlot4);
                AppointSlot aSlot5 = new AppointSlot
                {
                    RequestId = requestId,
                    Slot = sendAppointSlots.Slot5,
                    Selected = false
                };

                db.AppointSlots.Add(aSlot5);
                try
                {
                    db.SaveChanges();
                    jsonResults = "Time slots sent";
                }
                catch
                {
                    jsonResults = "Could not send time slots";
                }
            }
            return jsonResults;
        }
        }
}

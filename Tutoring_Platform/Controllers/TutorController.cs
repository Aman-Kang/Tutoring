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
            Console.WriteLine("******************************************************");
            Console.WriteLine(userId.ToString());
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
                Console.WriteLine(getRequestStudId.ToString());
                var getStudData = from sti in db.StudTutorInfos
                                  where sti.Id == getRequestStudId
                                  select new {sti.UserId, sti.Semester, sti.Program, sti.School };
                IEnumerable<string> getStudName = from u in db.Users
                                  where u.Id == getStudData.First().UserId
                                  select u.Name;
                results.Add(new DisplayRequestsReturn
                {
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
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Newtonsoft.Json;
using Tutoring_Platform.Models;
using Tutoring_Platform.CustomModels;
using Microsoft.AspNetCore.Authorization;

namespace Tutoring_Platform.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private tutoringContext db;
        public AdminController(tutoringContext dbModel)
        {
            db = dbModel;
        }
        [Route("GetQueries")]
        [HttpPost]
        public string GetQueries([FromBody]string userId)
        {
            string jsonResults = "";
            var queries = from hq in db.HelpQueries
                              //where hq.responded == false
                          select hq;
            List<AskQuery> results = new List<AskQuery>();
            if(queries.Count() > 0)
            {
                foreach(var query in queries)
                {
                    string getName = (from sti in db.StudTutorInfos
                                  where sti.Id == query.UserId
                                  select sti.User.Name).First();
                    AskQuery askQuery = new AskQuery
                    {
                        UserId = getName,
                        Query = query.Query,
                        QueryId = query.Id
                    };
                    results.Add(askQuery);
                }
                jsonResults = JsonConvert.SerializeObject(results);
            }
            return jsonResults;
        }

        [Route("SendReply")]
        [HttpPost]
        public string SendReply([FromBody] AskQuery reply)
        {
            if(reply.UserId != null && reply.QueryId != null && reply.Query != null)
            {
                AdminReply adminReply = new AdminReply
                {
                    AdminId = Convert.ToInt32(reply.UserId),
                    Message = reply.Query,
                    QueryId = Convert.ToInt32(reply.QueryId)
                };
                db.AdminReplies.Add(adminReply);
                db.SaveChanges();
                return "Reply Sent!";
            }
            return "Reply could not be sent!";
        }

        [Route("GetReportedAcc")]
        [HttpPost]
        public string GetReportedAcc([FromBody] string userId)
        {

            string jsonResults = "";
            var requests = from ra in db.ReportAccounts
                           select ra;
            List<GetReportAccount> results = new List<GetReportAccount>();
            if (requests.Count() > 0)
            {
                foreach (var request in requests)
                {
                    string getName = (from u in db.Users
                                      where u.Id == request.AccountId
                                      select u.Name).First();
                    string reportedBy = (from sti in db.StudTutorInfos
                                         where sti.Id == request.UserId
                                         select sti.User.Name).First();
                    GetReportAccount getReportAccount = new GetReportAccount
                    {
                        AccountId = request.AccountId,
                        Name = getName,
                        By = reportedBy
                    };
                    results.Add(getReportAccount);
                }
                jsonResults = JsonConvert.SerializeObject(results);
            }
            return jsonResults;
        }

        [Route("DeleteUser")]
        [HttpPost]
        public string DeleteUser([FromBody] string accountId)
        {
            int user = Convert.ToInt32(accountId);
            var userAccount =
                        from u in db.Users
                        where u.Id == user
                        select u;

            foreach (var u in userAccount)
            {
                db.Users.Remove(u);
            }
            try
            {
                db.SaveChanges();
                return "Account deleted";
            }
            catch
            {
                return "Account could not be deleted";
            }
            
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tutoring_Platform.Models;
using Tutoring_Platform.CustomModels;

namespace Tutoring_Platform.Controllers
{
    /// <summary>
    /// All the API calls made on Admin side are handled by Admin Controller
    /// </summary>
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private tutoringContext db;
        public AdminController(tutoringContext dbModel)
        {
            db = dbModel;
        }

        /// <summary>
        /// Retreives the queries asked by the users from HelpQueries class and serialize them into json object.
        /// </summary>
        /// <param name="userId">Admin Id</param>
        /// <returns>JSON Object of the queries data</returns>
        [Route("GetQueries")]
        [HttpPost]
        public string GetQueries([FromBody]string userId)
        {
            string jsonResults = "";
            try
            {
                var queries = from hq in db.HelpQueries
                              select hq;
                List<AskQuery> results = new List<AskQuery>();
                if (queries.Count() > 0)
                {
                    foreach (var query in queries)
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
            catch
            {
                return jsonResults;
            }
            
        }

        /// <summary>
        /// Sends the reply to User for any specific query by creating a reply record in AdminReply class.
        /// </summary>
        /// <param name="reply">The reply entered by admin</param>
        /// <returns>Success or failure message</returns>
        [Route("SendReply")]
        [HttpPost]
        public string SendReply([FromBody] AskQuery reply)
        {
            try
            {
                if (reply.UserId != null && reply.QueryId != null && reply.Query != null)
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
                return "Reply Sent!";
            }
            catch
            {
                return "Reply could not be sent!";
            }
        }

        /// <summary>
        /// Retreives the reported accounts data from ReportAccounts class
        /// </summary>
        /// <param name="userId">The admin id</param>
        /// <returns>The JSON object list of the reported accounts</returns>
        [Route("GetReportedAcc")]
        [HttpPost]
        public string GetReportedAcc([FromBody] string userId)
        {
            string jsonResults = "";
            try
            {
                ReportAccount[] requests = (from ra in db.ReportAccounts
                                            select ra).ToArray();

                List<GetReportAccount> results = new List<GetReportAccount>();
                if (requests.Length > 0)
                {
                    foreach (var request in requests)
                    {
                        var getName = from u in db.Users
                                      where u.Id == request.AccountId
                                      select u.Name;
                        var reportedBy = from sti in db.StudTutorInfos
                                         where sti.Id == request.UserId
                                         select sti.User.Name;
                        GetReportAccount getReportAccount = new GetReportAccount
                        {
                            AccountId = request.AccountId,
                            Name = getName.First(),
                            By = reportedBy.First()
                        };
                        results.Add(getReportAccount);
                    }
                    jsonResults = JsonConvert.SerializeObject(results);
                }
                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }

        }

        /// <summary>
        /// Deletes the user from database
        /// </summary>
        /// <param name="account">The account that has been reported and is displayed on the admin dashboard</param>
        /// <returns>Success or failure message</returns>
        [Route("DeleteUser")]
        [HttpPost]
        public string DeleteUser([FromBody] GetReportAccount account)
        {
            int user = Convert.ToInt32(account.AccountId);
            try
            {
                var adminReplies = from ar in db.AdminReplies
                                   where ar.Query.User.User.Id == user
                                   select ar;
                if (adminReplies.Count() > 0)
                {
                    foreach (var a in adminReplies)
                    {
                        db.AdminReplies.Remove(a);
                    }
                    db.SaveChanges();
                }
                var appointConfirm = from ac in db.AppointConfirms
                                     where ac.Slot.Request.Stud.User.Id == user
                                     select ac;
                if (appointConfirm.Count() > 0)
                {
                    foreach (var a in appointConfirm)
                    {
                        db.AppointConfirms.Remove(a);
                    }
                    db.SaveChanges();
                }
                
                var appointConfirm2 = from ac in db.AppointConfirms
                                      where ac.Slot.Request.Tutor.User.User.Id == user
                                      select ac;
                if(appointConfirm2.Count() > 0)
                {
                    foreach (var a in appointConfirm2)
                    {
                        db.AppointConfirms.Remove(a);
                    }
                    db.SaveChanges();
                }
                
                var appointSlot = from asl in db.AppointSlots
                                  where asl.Request.Stud.User.Id == user
                                  select asl;
                if (appointSlot.Count() > 0)
                {
                    foreach (var a in appointSlot)
                    {
                        db.AppointSlots.Remove(a);
                    }
                    db.SaveChanges();
                }
                
                var appointSlot2 = from asl in db.AppointSlots
                                   where asl.Request.Tutor.User.User.Id == user
                                   select asl;
                if (appointSlot2.Count() > 0)
                {
                    foreach (var a in appointSlot2)
                    {
                        db.AppointSlots.Remove(a);
                    }
                    db.SaveChanges();
                }
                
                var appointRequest = from ar in db.AppointRequests
                                     where ar.Stud.User.Id == user
                                     select ar;
                if (appointRequest.Count() > 0)
                {
                    foreach (var a in appointRequest)
                    {
                        db.AppointRequests.Remove(a);
                    }
                    db.SaveChanges();
                }

                var appointRequest2 = from ar in db.AppointRequests
                                     where ar.Tutor.User.User.Id == user
                                     select ar;
                if (appointRequest2.Count() > 0)
                {
                    foreach (var a in appointRequest2)
                    {
                        db.AppointRequests.Remove(a);
                    }
                    db.SaveChanges();
                }

                var helpQueries = from hq in db.HelpQueries
                                  where hq.User.User.Id == user
                                  select hq;
                if (helpQueries.Count() > 0)
                {
                    foreach (var h in helpQueries)
                    {
                        db.HelpQueries.Remove(h);
                    }
                    db.SaveChanges();
                } 
                
                var reportAccount = from ra in db.ReportAccounts
                                    where ra.User.User.Id == user
                                    select ra;
                if (reportAccount.Count() > 0)
                {
                    foreach (var r in reportAccount)
                    {
                        db.ReportAccounts.Remove(r);
                    }
                    db.SaveChanges();
                }

                var reportAccount2 = from ra in db.ReportAccounts
                                    where ra.Account.Id == user
                                    select ra;
                if (reportAccount2.Count() > 0)
                {
                    foreach (var r in reportAccount2)
                    {
                        db.ReportAccounts.Remove(r);
                    }
                    db.SaveChanges();
                }


                var daysAvail = from da in db.DaysAvailables
                                where da.User.User.User.Id == user
                                select da;
                if (daysAvail.Count() > 0)
                {
                    foreach (var d in daysAvail)
                    {
                        db.DaysAvailables.Remove(d);
                    }
                    db.SaveChanges();
                }
                
                var tutorCourse = from tc in db.TutorCourses
                                  where tc.Tutor.User.User.Id == user
                                  select tc;
                if (tutorCourse.Count() > 0)
                {
                    foreach (var t in tutorCourse)
                    {
                        db.TutorCourses.Remove(t);
                    }
                    db.SaveChanges();
                }
                    
                
                var tutorInfo = from ti in db.TutorInfos
                                where ti.User.User.Id == user
                                select ti;
                if (tutorInfo.Count() > 0)
                {
                    foreach (var t in tutorInfo)
                    {
                        db.TutorInfos.Remove(t);
                    }
                    db.SaveChanges();
                }
               
                var studTutor = from sti in db.StudTutorInfos
                                where sti.User.Id == user
                                select sti;
                if (studTutor.Count() > 0)
                {
                    foreach (var s in studTutor)
                    {
                        db.StudTutorInfos.Remove(s);
                    }
                    db.SaveChanges();
                }
                
                var userAccount =
                            from u in db.Users
                            where u.Id == user
                            select u;
                foreach (var u in userAccount)
                {
                    db.Users.Remove(u);
                }
                db.SaveChanges();
                return "Account deleted";
            }
            catch
            {
                return "Account could not be deleted";
            }

        }

        /// <summary>
        /// Get the users statistics to display on Admin side
        /// </summary>
        /// <param name="userId">The admin id</param>
        /// <returns>The statistic data</returns>
        [Route("GetStats")]
        [HttpPost]
        public string GetStats([FromBody] string userId)
        {
            string jsonResults = "";
            try
            {
                List<GetStats> getStats = new List<GetStats>();
                var users = from u in db.Users
                            select u;
                GetStats stats = new GetStats
                {
                    Name = "Total number of Users",
                    Data = users.Count().ToString()
                };
                getStats.Add(stats);

                var students = from u in db.Users
                               where u.Role == "student"
                               select u;
                stats = new GetStats
                {
                    Name = "Total number of Students",
                    Data = students.Count().ToString()
                };
                getStats.Add(stats);

                var tutors = from u in db.Users
                             where u.Role == "tutor"
                             select u;
                stats = new GetStats
                {
                    Name = "Total number of Tutors",
                    Data = tutors.Count().ToString()
                };
                getStats.Add(stats);

                var usersProvince = db.StudTutorInfos
                                    .GroupBy(s => s.Province)
                                    .OrderByDescending(gs => gs.Count())
                                    .Take(1)
                                    .Select(s => s.Key).ToList();
                stats = new GetStats
                {
                    Name = "Province with maximum users",
                    Data = usersProvince.First()
                };
                getStats.Add(stats);

                var usersField = db.StudTutorInfos
                                    .GroupBy(s => s.StudyField)
                                    .OrderByDescending(gs => gs.Count())
                                    .Take(1)
                                    .Select(s => s.Key).ToList();
                stats = new GetStats
                {
                    Name = "Study Field with maximum users",
                    Data = usersField.First()
                };
                getStats.Add(stats);

                jsonResults = JsonConvert.SerializeObject(getStats);
                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }
        }

        /// <summary>
        /// Gets the admin profile info from database
        /// </summary>
        /// <param name="userId">Admin id</param>
        /// <returns>The profile data</returns>
        [Route("GetInfo")]
        [HttpPost]
        public string GetInfo([FromBody] string userId)
        {
            int user = Convert.ToInt32(userId);
            string jsonResults = "";
            try
            {
                List<ProfileData> results = new List<ProfileData>();
                var users = (from u in db.Users
                            where u.Id == user
                            select new { u.Name, u.Email }).ToArray();

                if (users.Length > 0)
                {
                    ProfileData admin = new ProfileData
                    {
                        Name = users[0].Name,
                        Address = users[0].Email
                    };
                    results.Add(admin);
                    jsonResults = JsonConvert.SerializeObject(results);
                }
                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }
        }
    }
}

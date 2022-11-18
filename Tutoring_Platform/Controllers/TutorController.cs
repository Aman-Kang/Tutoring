using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Newtonsoft.Json;
using Tutoring_Platform.Models;
using Tutoring_Platform.CustomModels;

namespace Tutoring_Platform.Controllers
{
    /// <summary>
    /// Any calls made on tutor side which are not similar to student side are addressed by the tutor controller
    /// </summary>
    [ApiController]
    [Route("tutor")]
    public class TutorController : ControllerBase
    {
        private tutoringContext db;
        public TutorController(tutoringContext dbModel)
        {
            db = dbModel;
        }
        
        /// <summary>
        /// Gets the tutoring requests made by the students stored in the AppointRequests table
        /// </summary>
        /// <param name="userId">The id of logged in tutor</param>
        /// <returns>JSON Object List of the tutoring requests data</returns>
        [Route("GetTutorRequests")]
        [HttpPost]
        public string GetTutorRequests([FromBody] string userId)
        {
            string jsonResults = "";
            try
            {
                int user = Convert.ToInt32(userId);
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
                    var getStudData = from sti in db.StudTutorInfos
                                      where sti.Id == getRequestStudId
                                      select new { sti.UserId, sti.Semester, sti.Program, sti.School };
                    IEnumerable<string> getStudName = from u in db.Users
                                                      where u.Id == getStudData.First().UserId
                                                      select u.Name;
                    results.Add(new DisplayRequestsReturn
                    {
                        Id = requestId,
                        StudId = getRequestStudId,
                        Name = getStudName.First(),
                        Semester = getStudData.First().Semester.ToString(),
                        Program = getStudData.First().Program,
                        School = getStudData.First().School,
                        CourseName = appointRequests[i].Course,
                        Days = new int[] {Convert.ToInt32(appointRequests[i].Sunday), Convert.ToInt32(appointRequests[i].Monday) , Convert.ToInt32(appointRequests[i].Tuesday) , Convert.ToInt32(appointRequests[i].Wednesday) ,
                        Convert.ToInt32(appointRequests[i].Thursday),Convert.ToInt32(appointRequests[i].Friday),Convert.ToInt32(appointRequests[i].Saturday)},
                        Message = appointRequests[i].Message
                    });
                }
                jsonResults = JsonConvert.SerializeObject(results);

                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }
            
        }

        /// <summary>
        /// When tutor receives the tutoring requests, then tutor can send appointment time slots to the student
        /// This method creates a new record in the AppointSlots table where the five slots submitted by the tutor
        /// are stored
        /// </summary>
        /// <param name="sendAppointSlots">The slots submitted by tutor</param>
        /// <returns>Success or failure message that tells the tutor if the time slots have been sent.</returns>
        [Route("SendAppointSlots")]
        [HttpPost]
        public string SendAppointSlots([FromBody] SendAppointSlots sendAppointSlots)
        {
            string jsonResults = "";
            try
            {
                if (sendAppointSlots.requestId != null && sendAppointSlots.Slot1 != null && sendAppointSlots.Slot2 != null && sendAppointSlots.Slot3 != null &&
                sendAppointSlots.Slot4 != null && sendAppointSlots.Slot5 != null)
                {
                    int requestId = Convert.ToInt32(sendAppointSlots.requestId);
                    AppointSlot aSlot = new AppointSlot
                    {
                        RequestId = requestId,
                        Slot = sendAppointSlots.Slot1,
                        Selected = false,
                        Message = sendAppointSlots.Message
                    };

                    db.AppointSlots.Add(aSlot);
                    AppointSlot aSlot2 = new AppointSlot
                    {
                        RequestId = requestId,
                        Slot = sendAppointSlots.Slot2,
                        Selected = false,
                        Message = sendAppointSlots.Message
                    };

                    db.AppointSlots.Add(aSlot2);
                    AppointSlot aSlot3 = new AppointSlot
                    {
                        RequestId = requestId,
                        Slot = sendAppointSlots.Slot3,
                        Selected = false,
                        Message = sendAppointSlots.Message
                    };

                    db.AppointSlots.Add(aSlot3);
                    AppointSlot aSlot4 = new AppointSlot
                    {
                        RequestId = requestId,
                        Slot = sendAppointSlots.Slot4,
                        Selected = false,
                        Message = sendAppointSlots.Message
                    };

                    db.AppointSlots.Add(aSlot4);
                    AppointSlot aSlot5 = new AppointSlot
                    {
                        RequestId = requestId,
                        Slot = sendAppointSlots.Slot5,
                        Selected = false,
                        Message = sendAppointSlots.Message
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
            catch
            {
                return jsonResults;
            }
            
        }

        /// <summary>
        /// When student confirms an appointment then this method sets that selected appointment time slot to true
        /// in database. This method gets the selected appointment slot from database
        /// </summary>
        /// <param name="userId">The id of the tutor logged in</param>
        /// <returns>The JSON Object list of the confirmed appointments with selected slot</returns>
        [Route("GetConfirmedAppointments")]
        [HttpPost]
        public string GetConfirmedAppointments([FromBody] string userId)
        {
            string jsonResults = "";
            try
            {
                int user = Convert.ToInt32(userId);
                IEnumerable<int> studId = from sti in db.StudTutorInfos
                                          where sti.UserId == user
                                          select sti.Id;
                IEnumerable<int> tutorId = from ti in db.TutorInfos
                                           where ti.UserId == studId.First()
                                           select ti.Id;
                var requestId = (from ar in db.AppointRequests
                                 where ar.TutorId == tutorId.First()
                                 select new { ar.Id, ar.StudId, ar.Course }).ToArray();
                List<GetConfirmedAppoint> appointments = new List<GetConfirmedAppoint>();
                for (int i = 0; i < requestId.Length; i++)
                {
                    var appointSlots = from asl in db.AppointSlots
                                       where asl.Selected == true && asl.RequestId == requestId[i].Id
                                       select new { asl.Id, asl.Slot };
                    if (appointSlots.Count() > 0)
                    {
                        IEnumerable<string> getStudName = from sti in db.StudTutorInfos
                                                          where sti.Id == requestId[i].StudId
                                                          select sti.User.Name;
                        var courseName = requestId[i].Course;
                        GetConfirmedAppoint confirmedAppoint = new GetConfirmedAppoint
                        {
                            Name = getStudName.First(),
                            Course = courseName,
                            Slot = appointSlots.First().Slot,
                            Id = appointSlots.First().Id
                        };
                        appointments.Add(confirmedAppoint);
                    }
                    
                }
                jsonResults = JsonConvert.SerializeObject(appointments);
                return jsonResults;
            }
            catch
            { return jsonResults; }
            
        }

        /// <summary>
        /// When the tutor submits the meeting link and paypal link to the student, then this method creates a new record
        /// in the AppointConfirm table of the database
        /// </summary>
        /// <param name="appoints">The appointment data submitted by tutor</param>
        /// <returns>Success or failure message</returns>
        [Route("AddToAppoints")]
        [HttpPost]
        public string AddToAppoints([FromBody] AddToAppoints appoints)
        {
            string jsonResults = "";
            try
            {
                if (appoints.slotId != null && appoints.paypal != null && appoints.zoom != null)
                {
                    AppointConfirm appointConfirm = new AppointConfirm
                    {
                        SlotId = Convert.ToInt32(appoints.slotId),
                        PaypalLink = appoints.paypal,
                        MeetingLink = appoints.zoom
                    };
                    try
                    {
                        db.AppointConfirms.Add(appointConfirm);
                        db.SaveChanges();
                        jsonResults = "Appointment added";
                    }
                    catch
                    {
                        jsonResults = "Appointment could not be added";
                    }
                }
                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }
        }

        /// <summary>
        /// The final appointment that has all the information is stored in AppointConfirm table and this method
        /// retreives that appointment data
        /// </summary>
        /// <param name="userId">The id of the tutor logged in</param>
        /// <returns>The JSON Object list of the confirmed appointments</returns>
        [Route("GetAppointments")]
        [HttpPost]
        public string GetAppointments([FromBody] string userId)
        {
            int user = Convert.ToInt32(userId);
            string jsonResults = "";
            try
            {
                List<GetAppointments> results = new List<GetAppointments>();
                IEnumerable<int> studId = from sti in db.StudTutorInfos
                                          where sti.UserId == user
                                          select sti.Id;
                IEnumerable<int> tutorId = from ti in db.TutorInfos
                                           where ti.UserId == studId.First()
                                           select ti.Id;
                var requestId = (from ar in db.AppointRequests
                                 where ar.TutorId == tutorId.First()
                                 select new { ar.Id, ar.Course, ar.Stud.User.Name }).ToArray();

                for (int i = 0; i < requestId.Length; i++)
                {
                    var slotId = from asl in db.AppointSlots
                                 where asl.RequestId == requestId[i].Id && asl.Selected == true
                                 select new { asl.Id, asl.Slot };
                    foreach (var id in slotId)
                    {

                        var confirmAppoint = from ac in db.AppointConfirms
                                             where ac.SlotId == id.Id
                                             select new { ac.Id, ac.MeetingLink,ac.PaypalLink };
                        foreach (var confirmAppointment in confirmAppoint)
                        {
                            GetAppointments getAppointments = new GetAppointments
                            {
                                ConfirmId = confirmAppointment.Id,
                                Date = id.Slot,
                                Course = requestId[i].Course,
                                TutorStud = requestId[i].Name,
                                Paypal = confirmAppointment.PaypalLink,
                                Zoom = confirmAppointment.MeetingLink
                            };
                            results.Add(getAppointments);
                        }
                    }
                }
                jsonResults = JsonConvert.SerializeObject(results);
                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }
        }

        /// <summary>
        /// Gets the profile data of the tutor from database
        /// </summary>
        /// <param name="userId">The id of logged in tutor</param>
        /// <returns>Returns profile data</returns>
        [Route("GetInfo")]
        [HttpPost]
        public string GetInfo([FromBody] string userId)
        {
            string jsonResults = "";
            try
            {
                int user = Convert.ToInt32(userId);
                List<TutorParam> results = new List<TutorParam>();
                var tutor = (from ti in db.TutorInfos
                             where ti.User.UserId == user
                             select new { ti, ti.User, ti.User.User.Name, ti.User.User.Email }).ToArray();
                if (tutor.Length > 0)
                {
                    var tutorCourses = (from tc in db.TutorCourses
                                        where tc.TutorId == tutor[0].ti.Id
                                        select tc).ToArray();
                    string[] subjects = new string[3];
                    for (int i = 0; i < tutorCourses.Length && i < 3; i++)
                    {
                        subjects[i] = tutorCourses[i].Course;
                    }
                    var daysAvail = (from da in db.DaysAvailables
                                     where da.UserId == tutor[0].ti.Id
                                     select da).ToArray();
                    int[] days = new int[7];
                    if (daysAvail.Length > 0)
                    {
                        days[0] = daysAvail[0].Sunday;
                        days[1] = daysAvail[0].Monday;
                        days[2] = daysAvail[0].Tuesday;
                        days[3] = daysAvail[0].Wednesday;
                        days[4] = daysAvail[0].Thursday;
                        days[5] = daysAvail[0].Friday;
                        days[6] = daysAvail[0].Saturday;
                    }

                    TutorParam tutorObject = new TutorParam
                    {
                        Name = tutor[0].Name,
                        Address = tutor[0].User.Address,
                        City = tutor[0].User.City,
                        Postal = tutor[0].User.PostalCode,
                        Province = tutor[0].User.Province,
                        School = tutor[0].User.School,
                        Field = tutor[0].User.StudyField,
                        Program = tutor[0].User.Program,
                        Semester = tutor[0].User.Semester.ToString(),
                        Wage = tutor[0].ti.Wage.ToString(),
                        Subjects = subjects,
                        Days = days
                    };
                    results.Add(tutorObject);
                    jsonResults = JsonConvert.SerializeObject(results);
                }
                return jsonResults;
            }
            catch{
                return jsonResults;
            }
        }

        /// <summary>
        /// Updates the profile info of the tutor by using the newly submitted data by the tutor.
        /// </summary>
        /// <param name="tutor">The data submitted by tutor</param>
        /// <returns>Success or failure message</returns>
        [Route("UpdateInfo")]
        [HttpPost]
        public string UpdateInfo([FromBody] TutorParam tutor)
        {
            try
            {
                if (tutor.UserId != null && tutor.Address != null && tutor.City != null && tutor.Postal != null &&
                tutor.Province != null && tutor.School != null && tutor.Field != null && tutor.Program != null &&
                tutor.Semester != null && tutor.Days != null && tutor.Subjects != null && tutor.Wage != null)
                {
                    IEnumerable<StudTutorInfo> getUser = from u in db.StudTutorInfos
                                                         where u.UserId == Convert.ToInt32(tutor.UserId)
                                                         select u;
                    IEnumerable<TutorInfo> getTutor = from ti in db.TutorInfos
                                                      where ti.UserId == getUser.First().Id
                                                      select ti;
                    foreach (StudTutorInfo user in getUser)
                    {
                        user.Address = tutor.Address;
                        user.City = tutor.City;
                        user.PostalCode = tutor.Postal;
                        user.Program = tutor.Program;
                        user.Province = tutor.Province;
                        user.School = tutor.School;
                        user.StudyField = tutor.Field;
                        user.Semester = Convert.ToInt32(tutor.Semester);
                    }
                    db.SaveChanges();
                    foreach (TutorInfo user in getTutor)
                    {
                        user.Wage = Convert.ToInt32(tutor.Wage);
                        if (Convert.ToInt32(tutor.Semester) == 0)
                        {
                            user.Status = "Graduate";
                        }
                        else
                        {
                            user.Status = "Semester " + tutor.Semester + " student";
                        }
                        
                    }
                    db.SaveChanges();

                    var getCourses = (from tc in db.TutorCourses
                                      where tc.TutorId == getTutor.First().Id
                                      select tc).ToArray();
                    for(int i = 0; i < tutor.Subjects.Length; i++)
                    {
                        getCourses[i].Course = tutor.Subjects[i];
                        
                    }
                    db.SaveChanges();
                    var getDays = (from da in db.DaysAvailables
                                   where da.UserId == getTutor.First().Id
                                   select da).ToArray();
                    if(getDays.Length > 0)
                    {
                       foreach(DaysAvailable day in getDays)
                        {
                            day.Sunday = tutor.Days[0];
                            day.Monday = tutor.Days[1];
                            day.Tuesday = tutor.Days[2];
                            day.Wednesday = tutor.Days[3];
                            day.Thursday = tutor.Days[4];
                            day.Friday = tutor.Days[5];
                            day.Saturday = tutor.Days[6];
                        }
                    }
                    else
                    {
                        DaysAvailable day = new DaysAvailable
                        {
                            UserId = getTutor.First().Id,
                            Sunday = tutor.Days[0],
                            Monday = tutor.Days[1],
                            Tuesday = tutor.Days[2],
                            Wednesday = tutor.Days[3],
                            Thursday = tutor.Days[4],
                            Friday = tutor.Days[5],
                            Saturday = tutor.Days[6]
                        };
                        db.DaysAvailables.Add(day);
                    }
                    db.SaveChanges();
                    return "Profile Updated";
                                        
                }
                return "Profile Updated";
            }
            catch { return "Profile could not be updated!"; }
            
        }

        /// <summary>
        /// This method creates a new record in the ReportAccount table when the logged in tutor reports a student account 
        /// </summary>
        /// <param name="reportRequest">The data related to Reported account id and the id of user who is reporting</param>
        /// <returns>Success or failure message</returns>
        [Route("ReportUser")]
        [HttpPost]
        public string ReportUser([FromBody] ReportAccountRequest reportRequest)
        {
            try
            {
                if (reportRequest.AccountId != null && reportRequest.UserId != null)
                {
                    int user = (from ti in db.TutorInfos
                                where ti.User.UserId == Convert.ToInt32(reportRequest.UserId)
                                select ti.User.Id).First();
                    int student = (from sti in db.StudTutorInfos
                                   where sti.Id == Convert.ToInt32(reportRequest.AccountId)
                                   select sti.User.Id).First();
                    ReportAccount reportAccount = new ReportAccount
                    {
                        UserId = user,
                        AccountId = student
                    };
                    db.ReportAccounts.Add(reportAccount);
                    db.SaveChanges();
                    return "Report request sent!";
                }
                return "Report request sent!";
            }
            catch
            {
                return "Could not send the report request!";
            }
        }

    }
}

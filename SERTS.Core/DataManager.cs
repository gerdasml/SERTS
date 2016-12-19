using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERTS.Core.DB;
using System.Data.SqlClient;
using System.Data;

namespace SERTS.Core
{
    public class DataManager : IDataManager
    {
        //all methods for selection
        public IEnumerable<Event> GetEvents()
        {
            /*using (var cont = new SertsEntities())
            {
                foreach (var e in cont.Events)
                    yield return e;
            }*/
            using (var cont = new SertsEntities())
            {
                using (SqlConnection connection = new SqlConnection(cont.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataAdapter d = new SqlDataAdapter())
                    {
                        string sql = @"SELECT * FROM [dbo].[Renginys];";
                        d.SelectCommand = new SqlCommand(sql, connection);
                        d.SelectCommand.ExecuteNonQuery();
                        var table = new DataTable();
                        d.Fill(table);
                        foreach (DataRow row in table.Rows)
                        {
                            var @event = new Event()
                            {
                                AgeLimit = row["Amziaus_limitas"].ToString() == "" ? (short?)null : short.Parse(row["Amziaus_limitas"].ToString()),
                                DateTime = row["Laikas"].ToString() == "" ? (DateTime?)null : DateTime.Parse(row["Laikas"].ToString()),
                                Description = row["Aprasymas"].ToString(),
                                EventType = row["Renginio_tipas"].ToString(),
                                Guest = bool.Parse(row["Svecias"].ToString()),
                                Judge = bool.Parse(row["Teisejas"].ToString()),
                                Longitude = double.Parse(row["Vietos_ilguma"].ToString()),
                                Latitude = double.Parse(row["Vietos_platuma"].ToString()),
                                Number = int.Parse(row["Nr"].ToString()),
                                Name = row["Pavadinimas"].ToString(),
                                Sponcor = bool.Parse(row["Remejas"].ToString()),
                                ParticipantsAllowed = row["Max_dalyviu_skaicius"].ToString() == "" ? (short?)null : short.Parse(row["Max_dalyviu_skaicius"].ToString()),
                                Sport = row["Sporto_saka"].ToString()
                            };
                            yield return @event;
                        }
                    }
                }
            }
        }

        public IEnumerable<Student> GetStudents()
        {
            using (var cont = new SertsEntities())
            {
                foreach (var p in cont.Students)
                {
                    yield return p;
                }
            }
        }

        public Dictionary<int, Dictionary<int, short>> GetAllResults()
        {
            using (var cont = new SertsEntities())
            {
                var finalResult = new Dictionary<int, Dictionary<int, short>>();  // cia sudesim galutinius rezultatus
                var points = cont.Results;                                              // paimam visus rezultatus
                var resultsGroupedByStudentId = points.GroupBy(x => x.StudentId);       // sugrupuojam pagal mokinio id
                foreach(var result in resultsGroupedByStudentId)                        // iteruojam per gautas grupes
                {
                    int studentId = cont.Students                                  // paimam mokinio varda
                                             .Where(x => x.Id == result.Key)
                                             .Select(x => x.Id)
                                             .FirstOrDefault();
                    var eventToPoints = new Dictionary<int, short>();                // sukuriam dictionary, kuriam laikysim (renginio vardas -> taskai) sitam mokiniui
                    foreach(var e in cont.Events)                                       // uzpildom nuliais is pradziu
                    {
                        eventToPoints.Add(e.Number, 0);
                    }
                    foreach(var r in result)                                            // vaziuojam per rezultatus sitos grupes
                    {
                        eventToPoints[r.Event.Number] = r.Points;                         // priskiriam taskus tam tikram renginiui
                    }
                    finalResult.Add(studentId, eventToPoints);                        // prie galutinio rezultato pridedam ka pagaminom
                }
                return finalResult;
               
            }
        }
        public IEnumerable<Student> GetParticipants(int eventId)
        {
            using (var cont = new SertsEntities())
            {
                var allParticipants = from p in cont.Participants
                                      where p.EventNumber == eventId
                                      join s in cont.Students on p.StudentId equals s.Id
                                      select s;
                
                // pries tai iteravai per visus dalyvius, dabar iteruoji per jau atfiltruotus teisingai
                // belieka kiekviena is ju grazint
                foreach (var e in /*cont.Participants.Where(x => eventId == x.EventNumber).Select(x => x.Student)*/ allParticipants)
                {
                    // cia is visu dalyviu atfiltruoji tuos, kurie yra sito evento, ir tada pasirenki is to atfiltruoto saraso ne visa info, o mokinio id tik (gal reiktu visa mokini?)
                    // tj tau reik sita ka atfiltravai ir grazint is esmes
                    yield return e;
                }
            }
        }

        public short GetTotalScore(int id)
        {
            using (var cont = new SertsEntities())
            {
                var allPoints = cont.Results.Where(x => x.StudentId == id).Select(x => x.Points).ToList();
                return allPoints.Aggregate((x, y) => (short)(x + y));
            }
        }

        public short GetResult(int eventNr, int studentId)
        {
            using (var cont = new SertsEntities())
            {
                return cont.Results.Where(x => x.StudentId == studentId && x.EventNumber == eventNr).Select(x=> x.Points).FirstOrDefault();
            }
        }

        //all methods for insertion
        public void AddStudent(Student student)
        {
            using (var cont = new SertsEntities())
            {
                if (student == null) return;
                cont.Students.Add(student);
                cont.SaveChanges();
            }
        }

        public void AddEvent(Event @event)
        {
            using (var cont = new SertsEntities())
            {
                cont.Events.Add(@event);
                cont.SaveChanges();
            }
        }

        public void RegisterPerson(Participant participant)
        {
            using (var cont = new SertsEntities())
            {
                cont.Participants.Add(participant);
                cont.SaveChanges();
            }
        }

        public void AddResult(Result result)
        {
            using (var cont = new SertsEntities())
            {
                cont.Results.Add(result);
                cont.SaveChanges();
            }
        }


        //all methods for update
        public void UpdateStudent(int id, Student student)
        {
            using (var cont = new SertsEntities())
            {
                var userToUpdate = cont.Students.Where(x => x.Id == id).FirstOrDefault();
                if (userToUpdate == null)
                    return;
                //userToUpdate = student;
                //userToUpdate.Id = id; // cia jei netycia tame student yra kitoks id, kad islaikytume senaji, nes id keist tai kaip ir negalima
                userToUpdate.Class = student.Class;
                userToUpdate.DOB = student.DOB;
                userToUpdate.Email = student.Email;
                userToUpdate.Guest = student.Guest;
                //userToUpdate.Id = student.Id;
                userToUpdate.Login = student.Login;
                userToUpdate.Name = student.Name;
                userToUpdate.PhoneNumber = student.PhoneNumber;
                userToUpdate.School = student.School;
                userToUpdate.Surname = student.Surname;
                cont.SaveChanges();
            }
        }

        public void UpdateEvent(int id, Event @event)
        {
            using (var cont = new SertsEntities())
            {
                var eventToUpdate = cont.Events.Where(x => x.Number == id).FirstOrDefault();
                if (eventToUpdate == null)
                    return;
                //eventToUpdate = @event;
                //eventToUpdate.Number = id;
                eventToUpdate.AgeLimit = @event.AgeLimit;
                eventToUpdate.DateTime = @event.DateTime;
                eventToUpdate.Description = @event.Description;
                eventToUpdate.EventType = @event.EventType;
                eventToUpdate.Guest = @event.Guest;
                eventToUpdate.Judge = @event.Judge;
                eventToUpdate.Latitude = @event.Latitude;
                eventToUpdate.Longitude = @event.Longitude;
                eventToUpdate.Name = @event.Name;
                //eventToUpdate.Number = @event.Number;
                eventToUpdate.ParticipantsAllowed = @event.ParticipantsAllowed;
                eventToUpdate.Sponcor = @event.Sponcor;
                eventToUpdate.Sport = @event.Sport;
                cont.SaveChanges();
            }
        }

        public void UpdateResult(int id, int nr, short result)
        {
            using (var cont = new SertsEntities())
            {
                var resultToUpdate = cont.Results.Where(x => x.StudentId == id && x.EventNumber == nr).FirstOrDefault();
                if (resultToUpdate == null)
                    return;
                //resultToUpdate.EventNumber = result.EventNumber;
                //resultToUpdate.StudentId = result.StudentId;
                resultToUpdate.Points = result; // Points yra PK?
                cont.SaveChanges(); // <-
            }
        }


        //all methods for delete
        public void DeleteStudent(int id)
        {
            using (var cont = new SertsEntities())
            {
                var studentToDelete = cont.Students.Where(x => x.Id == id).FirstOrDefault();
                if (studentToDelete == null)
                    return;
                cont.Students.Remove(studentToDelete);
                cont.SaveChanges();
            }
        }

        public void DeleteEvent(int id)
        {
            using (var cont = new SertsEntities())
            {
                var eventToDelete = cont.Events.Where(x => x.Number == id).FirstOrDefault();
                if (eventToDelete == null)
                    return;
                cont.Events.Remove(eventToDelete);
                cont.SaveChanges();
            }
        }

        public void DeleteResult(int id, int nr)
        {
            using (var cont = new SertsEntities())
            {
                var resultToDelete = cont.Results.Where(x => x.StudentId == id && x.EventNumber == nr).FirstOrDefault();
                if (resultToDelete == null)
                    return;
                cont.Results.Remove(resultToDelete);
                cont.SaveChanges();
            }
        }

        public void DeleteParticipant(int id, int nr)
        {
            using (var cont = new SertsEntities())
            {
                var participantToDelete = cont.Participants.Where(x => x.StudentId == id && x.EventNumber == nr).FirstOrDefault();
                if (participantToDelete == null)
                    return; // siaip jau reiktu koki exception turbut ismest, bet del paprastumo galima ignore maybe
                cont.Participants.Remove(participantToDelete);
                cont.SaveChanges();
            }
        }
    }
}


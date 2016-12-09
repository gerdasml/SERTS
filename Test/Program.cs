using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERTS.Core.DB;
using SERTS.Core;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            /*using (var ctx = new SertsEntities())
            {
                var participants = ctx.Participants;
                foreach(var p in participants)
                {
                    Console.WriteLine("'{0}' '{1}' '{2}'", p.StudentId, p.TeamName, p.EventNumber);
                }
            }*/
            var manager = new DataManager();
            //manager.AddEvent(new Event { Name = "Test", EventType = "Individualus", Latitude = 0, Longitude = 0, Sponcor = false, Judge = true, Guest = false, DateTime = DateTime.Now });
            //manager.AddStudent(new Student { Guest=false, Name = "Test", Email="test@email.com", Surname = "Student", DOB = DateTime.Now.AddYears(-10), School = "Test Gymnasium" });
            //manager.RegisterPerson(new Participant { EventNumber=4, StudentId=4 });
            //manager.AddResult(new Result { StudentId = 4, EventNumber = 4, Points = 69 });
            //manager.UpdateStudent(4, new Student { Guest = false, Name = "Test2", Email = "test@email.com", Surname = "Student2", DOB = DateTime.Now.AddYears(-10), School = "Test2 Gymnasium" });
            //manager.UpdateEvent(4, new Event { Name = "Test2", EventType = "Individualus", Latitude = 0, Longitude = 0, Sponcor = false, Judge = true, Guest = false, DateTime = DateTime.Now.AddDays(3) });
            //manager.UpdateResult(4, 4, 70);
            foreach (var e in manager.GetEvents())
            {
                Console.WriteLine("{0}", e.Name);
            }
            foreach (var e in manager.GetResults())
            {
                Console.WriteLine("{0}:", e.Key);
                foreach(var v in e.Value)
                {
                    Console.WriteLine("\t{0} {1}", v.Key, v.Value);
                }
            }
            foreach (var e in manager.GetParticipants(1))
            {
                Console.WriteLine("{0}", e.Name);
            }
        }
    }
}

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
            foreach(var e in manager.GetEvents())
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

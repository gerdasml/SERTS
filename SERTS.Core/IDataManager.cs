using SERTS.Core.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERTS.Core
{
    public interface IDataManager
    {
        IEnumerable<Event> GetEvents();
        // studentName -> (eventName -> points)
        Dictionary<string, Dictionary<string, short>> GetResults();
        IEnumerable<Student> GetParticipants(int eventId);
        IEnumerable<Student> GetStudents();


        void AddStudent(Student student);
        void AddEvent(Event @event);
        void RegisterPerson(Participant participant);
        void AddResult(Result result);


        void UpdateStudent(int id, Student student);
        void UpdateEvent(int id, Event @event);
        void UpdateResult(int id, int nr, short result);



        void DeleteStudent(int id);
        void DeleteEvent(int id);
        void DeleteResult(int id, int nr);
        void DeleteParticipant(int id, int nr);
    }
}

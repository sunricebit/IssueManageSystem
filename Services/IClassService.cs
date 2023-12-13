using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace IMS.Services
{
    public interface IClassService
    {
        public IEnumerable<Class> GetClasses();
        public Class GetClass(int id);
        public void AddClass(Class Class);
        public void UpdateCLass(Class Class);
        public User GetTeacherById(int id);
        public Subject GetSubjectById(int id);
        public List<Subject> GetSubjects();
        public int GetTeacherIdByNameAndEmail(string name);
        public IEnumerable<User> GetAllTeachers();
        public IEnumerable<Class> GetClassesByStudent(int studentId);
        public bool ClassExist(string className);
        public IEnumerable<User> GetStudent(int classId);
        public List<Milestone> GetMilestone(int id);
        public int GetSubjectId(string name);
    }
}

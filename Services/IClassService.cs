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
        public int GetTeacherIdByName(string name);
        public IEnumerable<User> GetAllTeachers();
    }
}

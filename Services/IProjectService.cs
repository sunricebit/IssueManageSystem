namespace IMS.Services
{
    public interface IProjectService
    {
        public List<Project> GetAllProject();
        public List<Project> GetProjectByStudent(int studentId);
        public List<Project> GetProjectByTeacher(int teacherId);
        public Project GetProject(int id);
        public void UpdateProject(Project project);
        public void AddProject(Project project);
        public bool CheckProjectExist(Project project);
        bool CheckProjectUpdate(Project project);
        public IEnumerable<User> GetStudentInProject(int projectid);
        public bool RemoveStudentFromProject(int projectid, string email);
        public bool AddStudentToProject(int projectid, string email);
    }
}

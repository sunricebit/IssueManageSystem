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
    }
}

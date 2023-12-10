namespace IMS.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IMSContext _context;

        public ProjectService(IMSContext context)
        {
            _context = context;
        }

        public List<Project> GetAllProject()
        {
            List<Project> resultProject = _context.Projects.Include(p => p.Class).Include(p => p.Leader).ToList();
            return resultProject;
        }

        public List<Project> GetProjectByStudent(int studentId)
        {
            var projects = _context.Users.Include(u => u.ProjectsNavigation).FirstOrDefault(u => u.Id == studentId).ProjectsNavigation;
            List<Project> resultProject = new List<Project>();
            foreach (var project in projects)
            {
                Project pro = _context.Projects.Include(p => p.Class).Include(p => p.Leader).FirstOrDefault(p => p.Id == project.Id);
                resultProject.Add(pro);
            }
            return resultProject;
        }

        public Project GetProject(int id)
        {
            return _context.Projects.FirstOrDefault(p => p.Id == id);
        }

        public void UpdateProject(Project project)
        {
            _context.Projects.Update(project);
            _context.SaveChanges();
        }

        public void AddProject(Project project)
        {
            User leader = _context.Users.FirstOrDefault(item => item.Id == project.LeaderId);
            leader.ProjectsNavigation.Add(project);
            project.Leader = leader;
            _context.Projects.Add(project);
            _context.SaveChanges();
        }

        public bool CheckProjectExist(Project project)
        {
            Project p = _context.Projects.FirstOrDefault(item => item.Name == project.Name
            && item.ClassId == project.ClassId && item.LeaderId == project.LeaderId);
            if (p == null)
            {
                return false;
            }
            return true;
        }

        public bool CheckProjectUpdate(Project project)
        {
            Project p = _context.Projects.FirstOrDefault(item => item.Name == project.Name
            && item.ClassId == project.ClassId && item.LeaderId == project.LeaderId);
            if (p != null && p.Id != project.Id)
            {
                return false;
            }
            return true;
        }
    }
}

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
        public IEnumerable<User> GetStudentInProject(int projectid)
        {
            var members = _context.Projects.Where(c => c.Id == projectid).SelectMany(c => c.Students).ToList();
            return members;
        }


        public List<Project> GetProjectByTeacher(int teacherId)
        {
            List<Project> resultList = new List<Project>();
            User user = _context.Users.Include(u => u.Classes).ThenInclude(c => c.Projects).FirstOrDefault(u => u.Id == teacherId);
            foreach (Class c in user.Classes)
            {
                resultList.AddRange(c.Projects);
            }
            return resultList;
        }

        public bool RemoveStudentFromProject(int projectid, string email)
        {
            var project = _context.Projects.Include(p => p.Students).FirstOrDefault(c => c.Id == projectid);
            var student = _context.Users.FirstOrDefault(u => u.Email == email);
            project.Students.Remove(student);
            _context.SaveChanges();
            return true;
        }

        public bool AddStudentToProject(int projectid, string email)
        {
            Project project = _context.Projects.Include(c => c.Students).FirstOrDefault(c => c.Id == projectid);
            var student = _context.Users.FirstOrDefault(u => u.Email == email);

            if (project == null || student == null)
            {
                return false;
            }

            if (project.Students.Contains(student))
            {
                return false;
            }

            project.Students.Add(student);
            _context.SaveChanges();
            return true;
        }
    }
}

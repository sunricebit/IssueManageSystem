namespace IMS.DAO
{
    public class ProjectDAO
    {
        private readonly IMSContext _context;

        public ProjectDAO(IMSContext context)
        {
            _context = context;
        }
        public IEnumerable<User> GetStudentInProject(int projectid) 
        {
            var members = _context.Projects.Where(c => c.Id == projectid).SelectMany(c => c.Students).ToList();
            return members;
        }

    }
}

using NuGet.Packaging;

namespace IMS.DAO
{
    public class IssueSettingDAO
    {
        private readonly IMSContext _context;

        public IssueSettingDAO(IMSContext context)
        {
            _context = context;
        }

        public List<IssueSetting> GetIssueSettingByProject(int projectId)
        {
            //var common = _context.IssueSettings;
            Project project = _context.Projects.Include(p => p.IssueSettings).FirstOrDefault(p => p.Id == projectId);
            List<IssueSetting> IssueSettingList = new List<IssueSetting>();
            IssueSettingList.AddRange(project.IssueSettings);
            Class c = _context.Classes.Include(c => c.IssueSettings).FirstOrDefault(c => c.Id == project.ClassId);
            IssueSettingList.AddRange(c.IssueSettings.Where(i => i.ProjectId == null));
            IssueSettingList.AddRange(_context.IssueSettings.Where(item => item.ClassId == null));
            //common = common.Where(item => item.ClassId == null);
            return IssueSettingList.OrderBy(p => p.Id).ToList();
        }

        public List<IssueSetting> GetIssueSettingClass(int classId)
        {
            List<IssueSetting> IssueSettingList = new List<IssueSetting>();
            Class c = _context.Classes.Include(c => c.IssueSettings).FirstOrDefault(c => c.Id == classId);
            IssueSettingList.AddRange(c.IssueSettings.Where(i => i.ProjectId == null));
            IssueSettingList.AddRange(_context.IssueSettings.Where(i => i.ClassId == null));
            return IssueSettingList.OrderBy(p => p.Id).ToList();
        }

        public void AddIssueSetting(IssueSetting IssueSetting)
        {
            _context.IssueSettings.Add(IssueSetting);
            _context.SaveChanges();
        }

        public void UpdateIssueSetting(IssueSetting IssueSetting)
        {
            _context.IssueSettings.Update(IssueSetting);
            _context.SaveChanges();
        }

        public string CheckDuplicate(IssueSetting IssueSetting)
        {
            IssueSetting? IssueSettingCommon = _context.IssueSettings.FirstOrDefault(i => i.Type == IssueSetting.Type &&
                    i.Value == IssueSetting.Value);
            if (IssueSettingCommon != null)
            {
                return "Already have this Issue Setting in Common";
            }

            if (IssueSetting.ProjectId == null)
            {
                // IssueSetting trên class so sánh trên class
                IssueSetting? iS = _context.IssueSettings.FirstOrDefault(i => i.Type == IssueSetting.Type &&
                    i.Value == IssueSetting.Value && i.ClassId == IssueSetting.ClassId);
                if (iS != null)
                {

                    if (iS.ProjectId != null)
                    {
                        return "Already have this Issue Setting in Project";
                    }

                    return "Already have this Issue Setting in class";
                }
                return "Can Add";
            }
            else
            {
                // IssueSetting ở project
                // So sánh vs IssueSetting trên class
                IssueSetting? iS = _context.IssueSettings.FirstOrDefault(i => i.Type == IssueSetting.Type &&
                    i.Value == IssueSetting.Value && i.ClassId == IssueSetting.ClassId && i.ProjectId == null);
                if (iS != null)
                {
                    return "Already have this Issue Setting in class";
                }
                // IssueSetting ở project
                // So sánh vs IssueSetting trong project
                iS = _context.IssueSettings.FirstOrDefault(i => i.Type == IssueSetting.Type 
                    && i.Value == IssueSetting.Value && i.ClassId == IssueSetting.ClassId 
                    && i.ProjectId == IssueSetting.ProjectId);
                if (iS != null)
                {
                    return "Already have this Issue Setting in project";
                }
                return "Can Add";
            }
        }

        public IssueSetting GetIssueSettingById(int id)
        {
            return _context.IssueSettings.FirstOrDefault(i => i.Id == id);
        }
    }
}

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
            Project project = _context.Projects.Include(p => p.IssueSettings).FirstOrDefault(p => p.Id == projectId);
            List<IssueSetting> issueSettingList = new List<IssueSetting>();
            issueSettingList.AddRange(project.IssueSettings);
            Class c = _context.Classes.Include(c => c.IssueSettings).FirstOrDefault(c => c.Id == project.ClassId);
            issueSettingList.AddRange(c.IssueSettings.Where(i => i.ProjectId == null));
            return issueSettingList.OrderBy(p => p.Id).ToList();
        }

        public void AddIssueSetting(IssueSetting issueSetting)
        {
            _context.IssueSettings.Add(issueSetting);
            _context.SaveChanges();
        }

        public void UpdateIssueSetting(IssueSetting issueSetting)
        {
            _context.IssueSettings.Update(issueSetting);
            _context.SaveChanges();
        }

        public string CheckDuplicate(IssueSetting issueSetting)
        {
            if (issueSetting.ProjectId == null)
            {
                // issueSetting trên class so sánh trên class
                IssueSetting? iS = _context.IssueSettings.FirstOrDefault(i => i.Type == issueSetting.Type &&
                    i.Value == issueSetting.Value && i.ClassId == issueSetting.ClassId);
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
                // So sánh vs issueSetting trên class
                IssueSetting? iS = _context.IssueSettings.FirstOrDefault(i => i.Type == issueSetting.Type &&
                    i.Value == issueSetting.Value && i.ClassId == issueSetting.ClassId && i.ProjectId == null);
                if (iS != null)
                {
                    return "Already have this Issue Setting in class";
                }
                // IssueSetting ở project
                // So sánh vs issueSetting trong project
                iS = _context.IssueSettings.FirstOrDefault(i => i.Type == issueSetting.Type 
                    && i.Value == issueSetting.Value && i.ClassId == issueSetting.ClassId 
                    && i.ProjectId == issueSetting.ProjectId);
                if (iS != null)
                {
                    return "Already have this Issue Setting in project";
                }
                return "Can Add";
            }
        }
    }
}

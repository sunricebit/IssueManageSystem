namespace IMS.Services
{
    public class MilestoneService:IMilestoneService
    {
        private readonly IMSContext _context;
        public MilestoneService(IMSContext context)
        {
            _context = context;
        }
        public Milestone AddMilestone(Milestone milestone)
        {
            _context.Milestones.Add(milestone);
            _context.SaveChanges();
            return milestone;
        }
        public bool CheckMilestoneExist(int classid, int assginmentid, int subjectid)
        {
            return true;
        }
    }
}

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
        
        public Milestone GetMilestone(int id)
        {
            return _context.Milestones.FirstOrDefault(x => x.Id == id);
        }
        public bool CheckMilestoneExist(string name)
        {
            return true;
        }
        public void UpdateMilestone(Milestone milestone)
        {
            _context.Milestones.Update(milestone);
            _context.SaveChanges();
        }
    }
}

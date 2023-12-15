namespace IMS.Services
{
    public interface IMilestoneService
    {
        public Milestone AddMilestone(Milestone milestone);
        public bool CheckMilestoneExist(string name);
        public Milestone GetMilestone(int id);
        public void UpdateMilestone(Milestone milestone);

    }
}

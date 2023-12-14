namespace IMS.Services
{
    public interface IMilestoneService
    {
        public Milestone AddMilestone(Milestone milestone);
        public bool CheckMilestoneExist(int classid, int assginmentid, int subjectid);

    }
}

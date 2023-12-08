using System;
namespace IMS.Common
{
	public class Intermediate
    {
		public string? Error { get; set; }
		public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int SubjectManagerId { get; set; }

        public void Clear()
        {
            Error = null;
            Code = null;
            Name = null;
            Description = null;
            IsActive = false;
            SubjectManagerId = 0;
        }

    }
}


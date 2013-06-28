using System.Collections.Generic;

namespace EntityFrameworkTest.Model
{
    public class Arrangement : BusinessObject
    {
        public string Name { get; set; }
        public string BpNumber { get; set; }

        // Yes; we use a private collection setter, collections should not we re-assigned
        // because EntityFramework than cannot track the changes anymore.
        public virtual ICollection<ArrangementVersion> Versions { get; private set; }
        public virtual ICollection<User> Users { get; private set; }

        public Arrangement()
        {
            this.Versions = new List<ArrangementVersion>();
            this.Users = new List<User>();
        }
    }
}

using System.Collections.Generic;

namespace EntityFrameworkTest.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Arrangement : BusinessObject
    {
        public virtual string Name { get; set; }
        public virtual string BpNumber { get; set; }

        public virtual string LicensePlate { get; set; }

        // Yes; we use a private collection setter, collections should not we re-assigned
        // because EntityFramework than cannot track the changes anymore.
        public virtual ICollection<ArrangementVersion> Versions { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public Arrangement()
        {
            //this.Versions = new List<ArrangementVersion>();
            //this.Users = new List<User>();
        }
    }
}

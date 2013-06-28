using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EntityFrameworkTest.Model
{
    public class User : BusinessObject
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Privilege> Privileges { get; private set; }
        public virtual ICollection<Arrangement> Arrangements { get; private set; } 

        public User()
        {
            this.Privileges = new Collection<Privilege>();
            this.Arrangements = new Collection<Arrangement>();
        }
    }



}
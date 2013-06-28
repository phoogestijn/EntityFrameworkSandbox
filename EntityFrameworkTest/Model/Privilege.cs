using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EntityFrameworkTest.Model
{
    public class Privilege : BusinessObject
    {
        public string Name { get; set; }


        public ICollection<User> Users { get; private set; }

        public Privilege()
        {
            this.Users = new Collection<User>();
        }
    }
}

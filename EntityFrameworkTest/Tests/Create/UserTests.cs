using System.Linq;
using EntityFrameworkTest.Foundation.EF;
using EntityFrameworkTest.Foundation.EF.Repository;
using EntityFrameworkTest.Model;
using NUnit.Framework;

namespace EntityFrameworkTest.Tests.Create
{
    public class UserTests : EFTestBase
    {
        [Test]
        public void CreateUserWithoutArrangement()
        {
            using (DatabaseContext ctx = new DatabaseContext())
            {
                var userRepos = ctx.GetRepository<UserRepository>();
                var user = userRepos.Add();
                {
                    user.Email = "info@everest.nl";
                    user.FullName = "Everest BV";
                    user.UserName = "everest_nl";
                }

                ctx.SaveChanges();
            }
        }

        [Test]
        public void CreateUserWithMultipleArrangements()
        {
            Arrangement arr1, arr2;
            using (DatabaseContext ctx = new DatabaseContext())
            {
                var arrangementDao = ctx.GetRepository<ArrangementRepository>();
                {
                    arr1 = arrangementDao.Add();
                    arr1.BpNumber = "1";
                    arr1.Name = "Vendor 1";

                    arr2 = arrangementDao.Add();
                    arr2.BpNumber = "2";
                    arr2.Name = "Vendor 2";
                }
            
                var userDao = ctx.GetRepository<UserRepository>();
                var user = userDao.Add();
                {
                    user.FullName = "Everest BV";
                    user.Email = "info@everest.nl";
                    user.UserName = "everest_nl";

                    user.Arrangements.Add(arr1);
                    user.Arrangements.Add(arr2);
                }

                ctx.SaveChanges();
           
                {
                    Assert.AreEqual("Everest BV", user.FullName);
                    Assert.AreEqual("info@everest.nl", user.Email);
                    Assert.AreEqual("everest_nl", user.UserName);

                    Assert.AreEqual(2, user.Arrangements.Count());
                    Assert.AreEqual(1, arr1.Users.Count);             
                    Assert.AreEqual(1, arr2.Users.Count);             
                }
            }
        }
    }
}

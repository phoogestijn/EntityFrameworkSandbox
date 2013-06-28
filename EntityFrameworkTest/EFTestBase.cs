using EntityFrameworkTest.Foundation.EF;
using NUnit.Framework;

namespace EntityFrameworkTest
{
    [TestFixture]
    public abstract class EFTestBase
    {        
        [SetUp]
        public void Setup()
        {
            using (DatabaseContext ctx = new DatabaseContext())
            {
                ctx.Database.Delete();
                ctx.Database.Create();
            }
        }
    }
}

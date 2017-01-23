using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelAccess.Models;

namespace ModelTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateAndDeleteRoutingRule()
        {
            var dialplanRepo = new Repository<IDialplan>();
            var repo = new Repository<IRoutingRule>();
            var rule = repo.Add();
            rule.Dialplan = dialplanRepo.GetFromName("default");
            rule.Number = "123456";
            rule.Order = 2;
            rule.Time = 10;

            rule.Update();
            Assert.IsTrue(rule.Id > 0);

            rule.Delete();
            Assert.IsNull(repo.GetFromName("123456"));

        }
    }
}

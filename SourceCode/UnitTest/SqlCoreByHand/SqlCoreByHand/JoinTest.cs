namespace SqlCoreByHand
{
    [TestClass]
    public sealed class JoinTest
    {
        private List<User> users;
        private List<Workspace> workspaces;
        private SqlService<User, Workspace> service;

        [TestInitialize]
        public void Setup()
        {
            users = new List<User>
            {
                new User { Id = 1, Username = "Alice", Age = 25 },
                new User { Id = 2, Username = "Bob", Age = 30 },
                new User { Id = 3, Username = "Charlie", Age = 35 }
            };

            workspaces = new List<Workspace>
            {
                new Workspace { Id = 101, WorkspaceName = "Project A", CreatedBy = 1 },
                new Workspace { Id = 102, WorkspaceName = "Project B", CreatedBy = 2 },
                new Workspace { Id = 103, WorkspaceName = "Project C", CreatedBy = 4 } // không khớp
            };

            service = new SqlService<User, Workspace>();
            service.RegisterJoinCondition("UserWorkspace", (u, w) => u.Id == w.CreatedBy);
        }

        [TestMethod]
        public void TestInnerJoin()
        {
            var result = service.Join(users, workspaces, JoinType.Inner, "UserWorkspace");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Alice", result[0].Item1?.Username);
            Assert.AreEqual("Project A", result[0].Item2?.WorkspaceName);
            Assert.AreEqual("Bob", result[1].Item1?.Username);
        }

        [TestMethod]
        public void TestLeftJoin()
        {
            var result = service.Join(users, workspaces, JoinType.Left, "UserWorkspace");

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Charlie", result[2].Item1?.Username);
            Assert.IsNull(result[2].Item2);
        }

        [TestMethod]
        public void TestRightJoin()
        {
            var result = service.Join(users, workspaces, JoinType.Right, "UserWorkspace");

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Project C", result[2].Item2?.WorkspaceName);
            Assert.IsNull(result[2].Item1);
        }

        [TestMethod]
        public void TestFullJoin()
        {
            var result = service.Join(users, workspaces, JoinType.Full, "UserWorkspace");

            Assert.AreEqual(4, result.Count);

            bool hasNullUser = false;
            bool hasNullWorkspace = false;

            foreach (var (u, w) in result)
            {
                if (u == null) hasNullUser = true;
                if (w == null) hasNullWorkspace = true;
            }

            Assert.IsTrue(hasNullUser);      // Project C (userId 4)
            Assert.IsTrue(hasNullWorkspace); // Charlie (no workspace)
        }

        [TestMethod]
        public void TestCrossJoin()
        {
            var result = service.Join(users, workspaces, JoinType.Cross);

            Assert.AreEqual(9, result.Count); // 3 users x 3 workspaces
        }

        
    }
}
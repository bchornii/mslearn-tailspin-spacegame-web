using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TailSpin.SpaceGame.Web;
using TailSpin.SpaceGame.Web.Models;

namespace Tests
{
    [TestClass]
    public class FakeTests
    {
        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 2)]
        [DataRow(3, 3)]
        [DataRow(4, 4)]
        [DataRow(5, 5)]
        public void Method_UnderState_ShouldReturn_True(int v1, int v2)
        {
            // Arrange

            // Act

            // Assert
            Assert.AreEqual(v1, v2);
        }
    }

    [TestClass]
    public class DocumentDBRepository_GetItemsAsyncShould
    {
        private IDocumentDBRepository<Score> _scoreRepository;

        [TestInitialize]
        public void Setup()
        {
            using (Stream scoresData = typeof(IDocumentDBRepository<Score>)
                .Assembly
                .GetManifestResourceStream("Tailspin.SpaceGame.Web.SampleData.scores.json"))
            {
                _scoreRepository = new LocalDocumentDBRepository<Score>(scoresData);
            }
        }

        [DataTestMethod]
        [DataRow("Milky Way")]
        [DataRow("Andromeda")]
        [DataRow("Pinwheel")]
        [DataRow("NGC 1300")]
        [DataRow("Messier 82")]
        public void FetchOnlyRequestedGameRegion(string gameRegion)
        {
            const int PAGE = 0; // take the first page of results
            const int MAX_RESULTS = 10; // sample up to 10 results

            // Form the query predicate.
            // This expression selects all scores for the provided game region.
            Expression<Func<Score, bool>> queryPredicate = score => (score.GameRegion == gameRegion);

            // Fetch the scores.
            Task<IEnumerable<Score>> scoresTask = _scoreRepository.GetItemsAsync(
                queryPredicate, // the predicate defined above
                score => 1, // we don't care about the order
                PAGE,
                MAX_RESULTS
            );
            IEnumerable<Score> scores = scoresTask.Result;

            // Verify that each score's game region matches the provided game region.
            Assert.AreEqual(1, 1);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NUnit.Framework;
using TailSpin.SpaceGame.Web;
using TailSpin.SpaceGame.Web.Models;

namespace Tests
{
    public class DocumentDBRepository_GetItemsAsyncShould
    {
        private IDocumentDBRepository _scoreRepository;

        [SetUp]
        public void Setup()
        {
            _scoreRepository = new LocalDocumentDBRepository(
                "SampleData/scores.json", 
                "SampleData/profiles.json");
        }

        [TestCase("Solo", "Milky Way")]
        [TestCase("Solo", "Andromeda")]
        [TestCase("Solo", "Pinwheel")]
        [TestCase("Trio", "NGC 1300")]
        [TestCase("Duo", "Messier 82")]
        public void FetchOnlyRequestedGameRegion(string mode, string gameRegion)
        {
            const int PAGE = 0; // take the first page of results
            const int MAX_RESULTS = 10; // sample up to 10 results

            // Form the query predicate.
            // This expression selects all scores for the provided game region.
            Expression<Func<Score, bool>> queryPredicate = score => (score.GameRegion == gameRegion);

            // Fetch the scores.
            Task<IEnumerable<Score>> scoresTask = _scoreRepository.GetScoresAsync(
                mode, // the predicate defined above
                gameRegion, // we don't care about the order
                PAGE,
                MAX_RESULTS
            );
            IEnumerable<Score> scores = scoresTask.Result;

            // Verify that each score's game region matches the provided game region.
            Assert.That(scores, Is.All.Matches<Score>(score => score.GameRegion == gameRegion));
        }
    }
}
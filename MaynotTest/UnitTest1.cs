using MaynotModel;
using MaynotPersistence;
using Moq;
using System.Data;
using System.Text.Json;

namespace MaynotTest
{
    public class MockPersistence : IPersistence
    {
        public async Task SaveAsync(String path, MaynotGameState state)
        {
        }

        public async Task<MaynotGameState> LoadAsync(String path)
        {
            return new MaynotGameState(10);
        }
    }
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void NewGamePopulationIsZero()
        {

            MaynotGameModel model = new MaynotGameModel(new MockPersistence());
            model.newGame();
            Assert.IsTrue(model.Population == 0);
            Assert.Pass();
        }
    }
}
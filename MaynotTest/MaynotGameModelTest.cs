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

    [TestFixture]
    public class Tests
    {
        private Mock<IPersistence> _mock = null!;
        private MaynotGameModel _model = null!;
        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IPersistence>();
            _model = new MaynotGameModel(_mock.Object);
        }
        [Test]
        public void ConstructorTest()
        {
            _model.newGame();
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                    Assert.True(_model.GameBoard[i, j] is Tile);

        }
        [Test]
        public void NewGamePopulationIsZero()
        {
            _model.newGame();
            Assert.AreEqual(0,_model.Population);
            Assert.Pass();
        }
        [Test]
        public void PlaceRoadTest()
        {
            _model.newGame();
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    if (_model.GameBoard[i, j] is Empty)
                    {
                        Assert.AreEqual(true, _model.placeRoad(i, j));
                    }
                }
        }
        [Test]
        public void PlaceForestTest()
        {
            _model.newGame();
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    if (_model.GameBoard[i, j] is Empty)
                    {
                        Assert.AreEqual(true, _model.placeForest(i, j));
                    }
                }
        }
        [Test]
        public void PlacePoliceStationTest()
        {
            _model.newGame();
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    if (_model.GameBoard[i, j] is Empty)
                    {
                        Assert.AreEqual(true, _model.placePoliceStation(i, j));
                    }
                }
        }
        [Test]
        public void DestroyRoadTest()
        {
            _model.newGame();
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    if (_model.GameBoard[i, j] is Empty)
                    {
                         _model.placeRoad(i, j);
                    }
                }
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    if (_model.GameBoard[i, j] is Road)
                    {
                        _model.destroyRoad(i, j);
                    }
                }
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    Assert.IsFalse(_model.GameBoard[i, j] is Road);
                }
        }
    }
}
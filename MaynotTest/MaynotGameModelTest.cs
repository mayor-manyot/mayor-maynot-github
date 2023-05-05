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
        public void NewGamePopulationIsZeroTest()
        {
            _model.newGame();
            Assert.AreEqual(0, _model.Population);
            Assert.Pass();
        }
        public void NewGameHasStartingRoadTest()
        {
            _model.newGame();
            bool hasStartingRoad = false;
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    if (_model.GameBoard[i, j] is Road)
                    {
                        hasStartingRoad = true;
                    }
                }
            Assert.IsTrue(hasStartingRoad);
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
        public void PlaceIndustrialZoneTest()
        {
            _model.newGame();
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    if (_model.GameBoard[i, j] is Empty)
                    {
                        _model.placeIndustrialZone(i, j);
                    }
                }
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    if (_model.GameBoard[i, j] is not Forest && _model.GameBoard[i, j] is not Road)
                    {
                        Assert.IsTrue(_model.GameBoard[i, j] is IndustrialZone);
                    }
                }
        }
        [Test]
        public void PlaceResidentialZoneTest()
        {
            _model.newGame();
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    if (_model.GameBoard[i, j] is Empty)
                    {
                        _model.placeResidentialZone(i, j);
                    }
                }
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    if (_model.GameBoard[i, j] is not Forest && _model.GameBoard[i, j] is not Road)
                    {
                        Assert.IsTrue(_model.GameBoard[i, j] is ResidentialZone);
                    }
                }
        }
        [Test]
        public void PlaceServiceZoneTest()
        {
            _model.newGame();
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    if (_model.GameBoard[i, j] is Empty)
                    {
                        _model.placeServiceZone(i, j);
                    }
                }
            for (int i = 0; i < _model.GameBoardSize; i++)
                for (int j = 0; j < _model.GameBoardSize; j++)
                {
                    if (_model.GameBoard[i, j] is not Forest && _model.GameBoard[i, j] is not Road)
                    {
                        Assert.IsTrue(_model.GameBoard[i, j] is ServiceZone);
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
        [Test]
        public void SpeedUpTimeTest()
        {
            _model.newGame();
            _model.speedUpTime();
            Assert.AreEqual(2, _model.Speed);
            _model.speedUpTime();
            Assert.AreEqual(3, _model.Speed);
        }
        [Test]
        public void SlowerTimeTest()
        {
            _model.newGame();
            _model.speedUpTime();
            Assert.AreEqual(2, _model.Speed);
            _model.speedUpTime();
            Assert.AreEqual(3, _model.Speed);
            _model.slowTime();
            Assert.AreEqual(2, _model.Speed);
            _model.slowTime();
            Assert.AreEqual(1, _model.Speed);
        }
        [Test]
        public void StopAndResumeTimeTest()
        {
            _model.newGame();
            _model.speedUpTime();
            Assert.AreEqual(2, _model.Speed);
            _model.stopTime();
            Assert.AreEqual(0, _model.Speed);
            _model.resumeTime();
            Assert.AreEqual(2, _model.Speed);
        }
    }
}
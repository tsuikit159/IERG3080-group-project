using NUnit.Framework;

namespace Project.Tests
{
    [TestFixture]
    public class CircleTests
    {
        [Test]
        public void CheckCollision_ShouldReturnTrue_WhenCirclesOverlap()
        {
            // Arrange
            var circle1 = new Ellipse();
            var circle2 = new Ellipse();

            // Act
            bool result = MainWindow.CheckCollision(circle1, circle2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckCollision_ShouldReturnFalse_WhenCirclesDoNotOverlap()
        {
            // Arrange
            var circle1 = new Ellipse();
            var circle2 = new Ellipse();

            // Act
            bool result = MainWindow.CheckCollision(circle1, circle2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GenerateCircle_ShouldIncreasePlayerSize_WhenCircleCollides()
        {
            // Arrange
            var player = new Ellipse();
            var circle = new Ellipse();

            // Act
            MainWindow.GenerateCircle(player, circle);

            // Assert
            Assert.IsTrue(player.Width > circle.Width);
        }

        [Test]
        public void GenerateCircle_ShouldNotIncreasePlayerSize_WhenCircleDoesNotCollide()
        {
            // Arrange
            var player = new Ellipse();
            var circle = new Ellipse();

            // Act
            MainWindow.GenerateCircle(player, circle);

            // Assert
            Assert.IsFalse(player.Width > circle.Width);
        }
    }
}

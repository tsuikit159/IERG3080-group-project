using NUnit.Framework;

namespace Project.Tests
{
    [TestFixture]
    public class MainWindowTests
    {
        [Test]
        public void CheckCollision_ReturnsTrue_WhenEllipsesCollide()
        {
            // Arrange
            var mainWindow = new MainWindow();
            var ellipse1 = new Ellipse();
            var ellipse2 = new Ellipse();

            // Act
            bool result = mainWindow.CheckCollision(ellipse1, ellipse2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckCollision_ReturnsFalse_WhenEllipsesDoNotCollide()
        {
            // Arrange
            var mainWindow = new MainWindow();
            var ellipse1 = new Ellipse();
            var ellipse2 = new Ellipse();

            // Act
            bool result = mainWindow.CheckCollision(ellipse1, ellipse2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void MouseDown_SetsMousePressedToTrue_WhenLeftButtonIsPressed()
        {
            // Arrange
            var mainWindow = new MainWindow();
            var sender = new object();
            var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);

            // Act
            mainWindow.MouseDown(sender, e);

            // Assert
            Assert.IsTrue(mainWindow.MousePressed);
        }

        [Test]
        public void MouseUp_SetsMousePressedToFalse_WhenLeftButtonIsReleased()
        {
            // Arrange
            var mainWindow = new MainWindow();
            var sender = new object();
            var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);

            // Act
            mainWindow.MouseUp(sender, e);

            // Assert
            Assert.IsFalse(mainWindow.MousePressed);
        }

        [Test]
        public void Collide_IncreasesPlayerSize_WhenPlayerCollidesWithCircle()
        {
            // Arrange
            var mainWindow = new MainWindow();
            var player = new Ellipse();
            var circle = new Ellipse();
            player.Width = 10;
            player.Height = 10;
            circle.Width = 5;
            circle.Height = 5;
            mainWindow.GameScreen.Children.Add(player);
            mainWindow.GameScreen.Children.Add(circle);

            // Act
            mainWindow.collide();

            // Assert
            Assert.AreEqual(12, player.Width);
            Assert.AreEqual(12, player.Height);
        }

        [Test]
        public void Collide_RemovesCircle_WhenPlayerDoesNotCollideWithCircle()
        {
            // Arrange
            var mainWindow = new MainWindow();
            var player = new Ellipse();
            var circle = new Ellipse();
            player.Width = 10;
            player.Height = 10;
            circle.Width = 5;
            circle.Height = 5;
            mainWindow.GameScreen.Children.Add(player);
            mainWindow.GameScreen.Children.Add(circle);

            // Act
            mainWindow.collide();

            // Assert
            Assert.IsFalse(mainWindow.GameScreen.Children.Contains(circle));
        }

        [Test]
        public void Collide_IncreasesSunSize_WhenPlayerCollidesWithSun()
        {
            // Arrange
            var mainWindow = new MainWindow();
            var player = new Ellipse();
            var sun = new Ellipse();
            player.Width = 10;
            player.Height = 10;
            sun.Width = 5;
            sun.Height = 5;
            mainWindow.GameScreen.Children.Add(player);
            mainWindow.GameScreen.Children.Add(sun);

            // Act
            mainWindow.collide();

            // Assert
            Assert.AreEqual(7, sun.Width);
            Assert.AreEqual(7, sun.Height);
        }

        [Test]
        public void Collide_ShowsGameOverMessageBox_WhenPlayerCollidesWithSun()
        {
            // Arrange
            var mainWindow = new MainWindow();
            var player = new Ellipse();
            var sun = new Ellipse();
            player.Width = 10;
            player.Height = 10;
            sun.Width = 5;
            sun.Height = 5;
            mainWindow.GameScreen.Children.Add(player);
            mainWindow.GameScreen.Children.Add(sun);

            // Act
            mainWindow.collide();

            // Assert
            Assert.IsTrue(mainWindow.MessageBoxShown);
        }
    }
}

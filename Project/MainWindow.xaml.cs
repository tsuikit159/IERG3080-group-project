private void GameTick(object sender, EventArgs e)
{
    if (MousePressed)
    {
        Point oppositeLocation = new Point(Canvas.GetLeft(circleFactory) + circleFactory.Width / 2, Canvas.GetTop(circleFactory) + circleFactory.Height / 2);
        oppositeLocation.X = oppositeLocation.X - (Mouse.GetPosition(canvas).X - oppositeLocation.X);
        oppositeLocation.Y = oppositeLocation.Y - (Mouse.GetPosition(canvas).Y - oppositeLocation.Y);

        Circle smallCircle = new Circle(oppositeLocation, smallCircleRadius);
        canvas.Children.Add(smallCircle);
    }
}
private void UpdateTimer()
{
    // Create a new timer label
    Label timerLabel = new Label();
    timerLabel.Content = "00:00:00";
    timerLabel.FontSize = 20;
    timerLabel.HorizontalAlignment = HorizontalAlignment.Left;
    timerLabel.VerticalAlignment = VerticalAlignment.Center;
    timerLabel.Margin = new Thickness(10, 0, 0, 0);

    // Add the timer label to the left side of the screen
    Grid.SetColumn(timerLabel, 0);
    Grid.SetRow(timerLabel, 0);
    MainGrid.Children.Add(timerLabel);

    // Start the timer
    DispatcherTimer timer = new DispatcherTimer();
    timer.Interval = TimeSpan.FromSeconds(1);
    timer.Tick += (sender, e) =>
    {
        // Update the timer label with the current time
        timerLabel.Content = DateTime.Now.ToString("HH:mm:ss");
    };
    timer.Start();
}
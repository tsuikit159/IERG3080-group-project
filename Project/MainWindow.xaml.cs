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

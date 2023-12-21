using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
/*  @Author: Tsui Man Kit 
 *  @Purpose: Create a circle shape with circle factory 
 *  @usage : double radius = 10;
             IShapeFactory factory = new CircleFactory(radius);
             Shape circle = factory.CreateShape();
 */
namespace Circle
{
    public interface IShapeFactory
    {
        Shape CreateShape();
    }

    public class CircleFactory : IShapeFactory
    {
        private double radius;

        public CircleFactory(double radius)
        {
            this.radius = radius;
        }

        public Shape CreateShape()
        {
            Ellipse circle = new Ellipse()
            {
                Width = radius * 2,
                Height = radius * 2,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = Brushes.Black ,
                
            };
            circle.Tag = "circle";
            return circle;
        }
    }
}

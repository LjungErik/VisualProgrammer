using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VisualProgrammer.Controls
{
    public class ConnectionLink : FrameworkElement
    {
        #region Dependency Property/Event Definitions

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(PointCollection), typeof(ConnectionLink),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register("LineColor", typeof(SolidColorBrush), typeof(ConnectionLink),
                new FrameworkPropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty LineThicknessProperty = 
            DependencyProperty.Register("LineThickness", typeof(double), typeof(ConnectionLink),
                new FrameworkPropertyMetadata(2.0));

        #endregion Dependency Property/Event Definitions

        /// <summary>
        /// The intermediate points that make up the line between the start and the end.
        /// </summary>
        public PointCollection Points
        {
            get
            {
                return (PointCollection)GetValue(PointsProperty);
            }
            set
            {
                SetValue(PointsProperty, value);
            }
        }

        /// <summary>
        /// The color of the line that works as a connection
        /// </summary>
        public SolidColorBrush LineColor
        {
            get
            {
                return (SolidColorBrush)GetValue(LineColorProperty);
            }
            set
            {
                SetValue(LineColorProperty, value);
            }
        }

        public double LineThickness
        {
            get
            {
                return (double)GetValue(LineThicknessProperty);
            }
            set
            {
                SetValue(LineThicknessProperty, value);
            }
        }

        #region Private Methods

        protected override void OnRender(DrawingContext dc)
        {
            if(Points.Count >= 2){
                Pen linePen = new Pen(LineColor, LineThickness);
                //Draw the lines
                for(int i = 1; i < Points.Count; i++)
                {
                    dc.DrawLine(linePen, Points[i-1], Points[i]);
                }

                Point endPoint = Points[Points.Count-1];

                Rect rect = new Rect(endPoint.X - 7.0, (endPoint.Y - 14.0), 28.0, 28.0);

                BitmapImage img = new BitmapImage(new Uri("../../Resources/Images/connector.png", UriKind.Relative));

                dc.DrawImage(img, rect);
            }
        }

        #endregion Private Methods
    }
}

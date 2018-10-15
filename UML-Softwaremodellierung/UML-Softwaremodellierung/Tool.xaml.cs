using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UML_Softwaremodellierung
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    abstract public partial class Tool : UserControl
    {
        public TextBox textbox { get; }
        public Shape shape { get; }

        public Tool(int marginRight, Shape shape, string text, string name, string pointer)
        {
            InitializeComponent();

            grid.Name = pointer;
            grid.Margin = new Thickness(0, 0, 0, 0) { Right = marginRight };
            Width = 60;
            Height = 60;
            shape.Stroke = Brushes.Black;
            shape.StrokeThickness = 0.50;
            shape.Fill = Brushes.White;
            shape.VerticalAlignment = VerticalAlignment.Center;
            shape.HorizontalAlignment = HorizontalAlignment.Center;
            shape.Name = "sp_shape";
            grid.Children.Add(shape);

            TextBox textbox = new TextBox()
            {
                IsReadOnly = true,
                Text = text,
                FontSize = 10,
                Margin = new Thickness(0),
                Foreground = Brushes.Black,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Name = "tb_shape",
                IsEnabled = false,

            };

            grid.Children.Add(textbox);
        }

        public abstract Shape CreateShape();

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Package the data.
                DataObject data = new DataObject();
                data.SetData("Object", this);

                // Inititate the drag-and-drop operation.

                if (this.Parent is Canvas)
                {
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
                }
                else if (this.Parent is WrapPanel)
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
            }
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            // These Effects values are set in the drop target's
            // DragOver event handler.
            if (e.Effects.HasFlag(DragDropEffects.Copy))
            {
                Mouse.SetCursor(Cursors.Cross);
            }
            else if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                Mouse.SetCursor(Cursors.Pen);
            }
            else
            {
                Mouse.SetCursor(Cursors.No);
            }
            e.Handled = true;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            e.Handled = true;
        }
    }


    //
    // Shapes::Tool
    //

    public class RectangleTool : Tool
    {
        public RectangleTool(string name) : base(0, new Rectangle()
        {
            Width = 60,
            Height = 30,
        }, "Rechteck", "_rectangle", name)
        {
            //nothing / empty
        }

        public override Shape CreateShape()
        {
            throw new NotImplementedException();
        }
    }

    public class CircleTool : Tool
    {
        public CircleTool(string name) : base(0, new Ellipse()
        {
            Width = 35,
            Height = 35,
        }, "Kreis", "_circle", name)
        {
            //nothing / empty
        }

        //constructor
        public CircleTool() : base(0, new Ellipse()
        {
            Width = 35,
            Height = 35,
        }, "Kreis", "_circle", "grid")
        {
            //nothing / empty
        }


        public override Shape CreateShape()
        {
            throw new NotImplementedException();
        }
    }

    public class TriangleTool : Tool
    {
        static Polygon makeTriangle()
        {

            PointCollection polygonPoints = new PointCollection();
            polygonPoints.Add(new Point(0, 0));
            polygonPoints.Add(new Point(35, 17.5));
            polygonPoints.Add(new Point(0, 35));
            return new Polygon() { Points = polygonPoints };

        }
        public TriangleTool(string name) : base(0, makeTriangle(), "Dreieck", "_triangle", name)
        {
            //nothing / empty   <-- Textbox 
        }

        public override Shape CreateShape()
        {
            throw new NotImplementedException();
        }
    }

    public class CuboidTool : Tool
    {
        public CuboidTool(string name) : base(0, new Rectangle()
        {
            Width = 35,
            Height = 35,
        }, "Würfel", "_cuboid", name)
        {
            //nothing / empty
        }

        public override Shape CreateShape()
        {
            throw new NotImplementedException();
        }
    }
}


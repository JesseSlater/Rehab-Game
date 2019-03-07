using FitMi_Research_Puck;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace FitMi_Plotting_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private members

        private int my_selected_index = 0;
        private int max_elements = 500;

        private List<double> x_axis = new List<double>();
        private List<double> y_axis = new List<double>();
        private List<double> theta = new List<double>();
        private List<double> delta_theta = new List<double>();

        private List<int> data = new List<int>();
        private BackgroundWorker _worker = new BackgroundWorker();
        private List<string> items = new List<string>()
            {
                "Loadcell", "Accelerometer (x)", "Accelerometer (y)", "Accelerometer (z)",
                "Gyrometer (x)", "Gyrometer (y)", "Gyrometer (z)",
                "Magnetometer (x)", "Magnetometer (y)", "Magnetometer (z)",
                "Velocity (x)", "Velocity (y)", "Velocity (z)", "Theta", "Delta Theta"
            };

        #endregion

        #region Public properties

        public List<string> MyComboBoxItems
        {
            get
            {
                return items;
            }
        }

        public int MySelectedIndex
        {
            get
            {
                return my_selected_index;
            }
            set
            {
                my_selected_index = value;
                UpdateAxisLimits();
            }
        }

        public OxyPlot.PlotModel PlotModelObject { get; set; } = new OxyPlot.PlotModel();

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            data = Enumerable.Repeat(0, max_elements).ToList();

            InitializePlot();
            StartBackgroundTask();
            DataContext = this;
        }

        #endregion

        #region Background worker

        private void StartBackgroundTask ()
        {
            _worker.WorkerSupportsCancellation = true;
            _worker.WorkerReportsProgress = true;

            _worker.ProgressChanged += delegate
            {
                UpdatePlot();
            };

            _worker.DoWork += delegate
            {
                var puck = new HIDPuckDongle();
                puck.Open();
                puck.SendCommand(0, HidPuckCommands.SENDVEL, 0x00, 0x01);
                puck.SendCommand(1, HidPuckCommands.SENDVEL, 0x00, 0x01);

                while (!_worker.CancellationPending)
                {
                    puck.CheckForNewPuckData();
                    GetData(puck);

                    _worker.ReportProgress(0);

                    //Sleep the background thread so it doesn't consume the CPU
                    Thread.Sleep(33);
                }
            };

            _worker.RunWorkerAsync();
        }

        private void GetData (HIDPuckDongle puck)
        {
            int val = 0;
            string str_val = items[MySelectedIndex];
            
            if (str_val.Equals("Loadcell"))
            {
                val = puck.PuckPack0.Loadcell;
            }
            else if (str_val.Equals("Accelerometer (x)"))
            {
                val = puck.PuckPack0.Accelerometer[0];
            }
            else if (str_val.Equals("Accelerometer (y)"))
            {
                val = puck.PuckPack0.Accelerometer[1];
            }
            else if (str_val.Equals("Accelerometer (z)"))
            {
                val = puck.PuckPack0.Accelerometer[2];
            }
            else if (str_val.Equals("Gyrometer (x)"))
            {
                val = puck.PuckPack0.Gyrometer[0];
            }
            else if (str_val.Equals("Gyrometer (y)"))
            {
                val = puck.PuckPack0.Gyrometer[1];
            }
            else if (str_val.Equals("Gyrometer (z)"))
            {
                val = puck.PuckPack0.Gyrometer[2];
            }
            else if (str_val.Equals("Magnetometer (x)"))
            {
                val = Convert.ToInt32(puck.PuckPack0.Magnetometer[0]);
            }
            else if (str_val.Equals("Magnetometer (y)"))
            {
                val = Convert.ToInt32(puck.PuckPack0.Magnetometer[1]);
            }
            else if (str_val.Equals("Magnetometer (z)"))
            {
                val = Convert.ToInt32(puck.PuckPack0.Magnetometer[2]);
            }
            else if (str_val.Equals("Velocity (x)"))
            {
                val = puck.PuckPack0.Velocity[0];
            }
            else if (str_val.Equals("Velocity (y)"))
            {
                val = puck.PuckPack0.Velocity[1];
            }
            else if (str_val.Equals("Velocity (z)"))
            {
                val = puck.PuckPack0.Velocity[2];
            }

            double new_x = puck.PuckPack0.Gyrometer[1];
            double new_y = puck.PuckPack0.Gyrometer[0];
            double new_t = CartestianToPolar(new_x, new_y);

            if (theta.Count > 0)
            {
                double dt = new_t - theta.Last();
                if (Math.Abs(dt) >= 300)
                {
                    if (delta_theta.Count > 0)
                    {
                        dt = delta_theta.Last();
                    }
                    else
                    {
                        dt = 0;
                    }
                }

                delta_theta.Add(dt);
            }
            
            x_axis.Add(new_x);
            y_axis.Add(new_y);
            theta.Add(new_t);
            
            if (x_axis.Count >= max_elements)
            {
                int num_elem_to_remove = x_axis.Count - max_elements;
                x_axis.RemoveRange(0, num_elem_to_remove);
            }

            if (y_axis.Count >= max_elements)
            {
                int num_elem_to_remove = y_axis.Count - max_elements;
                y_axis.RemoveRange(0, num_elem_to_remove);
            }

            if (theta.Count >= max_elements)
            {
                int num_elem_to_remove = theta.Count - max_elements;
                theta.RemoveRange(0, num_elem_to_remove);
            }

            if (delta_theta.Count >= max_elements)
            {
                int num_elem_to_remove = delta_theta.Count - max_elements;
                delta_theta.RemoveRange(0, num_elem_to_remove);
            }

            data.Add(val);
            if (data.Count >= max_elements)
            {
                int num_elem_to_remove = data.Count - max_elements;
                data.RemoveRange(0, num_elem_to_remove);
            }
        }

        private const double RadiansToDegrees = (180.0 / Math.PI);

        private double CartestianToPolar (double x, double y)
        {
            //Given a cartesian coordinate, this returns an angle from 0 to 360
            double result = Math.Atan(y / x) * RadiansToDegrees;
            if (x < 0)
            {
                result += 180;
            }
            else if (y < 0)
            {
                result += 360;
            }

            return result;
        }

        #endregion

        #region Plotting

        private void UpdateAxisLimits ()
        {
            //Grab the y-axis
            var y_axis = PlotModelObject.Axes.Where(x => x.Position == OxyPlot.Axes.AxisPosition.Left).FirstOrDefault();
            if (y_axis != null)
            {
                string str_val = items[MySelectedIndex];
                if (str_val.Equals("Delta Theta"))
                {
                    y_axis.Minimum = -50;
                    y_axis.Maximum = 50;
                }
                else
                {
                    y_axis.Minimum = -1050;
                    y_axis.Maximum = 1050;
                }

                PlotModelObject.InvalidatePlot(true);
            }
        }

        private void UpdatePlot ()
        {
            var series = PlotModelObject.Series.FirstOrDefault() as OxyPlot.Series.LineSeries;
            if (series != null)
            {
                series.Points.Clear();

                string str_val = items[MySelectedIndex];
                if (str_val.Equals("Theta"))
                {
                    var points = theta.Select((y, x) => new OxyPlot.DataPoint(x, y)).ToList();
                    series.Points.AddRange(points);
                }
                else if (str_val.Equals("Delta Theta"))
                {
                    var points = delta_theta.Select((y, x) => new OxyPlot.DataPoint(x, y)).ToList();
                    series.Points.AddRange(points);
                }
                else
                {
                    var points = data.Select((y, x) => new OxyPlot.DataPoint(x, y)).ToList();
                    series.Points.AddRange(points);
                }
                
                PlotModelObject.InvalidatePlot(true);
            }
        }

        private void InitializePlot ()
        {
            //Set the borders of the plot
            PlotModelObject.PlotAreaBorderThickness = new OxyPlot.OxyThickness(0);
            PlotModelObject.PlotMargins = new OxyPlot.OxyThickness(0);

            //Set the axes
            PlotModelObject.Axes.Clear();

            var x_axis = new OxyPlot.Axes.LinearAxis()
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Minimum = 0,
                Maximum = max_elements
            };

            var y_axis = new OxyPlot.Axes.LinearAxis()
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Minimum = -1050,
                Maximum = 1050
            };

            PlotModelObject.Axes.Add(x_axis);
            PlotModelObject.Axes.Add(y_axis);

            //Create 2 series, one for each load cell
            PlotModelObject.Series.Clear();

            var s = new OxyPlot.Series.LineSeries()
            {
                Color = OxyPlot.OxyColors.CornflowerBlue
            };

            PlotModelObject.Series.Add(s);

            //Update the plot
            PlotModelObject.InvalidatePlot(true);
        }

        #endregion

        #region Window events

        private void Window_Closed(object sender, EventArgs e)
        {
            _worker.CancelAsync();
        }

        #endregion
    }
}

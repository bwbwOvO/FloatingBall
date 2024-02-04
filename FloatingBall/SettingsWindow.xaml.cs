using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using FloatingBall.Properties;
using System.Windows.Threading;

namespace FloatingBall
{
    public partial class SettingsWindow : Window
    {
        public static List<string> studentList = new List<string>();
        public bool repeatMode;
        public List<PopupWindow> popupWindows = new List<PopupWindow>();
        public MainWindow ParentMainWindow { get; set; }
        private Settings settings = new Settings();

        public SettingsWindow()
        {
            InitializeComponent();
            secondsSlider.ValueChanged += SecondsSlider_ValueChanged;
            sizeSlider.ValueChanged += SizeSlider_ValueChanged;
            LoadSettings();
            LoadStudentList();
        }

        public void LoadStudentList()
        {
            if (File.Exists("studentList.txt"))
            {
                studentList = new List<string>(File.ReadLines("studentList.txt"));
                studentListText.Text = $"已导入 {studentList.Count} 个学生";
            }
        }

        private void SaveStudentList()
        {
            File.WriteAllLines("studentList.txt", studentList);
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                studentList.Clear();
                foreach (string line in File.ReadLines(openFileDialog.FileName))
                {
                    studentList.Add(line);
                }
                studentListText.Text = $"已导入 {studentList.Count} 个学生";
                SaveStudentList();
            }
        }

        private void SecondsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int seconds = (int)secondsSlider.Value;
            secondsText.Text = seconds.ToString();
            settings.Seconds = seconds;
            settings.Save();
        }

        private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int size = (int)sizeSlider.Value;
            sizeText.Text = size.ToString();
            settings.Size = size;
            settings.Save();
        }

        public void LoadSettings()
        {
            secondsSlider.Value = settings.Seconds;
            sizeSlider.Value = settings.Size;
            colorComboBox.SelectedIndex = settings.ColorIndex;
            shapeComboBox.SelectedIndex = settings.ShapeIndex;
            repeatCheckBox.IsChecked = settings.RepeatMode;
        }
        /*
                public void CreatePopupWindow()
                {
                    PopupWindow popupWindow = new PopupWindow();
                    popupWindows.Add(popupWindow);
                    popupWindow.Show();
                }

                public int GetTotalSeconds()
                {
                    int total = 0;
                    foreach (PopupWindow popupWindow in popupWindows)
                    {
                        total += popupWindow.seconds;
                    }
                    return total;
                }*/

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            settings.Seconds = (int)secondsSlider.Value;
            settings.Size = (int)sizeSlider.Value;
            settings.ColorIndex = colorComboBox.SelectedIndex;
            settings.ShapeIndex = shapeComboBox.SelectedIndex;
            settings.RepeatMode = repeatMode = (bool)repeatCheckBox.IsChecked;

            ChangeBallParameters(settings.Seconds, settings.Size,
                (string)((ComboBoxItem)colorComboBox.SelectedItem).Content,
                (string)((ComboBoxItem)shapeComboBox.SelectedItem).Content);

            settings.Save();
            this.Close();
            // 关闭当前应用程序并启动新的实例
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }


        private void ChangeBallParameters(int seconds, int size, string color, string shape)
        {
            MainWindow ballWindow = (MainWindow)Application.Current.MainWindow;
            Shape ball = ballWindow.ball;
            DispatcherTimer timer = ballWindow.timer;
            UIElement oldBall;
            Shape newBall = null;
            foreach (var popupWindow in popupWindows)
            {
                popupWindow.seconds = seconds;
            }
            ball.Width = size;
            ball.Height = size;
            ball.Fill = GetColor(color);
            ballWindow.UpdateLayout();
            newBall = GetShape(shape, ball);
            if (newBall != null)
            {
                newBall.Width = ball.Width;
                newBall.Height = ball.Height;
                newBall.Fill = ball.Fill;
                newBall.Stroke = ball.Stroke;
                newBall.ContextMenu = ball.ContextMenu;
                newBall.MouseLeftButtonDown += ballWindow.Ball_MouseLeftButtonDown;
                oldBall = (UIElement)ballWindow.Content;
                ballWindow.Content = newBall;
            }
        }

        private Brush GetColor(string color)
        {
            switch (color)
            {
                case "红色": return Brushes.Red;
                case "橙色": return Brushes.Orange;
                case "黄色": return Brushes.Yellow;
                case "绿色": return Brushes.Green;
                case "青色": return Brushes.Cyan;
                case "蓝色": return Brushes.Blue;
                case "紫色": return Brushes.Purple;
                default: return Brushes.Black;
            }
        }

        private Shape GetShape(string shape, Shape ball)
        {
            switch (shape)
            {
                case "圆形": return new Ellipse();
                case "正方形": return new Rectangle();
                case "三角形":
                    Polygon triangle = new Polygon();
                    triangle.Points = new PointCollection();
                    triangle.Points.Add(new Point(ball.Width / 2, 0));
                    triangle.Points.Add(new Point(0, ball.Height));
                    triangle.Points.Add(new Point(ball.Width, ball.Height));
                    return triangle;
                case "星形":
                    Polygon star = new Polygon();
                    star.Points = new PointCollection();
                    star.Points.Add(new Point(ball.Width / 2, 0));
                    star.Points.Add(new Point(ball.Width * 0.62, ball.Height * 0.38));
                    star.Points.Add(new Point(ball.Width, ball.Height * 0.38));
                    star.Points.Add(new Point(ball.Width * 0.69, ball.Height * 0.62));
                    star.Points.Add(new Point(ball.Width * 0.81, ball.Height));
                    star.Points.Add(new Point(ball.Width / 2, ball.Height * 0.77));
                    star.Points.Add(new Point(ball.Width * 0.19, ball.Height));
                    star.Points.Add(new Point(ball.Width * 0.31, ball.Height * 0.62));
                    star.Points.Add(new Point(0, ball.Height * 0.38));
                    star.Points.Add(new Point(ball.Width * 0.38, ball.Height * 0.38));
                    return star;
                default: return null;
            }
        }
    }
}

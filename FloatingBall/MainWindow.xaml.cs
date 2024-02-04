// 这是 xaml.cs 的代码，保存为 FloatingBall.xaml.cs
using FloatingBall.Properties;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FloatingBall
{
    // 定义一个悬浮球的类，继承自 Window
    public partial class MainWindow : Window
    {
        // 添加一个字段来保存当前的形状对象
        public Shape CurrentShape { get; set; }

        // 在 mainwindow 的 Loaded 事件中，创建一个 Settings 类型的对象
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();

            // 使用 Settings 对象的属性来获取设置的值，并赋值给相应的变量
            int seconds = settings.Seconds; // 获取 Seconds 设置的值，并赋值给 seconds 变量
            int size = settings.Size; // 获取 Size 设置的值，并赋值给 size 变量
            int colorIndex = settings.ColorIndex; // 获取 ColorIndex 设置的值，并赋值给 colorIndex 变量
            int shapeIndex = settings.ShapeIndex; // 获取 ShapeIndex 设置的值，并赋值给 shapeIndex 变量
            // 使用读取的设置来更新界面
            ball.Width = size;
            ball.Height = size;

            // 根据 colorIndex 设置颜色
            switch (colorIndex)
            {
                case 0:
                    ball.Fill = Brushes.Red;
                    break;
                case 1:
                    ball.Fill = Brushes.Orange;
                    break;
                case 2:
                    ball.Fill = Brushes.Yellow;
                    break;
                case 3:
                    ball.Fill = Brushes.Green;
                    break;
                case 4:
                    ball.Fill = Brushes.Cyan;
                    break;
                case 5:
                    ball.Fill = Brushes.Blue;
                    break;
                case 6:
                    ball.Fill = Brushes.Purple;
                    break;
                default:
                    break;
            }

            // 加载形状设置
            switch (shapeIndex)
            {
                case 0:
                    CurrentShape = new Ellipse();
                    break;
                case 1:
                    CurrentShape = new Rectangle();
                    break;
                case 2:
                    CurrentShape = new Polygon();
                    ((Polygon)CurrentShape).Points = new PointCollection();
                    ((Polygon)CurrentShape).Points.Add(new Point(ball.Width / 2, 0));
                    ((Polygon)CurrentShape).Points.Add(new Point(0, ball.Height));
                    ((Polygon)CurrentShape).Points.Add(new Point(ball.Width, ball.Height));
                    break;
                case 3:
                    CurrentShape = new Polygon();
                    ((Polygon)CurrentShape).Points = new PointCollection();
                    ((Polygon)CurrentShape).Points.Add(new Point(ball.Width / 2, 0));
                    ((Polygon)CurrentShape).Points.Add(new Point(ball.Width * 0.62, ball.Height * 0.38));
                    ((Polygon)CurrentShape).Points.Add(new Point(ball.Width, ball.Height * 0.38));
                    ((Polygon)CurrentShape).Points.Add(new Point(ball.Width * 0.69, ball.Height * 0.62));
                    ((Polygon)CurrentShape).Points.Add(new Point(ball.Width * 0.81, ball.Height));
                    ((Polygon)CurrentShape).Points.Add(new Point(ball.Width / 2, ball.Height * 0.77));
                    ((Polygon)CurrentShape).Points.Add(new Point(ball.Width * 0.19, ball.Height));
                    ((Polygon)CurrentShape).Points.Add(new Point(ball.Width * 0.31, ball.Height * 0.62));
                    ((Polygon)CurrentShape).Points.Add(new Point(0, ball.Height * 0.38));
                    ((Polygon)CurrentShape).Points.Add(new Point(ball.Width * 0.38, ball.Height * 0.38));
                    break;
            }
            if (CurrentShape != null)
            {
                CurrentShape.Width = ball.Width;
                CurrentShape.Height = ball.Height;
                CurrentShape.Fill = ball.Fill;
                CurrentShape.Stroke = ball.Stroke;
                CurrentShape.ContextMenu = ball.ContextMenu;
                CurrentShape.MouseLeftButtonDown += Ball_MouseLeftButtonDown;

                // 将悬浮球的控件替换为新的控件
                this.Content = CurrentShape;
            }
        }

        // 定义一个随机数生成器
        private Random random = new Random();

        // 定义一个计时器，用来记录两次点击的间隔
        public DispatcherTimer timer = new DispatcherTimer();

        // 定义一个布尔值，用来标记是否已经点击过一次
        private bool isClicked = false;

        // 定义一个构造方法，初始化悬浮球的事件
        public MainWindow()
        {
            InitializeComponent();

            // 创建 SettingsWindow 实例
            SettingsWindow settingsWindow = new SettingsWindow();

            // 调用 LoadStudentList 方法
            settingsWindow.LoadStudentList();

            // 设置窗口的样式，使其无边框，透明，置顶，可拖动
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;
            this.Topmost = true;

            // 设置窗口的大小和调整模式
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;

            // 为窗口加载事件添加处理程序
            this.Loaded += MainWindow_Loaded;

            // 为悬浮球添加右键菜单和左键点击事件
            ball.ContextMenu = menu;
            ball.MouseLeftButtonDown += Ball_MouseLeftButtonDown;
        }

        // 当用户左键按下悬浮球时，触发此方法
        public void Ball_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 只有当鼠标左键是按下的状态时，才判断是否是双击事件
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // 如果已经点击过一次，且计时器还在运行，说明是双击事件
                if (isClicked && timer.IsEnabled)
                {
                    // 停止计时器
                    timer.Stop();

                    // 创建一个弹出窗口的实例，使用你提供的类
                    PopupWindow popup = new PopupWindow();

                    // 显示弹出窗口
                    popup.Show();
                }
                // 如果没有点击过一次，或者计时器已经停止，说明是第一次点击
                else
                {
                    // 设置计时器的间隔为 300 毫秒，这是双击的最大间隔
                    timer.Interval = TimeSpan.FromMilliseconds(300);

                    // 设置计时器的触发事件为一个方法，用来重置点击状态
                    timer.Tick += Timer_Tick;

                    // 启动计时器
                    timer.Start();

                    // 设置点击状态为真
                    isClicked = true;
                }

                // 调用 DragMove 方法，让悬浮球可以拖动
                this.DragMove();
            }
        }

        // 当计时器触发时，执行此方法
        private void Timer_Tick(object sender, EventArgs e)
        {
            // 停止计时器
            timer.Stop();

            // 重置点击状态
            isClicked = false;
        }
        // 当用户点击菜单项 "Change Color" 时，触发此方法
        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            // 生成一个随机的颜色
            byte r = (byte)random.Next(256);
            byte g = (byte)random.Next(256);
            byte b = (byte)random.Next(256);
            Color color = Color.FromRgb(r, g, b);

            // 将悬浮球的颜色设置为随机颜色
            ball.Fill = new SolidColorBrush(color);

            // 将当前形状的颜色也设置为随机颜色
            if (CurrentShape != null)
            {
                CurrentShape.Fill = new SolidColorBrush(color);
            }
        }


        // 当用户点击菜单项 "设置" 时，触发此方法
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            // 创建一个设置窗口的实例，使用你提供的类
            SettingsWindow settings = new SettingsWindow();

            // 将当前 MainWindow 的引用传递给 SettingsWindow
            settings.ParentMainWindow = this;

            // 显示设置窗口
            settings.Show();
        }

    }
}

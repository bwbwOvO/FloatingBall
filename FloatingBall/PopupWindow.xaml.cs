using FloatingBall.Properties;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace FloatingBall
{
    public partial class PopupWindow : Window
    {
        public DispatcherTimer timer;
        public int seconds;
        public List<string> askedStudents;
        private Random random;  // 将 Random 对象作为类的一个字段

        public PopupWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            seconds = Settings.Default.Seconds;
            timer.Start();

            askedStudents = new List<string>();
            random = new Random();  // 在类初始化时创建 Random 对象

            // 将 RandomAsk() 方法的调用移动到这里，确保在窗口创建时调用
            RandomAsk();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (seconds > 0)
            {
                countdown.Text = $"窗口将在 {seconds} 秒后关闭";
                seconds--;
            }
            else
            {
                timer.Stop();
                this.Close();
            }
        }

        private void RandomAsk()
        {
            if (SettingsWindow.studentList.Count > 0)
            {
                int index = random.Next(0, SettingsWindow.studentList.Count);  // 使用同一个 Random 对象
                string studentName = SettingsWindow.studentList[index];

                if (Settings.Default.RepeatMode)
                {
                    studentNameText.Text = $"你真幸运♥~ >{studentName}< (重复)";
                }
                else
                {
                    while (askedStudents.Contains(studentName))
                    {
                        index = random.Next(0, SettingsWindow.studentList.Count);
                        studentName = SettingsWindow.studentList[index];
                    }

                    studentNameText.Text = $"你真幸运♥~ >{studentName}< (不重复)";
                    askedStudents.Add(studentName);

                    if (askedStudents.Count == SettingsWindow.studentList.Count)
                    {
                        askedStudents.Clear();
                    }
                }
            }
        }
    }
}

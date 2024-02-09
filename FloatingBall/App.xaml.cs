using System;
using System.Threading;
using System.Windows;

namespace FloatingBall
{
    public partial class App : Application
    {
        // 定义一个互斥锁的名称，你可以自己修改，但要保证唯一性
        private const string MutexName = "FloatingBallMutex";

        // 定义一个互斥锁的变量
        private static Mutex mutex;

        // 定义一个程序启动的方法
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 尝试创建一个互斥锁，如果已经存在同名的互斥锁，返回 false
            bool createdNew = false;
            try
            {
                mutex = new Mutex(true, MutexName, out createdNew);
            }
            catch (Exception ex)
            {
                // 如果创建互斥锁失败，显示异常信息，并退出程序
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            // 如果已经存在同名的互斥锁，说明已经有一个程序在运行
            if (!createdNew)
            {
                // 显示提示信息，并退出程序
                MessageBox.Show("已经有一个程序在运行，请关闭后再试。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                Environment.Exit(0);
            }

            // 如果不存在同名的互斥锁，说明这是第一个运行的程序，继续执行
            // 你可以在这里添加你的程序逻辑
        }

        // 定义一个程序退出的方法
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // 如果互斥锁不为空，释放互斥锁
            if (mutex != null)
            {
                mutex.ReleaseMutex();
                mutex.Dispose();
            }
        }
    }
}

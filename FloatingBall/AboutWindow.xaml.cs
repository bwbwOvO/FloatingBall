using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FloatingBall
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 获取 Button 的 Tag 属性，它是一个网址
            string url = (sender as Button).Tag.ToString();
            // 使用 Windows 资源管理器打开网址
            System.Diagnostics.Process.Start("explorer.exe", url);
        }


    }
}

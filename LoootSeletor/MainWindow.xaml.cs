using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace LoootSeletor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnRun.Click += BtnRun_Click;
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("Chrome");
            var titles = processes.Select(x => x.MainWindowTitle).ToList();
            var title = titles.Find(x => x == "동행복권 - Chrome");

            Process p = processes[0];

            //NativeMethods.FindWindow();
            //Point pointToWindow = Mouse.GetPosition(this);

            NativeMethods.SetCursorPos(300, 400);

            Thread.Sleep(1000);

            NativeMethods.SetCursorPos(310, 410);
            //Mouse.GetPosition
            //Point pointToWindow = Mouse.GetPosition(this);
            //Point pointToScreen = PointToScreen(pointToWindow);
            ////label.Content = pointToScreen.ToString();

            //int x = (int)pointToScreen.X + 10;
            //int y = (int)pointToScreen.Y + 10;

            //NativeMethods.SetCursorPos(x, y);

            return;

            List<int[]> lstnumbers = GetLottoNumbers();

            foreach (int[] n in lstnumbers)
            {
                SetCheckWebPage(n);

                //완료되면 체크
            }

            // 그리고 난 뒤 완료까지 마무리
        }

        private void SetCheckWebPage(int[] arrs)
        {
            Mouse.Capture(this);
            Point pointToWindow = Mouse.GetPosition(this);
            Point pointToScreen = PointToScreen(pointToWindow);
            //label.Content = pointToScreen.ToString();

            int x = (int)pointToScreen.X + 1;
            int y = (int)pointToScreen.Y;

            NativeMethods.SetCursorPos(x, y);
            

            Mouse.Capture(null);
        }

        private List<int[]> GetLottoNumbers()
        {
            var strInputs = tbnumbers.Text;
            var splittedString = strInputs.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            List<int[]> arrLotto = new List<int[]>();

            foreach (var input in splittedString)
            {
                if (true == input.Contains("pjh2104_msg_bo")) continue;

                var strarrNums = input.Split(",");

                int[] arrNums = new int[6];
                for (int i = 0; i < strarrNums.Length; i++)
                {
                    arrNums[i] = Convert.ToInt32(strarrNums[i]);
                }

                arrLotto.Add(arrNums);
            }

            return arrLotto;
        }

        public partial class NativeMethods
        {
            /// Return Type: BOOL->int
            ///X: int
            ///Y: int
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetCursorPos")]
            [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool SetCursorPos(int X, int Y);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr FindWindow(string className, string windowName);
        }
    }
}
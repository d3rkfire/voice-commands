using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using NAudio.Wave;

namespace VoiceCommands
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    class VoiceCommand
    {
        public string Cmd { set; get; }
        public string[] SubCmd { set; get; }
        public string Display { set; get; }

        public VoiceCommand(string cmd)
        {
            Cmd = cmd;
            try
            {
                SubCmd = Directory.GetDirectories(Cmd);
                Display = Cmd.Substring(Cmd.LastIndexOf('.') + 2) + ":\n";
                foreach (string sub in SubCmd) Display += sub.Substring(sub.LastIndexOf("\\") + 1) + "\n";
            }
            catch (Exception e) { }
        }
    }


    public partial class MainWindow : Window
    {
        private GlobalKeyboardListener listener;
        private bool isMainMenu = true;
        private int currentMenu;
        private string voiceFolder;
        private Random rand = new Random();
        private WaveOut waveOut = new WaveOut();

        private List<VoiceCommand> vCmd = new List<VoiceCommand>();
        private string mainMenu;

        private Dictionary<Key, int> keyToNum = new Dictionary<Key, int>() {
            { Key.NumPad0, 0 },
            { Key.NumPad1, 1 },
            { Key.NumPad2, 2 },
            { Key.NumPad3, 3 },
            { Key.NumPad4, 4 },
            { Key.NumPad5, 5 },
            { Key.NumPad6, 6 },
            { Key.NumPad7, 7 },
            { Key.NumPad8, 8 },
            { Key.NumPad9, 9 },
            { Key.Add, 10 }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desk = System.Windows.SystemParameters.WorkArea;
            this.Left = desk.Right - this.Width;
            this.Top = desk.Bottom / 2;

            findVBAudio();

            string[] allVoiceFolders = Directory.GetDirectories("./");
            foreach (string a in allVoiceFolders) cboVoiceFolder.Items.Add(a.Replace("./", ""));
            cboVoiceFolder.SelectedIndex = 0;

            listener = new GlobalKeyboardListener();
            listener.OnKeyPressed += _listener_OnKeyPressed;
            listener.HookKeyboard();
        }

        private void findVBAudio()
        {
            int i;
            for (i = 0; i < WaveOut.DeviceCount; i++)
                if (WaveOut.GetCapabilities(i).ProductName.Contains("VB-Audio")) waveOut.DeviceNumber = i;
        }

        private void runVoice(int curMenu, int cmd)
        {
            string[] allVoices = Directory.GetFiles(vCmd.ElementAt(curMenu).SubCmd[cmd]);

            if (allVoices.Length > 0)
            {
                int chosen = rand.Next(allVoices.Length);
                try
                {
                    waveOut.Stop();
                    waveOut.Init(new WaveFileReader(allVoices[chosen]));
                    waveOut.Play();
                }
                catch (Exception e) { }
            }
        }

        void _listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            if (keyToNum.ContainsKey(e.KeyPressed))
            {
                if (isMainMenu && keyToNum[e.KeyPressed] >= 1 && keyToNum[e.KeyPressed] <= vCmd.Count)
                {
                    currentMenu = keyToNum[e.KeyPressed] - 1;
                    isMainMenu = false;
                    txtMain.Text = vCmd.ElementAt(currentMenu).Display;
                }
                else if (!isMainMenu && keyToNum[e.KeyPressed] != 10)
                {
                    if (keyToNum[e.KeyPressed] >= 1 && keyToNum[e.KeyPressed] <= vCmd.ElementAt(currentMenu).SubCmd.Length) runVoice(currentMenu, keyToNum[e.KeyPressed] - 1);
                    txtMain.Text = mainMenu;
                    isMainMenu = true;
                }
                else if (keyToNum[e.KeyPressed] == 10)
                {
                    this.Close();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listener.UnhookKeyboard();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void cboVoiceFolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            voiceFolder = "./" + cboVoiceFolder.SelectedItem;

            // Set All Commands of New Voice Directory
            vCmd.Clear();
            string[] allCmd = Directory.GetDirectories(voiceFolder);
            foreach (string cmd in allCmd) vCmd.Add(new VoiceCommand(cmd));

            // Set Main Menu Text
            mainMenu = "Main Menu:\n";
            for (int i = 0; i < vCmd.Count; i++) mainMenu += vCmd.ElementAt(i).Cmd.Replace(voiceFolder + "\\", "") + "\n";
            mainMenu += "\n+. Exit";

            isMainMenu = true;
            txtMain.Text = mainMenu;
        }
    }
}

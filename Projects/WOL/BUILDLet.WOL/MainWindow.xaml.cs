/*******************************************************************************
 The MIT License (MIT)

 Copyright (c) 2015-2017 Daiki Sakamoto

 Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files (the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
  all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
  THE SOFTWARE.
********************************************************************************/
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

using System.IO;
using System.Collections.ObjectModel;
using System.Reflection;

using BUILDLet.Utilities;
using BUILDLet.Utilities.Network;


namespace BUILDLet.WOL
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int sendCount = int.Parse(Properties.Resources.SendCount);
        private readonly int historyCount = int.Parse(Properties.Resources.HistoryCount);
        private readonly string sendMessage = Properties.Resources.SendMessage;
        private readonly string configFileName = Properties.Resources.ConfigurationFileName;
        private readonly string configFileDirName = Properties.Resources.ConfigurationFileFolderName;
        private readonly string configFileUtilitiesDir = Properties.Resources.ConfigurationFileUtilitiesFolder;

        // Configuration File Path
        private string configFilePath;

        // MAC Address List
        public static ObservableCollection<string> Addresses { get; protected set; }


        public MainWindow()
        {
            try
            {
                // Load MAC Address List
                // (Should be called before InitializeComponent())
                this.loadMacAddressList();

                InitializeComponent();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // (Send Button) Click Event Handler
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Magic Packet
                MagicPacket packet = new MagicPacket((string)MacAddressComboBox.Text);

                // Send Magic Packet
                packet.Send(this.sendCount);

                // Show message
                MessageBox.Show(string.Format(this.sendMessage, packet.MacAddress, this.sendCount), App.Name, MessageBoxButton.OK, MessageBoxImage.Information);

                // Save Updated MAC Address List
                this.saveMacAddressList();

                // Close MainWindow
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // (Window) KeyDown Event Handler
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { this.Close(); }
        }


        // Load Default MAC Address List
        private void loadMacAddressList()
        {
            MainWindow.Addresses = new ObservableCollection<string>();

            try
            {
                SimpleFileFinder finder = new SimpleFileFinder();

                // Clear & Set Search Path
                finder.SearchPath.Clear();
                finder.SearchPath.AddRange(new List<string> {
                    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), this.configFileDirName),
                    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), this.configFileUtilitiesDir),
                    System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), this.configFileUtilitiesDir),
                    Environment.CurrentDirectory
                });

                // Find File Path
                string[] files = finder.Find(this.configFileName);

                if (files != null)
                {
                    // Set File Path
                    this.configFilePath = files[0];

                    // Read File Stream
                    using (StreamReader sr = new StreamReader(this.configFilePath))
                    {
                        int addressLength = ("FF:FF:FF:FF:FF:FF").Length;
                        int lines = 0;
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            // Maximum count
                            if (++lines > this.historyCount) { break; }

                            // Append List
                            if (line.Length > addressLength) { MainWindow.Addresses.Add(line.Substring(0, addressLength)); }
                            else { MainWindow.Addresses.Add(line); }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Save Updated MAC Address List
        private void saveMacAddressList()
        {
            try
            {
                string current = this.MacAddressComboBox.Text;
                int index = MainWindow.Addresses.IndexOf(current);

                if (index >= 0)
                {
                    // Remove item, if selected MAC Address is already included.
                    MainWindow.Addresses.RemoveAt(index);
                }

                // Insert selected MAC Address
                MainWindow.Addresses.Insert(0, current);

                // Remove last item, if over
                if (MainWindow.Addresses.Count > this.historyCount)
                {
                    MainWindow.Addresses.RemoveAt(MainWindow.Addresses.Count - 1);
                }
                
                // Write to Configuration File
                if (!string.IsNullOrEmpty(this.configFilePath))
                {
                    using (StreamWriter sw = new StreamWriter(this.configFilePath))
                    {
                        foreach (var item in MainWindow.Addresses)
                        {
                            sw.WriteLine(item);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

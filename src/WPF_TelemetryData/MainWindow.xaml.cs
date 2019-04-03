using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Autofac;
using LogicLayer;
using LogicLayer.Model;
using Microsoft.Win32;

namespace WPF_TelemetryData
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IJsonParser parser;
        private readonly IJsonFileWriter writer;

        private Dictionary<string, ProcessedData> currentDay;


        public MainWindow()
        {
            InitializeComponent();

            var container = IoCBuilder.Build();

            parser = container.Resolve<IJsonParser>();
            writer = container.Resolve<IJsonFileWriter>();
        }

        private Dictionary<string, Dictionary<string, ProcessedData>> ParsedData { get; set; } =
            new Dictionary<string, Dictionary<string, ProcessedData>>();

        private ProcessedData[] UnorganizedProcessedData { get; set; }

        private async void OnLoadFileButtonClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog {Filter = "Json (*.json|*.json"};


            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            ParsedData = new Dictionary<string, Dictionary<string, ProcessedData>>();

            try
            {
                LabelFileName.Content = openFileDialog.SafeFileName;
                LabelFileName.Foreground = new SolidColorBrush(Colors.Black);
                var data = await parser.ParseFile(openFileDialog.FileName, UpDownRowsToSkip.Value.Value)
                    .ConfigureAwait(false);
                UnorganizedProcessedData = data.ToArray();
                ParsedData = await Utilities.PrepareDictionaries(UnorganizedProcessedData).ConfigureAwait(false);

                ListViewDate.ItemsSource = ParsedData.Keys;
                ListViewHour.ItemsSource = null;
                TextBoxData.Text = null;
                SaveButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                LabelFileName.Content = "Error! Can not read this file!";
                LabelFileName.Foreground = new SolidColorBrush(Colors.Red);
                ParsedData = new Dictionary<string, Dictionary<string, ProcessedData>>();
                SaveButton.IsEnabled = false;
                ListViewDate.ItemsSource = null;
                ListViewHour.ItemsSource = null;
                TextBoxData.Text = null;
            }
        }

        private void OnDateListItemClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView)?.SelectedItem;
            if (item == null)
            {
                return;
            }

            currentDay = ParsedData[item.ToString()];
            ListViewHour.ItemsSource = currentDay.Keys;
        }

        private void OnDataSelected(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView)?.SelectedItem;
            if (item != null)
            {
                TextBoxData.Text = currentDay[item.ToString()].ToJson();
            }
        }

        private void OnSaveFileButtonClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog {Filter = "Json (*.json|*.json"};


            if (saveFileDialog.ShowDialog() == true)
            {
                writer.Write(saveFileDialog.FileName, new ProcessedDataToWrite {Data = UnorganizedProcessedData});
            }
        }
    }
}
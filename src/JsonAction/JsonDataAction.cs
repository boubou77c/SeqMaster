using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace SeqMaster.JsonAction
{
    internal class JsonDataAction
    {
        private OpenFileDialog Opendialog;
        private SaveFileDialog SaveFileDialog;

        public void SaveData(DataSaved data)
        {
            string json = JsonConvert.SerializeObject(data);

            SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.Filter = "json | *.json";
            if(SaveFileDialog.ShowDialog() == true)
            {
                string filePath = SaveFileDialog.FileName;
                File.WriteAllText(filePath, json);
                MessageBox.Show("File Saved successfully");

            }

        }

        public DataSaved LoadData()
        {
            Opendialog = new OpenFileDialog();
            if(Opendialog.ShowDialog() == true)
            {
                string filePath = Opendialog.FileName;
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    try
                    {
                        return JsonConvert.DeserializeObject<DataSaved>(json);

                    }
                    catch (Newtonsoft.Json.JsonReaderException) { }
                }
            }
            return null;

        }

        public DataSaved LoadSettings()
        {
            string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json"); // Dynamic path

            if (File.Exists(settingsPath))
            {
                string json = File.ReadAllText(settingsPath);
                return JsonConvert.DeserializeObject<DataSaved>(json);
            }

            return null;
        }

        public void SaveSettings(DataSaved data)
        {
            string json = JsonConvert.SerializeObject(data);

            string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json"); // Dynamic path
            File.WriteAllText(settingsPath, json);
        }

    }
}

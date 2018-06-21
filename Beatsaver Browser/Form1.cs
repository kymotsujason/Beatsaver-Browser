using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace Beatsaver_Browser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            checkFolder();
        }

        private void checkFolder()
        {
            string path = "./test";
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    //Console.WriteLine("That path exists already.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                //Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

                // Delete the directory.
                //di.Delete();
                //Console.WriteLine("The directory was deleted successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image image = null;
            for (int num = 14; num < 24; num++)
            {
                Uri HostURI = new Uri("https://beatsaver.com/api.php?mode=details&id=" + num);
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(HostURI);
                    var obj = JsonConvert.DeserializeObject<List<RootObject>>(json);
                    if (obj.Count() > 0)
                    {
                        dataGridView1.Rows.Add(Action.FalseValue, image, obj[0].id, obj[0].beatname, obj[0].songName, obj[0].beatsPerMinute, obj[0].difficultyLevels, obj[0].downloads, obj[0].plays, obj[0].upvotes, obj[0].uploader, obj[0].beattext, "https://beatsaver.com/details.php?id=" + obj[0].id);
                    }
                }
            }
            dataGridView1.Update();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_OnCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == Action.Index && e.RowIndex != -1)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                    if (chk.Value == chk.TrueValue)
                    {
                        chk.Value = chk.FalseValue;
                    }
                    else
                    {
                        chk.Value = chk.TrueValue;
                    }
                }
            }
        }

        private void dataGridView1_OnCellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // End of edition on each click on column of checkbox
            if (e.ColumnIndex == Action.Index && e.RowIndex != -1)
            {
                dataGridView1.EndEdit();
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            List<string> downloadID = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value == chk.TrueValue)
                {
                    chk.Value = chk.FalseValue;
                    downloadID.Add(dataGridView1.Rows[row.Index].Cells[2].Value.ToString());
                }
            }

            List<string> urlList = new List<string>();
            for (int i = 0; i < downloadID.Count(); i++)
            {
                urlList.Add("https://beatsaver.com/dl.php?id=" + downloadID[i]);
            }

            await DownloadFiles(urlList, downloadID);

            for (int i = 0; i < downloadID.Count(); i++)
            {
                string name = "./test/" + downloadID[i] + ".zip";
                string extractPath = @"C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\CustomSongs";
                ZipFile.ExtractToDirectory(name, extractPath);
            }
            /*for (int i = 0; i < downloadID.Count(); i++)
            {
                string name = "./test/" + downloadID[i] + ".zip";
                using (var client = new WebClient())
                {
                    Uri path = new Uri("https://beatsaver.com/dl.php?id=" + downloadID[i]);
                    
                    client.DownloadFileAsync(path, name);
                    
                }
                
                string extractPath = @"C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\CustomSongs";
                ZipFile.ExtractToDirectory(name, extractPath);
            }*/
        }

        private async Task DownloadFile(string url, string currentID)
        {

            using (var client = new WebClient())
            {
                string name = "./test/" + currentID + ".zip";
                await client.DownloadFileTaskAsync(url, name);
            }
        }

        private async Task DownloadFiles(IEnumerable<string> urlList, IEnumerable<string> downloadID)
        {
            using (var url = urlList.GetEnumerator())
            using (var ID1 = downloadID.GetEnumerator())
            {
                while (url.MoveNext() && ID1.MoveNext())
                {
                    var currentURL = url.Current;
                    var currentID = ID1.Current;

                    await DownloadFile(currentURL, currentID);
                }
            }
                //foreach (var url in urlList)
                //{
                    //await DownloadFile(url);
                //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

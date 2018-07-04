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
        String dataPath = @".\Beatsaver_Browser_Data\";
        String zipsPath = @".\Beatsaver_Browser_Data\zips\";
        String imgPath = @".\Beatsaver_Browser_Data\img\";
        String extractPath = @"C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\CustomSongs\";
        List<Beatmap> jsonInfo = new List<Beatmap>();
        List<String> listID = new List<string>();

        public Form1()
        {
            InitializeComponent();
            CheckFolder();
            PopulateList();
            UpdateList();
        }

        private void CheckFolder()
        {
            // Determine whether the directory exists.
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            if (!Directory.Exists(zipsPath))
            {
                Directory.CreateDirectory(zipsPath);
            }
            if (!Directory.Exists(imgPath))
            {
                Directory.CreateDirectory(imgPath);
            }
        }

        private void PopulateList()
        {
            string beatmapJSON = dataPath + "beatmaps.json";
            if (File.Exists(beatmapJSON))
            {
                var json = File.ReadAllText(beatmapJSON);
                var objList = JsonConvert.DeserializeObject<List<Beatmap>>(json);
                foreach (var obj in objList)
                {
                    Image image = Image.FromFile(imgPath + obj.id + "." + obj.img);
                    dataGridView1.Rows.Add(Action.FalseValue, image, obj.id, obj.beatname,
                        obj.songName, obj.beatsPerMinute, obj.difficultyLevels, obj.downloads, obj.plays,
                        obj.upvotes, obj.uploader, obj.beattext, "https://beatsaver.com/details.php?id=" + obj.id);
                    listID.Add(obj.id);
                    dataGridView1.Update();
                }
            }
        }

        private void UpdateList()
        {
            string installTxt = dataPath + "install.txt";
            string downloadTxt = dataPath + "download.txt";
            List<String> installList = new List<string>();
            if (File.Exists(installTxt) && File.Exists(downloadTxt))
            {
                List<String> downloadID = File.ReadAllLines(downloadTxt).ToList();
                List<String> installID = File.ReadAllLines(installTxt).ToList();
                foreach (var item in installID)
                {
                    List<String> words = new List<String>(item.ToString().Split('|'));
                    installList.Add(words[0]);
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    String currentID = dataGridView1.Rows[row.Index].Cells[2].Value.ToString();
                    if (installList.Any(s => currentID.Equals(s)))
                    {
                        row.DefaultCellStyle.BackColor = Color.Green;
                    }
                    else if (downloadID.Any(s => currentID.Equals(s)))
                    {
                        row.DefaultCellStyle.BackColor = Color.Blue;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
            else if (File.Exists(installTxt))
            {
                List<String> installID = File.ReadAllLines(installTxt).ToList();
                foreach (var item in installID)
                {
                    List<String> words = new List<String>(item.ToString().Split('|'));
                    installList.Add(words[0]);
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    String currentID = dataGridView1.Rows[row.Index].Cells[2].Value.ToString();
                    if (installList.Any(s => currentID.Equals(s)))
                    {
                        row.DefaultCellStyle.BackColor = Color.Green;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
            else if (File.Exists(downloadTxt))
            {
                List<String> downloadID = File.ReadAllLines(downloadTxt).ToList();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    String currentID = dataGridView1.Rows[row.Index].Cells[2].Value.ToString();
                    if (downloadID.Any(s => currentID.Equals(s)))
                    {
                        row.DefaultCellStyle.BackColor = Color.Blue;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private async Task DownloadInfo(Uri url, String currentID)
        {
            using (WebClient wc = new WebClient())
            {
                var json = await wc.DownloadStringTaskAsync(url);
                var obj = JsonConvert.DeserializeObject<List<Beatmap>>(json);
                if (obj.Count() > 0)
                {
                    if (File.Exists(imgPath + currentID + "." + obj[0].img))
                    {
                        Image image = Image.FromFile(imgPath + currentID + "." + obj[0].img);
                        dataGridView1.Rows.Add(Action.FalseValue, image, obj[0].id, obj[0].beatname,
                            obj[0].songName, obj[0].beatsPerMinute, obj[0].difficultyLevels, obj[0].downloads, obj[0].plays,
                            obj[0].upvotes, obj[0].uploader, obj[0].beattext, "https://beatsaver.com/details.php?id=" + obj[0].id);
                    }
                    else
                    {
                        await DownloadImage(currentID, obj[0].img);
                        Image image = Image.FromFile(imgPath + currentID + "." + obj[0].img);
                        dataGridView1.Rows.Add(Action.FalseValue, image, obj[0].id, obj[0].beatname,
                            obj[0].songName, obj[0].beatsPerMinute, obj[0].difficultyLevels, obj[0].downloads, obj[0].plays,
                            obj[0].upvotes, obj[0].uploader, obj[0].beattext, "https://beatsaver.com/details.php?id=" + obj[0].id);
                    }
                    //sw.Write(JsonConvert.SerializeObject(obj.ToArray(), Formatting.Indented));
                    jsonInfo.Add(obj[0]);
                    listID.Add(obj[0].id);
                }
            }
            dataGridView1.Update();
        }

        private async Task DownloadInfo(IEnumerable<Uri> urlList, IEnumerable<String> downloadID)
        {
            using (var url = urlList.GetEnumerator())
            using (var ID1 = downloadID.GetEnumerator())
            {
                StreamWriter sw = new StreamWriter(dataPath + "beatmaps.json");
                while (url.MoveNext() && ID1.MoveNext())
                {
                    var currentURL = url.Current;
                    var currentID = ID1.Current;
                    if (listID.Any(s => currentID.Equals(s)))
                    {
                        continue;
                    }
                    await DownloadInfo(currentURL, currentID);
                }
                sw.Write(JsonConvert.SerializeObject(jsonInfo.ToArray(), Formatting.Indented));
                sw.Close();
            }
        }

        private async Task DownloadImage(String currentID, String ext)
        {
            using (var client = new WebClient())
            {
                String url = "https://beatsaver.com/img/" + currentID + "." + ext;
                String name = currentID + "." + ext;
                await client.DownloadFileTaskAsync(url, name);
                if (File.Exists(name) && Directory.Exists(imgPath))
                {
                    File.Move(name, imgPath + name);
                }
            }
        }

        private async Task DownloadFile(String url, String currentID, StreamWriter sw)
        {

            using (var client = new WebClient())
            {
                String name = currentID + ".zip";
                await client.DownloadFileTaskAsync(url, name);
                if (File.Exists(name) && Directory.Exists(zipsPath))
                {
                    File.Move(name, zipsPath + name);
                    sw.WriteLine(currentID);
                }
            }
        }

        private async Task DownloadFiles(IEnumerable<String> urlList, IEnumerable<String> downloadID)
        {
            using (var url = urlList.GetEnumerator())
            using (var ID1 = downloadID.GetEnumerator())
            {
                StreamWriter sw = new StreamWriter(dataPath + "download.txt", true, Encoding.ASCII);
                while (url.MoveNext() && ID1.MoveNext())
                {
                    var currentURL = url.Current;
                    var currentID = ID1.Current;

                    await DownloadFile(currentURL, currentID, sw);
                }
                sw.Close();
            }
        }

        private void DeleteFiles(List<String> downloadID)
        {
            StreamReader sr = new StreamReader(dataPath + "download.txt");
            for (int i = 0; i < downloadID.Count(); i++)
            {

            }
            List<String> keep = new List<string>();
            String line;
            line = sr.ReadLine();
            while (line != null && line != "")
            {
                // For any s in downloadID, if downloadID Equals s...
                if (downloadID.Any(s => line.Equals(s)))
                {
                    File.Delete(zipsPath + line + ".zip");
                }
                else
                {
                    keep.Add(line);
                }
                line = sr.ReadLine();
            }
            sr.Close();
            UpdateDownloaded(keep);
        }

        private void InstallFiles(List<String> downloadID)
        {
            StreamWriter sw = new StreamWriter(dataPath + "install.txt", true, Encoding.ASCII);
            for (int i = 0; i < downloadID.Count(); i++)
            {
                String name = zipsPath + downloadID[i] + ".zip";

                //ZipFile.ExtractToDirectory(name, extractPath);
                using (ZipArchive archive = ZipFile.OpenRead(name))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        //Open the File
                        String line = entry.ToString();
                        List<String> words = new List<String>(line.Split('/'));

                        sw.WriteLine(downloadID[i] + "|" + words[0]);
                        archive.ExtractToDirectory(extractPath);
                        break;
                        //close the file

                        //if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                        //{
                        //entry.ExtractToFile(Path.Combine(extractPath, entry.FullName));
                        //}
                    }
                }
            }
            sw.Close();
        }

        private void UninstallFiles(List<String> downloadID)
        {
            StreamReader sr = new StreamReader(dataPath + "install.txt");
            String line;
            //Pass the file path and file name to the StreamReader constructor

            //Read the first line of text
            List<String> keep = new List<string>();
            line = sr.ReadLine();
            while (line != null && line != "")
            {
                List<String> words = new List<String>(line.Split('|'));
                // For any s in downloadID, if downloadID Equals s...
                if (downloadID.Any(s => words[0].Equals(s)))
                {

                    if (Directory.Exists(extractPath))
                    {
                        Directory.Delete(extractPath + words[1], true);
                    }
                }
                else
                {
                    keep.Add(line);
                }
                line = sr.ReadLine();
            }
            sr.Close();

            UpdateInstalled(keep);
        }

        private void UpdateInstalled(List<String> keep)
        {
            StreamWriter sw = new StreamWriter(dataPath + "install.txt.tmp", true, Encoding.ASCII);
            for (int i = 0; i < keep.Count(); i++)
            {
                sw.WriteLine(keep[i]);
            }
            sw.Close();

            File.Delete(dataPath + "install.txt");
            File.Move(dataPath + "install.txt.tmp", dataPath + "install.txt");
        }

        private void UpdateDownloaded(List<String> keep)
        {
            StreamWriter sw = new StreamWriter(dataPath + "download.txt.tmp", true, Encoding.ASCII);
            for (int i = 0; i < keep.Count(); i++)
            {
                sw.WriteLine(keep[i]);
            }
            sw.Close();

            File.Delete(dataPath + "download.txt");
            File.Move(dataPath + "download.txt.tmp", dataPath + "download.txt");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Load
        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            List<String> downloadID = new List<String>();
            List<Uri> urlList = new List<Uri>();
            for (int num = 14; num < 24; num++)
            {
                Uri HostURI = new Uri("https://beatsaver.com/api.php?mode=details&id=" + num);
                downloadID.Add(num.ToString());
                urlList.Add(HostURI);
            }

            await DownloadInfo(urlList, downloadID);
            button1.Enabled = true;
        }

        // Download and Install
        private async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            List<String> downloadID = new List<String>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value != chk.TrueValue && (row.DefaultCellStyle.BackColor != Color.Blue || row.DefaultCellStyle.BackColor != Color.Green))
                {
                    chk.Value = chk.FalseValue;
                    row.DefaultCellStyle.BackColor = Color.Green;
                    downloadID.Add(dataGridView1.Rows[row.Index].Cells[2].Value.ToString());
                }
            }

            List<String> urlList = new List<String>();
            for (int i = 0; i < downloadID.Count(); i++)
            {
                urlList.Add("https://beatsaver.com/dl.php?id=" + downloadID[i]);
            }

            await DownloadFiles(urlList, downloadID);
            InstallFiles(downloadID);
            button2.Enabled = true;
        }

        // Download
        private async void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            List<String> downloadID = new List<String>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value != chk.TrueValue && (row.DefaultCellStyle.BackColor != Color.Blue || row.DefaultCellStyle.BackColor != Color.Green))
                {
                    chk.Value = chk.FalseValue;
                    row.DefaultCellStyle.BackColor = Color.Blue;
                    downloadID.Add(dataGridView1.Rows[row.Index].Cells[2].Value.ToString());
                }
            }

            List<String> urlList = new List<String>();
            for (int i = 0; i < downloadID.Count(); i++)
            {
                urlList.Add("https://beatsaver.com/dl.php?id=" + downloadID[i]);
            }

            await DownloadFiles(urlList, downloadID);
            button3.Enabled = true;
        }

        // Install
        private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            List<String> downloadID = new List<String>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value != chk.TrueValue && row.DefaultCellStyle.BackColor == Color.Blue)
                {
                    chk.Value = chk.FalseValue;
                    row.DefaultCellStyle.BackColor = Color.Green;
                    downloadID.Add(dataGridView1.Rows[row.Index].Cells[2].Value.ToString());
                }
            }

            InstallFiles(downloadID);
            button5.Enabled = true;
        }

        // Uinstall
        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            List<String> downloadID = new List<String>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value != chk.TrueValue && row.DefaultCellStyle.BackColor == Color.Green)
                {
                    chk.Value = chk.FalseValue;
                    row.DefaultCellStyle.BackColor = Color.Blue;
                    downloadID.Add(dataGridView1.Rows[row.Index].Cells[2].Value.ToString());
                }
            }
            UninstallFiles(downloadID);

            button4.Enabled = true;
        }

        // Uninstall and Delete Files
        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            List<String> downloadID = new List<String>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value != chk.TrueValue && row.DefaultCellStyle.BackColor == Color.Green)
                {
                    chk.Value = chk.FalseValue;
                    row.DefaultCellStyle.BackColor = Color.White;
                    downloadID.Add(dataGridView1.Rows[row.Index].Cells[2].Value.ToString());
                }
            }

            UninstallFiles(downloadID);
            DeleteFiles(downloadID);
            button6.Enabled = true;
        }

        // Delete Files
        private void button7_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            List<String> downloadID = new List<String>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value != chk.TrueValue && (row.DefaultCellStyle.BackColor == Color.Blue || row.DefaultCellStyle.BackColor == Color.Green))
                {
                    chk.Value = chk.FalseValue;
                    row.DefaultCellStyle.BackColor = Color.White;
                    downloadID.Add(dataGridView1.Rows[row.Index].Cells[2].Value.ToString());
                }
            }
            DeleteFiles(downloadID);
            button7.Enabled = true;
        }
    }
}

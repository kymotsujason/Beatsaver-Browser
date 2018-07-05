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
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Vorbis;

namespace Beatsaver_Browser
{
    public partial class Form1 : Form
    {
        String dataPath = @".\Beatsaver_Browser_Data\";
        String zipsPath = @".\Beatsaver_Browser_Data\zips\";
        String imgPath = @".\Beatsaver_Browser_Data\img\";
        String previewPath = @".\Beatsaver_Browser_Data\preview\";
        String extractPath = @"C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\CustomSongs\";
        List<Beatmap> jsonInfo = new List<Beatmap>();
        List<String> listID = new List<string>();
        private WaveOutEvent outputDevice;
        private VorbisWaveReader audioFile;

        public Form1()
        {
            InitializeComponent();
            CheckFolder();
            PopulateList();
            UpdateList();
            SetRange();
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
            if (!Directory.Exists(previewPath))
            {
                Directory.CreateDirectory(previewPath);
            }
        }

        private void PopulateList()
        {
            string beatmapJSON = dataPath + "beatmaps.json";
            if (File.Exists(beatmapJSON))
            {
                var json = File.ReadAllText(beatmapJSON);
                var objList = JsonConvert.DeserializeObject<List<Beatmap>>(json);
                if (objList != null)
                {
                    if (objList.Count() > 0)
                    {
                        foreach (var obj in objList)
                        {
                            Button btn = new Button();
                            btn.Text = "Preview";
                            Image image = Image.FromFile(imgPath + obj.id + "." + obj.img);
                            dataGridView1.Rows.Add(Action.FalseValue, null, image, obj.id, obj.beatname,
                                obj.songName, obj.beatsPerMinute, obj.difficultyLevels, obj.downloads, obj.plays,
                                obj.upvotes, obj.uploader, obj.beattext, "https://beatsaver.com/details.php?id=" + obj.id);
                            listID.Add(obj.id);
                            dataGridView1.Update();
                        }
                    }
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
                    String currentID = dataGridView1.Rows[row.Index].Cells[3].Value.ToString();
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
                    String currentID = dataGridView1.Rows[row.Index].Cells[3].Value.ToString();
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
                    String currentID = dataGridView1.Rows[row.Index].Cells[3].Value.ToString();
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

        private async void SetRange()
        {
            using (WebClient wc = new WebClient())
            {
                Uri url = new Uri("https://beatsaver.com/api.php?mode=new");
                var json = await wc.DownloadStringTaskAsync(url);
                var obj = JsonConvert.DeserializeObject<List<Beatmap>>(json);
                if (obj != null)
                {
                    if (obj.Count() > 0)
                    {
                        int max = 1;
                        max = Convert.ToInt32(obj[0].id);
                        numericUpDown2.Maximum = max;
                        numericUpDown2.Minimum = 1;
                        numericUpDown1.Maximum = max;
                        numericUpDown1.Minimum = 1;
                        numericUpDown2.Value = max;
                    }
                }
            }
        }

        private async Task DownloadInfo(Uri url, String currentID)
        {

            DownloadProgressChangedEventHandler DownloadProgressChangedEvent = (s, e) =>
            {
                progressBar1.BeginInvoke((Action)(() =>
                {
                    progressBar1.Value = e.ProgressPercentage;
                }));

                var downloadProgress = string.Format("{0} MB / {1} MB",
                        (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                        (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));

                label1.BeginInvoke((Action)(() =>
                {
                    label1.Text = "Loading map: " + currentID + " | " + downloadProgress;
                }));
            };
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += DownloadProgressChangedEvent;
                var json = await wc.DownloadStringTaskAsync(url);
                var obj = JsonConvert.DeserializeObject<List<Beatmap>>(json);
                if (obj != null)
                {
                    if (obj.Count() > 0)
                    {
                        if (File.Exists(imgPath + currentID + "." + obj[0].img))
                        {
                            Button btn = new Button();
                            btn.Text = "Preview";
                            Image image = Image.FromFile(imgPath + currentID + "." + obj[0].img);
                            dataGridView1.Rows.Add(Action.FalseValue, null, image, obj[0].id, obj[0].beatname,
                                obj[0].songName, obj[0].beatsPerMinute, obj[0].difficultyLevels, obj[0].downloads, obj[0].plays,
                                obj[0].upvotes, obj[0].uploader, obj[0].beattext, "https://beatsaver.com/details.php?id=" + obj[0].id);
                        }
                        else
                        {
                            Button btn = new Button();
                            btn.Text = "Preview";
                            await DownloadImage(currentID, obj[0].img);
                            Image image = Image.FromFile(imgPath + currentID + "." + obj[0].img);
                            dataGridView1.Rows.Add(Action.FalseValue, null, image, obj[0].id, obj[0].beatname,
                                obj[0].songName, obj[0].beatsPerMinute, obj[0].difficultyLevels, obj[0].downloads, obj[0].plays,
                                obj[0].upvotes, obj[0].uploader, obj[0].beattext, "https://beatsaver.com/details.php?id=" + obj[0].id);
                        }
                        //sw.Write(JsonConvert.SerializeObject(obj.ToArray(), Formatting.Indented));
                        jsonInfo.Add(obj[0]);
                        listID.Add(obj[0].id);
                    }
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
            String name = currentID + ".zip";
            DownloadProgressChangedEventHandler DownloadProgressChangedEvent = (s, e) =>
            {
                progressBar1.BeginInvoke((Action)(() =>
                {
                    progressBar1.Value = e.ProgressPercentage;
                }));

                var downloadProgress = string.Format("{0} MB / {1} MB",
                        (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                        (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));

                label1.BeginInvoke((Action)(() =>
                {
                    label1.Text = "Loading map: " + currentID + " | " + downloadProgress;
                }));
            };
            using (var client = new WebClient())
            {
                client.DownloadProgressChanged += DownloadProgressChangedEvent;
                await client.DownloadFileTaskAsync(url, name);
                if (File.Exists(name))
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
                    if (File.Exists(zipsPath + currentID + ".zip"))
                    {
                        continue;
                    }
                    await DownloadFile(currentURL, currentID, sw);
                }
                sw.Close();
            }
        }

        private void DeleteFiles(List<String> downloadID)
        {
            StreamReader sr = new StreamReader(dataPath + "download.txt");
            List<String> keep = new List<string>();
            String line;
            line = sr.ReadLine();
            while (line != null && line != "")
            {
                // For any s in downloadID, if downloadID Equals s...
                if (downloadID.Any(s => line.Equals(s)))
                {
                    label1.Text = "Deleting: " + line + ".zip";
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
                        if (!Directory.Exists(extractPath + words[0]))
                        {
                            label1.Text = "Installing: " + words[0];
                            archive.ExtractToDirectory(extractPath);
                        }
                        break;
                    }
                }
            }
            sw.Close();
        }

        private void UninstallFiles(List<String> downloadID)
        {
            if (File.Exists(dataPath + "install.txt"))
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
                            label1.Text = "Uninstalling: " + words[1];
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
        }

        private async void PreviewFiles(List<String> downloadID, List<String> urlList)
        {
            for (int i = 0; i < downloadID.Count(); i++)
            {
                String name = zipsPath + downloadID[i] + ".zip";

                if (!File.Exists(name))
                {
                    await DownloadFiles(urlList, downloadID);
                }

                //ZipFile.ExtractToDirectory(name, extractPath);
                using (ZipArchive archive = ZipFile.OpenRead(name))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.Name.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!File.Exists(previewPath + entry.Name))
                            {
                                entry.ExtractToFile(Path.Combine(previewPath, entry.Name));
                            }
                            String audioPath = previewPath + entry.Name;
                            if (outputDevice == null && audioFile == null)
                            {
                                PlayMusic(audioPath);
                            }
                            else
                            {
                                StopMusic(audioPath);
                            }
                        }
                    }
                }
            }
        }

        private void PlayMusic(String audioPath)
        {
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                audioFile = new VorbisWaveReader(audioPath);
                outputDevice.Init(audioFile);
            }
            outputDevice.Play();
        }

        private void StopMusic(String audioPath)
        {
            outputDevice?.Stop();
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            outputDevice.Dispose();
            outputDevice = null;
            audioFile.Dispose();
            audioFile = null;
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
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                List<String> downloadID = new List<String>();
                downloadID.Add(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                List<String> urlList = new List<String>();
                for (int i = 0; i < downloadID.Count(); i++)
                {
                    urlList.Add("https://beatsaver.com/dl.php?id=" + downloadID[i]);
                }

                PreviewFiles(downloadID, urlList);
            }

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex >= 0)
            {
                String url = (dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString());

                System.Diagnostics.Process.Start(url);
            }

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
            for (int num = (int)numericUpDown1.Value; num <= (int)numericUpDown2.Value; num++)
            {
                Uri HostURI = new Uri("https://beatsaver.com/api.php?mode=details&id=" + num);
                downloadID.Add(num.ToString());
                urlList.Add(HostURI);
            }

            await DownloadInfo(urlList, downloadID);
            label1.Text = "Complete";
            button1.Enabled = true;
        }

        // Download and Install
        private async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            List<String> downloadID = new List<String>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value != chk.TrueValue && (row.DefaultCellStyle.BackColor != Color.Blue || row.DefaultCellStyle.BackColor != Color.Green))
                {
                    chk.Value = chk.FalseValue;
                    row.DefaultCellStyle.BackColor = Color.Green;
                    downloadID.Add(dataGridView1.Rows[row.Index].Cells[3].Value.ToString());
                }
            }

            List<String> urlList = new List<String>();
            for (int i = 0; i < downloadID.Count(); i++)
            {
                urlList.Add("https://beatsaver.com/dl.php?id=" + downloadID[i]);
            }

            await DownloadFiles(urlList, downloadID);
            label1.Text = "Complete";
            InstallFiles(downloadID);
            label1.Text = "Complete";
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
        }

        // Download
        private async void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            List<String> downloadID = new List<String>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value != chk.TrueValue && row.DefaultCellStyle.BackColor != Color.Blue && row.DefaultCellStyle.BackColor != Color.Green)
                {
                    chk.Value = chk.FalseValue;
                    row.DefaultCellStyle.BackColor = Color.Blue;
                    downloadID.Add(dataGridView1.Rows[row.Index].Cells[3].Value.ToString());
                }
            }

            List<String> urlList = new List<String>();
            for (int i = 0; i < downloadID.Count(); i++)
            {
                urlList.Add("https://beatsaver.com/dl.php?id=" + downloadID[i]);
            }

            await DownloadFiles(urlList, downloadID);
            label1.Text = "Complete";
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
        }

        // Install
        private void button5_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            List<String> downloadID = new List<String>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value != chk.TrueValue && row.DefaultCellStyle.BackColor == Color.Blue)
                {
                    chk.Value = chk.FalseValue;
                    row.DefaultCellStyle.BackColor = Color.Green;
                    downloadID.Add(dataGridView1.Rows[row.Index].Cells[3].Value.ToString());
                }
            }

            InstallFiles(downloadID);
            label1.Text = "Complete";
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
        }

        // Uinstall
        private void button4_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            List<String> downloadID = new List<String>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value != chk.TrueValue && row.DefaultCellStyle.BackColor == Color.Green)
                {
                    chk.Value = chk.FalseValue;
                    row.DefaultCellStyle.BackColor = Color.Blue;
                    downloadID.Add(dataGridView1.Rows[row.Index].Cells[3].Value.ToString());
                }
            }
            UninstallFiles(downloadID);
            label1.Text = "Complete";
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
        }

        // Uninstall and Delete Files
        private void button6_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            List<String> downloadID = new List<String>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                if (chk.Value != chk.TrueValue && (row.DefaultCellStyle.BackColor == Color.Green || row.DefaultCellStyle.BackColor == Color.Blue))
                {
                    chk.Value = chk.FalseValue;
                    row.DefaultCellStyle.BackColor = Color.White;
                    downloadID.Add(dataGridView1.Rows[row.Index].Cells[3].Value.ToString());
                }
            }

            UninstallFiles(downloadID);
            label1.Text = "Complete";
            DeleteFiles(downloadID);
            label1.Text = "Complete";
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > numericUpDown2.Value)
            {
                numericUpDown1.Value = numericUpDown2.Value;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value < numericUpDown1.Value)
            {
                numericUpDown2.Value = numericUpDown1.Value;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 secondForm = new Form2();
            secondForm.Show();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("What options?");
        }
    }
}

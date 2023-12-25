using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;

namespace WFC
{
    public partial class Form1 : Form
    {
        private const string FilePath = "PathToTileFolder.txt";

        private TilesHandler _tilesHandler;
        TileGrid _tileGrid;
        private System.Windows.Forms.Timer _timer;
        private bool _isScriptRunning;
        private string _path = "";
        private int _stepsPerTick = 4;
        bool _isStarted = false;

        public Form1()
        {
            LoadPath();

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1;
            _timer.Tick += Timer_Tick;
            _isScriptRunning = false;

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Reset()
        {
            if (System.IO.Directory.Exists(_path))
            {
                _tilesHandler = new TilesHandler(_path);
                _tileGrid = new TileGrid(_tilesHandler, 50, 25);
            }
            _isStarted = false;
            _isScriptRunning = false;
            _timer.Stop();
            DisplayImage(_tileGrid.GetImage());
        }

        private void AddRand()
        {
            _tileGrid.AddRandom();
            DisplayImage(_tileGrid.GetImage());
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            OneStep_Click(sender, e);
        }

        private void DisplayImage(SixLabors.ImageSharp.Image<Rgba32> image)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                image.SaveAsBmp(stream);
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = img;
            }
        }


        private void StartScript_Click(object sender, EventArgs e)
        {
            if (!_isScriptRunning)
            {
                _isScriptRunning = true;
                _timer.Start();
            }
            else
            {
                _isScriptRunning = false;
                _timer.Stop();
            }
        }

        private void OneStep_Click(object sender, EventArgs e)
        {

            DoSteps(_stepsPerTick);
        }

        private void DoSteps(int steps)
        {
            if (!_isStarted)
            {
                Reset();
                _isStarted = true;
                AddRand();
            }
            else
            {
                for (int i = 0; i < steps; i++)
                {
                    _tileGrid.GenNextStep();
                }
            }
            DisplayImage(_tileGrid.GetImage());
        }

        private void ChangeFolder_Click_1(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Choose folder with tiles.";

                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    _path = folderBrowserDialog.SelectedPath;
                    SavePath();
                }
            }
        }

        private void LoadPath()
        {
            if (File.Exists(FilePath))
            {
                _path = File.ReadAllText(FilePath);
            }
        }

        private void SavePath()
        {
            File.WriteAllText(FilePath, _path);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}

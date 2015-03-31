using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var dataSource = new List<Thumbnail>()
            {
                new Thumbnail{Text ="指定高宽缩放（可能变形）",Value="HW"},
                new Thumbnail{Text ="指定宽，高按比例",Value="W"},
                new Thumbnail{Text ="指定高，宽按比例",Value="H"},
                new Thumbnail{Text ="指定高宽裁减（不变形）",Value="Cut"}
            };

            this.cbType.DataSource = dataSource;
            this.cbType.DisplayMember = "Text";
            this.cbType.ValueMember = "Value";
            this.cbType.SelectedIndex = 2;

        }

        private void btnOpenDirectory_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            this.txtPath.Text = dialog.SelectedPath;
        }

        private void btnExcute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtPath.Text)) return;

            if (
                MessageBox.Show(@"该目录 " + this.txtPath.Text + @" 及子目录里下所有图片都将生成缩略图", @"警告", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning) != DialogResult.OK) return;

            this.btnExcute.Enabled = false;
            Task.Factory.StartNew(Excute);

        }

        private void Excute()
        {
            this.txtProcessInfo.Text = @"正在准备生成缩略图...";
            var subDirectories = DirectoryHelper.FindSubDirectories(this.txtPath.Text);

            foreach (var item in subDirectories)
            {
                this.txtProcessInfo.AppendText("\r\n当前目录：" + item);

                var dir = new DirectoryInfo(item);
                var files = dir.GetFiles();

                if (files.Length == 0) continue;

                foreach (var t in files)
                {
                    var sourcePath = t.FullName;
                    var thumbnailPath = sourcePath.Insert(sourcePath.LastIndexOf(".", StringComparison.Ordinal), "_t");

                    ImageHelper.MakeThumbnail(sourcePath, thumbnailPath, 120, 120, "H");

                    this.txtProcessInfo.AppendText("\r\n原图：" + sourcePath);
                    this.txtProcessInfo.AppendText("\r\n缩略图：" + thumbnailPath);
                }
            }
            this.txtProcessInfo.AppendText("\r\n*******************");
            this.txtProcessInfo.AppendText("\r\n已完成！");
            this.btnExcute.Enabled = true;
        }

    }

    internal class Thumbnail
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}

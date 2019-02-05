using System;
using System.Text;
using System.Windows.Forms;
using Svg;
using System.IO;

namespace SVGViewer
{
    public partial class SvgViewer : Form
    {
        public SvgViewer()
        {
            InitializeComponent();
        }

        private void open_Click(object sender, EventArgs e)
        {
            try
            {
                if (openSvgFile.ShowDialog() != DialogResult.OK) return;
                var svgDoc = SvgDocument.Open(openSvgFile.FileName);
                RenderSvg(svgDoc);
            }
            catch
            {
                // ignored
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (var s = new MemoryStream(Encoding.UTF8.GetBytes(textBox1.Text)))
                {
                    SvgDocument svgDoc = SvgDocument.Open<SvgDocument>(s, null);
        
                    RenderSvg(svgDoc);
                }
            }
            catch
            {
                // ignored
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    (sender as TextBox)?.SelectAll();
                }
            }
            catch
            {
                // ignored
            }
        }

        private void RenderSvg(SvgDocument svgDoc)
        {
            //var render = new DebugRenderer();
            //svgDoc.Draw(render);
            svgImage.Image = svgDoc.Draw();

            string outputDir;
            if (svgDoc.BaseUri == null)
                outputDir = Path.GetDirectoryName(Application.ExecutablePath); 
            else
                outputDir = Path.GetDirectoryName(svgDoc.BaseUri.LocalPath);
            svgImage.Image.Save(Path.Combine(outputDir ?? throw new InvalidOperationException(), "output.png"));
            // svgDoc.Write(System.IO.Path.Combine(outputDir, "output.svg"));
        }

    }
}

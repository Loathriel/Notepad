using System.Text;

namespace Notepad
{

    public partial class Form1 : Form
    {
        private ActiveEncoding activeEncoding;
        private string fileName;

        private const string defaultTitle = "Waldemar's Notepad";
        private readonly Encoding[] encodings = { new UnicodeEncoding(false, false), new UTF8Encoding(false), new ASCIIEncoding() };

        public Form1()
        {
            InitializeComponent();

            activeEncoding = new ActiveEncoding(unicodeToolStrip, encodings[0]);

            fileName = string.Empty;
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            /*  Очищення поля вводу та збереженого шляху файлу
             *  Використовується для "створення" нового файлу
             */

            fileName = string.Empty;
            textBox.Text = string.Empty;
            Text = defaultTitle;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Text = fileName = openFileDialog.FileName;

                textBox.Text = File.ReadAllText(fileName, activeEncoding.encoding);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (fileName == string.Empty) 
            {
                saveAs();
                return;
            }

            File.WriteAllText(fileName, textBox.Text, activeEncoding.encoding);
        }

        private void saveAsButton_Click(object sender, EventArgs e) { saveAs(); }

        private void saveAs()
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Text = fileName = saveFileDialog.FileName;
                saveFileDialog.FileName = string.Empty;

                File.WriteAllText(fileName, textBox.Text, activeEncoding.encoding);
            }
        }

        private void asciiToolStrip_Click(object sender, EventArgs e) { reloadWithEcnoding(sender, encodings[2]); }

        private void utf8ToolStrip_Click(object sender, EventArgs e) { reloadWithEcnoding(sender, encodings[1]); }

        private void unicodeToolStrip_Click(object sender, EventArgs e) { reloadWithEcnoding(sender, encodings[0]); }

        private void reloadWithEcnoding(object sender, Encoding newEncoding)
        {
            activeEncoding.ChangeEncoding((ToolStripMenuItem) sender, newEncoding);

            if (fileName != string.Empty)
                textBox.Text = File.ReadAllText(fileName, activeEncoding.encoding);
        }
    }

    class ActiveEncoding
    {
        /*  Клас для збереження інформація про обране кодування
         *  Для зручної деактивації відповідної кнопки та для використання 
         *  правильного кодування при збереженні/відкритті файлів
         */

        public ActiveEncoding(ToolStripMenuItem item, Encoding encoding)
        {
            this.encoding = encoding;
            this.item = item;
            item.Enabled = false;
        }

        public Encoding encoding
        {
            get;
            private set;
        }

        private ToolStripMenuItem item;

        public void ChangeEncoding(ToolStripMenuItem sender, Encoding newEncoding)
        {
            item.Enabled = true;
            item = sender;
            item.Enabled = false;

            encoding = newEncoding;
        }
    }
}
namespace PWDGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            keyWordBox.Text = "";
            maxNumBox.Text = "";
            birthdayPicker.Value = DateTime.Now;
            symbolsBox.Text = "";
            fileNameBox.Text = "generatedPasswords.txt";
            savePathBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
            maxCharLengthBox.Text = "10";
        }

        private void MaxNumBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char numsOnly = e.KeyChar;

            if (!char.IsDigit(numsOnly) && numsOnly != 8)
            {
                e.Handled = true;
            }
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (keyWordBox.Text == "" && maxNumBox.Text == "") return;
                DateTime? bdValue = null;
                if (enabledBDCheckBox.Checked) bdValue = birthdayPicker.Value;
                string savePath = savePathBox.Text.EndsWith('\\') ? savePathBox.Text : savePathBox.Text + "\\";
                Program.GenerateCombinations(keyWordBox.Text, string.IsNullOrEmpty(maxNumBox.Text) ? 0 : int.Parse(maxNumBox.Text), savePath + fileNameBox.Text, bdValue, symbolsBox.Text, string.IsNullOrEmpty(maxCharLengthBox.Text) ? 10 : int.Parse(maxCharLengthBox.Text));
                MessageBox.Show("Vygenerová a uloženo do: " + savePathBox.Text + fileNameBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba:\n" + ex);
            }
        }

        private void EnabledBDCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledBDCheckBox.Checked) birthdayPicker.Enabled = true;
            else birthdayPicker.Enabled = false;
        }
    }
}

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
            numOfSymbolsNBox.Value = 0;
            symbolsBox.Text = "";
            fileNameBox.Text = "generatedPasswords.txt";
            savePathBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
            maxCharLengthBox.Text = "10";
            usedWordsBox.Text = "";
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            /*try
            {*/
            if (keyWordBox.Text == "" && maxNumBox.Text == "") return;
            DateTime? bdValue = null;
            if (enabledBDCheckBox.Checked) bdValue = birthdayPicker.Value;
            string savePath = savePathBox.Text.EndsWith('\\') ? savePathBox.Text : savePathBox.Text + "\\";
            Program.GeneratePWDCombinations(keyWordBox.Text, string.IsNullOrEmpty(maxNumBox.Text) ? 0 : int.Parse(maxNumBox.Text), savePath + fileNameBox.Text,
                bdValue, symbolsBox.Text, string.IsNullOrEmpty(maxCharLengthBox.Text) ? 10 : int.Parse(maxCharLengthBox.Text), (int)numOfSymbolsNBox.Value, usedWordsBox.Text);
            MessageBox.Show("Vygenerov� a ulo�eno do: " + savePathBox.Text + fileNameBox.Text, "�sp�ch");
            /*}
            catch (Exception ex)
            {
                MessageBox.Show("Chyba:\n" + ex, "CHYBA");
            }*/
            HashSet<string> usedWords = [.. usedWordsBox.Text.Split([',', ';', ' '], StringSplitOptions.RemoveEmptyEntries)];
            foreach(string word in keyWordBox.Text.Split([',', ';', ' '], StringSplitOptions.RemoveEmptyEntries))
            {
                if (word.Length > 0)
                {
                    usedWords.Add(word);
                }
            }
            keyWordBox.Text = "";
            usedWordsBox.Text = string.Join(",", usedWords);
            Cursor.Current = Cursors.Default;
        }

        private void MaxNumBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char numsOnly = e.KeyChar;
            // 8 = backspace
            if (!char.IsDigit(numsOnly) && numsOnly != 8)
            {
                e.Handled = true;
            }
        }

        private void MaxCharLengthBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char numsOnly = e.KeyChar;
            // 8 = backspace
            if (!char.IsDigit(numsOnly) && numsOnly != 8)
            {
                e.Handled = true;
            }
        }

        private void EnabledBDCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledBDCheckBox.Checked) birthdayPicker.Enabled = true;
            else birthdayPicker.Enabled = false;
        }

        private void NumOfSymbolsNBox_ValueChanged(object sender, EventArgs e)
        {
            if (numOfSymbolsNBox.Value > 0)
            {
                symbolsBox.Enabled = true;
            }
            else
            {
                symbolsBox.Enabled = false;
                symbolsBox.Text = "";
            }
        }
    }
}

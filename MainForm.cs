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
            maxNumBox.Text = "99";
            birthdayPicker.Value = DateTime.Now;
            fileNameBox.Text = "generatedPasswords.txt";
            savePathBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
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
                string savePath = savePathBox.Text.EndsWith('\\') ? savePathBox.Text : savePathBox.Text + "\\";
                Program.GenerateCombinations(keyWordBox.Text, int.Parse(maxNumBox.Text.ToLower()), savePath + fileNameBox.Text, birthdayPicker.Value);
                MessageBox.Show("Vygenerová a uloženo do: " + savePathBox.Text + fileNameBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba:\n" + ex);
            }
        }
    }
}

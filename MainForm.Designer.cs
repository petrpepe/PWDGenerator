namespace PWDGenerator
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            generateBtn = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            keyWordBox = new TextBox();
            savePathBox = new TextBox();
            maxNumBox = new TextBox();
            birthdayPicker = new DateTimePicker();
            label5 = new Label();
            label6 = new Label();
            fileNameBox = new TextBox();
            enabledBDCheckBox = new CheckBox();
            label7 = new Label();
            symbolsBox = new TextBox();
            maxCharLengthBox = new TextBox();
            label8 = new Label();
            numOfSymbolsNBox = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numOfSymbolsNBox).BeginInit();
            SuspendLayout();
            // 
            // generateBtn
            // 
            generateBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            generateBtn.Font = new Font("Segoe UI", 12F);
            generateBtn.ForeColor = SystemColors.ControlText;
            generateBtn.Location = new Point(490, 36);
            generateBtn.Name = "generateBtn";
            generateBtn.Size = new Size(123, 260);
            generateBtn.TabIndex = 8;
            generateBtn.Text = "Generuj";
            generateBtn.UseVisualStyleBackColor = true;
            generateBtn.Click += GenerateBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(16, 42);
            label1.Name = "label1";
            label1.Size = new Size(87, 19);
            label1.TabIndex = 1;
            label1.Text = "Klíčové slovo";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(16, 82);
            label2.Name = "label2";
            label2.Size = new Size(101, 19);
            label2.TabIndex = 2;
            label2.Text = "Maximální číslo";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(16, 125);
            label3.Name = "label3";
            label3.Size = new Size(107, 19);
            label3.TabIndex = 3;
            label3.Text = "Datum narození";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F);
            label4.Location = new Point(16, 277);
            label4.Name = "label4";
            label4.Size = new Size(90, 19);
            label4.TabIndex = 4;
            label4.Text = "Cesta uložení";
            // 
            // keyWordBox
            // 
            keyWordBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            keyWordBox.Font = new Font("Segoe UI", 10F);
            keyWordBox.Location = new Point(149, 39);
            keyWordBox.Name = "keyWordBox";
            keyWordBox.PlaceholderText = "Klíčové slovo";
            keyWordBox.Size = new Size(335, 25);
            keyWordBox.TabIndex = 1;
            // 
            // savePathBox
            // 
            savePathBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            savePathBox.Font = new Font("Segoe UI", 10F);
            savePathBox.Location = new Point(16, 309);
            savePathBox.Multiline = true;
            savePathBox.Name = "savePathBox";
            savePathBox.PlaceholderText = "C:\\...";
            savePathBox.Size = new Size(597, 60);
            savePathBox.TabIndex = 10;
            // 
            // maxNumBox
            // 
            maxNumBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            maxNumBox.Font = new Font("Segoe UI", 10F);
            maxNumBox.Location = new Point(149, 79);
            maxNumBox.Name = "maxNumBox";
            maxNumBox.PlaceholderText = "99";
            maxNumBox.Size = new Size(335, 25);
            maxNumBox.TabIndex = 2;
            maxNumBox.KeyPress += MaxNumBox_KeyPress;
            // 
            // birthdayPicker
            // 
            birthdayPicker.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            birthdayPicker.CustomFormat = " dd.MM.yyyy";
            birthdayPicker.Enabled = false;
            birthdayPicker.Font = new Font("Segoe UI", 10F);
            birthdayPicker.Location = new Point(220, 119);
            birthdayPicker.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            birthdayPicker.Name = "birthdayPicker";
            birthdayPicker.Size = new Size(264, 25);
            birthdayPicker.TabIndex = 4;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label5.Location = new Point(177, 9);
            label5.Name = "label5";
            label5.Size = new Size(274, 25);
            label5.TabIndex = 11;
            label5.Text = "Generování hesel do souboru";
            label5.UseMnemonic = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.Location = new Point(16, 241);
            label6.Name = "label6";
            label6.Size = new Size(101, 19);
            label6.TabIndex = 12;
            label6.Text = "Název souboru";
            // 
            // fileNameBox
            // 
            fileNameBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            fileNameBox.Font = new Font("Segoe UI", 10F);
            fileNameBox.Location = new Point(149, 238);
            fileNameBox.Name = "fileNameBox";
            fileNameBox.PlaceholderText = "*.txt / *.csv ...";
            fileNameBox.Size = new Size(335, 25);
            fileNameBox.TabIndex = 9;
            // 
            // enabledBDCheckBox
            // 
            enabledBDCheckBox.AutoSize = true;
            enabledBDCheckBox.Font = new Font("Segoe UI", 10F);
            enabledBDCheckBox.Location = new Point(149, 123);
            enabledBDCheckBox.Name = "enabledBDCheckBox";
            enabledBDCheckBox.Size = new Size(65, 23);
            enabledBDCheckBox.TabIndex = 3;
            enabledBDCheckBox.Text = "Použít";
            enabledBDCheckBox.UseVisualStyleBackColor = true;
            enabledBDCheckBox.CheckedChanged += EnabledBDCheckBox_CheckedChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F);
            label7.Location = new Point(16, 162);
            label7.Name = "label7";
            label7.Size = new Size(61, 19);
            label7.TabIndex = 15;
            label7.Text = "Symboly";
            // 
            // symbolsBox
            // 
            symbolsBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            symbolsBox.Enabled = false;
            symbolsBox.Font = new Font("Segoe UI", 10F);
            symbolsBox.Location = new Point(220, 159);
            symbolsBox.Name = "symbolsBox";
            symbolsBox.PlaceholderText = "!,.* (neoddělovat, napsat za sebou)";
            symbolsBox.Size = new Size(264, 25);
            symbolsBox.TabIndex = 6;
            // 
            // maxCharLengthBox
            // 
            maxCharLengthBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            maxCharLengthBox.Font = new Font("Segoe UI", 10F);
            maxCharLengthBox.Location = new Point(149, 199);
            maxCharLengthBox.Name = "maxCharLengthBox";
            maxCharLengthBox.PlaceholderText = "10";
            maxCharLengthBox.Size = new Size(335, 25);
            maxCharLengthBox.TabIndex = 7;
            maxCharLengthBox.KeyPress += MaxCharLengthBox_KeyPress;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10F);
            label8.Location = new Point(16, 202);
            label8.Name = "label8";
            label8.Size = new Size(106, 19);
            label8.TabIndex = 18;
            label8.Text = "Max délka hesla";
            // 
            // numOfSymbolsNBox
            // 
            numOfSymbolsNBox.Location = new Point(149, 161);
            numOfSymbolsNBox.Name = "numOfSymbolsNBox";
            numOfSymbolsNBox.Size = new Size(65, 23);
            numOfSymbolsNBox.TabIndex = 5;
            numOfSymbolsNBox.ValueChanged += NumOfSymbolsNBox_ValueChanged;
            numOfSymbolsNBox.Click += NumOfSymbolsNBox_ValueChanged;
            numOfSymbolsNBox.KeyUp += NumOfSymbolsNBox_ValueChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(624, 381);
            Controls.Add(numOfSymbolsNBox);
            Controls.Add(label8);
            Controls.Add(maxCharLengthBox);
            Controls.Add(symbolsBox);
            Controls.Add(label7);
            Controls.Add(enabledBDCheckBox);
            Controls.Add(fileNameBox);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(birthdayPicker);
            Controls.Add(maxNumBox);
            Controls.Add(savePathBox);
            Controls.Add(keyWordBox);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(generateBtn);
            MinimumSize = new Size(640, 420);
            Name = "MainForm";
            Text = "Vygeneruj kombinace hesel a ulož do souboru";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numOfSymbolsNBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Button generateBtn;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox keyWordBox;
        private TextBox savePathBox;
        private TextBox maxNumBox;
        private DateTimePicker birthdayPicker;
        private Label label5;
        private Label label6;
        private TextBox fileNameBox;
        private CheckBox enabledBDCheckBox;
        private Label label7;
        private TextBox symbolsBox;
        private TextBox maxCharLengthBox;
        private Label label8;
        private NumericUpDown numOfSymbolsNBox;
    }
}

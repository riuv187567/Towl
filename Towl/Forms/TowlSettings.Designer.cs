namespace Towl;

partial class TowlSettings
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        textBox1 = new TextBox();
        label1 = new Label();
        label2 = new Label();
        numericUpDown1 = new NumericUpDown();
        label3 = new Label();
        ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
        SuspendLayout();
        // 
        // textBox1
        // 
        textBox1.Location = new Point(12, 27);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(295, 23);
        textBox1.TabIndex = 0;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(12, 9);
        label1.Name = "label1";
        label1.Size = new Size(124, 15);
        label1.TabIndex = 1;
        label1.Text = "Tracked process name";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.ForeColor = Color.Red;
        label2.Location = new Point(193, 9);
        label2.Name = "label2";
        label2.Size = new Size(114, 15);
        label2.TabIndex = 2;
        label2.Text = "Process is not found";
        // 
        // numericUpDown1
        // 
        numericUpDown1.Location = new Point(12, 77);
        numericUpDown1.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
        numericUpDown1.Name = "numericUpDown1";
        numericUpDown1.Size = new Size(134, 23);
        numericUpDown1.TabIndex = 3;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(12, 59);
        label3.Name = "label3";
        label3.Size = new Size(100, 15);
        label3.TabIndex = 4;
        label3.Text = "Speculative hours";
        // 
        // TowlSettings
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(319, 116);
        Controls.Add(label3);
        Controls.Add(numericUpDown1);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(textBox1);
        Name = "TowlSettings";
        Text = "Towl Settings";
        ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox textBox1;
    private Label label1;
    private Label label2;
    private NumericUpDown numericUpDown1;
    private Label label3;
}

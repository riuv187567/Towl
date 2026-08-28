namespace Towl;

partial class Towl
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
        sessionTime = new Label();
        SuspendLayout();
        // 
        // sessionTime
        // 
        sessionTime.AutoSize = true;
        sessionTime.BackColor = Color.Transparent;
        sessionTime.Font = new Font("Segoe UI Semibold", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
        sessionTime.Location = new Point(0, 0);
        sessionTime.Margin = new Padding(0);
        sessionTime.Name = "sessionTime";
        sessionTime.Size = new Size(214, 65);
        sessionTime.TabIndex = 0;
        sessionTime.Text = "00:00:00";
        sessionTime.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // Towl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(208, 62);
        Controls.Add(sessionTime);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Towl";
        Text = "Towl";
        TopMost = true;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label sessionTime;
}

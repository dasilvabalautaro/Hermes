namespace Hermes.presentation.view
{
    partial class frmConfigurationDatabase
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfigurationDatabase));
            this.grbOptions = new System.Windows.Forms.GroupBox();
            this.cmdConnectDatabase = new System.Windows.Forms.Button();
            this.cmdCrearDatabase = new System.Windows.Forms.Button();
            this.txtNameDatabase = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.cmdClose = new System.Windows.Forms.Button();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.cboServers = new System.Windows.Forms.ComboBox();
            this.cmdTest = new System.Windows.Forms.Button();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.rbAuthenticationSQLServer = new System.Windows.Forms.RadioButton();
            this.rbAuthenticationWindows = new System.Windows.Forms.RadioButton();
            this.Label1 = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.grbOptions.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbOptions
            // 
            this.grbOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbOptions.BackColor = System.Drawing.SystemColors.Control;
            this.grbOptions.Controls.Add(this.cmdConnectDatabase);
            this.grbOptions.Controls.Add(this.cmdCrearDatabase);
            this.grbOptions.Controls.Add(this.txtNameDatabase);
            this.grbOptions.Controls.Add(this.lblNombre);
            this.grbOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grbOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbOptions.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grbOptions.Location = new System.Drawing.Point(19, 257);
            this.grbOptions.Margin = new System.Windows.Forms.Padding(2);
            this.grbOptions.Name = "grbOptions";
            this.grbOptions.Padding = new System.Windows.Forms.Padding(2);
            this.grbOptions.Size = new System.Drawing.Size(278, 116);
            this.grbOptions.TabIndex = 15;
            this.grbOptions.TabStop = false;
            this.grbOptions.Text = "Crear Base de Datos";
            // 
            // cmdConnectDatabase
            // 
            this.cmdConnectDatabase.BackColor = System.Drawing.SystemColors.Control;
            this.cmdConnectDatabase.Enabled = false;
            this.cmdConnectDatabase.Image = ((System.Drawing.Image)(resources.GetObject("cmdConnectDatabase.Image")));
            this.cmdConnectDatabase.Location = new System.Drawing.Point(105, 55);
            this.cmdConnectDatabase.Margin = new System.Windows.Forms.Padding(2);
            this.cmdConnectDatabase.Name = "cmdConnectDatabase";
            this.cmdConnectDatabase.Size = new System.Drawing.Size(68, 48);
            this.cmdConnectDatabase.TabIndex = 9;
            this.cmdConnectDatabase.Tag = "1";
            this.cmdConnectDatabase.UseVisualStyleBackColor = false;
            this.cmdConnectDatabase.Click += new System.EventHandler(this.cmdConnectDatabase_Click);
            // 
            // cmdCrearDatabase
            // 
            this.cmdCrearDatabase.BackColor = System.Drawing.SystemColors.Control;
            this.cmdCrearDatabase.Enabled = false;
            this.cmdCrearDatabase.Image = ((System.Drawing.Image)(resources.GetObject("cmdCrearDatabase.Image")));
            this.cmdCrearDatabase.Location = new System.Drawing.Point(195, 55);
            this.cmdCrearDatabase.Margin = new System.Windows.Forms.Padding(2);
            this.cmdCrearDatabase.Name = "cmdCrearDatabase";
            this.cmdCrearDatabase.Size = new System.Drawing.Size(68, 48);
            this.cmdCrearDatabase.TabIndex = 8;
            this.cmdCrearDatabase.Tag = "1";
            this.cmdCrearDatabase.UseVisualStyleBackColor = false;
            this.cmdCrearDatabase.Click += new System.EventHandler(this.cmdCrearDatabase_Click);
            // 
            // txtNameDatabase
            // 
            this.txtNameDatabase.Location = new System.Drawing.Point(79, 24);
            this.txtNameDatabase.Margin = new System.Windows.Forms.Padding(2);
            this.txtNameDatabase.Name = "txtNameDatabase";
            this.txtNameDatabase.Size = new System.Drawing.Size(187, 23);
            this.txtNameDatabase.TabIndex = 7;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.BackColor = System.Drawing.SystemColors.Control;
            this.lblNombre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNombre.Location = new System.Drawing.Point(8, 26);
            this.lblNombre.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(64, 19);
            this.lblNombre.TabIndex = 6;
            this.lblNombre.Text = "Nombre:";
            this.lblNombre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.BackColor = System.Drawing.SystemColors.Control;
            this.cmdClose.Image = ((System.Drawing.Image)(resources.GetObject("cmdClose.Image")));
            this.cmdClose.Location = new System.Drawing.Point(229, 379);
            this.cmdClose.Margin = new System.Windows.Forms.Padding(2);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(68, 48);
            this.cmdClose.TabIndex = 14;
            this.cmdClose.Tag = "1";
            this.cmdClose.UseVisualStyleBackColor = false;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.lblCount);
            this.GroupBox1.Controls.Add(this.cboServers);
            this.GroupBox1.Controls.Add(this.cmdTest);
            this.GroupBox1.Controls.Add(this.GroupBox2);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox1.Location = new System.Drawing.Point(19, 7);
            this.GroupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.GroupBox1.Size = new System.Drawing.Size(278, 246);
            this.GroupBox1.TabIndex = 13;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Conexión al Servidor";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.ForeColor = System.Drawing.Color.Red;
            this.lblCount.Location = new System.Drawing.Point(251, 21);
            this.lblCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(14, 13);
            this.lblCount.TabIndex = 6;
            this.lblCount.Text = "1";
            // 
            // cboServers
            // 
            this.cboServers.FormattingEnabled = true;
            this.cboServers.Location = new System.Drawing.Point(71, 19);
            this.cboServers.Margin = new System.Windows.Forms.Padding(2);
            this.cboServers.Name = "cboServers";
            this.cboServers.Size = new System.Drawing.Size(175, 24);
            this.cboServers.TabIndex = 1;
            // 
            // cmdTest
            // 
            this.cmdTest.BackColor = System.Drawing.SystemColors.Control;
            this.cmdTest.Image = ((System.Drawing.Image)(resources.GetObject("cmdTest.Image")));
            this.cmdTest.Location = new System.Drawing.Point(198, 189);
            this.cmdTest.Margin = new System.Windows.Forms.Padding(2);
            this.cmdTest.Name = "cmdTest";
            this.cmdTest.Size = new System.Drawing.Size(68, 48);
            this.cmdTest.TabIndex = 5;
            this.cmdTest.Tag = "1";
            this.cmdTest.UseVisualStyleBackColor = false;
            this.cmdTest.Click += new System.EventHandler(this.cmdTest_Click);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.txtPassword);
            this.GroupBox2.Controls.Add(this.txtLogin);
            this.GroupBox2.Controls.Add(this.lblPassword);
            this.GroupBox2.Controls.Add(this.lblLogin);
            this.GroupBox2.Controls.Add(this.rbAuthenticationSQLServer);
            this.GroupBox2.Controls.Add(this.rbAuthenticationWindows);
            this.GroupBox2.Location = new System.Drawing.Point(12, 51);
            this.GroupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.GroupBox2.Size = new System.Drawing.Size(254, 131);
            this.GroupBox2.TabIndex = 3;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Parámetros de Acceso";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(101, 101);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.ReadOnly = true;
            this.txtPassword.Size = new System.Drawing.Size(140, 23);
            this.txtPassword.TabIndex = 4;
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(101, 71);
            this.txtLogin.Margin = new System.Windows.Forms.Padding(2);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.ReadOnly = true;
            this.txtLogin.Size = new System.Drawing.Size(140, 23);
            this.txtLogin.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPassword.Location = new System.Drawing.Point(8, 102);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(87, 19);
            this.lblPassword.TabIndex = 8;
            this.lblPassword.Text = "Contraseña:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLogin.Location = new System.Drawing.Point(8, 72);
            this.lblLogin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(63, 19);
            this.lblLogin.TabIndex = 7;
            this.lblLogin.Text = "Usuario:";
            this.lblLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbAuthenticationSQLServer
            // 
            this.rbAuthenticationSQLServer.AutoSize = true;
            this.rbAuthenticationSQLServer.Location = new System.Drawing.Point(20, 43);
            this.rbAuthenticationSQLServer.Margin = new System.Windows.Forms.Padding(2);
            this.rbAuthenticationSQLServer.Name = "rbAuthenticationSQLServer";
            this.rbAuthenticationSQLServer.Size = new System.Drawing.Size(196, 21);
            this.rbAuthenticationSQLServer.TabIndex = 2;
            this.rbAuthenticationSQLServer.Text = "Autentificación SQL Server";
            this.rbAuthenticationSQLServer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbAuthenticationSQLServer.UseVisualStyleBackColor = true;
            // 
            // rbAuthenticationWindows
            // 
            this.rbAuthenticationWindows.AutoSize = true;
            this.rbAuthenticationWindows.Checked = true;
            this.rbAuthenticationWindows.Location = new System.Drawing.Point(20, 21);
            this.rbAuthenticationWindows.Margin = new System.Windows.Forms.Padding(2);
            this.rbAuthenticationWindows.Name = "rbAuthenticationWindows";
            this.rbAuthenticationWindows.Size = new System.Drawing.Size(178, 21);
            this.rbAuthenticationWindows.TabIndex = 1;
            this.rbAuthenticationWindows.TabStop = true;
            this.rbAuthenticationWindows.Text = "Autentificación Windows";
            this.rbAuthenticationWindows.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbAuthenticationWindows.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(5, 21);
            this.Label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(65, 17);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Servidor:";
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // frmConfigurationDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 434);
            this.Controls.Add(this.grbOptions);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.GroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmConfigurationDatabase";
            this.Text = "Conectar y Crear Base de Datos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConfigurationDatabase_FormClosing);
            this.Load += new System.EventHandler(this.frmConfigurationDatabase_Load);
            this.grbOptions.ResumeLayout(false);
            this.grbOptions.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox grbOptions;
        internal System.Windows.Forms.Button cmdConnectDatabase;
        internal System.Windows.Forms.Button cmdCrearDatabase;
        internal System.Windows.Forms.TextBox txtNameDatabase;
        internal System.Windows.Forms.Label lblNombre;
        internal System.Windows.Forms.Button cmdClose;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Label lblCount;
        internal System.Windows.Forms.ComboBox cboServers;
        internal System.Windows.Forms.Button cmdTest;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.TextBox txtPassword;
        internal System.Windows.Forms.TextBox txtLogin;
        internal System.Windows.Forms.Label lblPassword;
        internal System.Windows.Forms.Label lblLogin;
        internal System.Windows.Forms.RadioButton rbAuthenticationSQLServer;
        internal System.Windows.Forms.RadioButton rbAuthenticationWindows;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Timer timer;
    }
}
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hermes.tools
{
    class ControlManager
    {
        #region
        public ControlManager()
        {

        }

        public void createStatusBar(Form frmWork, StatusStrip statusBar)
        {
            
            ToolStripLabel panel1 = new ToolStripLabel();
            ToolStripProgressBar panel2 = new ToolStripProgressBar();            
            ToolStripLabel panel3 = new ToolStripLabel();
            System.Drawing.Size size = new System.Drawing.Size();

            size.Width = Convert.ToInt32(frmWork.Width * 0.2);
            panel1.Text = "";
            panel1.Size = size;
            panel1.ForeColor = System.Drawing.Color.Blue;
            panel2.Minimum = 0;
            panel2.Maximum = 10;
            panel2.Step = 1;
            panel2.Size = size;
            panel3.Text = setDateLocale();
            panel3.BackColor = System.Drawing.Color.Black;
            panel3.ForeColor = System.Drawing.Color.White;
            panel3.Alignment = ToolStripItemAlignment.Right;                              
            statusBar.Show();
            statusBar.Items.Add(panel1);
            statusBar.Items.Add(panel2);
            statusBar.Items.Add(panel3);

        }
        public void setValueTextStatusStrip(string strMessage,
           int index, StatusStrip ssCtrl)
        {

            StatusStrip ssMain = new StatusStrip();
            ssMain = ssCtrl;
            ssMain.Items[index].Text = strMessage;
        }

        public void startProgressStatusStrip(int index, StatusStrip ssCtrl)
        {
            StatusStrip ssMain = new StatusStrip();
            ssMain = ssCtrl;

            ToolStripProgressBar tsProgressBar = ssMain.Items[index] as ToolStripProgressBar;
            tsProgressBar.Style = ProgressBarStyle.Marquee;
            tsProgressBar.MarqueeAnimationSpeed = 30;

        }

        public void stopProgressStatusStrip(int index, StatusStrip ssCtrl)
        {
            StatusStrip ssMain = new StatusStrip();
            ssMain = ssCtrl;

            ToolStripProgressBar tsProgressBar = ssMain.Items[index] as ToolStripProgressBar;
            tsProgressBar.Style = ProgressBarStyle.Continuous;
            tsProgressBar.MarqueeAnimationSpeed = 0;

        }

        public StatusStrip getStatusStripMain(string nameMDI)
        {
            StatusStrip status = null;

            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == nameMDI)
                {
                    mdiMain md = (mdiMain)f;

                    foreach (Control c in f.Controls)
                    {
                        if (c is StatusStrip)
                        {
                            status = (StatusStrip)c;
                            break;

                        }
                    }
                }
            }

            return status;
        }


        public string setDateLocale()
        {
            string dateLocale;
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES");
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            dateLocale = string.Format("{0:D}", DateTime.Now);
            ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-EN");
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            return dateLocale;
        }

        public void setComboBox(ComboBox cbo, List<string> values)
        {

            if (values != null)
            {
                foreach (string s in values)
                {
                    cbo.Items.Add(s);
                }
            }
        }


        public void enabledOptionMenu(string nameMenu, string nameMDI)
        {
            Form frm = new Form();
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == nameMDI)
                    frm = f;
            }


            foreach (ToolStripMenuItem mnu in frm.MainMenuStrip.Items)
            {
                foreach (ToolStripDropDownItem smnu in mnu.DropDownItems)
                {

                    if (smnu.Name == nameMenu)
                    {
                        smnu.Enabled = true;
                        smnu.Visible = true;
                    }


                }
            }
        }

        public void setValueComboBox(ComboBox cbo, string value)
        {
            for (int i = 0; i < cbo.Items.Count; i++)
            {
                cbo.SelectedIndex = i;
                if (cbo.Text == value || Convert.ToString(cbo.SelectedValue) == value)
                {
                    return;
                }
            }
        }

        public void enabledOptionSubSubMenu(string nameOption, string nameMDI)
        {
            Form frm = new Form();
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == nameMDI)
                    frm = f;

            }

            foreach (ToolStripMenuItem mnu in frm.MainMenuStrip.Items)
            {
                foreach (ToolStripDropDownItem smnu in mnu.DropDownItems)
                {
                    foreach (ToolStripDropDownItem semnu in smnu.DropDownItems)
                    {
                        foreach (ToolStripDropDownItem seemnu in semnu.DropDownItems)
                        {
                            if (seemnu.Name == nameOption)
                            {
                                seemnu.Enabled = true;
                                seemnu.Visible = true;
                            }
                        }
                    }


                }
            }
        }

        public void enabledOptionSubMenu(string nameOption, string nameMDI)
        {
            Form frm = new Form();
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == nameMDI)
                    frm = f;

            }

            foreach (ToolStripMenuItem mnu in frm.MainMenuStrip.Items)
            {
                foreach (ToolStripDropDownItem smnu in mnu.DropDownItems)
                {
                    foreach (ToolStripDropDownItem semnu in smnu.DropDownItems)
                    {
                        if (semnu.Name == nameOption)
                        {
                            semnu.Enabled = true;
                            semnu.Visible = true;
                        }
                    }


                }
            }
        }

        public void enabledMenuAdministrator(string nameMDI)
        {
            Form frm = new Form();
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == nameMDI)
                    frm = f;
            }


            foreach (ToolStripMenuItem mnu in frm.MainMenuStrip.Items)
            {
                if (mnu.Name == mdiMain.MENU_REGISTER)
                {
                    mnu.Enabled = true;
                    mnu.Visible = true;
                    foreach (ToolStripDropDownItem smnu in mnu.DropDownItems)
                    {
                        smnu.Enabled = true;
                        smnu.Visible = true;
                    }
                }
                if (mnu.Name == mdiMain.MENU_CONFIGURATION)
                {
                    mnu.Enabled = true;
                    mnu.Visible = true;
                    foreach (ToolStripDropDownItem smnu in mnu.DropDownItems)
                    {
                        smnu.Enabled = true;
                        smnu.Visible = true;
                    }
                }
                if (mnu.Name == mdiMain.MENU_FILE)
                {                  
                    foreach (ToolStripDropDownItem smnu in mnu.DropDownItems)
                    {
                        smnu.Enabled = true;
                        smnu.Visible = true;
                    }
                }
            }
        }

        public void enabledMenuOperator(string nameMDI)
        {
            Form frm = new Form();
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == nameMDI)
                    frm = f;
            }
            foreach (ToolStripMenuItem mnu in frm.MainMenuStrip.Items)
            {
                if (mnu.Name == mdiMain.MENU_REGISTER)
                {
                    mnu.Enabled = true;
                    mnu.Visible = true;
                    foreach (ToolStripDropDownItem smnu in mnu.DropDownItems)
                    {
                        if (smnu.Name == mdiMain.MENU_PRODUCTS ||
                            smnu.Name == mdiMain.MENU_USERS || 
                            smnu.Name == mdiMain.MENU_MOBILE)
                        {
                            smnu.Enabled = false;
                            smnu.Visible = false;
                        }
                        else
                        {
                            smnu.Enabled = true;
                            smnu.Visible = true;
                        }

                    }
                }
                if (mnu.Name == mdiMain.MENU_FILE)
                {
                    foreach (ToolStripDropDownItem smnu in mnu.DropDownItems)
                    {
                        smnu.Enabled = true;
                        smnu.Visible = true;
                    }
                }
            }
        }

        public void enabledMenuCreateDB(string nameMDI)
        {
            Form frm = new Form();
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == nameMDI)
                    frm = f;
            }
            foreach (ToolStripMenuItem mnu in frm.MainMenuStrip.Items)
            {
                if (mnu.Name == mdiMain.MENU_CONFIGURATION)
                {
                    mnu.Enabled = true;
                    mnu.Visible = true;
                    foreach (ToolStripDropDownItem smnu in mnu.DropDownItems)
                    {
                        if (smnu.Name == mdiMain.MENU_DATABASE)
                        {
                            smnu.Enabled = true;
                            smnu.Visible = true;
                        }
                        
                    }
                }

                if (mnu.Name == mdiMain.MENU_FILE)
                {
                    mnu.Enabled = true;
                    mnu.Visible = true;
                    foreach (ToolStripDropDownItem smnu in mnu.DropDownItems)
                    {
                        if (smnu.Name == mdiMain.MENU_ACCESS)
                        {
                            smnu.Enabled = false;
                            smnu.Visible = false;
                        }

                    }
                }

            }
        }

        public string getPrintDefault()
        {
            PrinterSettings print = new PrinterSettings();
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                print.PrinterName = PrinterSettings.InstalledPrinters[i].ToString();
                if (print.IsDefaultPrinter)
                    return PrinterSettings.InstalledPrinters[i].ToString();
            }
            return String.Empty;

        }

        #endregion
    }
}

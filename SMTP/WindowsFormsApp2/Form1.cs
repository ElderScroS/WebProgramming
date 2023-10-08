using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SendMessageBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(EmailTextBox.Text) || string.IsNullOrEmpty(PasswdTextBox.Text) || string.IsNullOrEmpty(TitleTextBox.Text) || string.IsNullOrEmpty(MessageTextBox.Text))
                    MessageBox.Show("Some fields are empty!!!");
                else if (!(EmailTextBox.Text.Contains("@")))
                    MessageBox.Show("Email is incorrect!!!");
                else
                {
                    SmtpImap smtpImap = new SmtpImap(EmailTextBox.Text, PasswdTextBox.Text, TitleTextBox.Text, MessageTextBox.Text);
                    smtpImap.SMTP();
                    smtpImap.IMAP();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

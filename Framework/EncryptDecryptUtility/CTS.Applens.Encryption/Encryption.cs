using AVMCoE.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTS.Applens.Encryption
{
    public partial class Encryption : Form
    {
        public Encryption()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckKey() && checkMandatory())
                {
                    if (rbEncrypt.Checked)
                    {
                        txtResult.Text = Convert.ToBase64String(new AESEncryption().EncryptStringAsBytes(txtInput.Text.Trim(), Convert.FromBase64String(txtKey.Text.Trim())));
                    }
                    else if (rbDecrypt.Checked)
                    {
                        txtResult.Text = new AESEncryption().DecryptStringBytes(txtInput.Text.Trim(), Convert.FromBase64String(txtKey.Text.Trim()));
                        if (string.IsNullOrEmpty(txtResult.Text))
                        {
                            MessageBox.Show("Decryption failed. Please check your data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private bool checkMandatory()
        {
            if (rbEncrypt.Checked)
            {
                if (string.IsNullOrEmpty(txtInput.Text.Trim()))
                {
                    MessageBox.Show("Please enter text to encrypt","Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            if (rbDecrypt.Checked)
            {
                if (string.IsNullOrEmpty(txtInput.Text.Trim()))
                {
                    MessageBox.Show("Please enter text to decrypt", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        private bool CheckKey()
        {
            if (string.IsNullOrEmpty(txtKey.Text.Trim()))
            {
                MessageBox.Show("Key is mandatory", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                if (txtKey.Text.Trim().Length != 32)
                {
                    MessageBox.Show("Key length should be 32", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (!txtKey.Text.Trim().IsBase64String())
                {
                    MessageBox.Show("Key is not a valid base 64 string", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        private void rbEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            clearControls();
            btn.Text = "Encrypt";
            label1.Text = "Text to Encrypt";
            label2.Text = "Encrypted text";
        }

        private void rbDecrypt_CheckedChanged(object sender, EventArgs e)
        {
            clearControls();
            btn.Text = "Decrypt";
            label1.Text = "Text to Decrypt";
            label2.Text = "Decrypted text";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rbEncrypt.Checked = true;
            clearControls();
            btn.Text = "Encrypt";
            label1.Text = "Text to Encrypt";
            label2.Text = "Encrypted text";
        }

        private void clearControls()
        {
            txtInput.Clear();
            txtResult.Clear();
            txtKey.Clear();
        }

    }

    public static class Base64
    {
        public static bool IsBase64String(this string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

    }
}

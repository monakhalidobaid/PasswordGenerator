using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PasswordGenerator
{

   enum RangeType {Uppercase,  Lowercase,   Numbers,   Symbols }
    
    public partial class Form1 : Form
    {
        struct PasswordInfo
        {
            public string Password;
            public int PasswordLength;
            public List<RangeType> RangeTypes;

        }
        PasswordInfo _passwordInfo = new PasswordInfo();

        Random rnd = new Random();

    

        public Form1()
        {
            InitializeComponent();
        }

        private char GetRandomChar(RangeType CharRange)
        {
            switch (CharRange)
            {
                case RangeType.Uppercase:
                    return (char)rnd.Next(65, 91);
                case RangeType.Lowercase:
                    return (char)rnd.Next(97, 123);
                case RangeType.Numbers:
                    return (char)rnd.Next(48, 58);
                default:
                    return (char)rnd.Next(33, 48);
            }
        }


        private string GeneratePassword()
        {
            RangeType CharRange = _passwordInfo.RangeTypes[rnd.Next(0, _passwordInfo.RangeTypes.Count)]; ;
            for (int i = 1; i <= _passwordInfo.PasswordLength; i++)
            {

                _passwordInfo.Password += GetRandomChar(CharRange).ToString();
                CharRange = _passwordInfo.RangeTypes[rnd.Next(0, _passwordInfo.RangeTypes.Count)]; 

            }
            return _passwordInfo.Password;
        }

        private void ReadPasswordLength()
        {
            _passwordInfo.PasswordLength = (int)nudPassLength.Value;
        }

        private void ReadRangeType(object sender, EventArgs e)
        {

            Guna2CheckBox checkBox = (Guna2CheckBox)sender;
            int.TryParse(checkBox.Tag.ToString(), out int checkBoxTag);
            RangeType rangeType = (RangeType)checkBoxTag;
            if (checkBox.Checked)
            {
                _passwordInfo.RangeTypes.Add(rangeType);
            }
            else
            {
                if (_passwordInfo.RangeTypes.Contains(rangeType))
                {
                    _passwordInfo.RangeTypes.Remove(rangeType);
                }
            }
        }



        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            if (_passwordInfo.RangeTypes.Count > 0)
            {
                _passwordInfo.Password = string.Empty;
                ReadPasswordLength();
                lblPass.Text = GeneratePassword();
                CheckPasswordStrength();
            }
            else
            {
                guna2MessageDialog1.Show();
            }

        }

        private void ClearPasswordStruct()
        {
            _passwordInfo.PasswordLength = 8;
            _passwordInfo.Password = string.Empty;
            _passwordInfo.RangeTypes.Clear();
        }

        private void ClearCheckBox()
        {
            List<Guna2CheckBox> controls = new List<Guna2CheckBox> { cbUppercaseLetters, cbLowercaseLetters, cbNumbers, cbSymbols };
            foreach (Guna2CheckBox control in controls)
            {
                control.Checked = false;
            }
        }
        private void Clear()
        {
            ClearPasswordStruct();
            ClearCheckBox();
            lblPass.Text = string.Empty;
            nudPassLength.Value = 8;
            lblpwdStrength.Text = string.Empty;
            pbInfo.Visible = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
             lblPass.Text = string.Empty;
            _passwordInfo.RangeTypes = new List<RangeType>();
            pbInfo.Visible = false;

        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void CheckPasswordStrength()
        {
            string password = lblPass.Text;

            int score = 0;

            if (password.Length >= 16)
                score += 3;
            else if (password.Length >= 12)
                score += 2;
            else if (password.Length >= 8)
                score += 1;

            if (password.Any(char.IsUpper))
                score++;

            if (password.Any(char.IsLower))
                score++;

            if (password.Any(char.IsDigit))
                score++;

            if (password.Any(c => !char.IsLetterOrDigit(c)))
                score++;

            pbInfo.Visible = true;

            if (score <= 2)
            {
                lblpwdStrength.ForeColor = Color.Red;
                lblpwdStrength.Text = "Your password is weak!";
            }
            else if (score <= 5)
            {
                lblpwdStrength.ForeColor = Color.DarkOrange;
                lblpwdStrength.Text = "Your password is moderate!";
            }
            else
            {
                lblpwdStrength.ForeColor = Color.PaleGreen;
                lblpwdStrength.Text = "Your password is strong!";
            }
        }


        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblPass.Text))
            {
                Clipboard.SetText(lblPass.Text);
            }
        }
    }
}

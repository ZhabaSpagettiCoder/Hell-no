using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Win32;
namespace Crpt
{
    public partial class Form1 : Form
    {
        
        public byte[] Key_IV_SETTER(bool a)
        {
            switch (a)
            {
                case true:
                    {
                        byte[] arr = new byte[32];
                        using (FileStream fs = new FileStream("D:\\CSHRP\\KEY.txt", FileMode.Open, FileAccess.Read))
                        {
                            fs.Read(arr, 0, 32);
                        }
                        return arr;

                    }
                case false:
                    {
                        byte[] arr = new byte[16];
                        using (FileStream fs = new FileStream("D:\\CSHRP\\IV.txt", FileMode.Open, FileAccess.Read))
                        {
                            fs.Read(arr, 0, 16);
                        }
                        return arr;
                    }
                default: { return null; }
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] encrypted;
            Aes crpt = Aes.Create();
            crpt.IV = Key_IV_SETTER(false);
            crpt.Key = Key_IV_SETTER(true);
            ICryptoTransform crypt = crpt.CreateEncryptor(crpt.Key, crpt.IV);
            
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(textBox1.Text);
                    }
                }
                encrypted = ms.ToArray();
                byte[] arr = encrypted.ToArray();
                using (FileStream fs = new FileStream("D:\\CSHRP\\ENCRYPTED.txt", FileMode.Open, FileAccess.Write))
                {
                    fs.Write(arr,0,16);
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        { 
            Aes dcrpt = Aes.Create();
            dcrpt.IV = Key_IV_SETTER(false);
            dcrpt.Key = Key_IV_SETTER(true);
            byte[] data = new byte[16];
            using(FileStream fs = new FileStream("D:\\CSHRP\\ENCRYPTED.txt", FileMode.Open, FileAccess.Read))
            {
                fs.Read(data, 0, 16);
            }
            string DecryptedText;
            ICryptoTransform dcrptr = dcrpt.CreateDecryptor(dcrpt.Key, dcrpt.IV);
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (CryptoStream cs = new CryptoStream(ms, dcrptr, CryptoStreamMode.Read))
                {
                    using (StreamReader sw = new StreamReader(cs))
                    {
                        DecryptedText = sw.ReadToEnd();
                    }
                }
            }
            textBox2.Text = DecryptedText;
        }
    }
}

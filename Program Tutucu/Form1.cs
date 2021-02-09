using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;


namespace Program_Tutucu
{
    using System;
    using System.Text;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<Dosya> dosya=new List<Dosya>();

        private string s;
        private void button1_Click(object sender, System.EventArgs e)//ekle
        {
            dosya.Add(new Dosya(this.adT.Text, this.adres(adresT.Text)));
            this.listele();
            this.xml_ekle();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            this.liste_sil(this.s);
            this.listele();
            this.xml_ekle();
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            Process.Start(@"" +this.bul(s)+ "");
        }

        private string bul(string ad)
        {
            return (from dos in this.dosya where dos.Ad == ad select dos.Adres).FirstOrDefault();
        }

        private void liste_sil(string ad)
        {
            dosya.RemoveAll(dos => dos.Ad == ad);
        }
        private void listele()
        {
            listBox1.Items.Clear();
            foreach (Dosya dos in this.dosya)
            {
                this.listBox1.Items.Add(dos.Ad);
            }
        }
       
        private void list_ekle()
        {
            XmlDocument xml = new XmlDocument();
            try
            {
               
                xml.Load(@"" + Application.StartupPath + "\\xml.xml");
                foreach (XmlNode xmlNode in xml.SelectNodes("/dosya/dosyaN"))
                {
                    this.dosya.Add(new Dosya(xmlNode.Attributes["Ad"].Value, xmlNode.Attributes["Adres"].Value));
                    this.listele();
                }
            }
            catch
            {
                // ignored
            }
            
        }

        private void xml_ekle()
        {
            XmlDocument xml = new XmlDocument();
            XmlNode dosyaa = xml.CreateElement("dosya");
            xml.AppendChild(dosyaa);
            foreach (Dosya dos in dosya)
            {
                
                XmlNode dosyaNode = xml.CreateElement("dosyaN");

                XmlAttribute adAttribute = xml.CreateAttribute("Ad");
                adAttribute.Value = dos.Ad;
                dosyaNode.Attributes.Append(adAttribute);

                XmlAttribute adresAttribute = xml.CreateAttribute("Adres");
                adresAttribute.Value = dos.Adres;
                dosyaNode.Attributes.Append(adresAttribute);

                dosyaa.AppendChild(dosyaNode);
            }

            XmlTextWriter xmlolustur = new XmlTextWriter(@"" + Application.StartupPath + "\\xml.xml", Encoding.UTF8);
            xmlolustur.Formatting = Formatting.Indented;
            xml.WriteContentTo(xmlolustur);
            xmlolustur.Close();
        }

        private void button4_Click(object sender, System.EventArgs e)//Gözat
        {
            file.ShowDialog();
            adT.Text = file.SafeFileName;
            adresT.Text = file.FileName;
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.list_ekle();
            }
            catch
            {
                // ignored
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.s = listBox1.SelectedItem.ToString();
           this.adT.Text=s;
            adresT.Text = bul(s);
        }

        private void guncel(string ad)
        {
            foreach (Dosya dos in dosya)
            {
                if (dos.Ad == ad)
                {
                    dos.Ad = adT.Text;
                    dos.Adres = this.adres(adresT.Text);
                }
            }
            this.listele();
        }
        private void button5_Click(object sender, EventArgs e)//güncelle
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(@"" + Application.StartupPath + "\\xml.xml");
            foreach (XmlNode xmlNode in xml.SelectNodes("/dosyaN"))
            {
                if (xmlNode.Attributes["Ad"].Value==adT.Text)
                this.guncel(adT.Text);
                this.listele();
            }
        }

        private string adres(string a)
        {
            string b=null;
            for (int i = 0; i < a.Length; i++)
            {

                if (Convert.ToByte(a[i]) == 92 && Convert.ToByte(a[i + 1]) != 92)
                {
                    b += Convert.ToChar(92).ToString();
                    b += Convert.ToChar(92).ToString();
                }
                else b += a[i];
            }
            return b;
        }
    }

    public class Dosya 
    {
       
        private string ad;
        private string adres;

        public Dosya(string ad, string adres)
        {
            
            this.Ad = ad;
            this.Adres = adres;
        }

       
        public string Ad
        {
            get { return ad; }
            set { ad = value; }
        }
        public string Adres
        {
            get { return adres; }
            set { adres = value; }
        }

       
    }

}

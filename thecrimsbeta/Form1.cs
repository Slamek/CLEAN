using System;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace thecrimsbeta
{
    /*TO DO:
     * 
     * Prodaj kurbe ki se nabirajo 
     * 
     * Ce prije do tega da nimas več nobenih drog za cuzat v klubi 
     * ->Poberes ce je ek v zgradbah
     * ->Kupis drogo ki je NA SEZNAMI v klubi
     * 
     * Vsakic ko se gre drogirat, če prve droge ni na lageri, more it še ostale poglejat če so
     * če pa jih ni pa beri odstavek gor :D
     * 
     * Da boš lahko 2 crimsa odprl (inkognito pomoje nebo dovol ker ip glejajo)
     * -> Da sodelujeta v gang ropih (en bo vedno ponudo rop ostali se joinajo zadnji izvede) <- zadnji je tisti keri ma zaporedno številko enako max roparov na ropi
     * 
     * Vse skup bi mogo v exception ovit ker včasih ga spegla pa ga logoutne, se pravi bi mogo poglejat še ma spet username pa pass okna na voljo(še en exception če obstajajo)
     * -> če bi to bil razlog bi se mogo pa loginat in dalje farmat
     * 
     * -> Razred Skladišče, ki ma vse droge prek pri dileri
     * 
     * 
     * 
     * 
     * 
     */
    public partial class Form1 : Form
    {
        IWebDriver driver;
        public void OpenDriver()

        {
            ChromeOptions profil = new ChromeOptions();
            //profil.AddArguments("user-data-dir=C:/Users/PALACE/AppData/Local/Google/Chrome/User Data/Profile 2");
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }
        public void Login(string username,string password)
        {
            if (tbPassword.Text.Length == 0 | tbUserName.Text.Length == 0)
            {
                MessageBox.Show("Vnesi acc podatke!");
                return;
            }
            try
            {
                driver.Url = "https://www.thecrims.com/";
                driver.FindElement(By.XPath("//*[@id='loginform']/input[1]")).Clear();
                driver.FindElement(By.XPath("//*[@id='loginform']/input[1]")).SendKeys(username);
                driver.FindElement(By.XPath("//*[@id='loginform']/input[2]")).SendKeys(password);
                driver.FindElement(By.XPath("//*[@id='loginform']/input[2]")).SendKeys(OpenQA.Selenium.Keys.Enter);
            }
            catch
            {
                //Nisma na login page oz sma že prijavlena
            }
            
            /*
            //Preveri če se je logino -> ce lahko prebere dnare
            try
            {
                //*[@id="content_right"]/div/div[6]/div[4]/span
                string a = driver.FindElement(By.XPath("//[@id='user-profile-stamina']")).Text;

            }
            catch{
                MessageBox.Show("Napacni podatki!");
                return;
            }
            */
        }


        public Form1()
        {
            OpenDriver();
            InitializeComponent();  
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //AKCIJA! BUTTON
        private void button1_Click(object sender, EventArgs e)
        {

            bool igraj = true;

            while (igraj)
            {
                Login(tbUserName.Text, tbPassword.Text);
                System.Threading.Thread.Sleep(5000);
                try
                {
                    Igraj();
                }
                catch
                {
                    //ce fukne vun je mozno da ga je logautali -> na novo se logina -> spila dalje
                    System.Threading.Thread.Sleep(5000);
                }
                

            }

        }

        private void Igraj()
        {
           
            Player player = new Player(driver);
      
            
            //se za zgradbe
            player.SetDejavnosti();
            player.SetSpostovanje();
            player.SetElvlP();


            while (true)
            {
                Rop rop = new Rop(driver);
                while (player.energija >= 10)
                {
                    if(cbAssault.Checked == true)
                    {
                        //TODO: DA SE ŠE TAM PRIDRUZI;
                        if (cbGang.Checked == true )
                        {
                            rop.RopajGang();
                            if (cbSolo.Checked == true & player.GetTickets() > 50)
                            {
                                rop.Ropaj();
                            }
                        }
                        else
                        {
                            if (cbSolo.Checked == true & player.GetTickets() > 50)
                            {
                                rop.Ropaj();
                            }

                        }
                    }
                    else
                    {
                        if(cbGang.Checked == true)
                        {
                            rop.RopajGang();
                            if (cbSolo.Checked == true & player.GetTickets() > 50)
                            {
                                rop.Ropaj();
                            }
                        }
                        else
                        {
                            if (cbSolo.Checked == true & player.GetTickets() > 50)
                            {
                                rop.Ropaj();
                            }

                        }
                    }


                    player.SetElvlP();
                    player.SetSpostovanje();


                }

                
                if(cbTickets.Checked == true && player.GetTickets() != 0 & player.GetTickets() > int.Parse(tbMinTickets.Text))
                {
                    player.dejavnosti[0].NastaviMaxSpostovanjeKlubu(1, player.dejavnosti[0].url, driver);
                    System.Threading.Thread.Sleep(1000);
                    player.dejavnosti[0].NastaviMaxSpostovanjeKlubu(((player.spostovanje + 50) / 100 * 100) + 500, player.dejavnosti[0].url, driver);
                    player.FillEnergy(0);
                }
                else
                {
                    player.FillEnergy(1);
                }
               
                player.Detox();
            }


            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tbUserName.Text = "samek";
            tbPassword.Text = "7da3dc8c44b8";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            tbUserName.Text = "bokoto";
            tbPassword.Text = "6ecb7b40a41a";
        }
    }
    
}

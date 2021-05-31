using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace MediaMarktBot 
{
    public partial class MediaMarktXboxBot : Form
    {
        public MediaMarktXboxBot()
        {
            InitializeComponent();
        }

        // initiate browser, etc.
        public static IWebDriver driver = new FirefoxDriver();
        public WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(300));
        public IWebElement element;



        // start process
        public void button1_Click(object sender, EventArgs e)
        {
            driver.Manage().Cookies.DeleteAllCookies();

            // open paypal.com
            driver.Url = "https://www.paypal.com/de/signin";

            // click on "Accept all cookies" in paypal to finish payment
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("acceptAllButton")));
            element = driver.FindElement(By.Id("acceptAllButton"));
            element.Click();

            // wait until user logged in and notifications from homescreen are visible
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("header-notifications")));

            // Open media markt website after successful paypal login
            driver.Url = "https://www.mediamarkt.de/";

            // click on privacy accept button
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("privacy-layer-accept-all-button")));
            element = driver.FindElement(By.Id("privacy-layer-accept-all-button"));
            element.Click();

            // MediaMarkt Login; open "Mein Konto" dropdown
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[2]/div[2]/header/div/div[2]/div[5]/button[1]/div/span[1]")));
            element = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[2]/header/div/div[2]/div[5]/button[1]/div/span[1]"));
            element.Click();
            //click on "Anmelden"
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[1]/div[2]/div[2]/header/div/div[2]/div[5]/div/div/div/div/button[1]")));
            element = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[2]/header/div/div[2]/div[5]/div/div/div/div/button[1]"));
            element.Click();
            // wait until login windows disappeared / user logged in and entered account details
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[1]/div/div[2]")));

            // go to xbox article site
            driver.Url = "https://www.mediamarkt.de/de/product/_microsoft-xbox-series-x-1-tb-2677360.html";

            // start refresh cycle until article is in stock
            timer1.Start();
        }


        void timer1_Tick(object sender, EventArgs e)
        {
            if (driver.FindElement(By.XPath("//*[contains(text(),'Dieser Artikel ist aktuell nicht verfügbar.')]")).Displayed | driver.FindElement(By.XPath("//*[contains(text(),'Leider keine Lieferung möglich')]")).Displayed)
            {
                driver.Navigate().Refresh();
            }
            else
            {
                timer1.Stop();
                BuyArticle();
            }
        }



        // Buy Xbox since it's confirmed to be in stock
        void BuyArticle()
        {

            // click on "In den Warenkorb"
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("pdp-add-to-cart-button")));
            element = driver.FindElement(By.Id("pdp-add-to-cart-button"));
            element.Click();

            // click on "Zum Warenkorb"
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(text(),'Zum Warenkorb')]")));
            element = driver.FindElement(By.XPath("//*[contains(text(),'Zum Warenkorb')]"));
            element.Click();

            // click on "Zur Kasse gehen"
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(text(),'Zur Kasse gehen')]")));
            element = driver.FindElement(By.XPath("//*[contains(text(),'Zur Kasse gehen')]"));
            element.Click();

            // click on "Weiter" button while Paypal is automatically chosen (since its the first entry in the payment option list
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("#root > div.indexstyled__StyledAppWrapper-sc-1hu9cx8-0.klAfyt > div.CheckoutPageWrapper__StyledCheckoutPage-sc-10wdmdy-0.huVVXd > div.Grid__StyledGrid-fs0zc2-0.cQIsoQ.CheckoutPageWrapper__StyledGrid-sc-10wdmdy-1.foupww > div > div:nth-child(2) > div > div.Cellstyled__StyledCell-sc-1wk5bje-0.iNYTnU.StepWrapper__StyledSummary-sc-8bgpux-4.gHCmSn > div > div > div > div > div.BasketResume__StyledSummaryButtons-l2uuxo-0.kUGHLa > div > button")));
            element = driver.FindElement(By.CssSelector("#root > div.indexstyled__StyledAppWrapper-sc-1hu9cx8-0.klAfyt > div.CheckoutPageWrapper__StyledCheckoutPage-sc-10wdmdy-0.huVVXd > div.Grid__StyledGrid-fs0zc2-0.cQIsoQ.CheckoutPageWrapper__StyledGrid-sc-10wdmdy-1.foupww > div > div:nth-child(2) > div > div.Cellstyled__StyledCell-sc-1wk5bje-0.iNYTnU.StepWrapper__StyledSummary-sc-8bgpux-4.gHCmSn > div > div > div > div > div.BasketResume__StyledSummaryButtons-l2uuxo-0.kUGHLa > div > button"));
            element.Click();

            // click on "Fortfahren und bezahlen"
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("#root > div.indexstyled__StyledAppWrapper-sc-1hu9cx8-0.klAfyt > div.CheckoutPageWrapper__StyledCheckoutPage-sc-10wdmdy-0.huVVXd > div.Grid__StyledGrid-fs0zc2-0.cQIsoQ.CheckoutPageWrapper__StyledGrid-sc-10wdmdy-1.foupww > div > div:nth-child(2) > div > div.Cellstyled__StyledCell-sc-1wk5bje-0.iNYTnU.StepWrapper__StyledSummary-sc-8bgpux-4 > div > div > div > div > div.BasketResume__StyledSummaryButtons-l2uuxo-0.eVfzHQ > div > button")));
            element = driver.FindElement(By.CssSelector("#root > div.indexstyled__StyledAppWrapper-sc-1hu9cx8-0.klAfyt > div.CheckoutPageWrapper__StyledCheckoutPage-sc-10wdmdy-0.huVVXd > div.Grid__StyledGrid-fs0zc2-0.cQIsoQ.CheckoutPageWrapper__StyledGrid-sc-10wdmdy-1.foupww > div > div:nth-child(2) > div > div.Cellstyled__StyledCell-sc-1wk5bje-0.iNYTnU.StepWrapper__StyledSummary-sc-8bgpux-4 > div > div > div > div > div.BasketResume__StyledSummaryButtons-l2uuxo-0.eVfzHQ > div > button"));
            element.Click();

            // click on "Pay now" in paypal to finish payment
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("payment-submit-btn")));
            element = driver.FindElement(By.Id("payment-submit-btn"));
            System.Threading.Thread.Sleep(8000);
            Actions actions = new Actions(driver);
            actions.MoveToElement(element);
            actions.Perform();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("payment-submit-btn")));
            element = driver.FindElement(By.Id("payment-submit-btn"));
            element.Click();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("README.txt");
        }
    }
}

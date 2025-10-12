using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.Json;

namespace E_Book_Tests;

public class UISeleniumTests : IDisposable
{
    private readonly IWebDriver driver;
    private readonly LoggingObj[] LoggingArr;

    public UISeleniumTests()
    {
        driver = new ChromeDriver();
        LoggingArr = LogginProvider.GetLoggingArr();
    }

    [Fact]
    public void ClientValidation()
    {
        //Arrange
        const string FIELD_IS_REQUIRED = "field is required.";
        const string PRICE_RANGE = "The field Price must be between 0 and 1000000.";

        Login(LoggingArr.First(login => login.Type == "Writer"));
        driver.Navigate().GoToUrl("https://localhost:7143/EBooks/Create");

        var AuthorField = driver.FindElement(By.XPath("//*[@id=\"Author\"]"));
        var TitleField = driver.FindElement(By.XPath("//*[@id=\"Title\"]"));
        var ContentField = driver.FindElement(By.XPath("//*[@id=\"Content\"]"));
        var PriceField = driver.FindElement(By.XPath("//*[@id=\"Price\"]"));

        //Act
        AuthorField.Click();
        TitleField.Click();
        ContentField.Click();
        PriceField.Click();
        AuthorField.Click();

        var AuthorErrorField = driver.FindElement(By.XPath("//*[@id=\"Author-error\"]"));
        var TitleErrorField = driver.FindElement(By.XPath("//*[@id=\"Title-error\"]"));
        var ContentErrorField = driver.FindElement(By.XPath("//*[@id=\"Content-error\"]"));
        var PriceErrorField = driver.FindElement(By.XPath("//*[@id=\"Price-error\"]"));

        var AuthorErrorText = AuthorErrorField.Text;
        var TitleErrorText = TitleErrorField.Text;
        var ContentErrorText = ContentErrorField.Text;
        var PriceErrorText = PriceErrorField.Text;

        PriceField.SendKeys("-1");
        PriceErrorField = driver.FindElement(By.XPath("//*[@id=\"Price-error\"]"));
        var PriceDownErrorRangeText = PriceErrorField.Text;

        PriceField.SendKeys("1000000000");
        PriceErrorField = driver.FindElement(By.XPath("//*[@id=\"Price-error\"]"));
        var PriceUpErrorRangeText = PriceErrorField.Text;

        //Assert
        Assert.Contains(FIELD_IS_REQUIRED, AuthorErrorText);
        Assert.Contains(FIELD_IS_REQUIRED, TitleErrorText);
        Assert.Contains(FIELD_IS_REQUIRED, ContentErrorText);
        Assert.Contains(FIELD_IS_REQUIRED, PriceErrorText);

        Assert.Equal(PRICE_RANGE, PriceUpErrorRangeText);
        Assert.Equal(PRICE_RANGE, PriceDownErrorRangeText);
    }

    private void Login(LoggingObj loggingData)
    {
        driver.Navigate().GoToUrl("https://localhost:7143/Identity/Account/Login");
        driver.FindElement(By.XPath("//*[@id=\"Input_Email\"]")).SendKeys(loggingData.Login);
        driver.FindElement(By.XPath("//*[@id=\"Input_Password\"]")).SendKeys(loggingData.Password);
        driver.FindElement(By.XPath("//*[@id=\"login-submit\"]")).Click();
    }

    public void Dispose()
    {
        driver.Quit();
        driver.Dispose();
    }
}
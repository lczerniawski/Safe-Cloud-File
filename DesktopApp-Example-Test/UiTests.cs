using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace DesktopApp_Example_Test
{
    [TestClass]
    public class UiTests
    {
        [TestMethod]
        public void TestRegister()
        {
            var loginWindowSession = GetLoginWindowSession();

            var buttonRegister = loginWindowSession.FindElementByAccessibilityId("buttonRegister");
            buttonRegister.Click();
            loginWindowSession.FindElementByAccessibilityId("textBoxName").SendKeys("Testowe Imie");
            loginWindowSession.FindElementByAccessibilityId("textBoxEmail").SendKeys("Testowy Mail");
            loginWindowSession.FindElementByAccessibilityId("textBoxPassword").SendKeys("Testowe Haslo");

            Assert.AreEqual(true, buttonRegister.Displayed);
            Assert.AreEqual(true,buttonRegister.Enabled);
        }

        [TestMethod]
        public void TestLogin()
        {
            var loginWindowSession = GetLoginWindowSession();

            var buttonLogin = loginWindowSession.FindElementByAccessibilityId("buttonLogin");
            buttonLogin.Click();
            loginWindowSession.FindElementByAccessibilityId("textBoxEmail").SendKeys("Testowy Mail");
            loginWindowSession.FindElementByAccessibilityId("textBoxPassword").SendKeys("Testowe Haslo");

            Assert.AreEqual(true, buttonLogin.Displayed);
            Assert.AreEqual(true, buttonLogin.Enabled);
        }

        [TestMethod]
        public void TestDriveSelect()
        {
            var authDataPath = @"D:\Safe-Cloud-File\DesktopApp-Example\bin\Debug\authdata.json";
            if (File.Exists(authDataPath))
                File.Delete(authDataPath);

            var loginWindowSession = GetLoginWindowSession();

            loginWindowSession.FindElementByAccessibilityId("textBoxEmail").SendKeys("test@testowy.pl");
            loginWindowSession.FindElementByAccessibilityId("textBoxPassword").SendKeys("testowe");
            loginWindowSession.FindElementByAccessibilityId("buttonLogin").Click();

            var appSessionFileServiceWindow = GetAppWindowSession("FileServiceSelect");

            var buttonGoogleDrive = appSessionFileServiceWindow.FindElementByAccessibilityId("buttonGoogleDrive");
            var buttonOneDrive = appSessionFileServiceWindow.FindElementByAccessibilityId("buttonOneDrive");
            var buttonOwnServer = appSessionFileServiceWindow.FindElementByAccessibilityId("buttonOwnServer");

            Assert.AreEqual(true, buttonGoogleDrive.Displayed);
            Assert.AreEqual(true, buttonOneDrive.Displayed);
            Assert.AreEqual(true, buttonOwnServer.Displayed);

            Assert.AreEqual(true, buttonGoogleDrive.Enabled);
            Assert.AreEqual(true, buttonOneDrive.Enabled);
            Assert.AreEqual(true, buttonOwnServer.Enabled);
        }

        [TestMethod]
        public void MainWindowTest()
        {
            var authDataPath = @"D:\Safe-Cloud-File\DesktopApp-Example\bin\Debug\authdata.json";
            if (File.Exists(authDataPath))
                File.Delete(authDataPath);

            var loginWindowSession = GetLoginWindowSession();

            loginWindowSession.FindElementByAccessibilityId("textBoxEmail").SendKeys("test@testowy.pl");
            loginWindowSession.FindElementByAccessibilityId("textBoxPassword").SendKeys("testowe");
            loginWindowSession.FindElementByAccessibilityId("buttonLogin").Click();

            var appSessionFileServiceWindow = GetAppWindowSession("FileServiceSelect");

            appSessionFileServiceWindow.FindElementByAccessibilityId("buttonGoogleDrive").Click();
            var appSessionMainWindow = GetAppWindowSession("MainWindow");

            var buttonDownloadShared = appSessionMainWindow.FindElementByAccessibilityId("buttonDownloadShared");
            var buttonUpload = appSessionMainWindow.FindElementByAccessibilityId("buttonUpload");
            var listBoxFiles = appSessionMainWindow.FindElementByAccessibilityId("listBoxFiles");


            Assert.AreEqual(true, buttonDownloadShared.Displayed);
            Assert.AreEqual(true, buttonUpload.Displayed);
            Assert.AreEqual(true, listBoxFiles.Displayed);

            Assert.AreEqual(true, buttonDownloadShared.Enabled);
            Assert.AreEqual(true, buttonUpload.Enabled);
            Assert.AreEqual(true, listBoxFiles.Enabled);
        }

        [TestMethod]
        public void ShareFileWindowTest()
        {
            var authDataPath = @"D:\Safe-Cloud-File\DesktopApp-Example\bin\Debug\authdata.json";
            if (File.Exists(authDataPath))
                File.Delete(authDataPath);

            var loginWindowSession = GetLoginWindowSession();

            loginWindowSession.FindElementByAccessibilityId("textBoxEmail").SendKeys("test@testowy.pl");
            loginWindowSession.FindElementByAccessibilityId("textBoxPassword").SendKeys("testowe");
            loginWindowSession.FindElementByAccessibilityId("buttonLogin").Click();

            var appSessionFileServiceWindow = GetAppWindowSession("FileServiceSelect");
            appSessionFileServiceWindow.FindElementByAccessibilityId("buttonGoogleDrive").Click();

            var appSessionMainWindow = GetAppWindowSession("MainWindow");
            appSessionMainWindow.FindElementByAccessibilityId("buttonUpload").Click();
            var checkBoxShare = appSessionMainWindow.FindElementByAccessibilityId("checkBoxShare");
            var listBoxUsers = appSessionMainWindow.FindElementByAccessibilityId("listBoxUsers");
            var buttonNext = appSessionMainWindow.FindElementByAccessibilityId("buttonNext");

            Assert.AreEqual(true, checkBoxShare.Displayed);
            Assert.AreEqual(true, listBoxUsers.Displayed);
            Assert.AreEqual(true, buttonNext.Displayed);
            Assert.AreEqual(false, checkBoxShare.Selected);
            Assert.AreEqual(false, listBoxUsers.Enabled);
            Assert.AreEqual(true, buttonNext.Enabled);

            checkBoxShare.Click();
            Assert.AreEqual(true, checkBoxShare.Selected);
            Assert.AreEqual(true, listBoxUsers.Enabled);
            Assert.AreEqual(false, buttonNext.Enabled);

            listBoxUsers.FindElementByName("Testowy").Click();
            Assert.AreEqual(true, checkBoxShare.Selected);
            Assert.AreEqual(true, listBoxUsers.Enabled);
            Assert.AreEqual(true, buttonNext.Enabled);
        }

        [TestMethod]
        public void DownloadShardWindowTest()
        {
            var authDataPath = @"D:\Safe-Cloud-File\DesktopApp-Example\bin\Debug\authdata.json";
            if (File.Exists(authDataPath))
                File.Delete(authDataPath);

            var loginWindowSession = GetLoginWindowSession();

            loginWindowSession.FindElementByAccessibilityId("textBoxEmail").SendKeys("test@testowy.pl");
            loginWindowSession.FindElementByAccessibilityId("textBoxPassword").SendKeys("testowe");
            loginWindowSession.FindElementByAccessibilityId("buttonLogin").Click();

            var appSessionFileServiceWindow = GetAppWindowSession("FileServiceSelect");
            appSessionFileServiceWindow.FindElementByAccessibilityId("buttonGoogleDrive").Click();

            var appSessionMainWindow = GetAppWindowSession("MainWindow");
            appSessionMainWindow.FindElementByAccessibilityId("buttonDownloadShared").Click();

            var textBoxJsonLink = appSessionMainWindow.FindElementByAccessibilityId("textBoxJsonLink");
            var textBoxFileLink = appSessionMainWindow.FindElementByAccessibilityId("textBoxFileLink");
            var buttonDownload = appSessionMainWindow.FindElementByAccessibilityId("buttonDownload");

            Assert.AreEqual(true, textBoxJsonLink.Displayed);
            Assert.AreEqual(true, textBoxFileLink.Displayed);
            Assert.AreEqual(true, buttonDownload.Displayed);
            Assert.AreEqual(true, textBoxJsonLink.Enabled);
            Assert.AreEqual(true, textBoxFileLink.Enabled);
            Assert.AreEqual(false, buttonDownload.Enabled);

            textBoxJsonLink.SendKeys("testlink.pl");
            textBoxFileLink.SendKeys("testlink.pl");

            Assert.AreEqual(true, buttonDownload.Enabled);
        }

        private WindowsDriver<WindowsElement> GetLoginWindowSession()
        {
            var loginWindowAppiumOptions = new OpenQA.Selenium.Appium.AppiumOptions();
            loginWindowAppiumOptions.AddAdditionalCapability("app",
                @"D:\Safe-Cloud-File\DesktopApp-Example\bin\Debug\DesktopApp-Example.exe");
            loginWindowAppiumOptions.AddAdditionalCapability("appWorkingDir",
                @"D:\Safe-Cloud-File\DesktopApp-Example\bin\Debug\");
            var loginWindowSesion =
                new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), loginWindowAppiumOptions);
            loginWindowSesion.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            return loginWindowSesion;
        }

        private WindowsDriver<WindowsElement> GetAppWindowSession(string windowName)
        {
            var appiumOptionsDesktop = new OpenQA.Selenium.Appium.AppiumOptions();
            appiumOptionsDesktop.AddAdditionalCapability("app", "Root");
            var appSesionDesktop = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptionsDesktop);
            appSesionDesktop.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            var appWindow = appSesionDesktop.FindElementByAccessibilityId(windowName);
            var appTopLevelWindowHandle = appWindow.GetAttribute("NativeWindowHandle");
            appTopLevelWindowHandle = (int.Parse(appTopLevelWindowHandle)).ToString("x");

            var appiumOptionsMainWindow = new OpenQA.Selenium.Appium.AppiumOptions();
            appiumOptionsMainWindow.AddAdditionalCapability("appTopLevelWindow", appTopLevelWindowHandle);
            appiumOptionsMainWindow.AddAdditionalCapability("appWorkingDir",@"D:\Safe-Cloud-File\DesktopApp-Example\bin\Debug\");
            var appSesionMainWindow =
                new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptionsMainWindow);
            appSesionMainWindow.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            return appSesionMainWindow;
        }
    }
}

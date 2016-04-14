﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using System.IO;
using System;

namespace EP_HSRlearnIT.BusinessLayer.Testing.UniversalToolsTest
{
    [TestClass]
    public class FileSaverTests
    {

        [TestMethod]
        public void SaveFileTest()
        {
            FileSaver.SaveFile(@"c:\temp\HSRlearnIT\Test", "AES-GCM.txt");
            Assert.IsTrue(File.Exists(@"c:\temp\HSRlearnIT\Test\AES-GCM.txt"));
        }

        /// <summary>
        /// Test isn't stable!
        /// </summary>
        [TestMethod]
        public void UpdateFileTest()
        {
            FileSaver.SaveFile(@"c:\temp\HSRlearnIT\Test", "UpdateTest.txt");
            String file = Path.Combine(@"c:\temp\HSRlearnIT\Test", "UpdateTest.txt");
            FileSaver.UpdateFileContent(file, "10 Hello World!");
            FileSaver.UpdateFileContent(file ,"30 Hello Member!");
            Assert.AreEqual("30 Hello Member!", FileSaver.ReadFile(file));
        }

        /// <summary>
        /// Test isn't stable!
        /// </summary>
        [TestMethod]
        public void AddToFileTest()
        {
            FileSaver.SaveFile(@"c:\temp\HSRlearnIT\Test", "AddTest.txt");
            String file = Path.Combine(@"c:\temp\HSRlearnIT\Test", "AddTest.txt");
            FileSaver.ContentAddToFile(file, "10 Hello World! ");
            FileSaver.ContentAddToFile(file, "30 Hello Member!");
            Assert.AreEqual("10 Hello World! 30 Hello Member!", FileSaver.ReadFile(file));
        }


        [ClassCleanup]
        public static void CleanUp()
        {
                Directory.Delete(@"c:\temp\HSRlearnIT\Test", true);

        }
    }
}
// <copyright file="LoggingHandlerTest.cs">Copyright ©  2016</copyright>
using System;
using EP_HSRlearnIT;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace EP_HSRlearnIT//.LoggingHandler.Tests
{

    [TestClass()]
    public class LoggingHandlerTest
    {
        [TestMethod()]
        public void testMethod()
        {
            String _nreMessage = null;
            String date = null;
            String exception = null;
            String test = null;

            LoggingHandler loghand = new LoggingHandler();
            try
            {
                string s = null;
                if (s.Length == 0)
                {
                    Console.WriteLine(s);
                }
            }
            catch (NullReferenceException nre)
            {
                loghand.writeToLogFile(nre.Message, "testMethod");
                date = DateTime.Now.ToString();
                _nreMessage = nre.Message;
                exception = "Exception :";
                test = exception + date + " " + _nreMessage + " " + "testMethod";
            }
            catch (Exception ex)
            {
                loghand.writeToLogFile(ex.Message, "testMethod");
                Console.WriteLine("Exception occured " + ex.Message);

            }
            finally
            {

            }
            Console.ReadLine();

            StreamReader reader = new StreamReader(@"c:\logs\log.txt");
            string strToCompare = reader.ReadLine();
            string expectedstr = test;
            Assert.AreEqual(strToCompare, expectedstr);
        }

    }
}
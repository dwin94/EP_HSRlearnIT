// <copyright file="LoggingHandlerTest.cs">Copyright ©  2016</copyright>
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace EP_HSRlearnIT.Tests//.LoggingHandler.Tests
{
   
    [TestClass()]
    public class LoggingHandlerTest
    {
        #region Test1
        [TestMethod()]
        public void testToWriteIntoLogFileCatchedException()
        {
            String _nreMessage = null;
            String date = null;
            String exception = null;
            String test = null;

            ExceptionLogger loghand = new ExceptionLogger();
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
                test = exception + date + ": " + _nreMessage + " " + "testMethod";
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

            string currentLine = reader.ReadLine();

            while (reader.EndOfStream == false)
            {
                currentLine = reader.ReadLine();
            } 
            
      
            string strToCompare = currentLine;
            string expectedstr = test;
            Assert.AreEqual(strToCompare, expectedstr);
        }
        #endregion Test1



      

    }

}

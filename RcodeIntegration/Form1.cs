using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RDotNet;
using System.IO;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*ServerConfigurationEntities entity = new ServerConfigurationEntities();
            entity.Loginname = "Sachin.Babladi@continuum.net";
            entity.Password = "nkomlp@1234";
            entity.Mailserver = "smtp.office365.com";
            entity.Port = 587;
            entity.Connectiontimeout = 20000;

            ETReceive email = new ETReceive();
            email.Body = "Test";
            email.To = "sachin.babladi@continuum.net";
            email.From = "sachin.babladi@continuum.net";
            email.Subject = "Test";
            email.AttachmentPath = @"D:\Test";
            email.Attachments = "ticket.txt;to-do actions.txt";

            ETReceive email1 = new ETReceive();
            email1.Body = "Test";
            email1.To = "sachin.babladi@continuum.net";
            email1.From = "sachin.babladi@continuum.net";
            email1.Subject = "Test";
            email1.AttachmentPath = @"D:\tTest";
            email1.Attachments = "ticket.txt;to-do actions.txt";

            ETReceive email2 = new ETReceive();
            email2.Body = "Test";
            email2.To = "sachin.babladi@continuum.nett";
            email2.From = "sachin.babladi@continuum.nett";
            email2.Subject = "Test";
            email2.AttachmentPath = @"D:\Test";
            email2.Attachments = "ticket.txt;to-do actions.txt";

            SmtpEmailSender mailSender = new SmtpEmailSender(new List<ETReceive>() { email,email1,email2 }, entity);
            mailSender.SendEmail(isHtmlFormat: true);
            mailSender.SendMailDetails.ForEach(m => MessageBox.Show(m.outStatus.ToString()));
            mailSender.SendMailDetails.ForEach(m => MessageBox.Show(m.outErrDesc));*/
            //string cmd = @"set R_Script=""D:\R\R-3.2.2\bin\RScript.exe""";
            //Execute(cmd);

            //string cmd = @"""D:\R\R-3.2.2\bin\RScript.exe"" C:\Users\sachin.babladi\Desktop\Test.R 2010-01-28 example 100 > exmpl.batch 2>&1";

            //string cmd1 = @"C:\Users\sachin.babladi\Desktop\exmpl.bat";
            //string cmd = @"cd C:\Users\sachin.babladi\Desktop";
            // Execute(cmd1);



            //Execute(cmd);

            //Process.Start(@"C:\Users\sachin.babladi\Desktop\exmpl.bat", "example");

            //string rhome = System.Environment.GetEnvironmentVariable("R_HOME");
            //if (string.IsNullOrEmpty(rhome))
            //    rhome = @"D:\R\R-3.2.2";

            //System.Environment.SetEnvironmentVariable("R_HOME", rhome);
            //System.Environment.SetEnvironmentVariable("PATH", System.Environment.GetEnvironmentVariable("PATH") + ";" + rhome + @"bin\x64");

            //REngine.SetEnvironmentVariables();

            // REngine.(@"C:Program FilesRR-2.15.0bini386");


            REngine engine = REngine.GetInstance();
            engine.Initialize();
            //engine.Evaluate(@".libPaths(""D:\R\R-3.2.2\library""");

            //var vector = engine.CreateComplex(new System.Numerics.Complex(2.2, 2.2));
            //Stream s = new FileStream(@".source(""D:/RStudio/TestPackage/R/hello.R"")", FileMode.Open);

            // Stream s = new FileStream(@"D:/RStudio/TestPackage/R/hello.R", FileMode.Open);
            //engine.Evaluate(s);
            engine.Evaluate("library(TestPackage)");

            string testString = "";
            //"we have made every major financial goal on a month-to-month and quarter-by-quarter basis,";
            // string param = "TestPackage::hello(\"" + testString + "\")";
            //var se = engine.Evaluate(param);


            //var cmplxVector = engine.CreateComplexVector(3);

            //foreach (var i in list)
            //{
            //    MessageBox.Show(i.AsVector()[0].ToString());
            //}
            testString = System.IO.File.ReadAllText(@"C:\Users\sachin.babladi\Desktop\test.txt");


            string testDate = "2010-01-28";

            string number = "100";

            engine.Evaluate("library(EmailToTicket)");
            //for (int i = 0; i < 100; i++)
            //{
            //string param1 = "MyTestPackage::hello(\"" + testDate + "\",\"" + testString + "\",\"" + number + "\"" + ")";\
            string param1 = "Email2TicketPriority(\"" + testString + "\"" + ")";
            var se1 = engine.Evaluate(param1);
            var list = se1.AsList();
            foreach (var a in list)
            {
                MessageBox.Show(a.AsVector()[0].ToString());
                //foreach (var item in a.AsVector())
                //    System.IO.File.WriteAllText(@"D:\Test\Files\test" + i + ".txt", item.ToString() + "\n");
            }
            //}

            MessageBox.Show("Done");
            //se = engine.Evaluate(@"source(""D:/RStudio/TestPackage/R/hello.R"")");
        }

        private static void Execute(string cmd)
        {
            int ExitCode;
            ProcessStartInfo ProcessInfo;
            Process Process;

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/c " + cmd);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = false;

            Process = Process.Start(ProcessInfo);
            Process.WaitForExit();

            ExitCode = Process.ExitCode;
            Process.Close();

            MessageBox.Show("ExitCode: " + ExitCode.ToString(), "ExecuteCommand");

        }
    }
}

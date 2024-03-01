using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostToUrl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private string FormatResponse(dynamic http)
        {
            StringBuilder response = new StringBuilder();
            response.AppendLine($"HTTP {http.Status} {http.StatusText}");
            foreach (var header in http.GetAllResponseHeaders().Trim().Split('\n'))
            {
                response.AppendLine(header.Trim());
            }
            response.AppendLine();
            response.AppendLine(http.ResponseText);
            return response.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dynamic http = Activator.CreateInstance(Type.GetTypeFromProgID("WinHttp.WinHttpRequest.5.1"));

                string url = textBoxUrl.Text;
                string jsonData = "{}";

                http.Open("POST", url, false);

                http.SetRequestHeader("Content-Type", "application/json");
                http.Send(jsonData);

                http.WaitForResponse();

                string responseText = FormatResponse(http);
                Log(responseText);

            }
            catch (Exception ex)
            {
                Log($"Exc {ex.Message}");
            }
        }

        private void Log(string v)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new Action<string>(Log), new object[] { v });
            }
            else
            {
                textBox1.AppendText(v + Environment.NewLine);
            }
        }
    }
}

using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace Arduino
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SQLitePCL.Batteries_V2.Init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ArduinoIHM());
        }
    }
}

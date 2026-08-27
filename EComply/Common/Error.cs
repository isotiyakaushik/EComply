using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComply.Common
{
    internal class Error
    {
        public static void HandleShow(Exception ex)
        {
            if (ex == null) return;

            // File/Serilog/NLog માં log કરો
            File.AppendAllText("error.log",
                $"{DateTime.Now}: {ex}\n{ex.StackTrace}\n\n");

            MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void HandleHide(Exception ex)
        {
            // File/Serilog/NLog માં log કરો
            File.AppendAllText("error.log",
                $"{DateTime.Now}: {ex}\n\n");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// Add a reference to System.Management.
using System.Management;

using howto_listview_extensions;

namespace howto_show_computer_memory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Prepare the ListView and display values.
        private void Form1_Load(object sender, EventArgs e)
        {
            // Make the columns.
            lvwInfo.View = View.Details;
            lvwInfo.SetColumnHeaders(new object[]
                {
                    "Property", HorizontalAlignment.Left,
                    "Value", HorizontalAlignment.Right
                });

            // Add the values.
            ManagementObjectSearcher os_searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject mobj in os_searcher.Get())
            {
                GetInfo(mobj, "FreePhysicalMemory");
                GetInfo(mobj, "FreeSpaceInPagingFiles");
                GetInfo(mobj, "FreeVirtualMemory");
                GetInfo(mobj, "SizeStoredInPagingFiles");
             //   GetInfo(mobj, "TotalSwapSpaceSize");
                GetInfo(mobj, "TotalVirtualMemorySize");
                GetInfo(mobj, "TotalVisibleMemorySize");
            }

            // Size the columns.
            lvwInfo.SizeColumnsToFitDataAndHeaders();
        }

        // Add information about the property to the ListView.
        private void GetInfo(ManagementObject mobj, string property_name)
        {
            object property_obj = mobj[property_name];
            if (property_obj == null)
            {
                lvwInfo.AddRow(property_name, "???");
            }
            else
            {
                ulong property_value = (ulong)property_obj * 1024;
                lvwInfo.AddRow(property_name, property_value.ToFileSizeApi());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GC.Collect();
            Form1_Load(sender, e);
        }
    }
}

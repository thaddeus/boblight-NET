using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace boblight_net
{
    public partial class boblightGUI : Form
    {
        private boblightClient client;
        public boblightGUI()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (client == null || client.isConnected() == false)
            {
                client = new boblightClient(textIP.Text, Int32.Parse(textPort.Text), Int32.Parse(textPriority.Text));
                if (client.isConnected())
                {
                    panelProperties.Enabled = true;
                    listLights.Enabled = true;
                    lblStatus.Text = "Connected";
                    buttonConnect.Text = "Disconnect";
                    foreach (light light in client.getLights())
                    {
                        listLights.Items.Add(light.getName());
                    }
                }
            }
            else
            {
                client.disconnect();
                listLights.Items.Clear();
                panelProperties.Enabled = false;
                listLights.Enabled = false;
                lblStatus.Text = "Disconnected";
                buttonConnect.Text = "Connect";
            }
        }

        private light[] getSelectedLights()
        {
            light[] lights = new light[listLights.SelectedIndices.Count];
            int i = 0;
            foreach (light light in client.getLights())
            {
                if(listLights.SelectedItems.Contains(light.getName()))
                {
                    lights[i] = light;
                    i++;
                }
            }
            return lights;
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            client.setColor(getSelectedLights(), textHex.Text, 1.0F);
            client.syncLights();
        }

        private void buttonSpeed_Click(object sender, EventArgs e)
        {
            client.setSpeed(getSelectedLights(), float.Parse(textSpeed.Text));
            client.syncLights();
        }

        private void buttonUse_Click(object sender, EventArgs e)
        {
            client.setUse(getSelectedLights(), checkUse.Checked);
            client.syncLights();
        }
    }
}

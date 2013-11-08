using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Xml;
using System.IO;

namespace boblight_net
{
    public partial class boblightGUI : Form
    {
        private boblightClient client;
        public boblightGUI()
        {
            InitializeComponent();
            //If the configuration file doesn't exist, start it.
            if (!File.Exists("bobClient.xml"))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter writer = XmlWriter.Create("bobClient.xml", settings);
                writer.WriteStartDocument();
                writer.WriteComment("Last used boblightConfiguration");
                writer.WriteStartElement("boblightClient");
                writer.WriteStartElement("LastServer");
                writer.WriteAttributeString("IP", "127.0.0.1");
                writer.WriteAttributeString("Port", "19333");
                writer.WriteAttributeString("Priority", "128");
                writer.WriteEndElement();
                writer.WriteStartElement("Servers");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            XmlReader reader = XmlReader.Create("bobClient.xml");

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "LastServer")
                {
                    textIP.Text = reader.GetAttribute("IP");
                    textPort.Text = reader.GetAttribute("Port");
                    textPriority.Text = reader.GetAttribute("Priority");
                }
            }

            reader.Close();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (client == null || client.isConnected() == false)
            {
                client = new boblightClient(textIP.Text, Int32.Parse(textPort.Text), Int32.Parse(textPriority.Text));
                if (client.isConnected())
                {
                    panelProperties.Enabled = true;
                    tabsControl.Enabled = true;
                    buttonMisc.Enabled = true;
                    textIP.Enabled = false;
                    textPort.Enabled = false;
                    textPriority.Enabled = false;
                    lblStatus.Text = "Connected";
                    buttonConnect.Text = "Disconnect";

                    //Process the configuration file
                    XmlDocument doc = new XmlDocument();
                    doc.Load("bobClient.xml");

                    //Get to the beginning of things
                    XmlNode root = doc.DocumentElement;
                    //Update last server info
                    XmlNode lastServ = root.SelectSingleNode("LastServer");
                    lastServ.Attributes["IP"].Value = textIP.Text;
                    lastServ.Attributes["Port"].Value = textPort.Text;
                    lastServ.Attributes["Priority"].Value = textPriority.Text;
                    //Look for a server node that matches our IP and Port (assuming the client isn't really going to be ported)
                    root = root.SelectSingleNode("Servers");
                    XmlNode myServ = root.SelectSingleNode("Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']");
                    if (myServ != null)
                    {
                        //Config exists, check it
                        XmlNode myLights = myServ.SelectSingleNode("Lights");
                        bool flagMatch = true;
                        foreach (XmlNode xLight in myLights.ChildNodes)
                        {
                            bool flagFound = false;
                            foreach (light light in client.getLights())
                            {
                                if (xLight.Attributes["Name"].Value == light.getName())
                                    flagFound = true;
                            }
                            if (!flagFound)
                            {
                                flagMatch = false;
                                break;
                            }
                        }
                        //Config didn't match, start from scratch
                        if (!flagMatch)
                        {
                            myLights.RemoveAll();
                            foreach (light light in client.getLights())
                            {
                                XmlElement newLight = doc.CreateElement("Light");
                                newLight.SetAttribute("Name", light.getName());
                                newLight.SetAttribute("Color", "000000");
                                newLight.SetAttribute("Speed", "100.0");
                                newLight.SetAttribute("Use", "true");
                                newLight.SetAttribute("Interpolate", "false");
                                myLights.AppendChild(newLight);
                            }
                        }
                    }
                    else
                    {
                        //Config doesnt exist, create it
                        root = doc.DocumentElement.SelectSingleNode("Servers");
                        XmlElement newServer = doc.CreateElement("Server");
                        newServer.SetAttribute("IP", textIP.Text);
                        newServer.SetAttribute("Port", textPort.Text);
                        XmlElement newLights = doc.CreateElement("Lights");
                        foreach (light light in client.getLights())
                        {
                            XmlElement newLight = doc.CreateElement("Light");
                            newLight.SetAttribute("Name", light.getName());
                            newLight.SetAttribute("Color", "000000");
                            newLight.SetAttribute("Speed", "100.0");
                            newLight.SetAttribute("Use", "true");
                            newLight.SetAttribute("Interpolate", "false");
                            newLights.AppendChild(newLight);
                        }
                        newServer.AppendChild(newLights);
                        root.AppendChild(newServer);
                    }
                    
                    
                    foreach (light light in client.getLights())
                    {
                        listLights.Items.Add(light.getName());
                        XmlNode lightNode = root.SelectSingleNode("Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']/Lights/Light[@Name='" + light.getName() + "']");
                        client.setUse(light, bool.Parse(lightNode.Attributes["Use"].Value));
                        client.setInterpolation(light, bool.Parse(lightNode.Attributes["Interpolate"].Value));
                        client.setSpeed(light, float.Parse(lightNode.Attributes["Speed"].Value));
                        client.setColor(light, lightNode.Attributes["Color"].Value);
                    }
                    client.syncLights();

                    //Save configuration file
                    doc.Save("bobClient.xml");
                }
            }
            else
            {
                client.disconnect();
                listLights.Items.Clear();
                panelProperties.Enabled = false;
                tabsControl.Enabled = false;
                buttonMisc.Enabled = false;
                textIP.Enabled = true;
                textPort.Enabled = true;
                textPriority.Enabled = true;
                lblStatus.Text = "Disconnected";
                buttonConnect.Text = "Connect";
            }
        }

        private light[] getSelectedLights()
        {
            if (tabsControl.SelectedIndex == tabLights.TabIndex)
            {
                light[] lights = new light[listLights.SelectedIndices.Count];
                int i = 0;
                foreach (light light in client.getLights())
                {
                    if (listLights.SelectedItems.Contains(light.getName()))
                    {
                        lights[i] = light;
                        i++;
                    }
                }
                return lights;
            }
            else
            {
                ICollection<light> lightsCollection = new List<light>();
                foreach (lightGroup lGroup in listGroups.SelectedItems)
                {
                    foreach (light light in lGroup.lights)
                    {
                        lightsCollection.Add(light);
                    }
                }
                return lightsCollection.ToArray();
            }
            
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            client.setColor(getSelectedLights(), textHex.Text, 1.0F);
            client.syncLights();

            //Update configuration
            XmlDocument doc = new XmlDocument();
            doc.Load("bobClient.xml");

            //Get to the beginning of things
            XmlNode root = doc.DocumentElement;
            root = root.SelectSingleNode("Servers");

            foreach (light light in getSelectedLights())
            {
                string selectString = "Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']/Lights/Light[@Name='" + light.getName() + "']";
                XmlNode lightNode = root.SelectSingleNode(selectString);
                lightNode.Attributes["Color"].Value = textHex.Text;
            }

            doc.Save("bobClient.xml");

        }

        private void buttonSpeed_Click(object sender, EventArgs e)
        {
            client.setSpeed(getSelectedLights(), float.Parse(textSpeed.Text));
            client.syncLights();

            //Update configuration
            XmlDocument doc = new XmlDocument();
            doc.Load("bobClient.xml");

            //Get to the beginning of things
            XmlNode root = doc.DocumentElement;
            root = root.SelectSingleNode("Servers");

            foreach (light light in getSelectedLights())
            {
                string selectString = "Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']/Lights/Light[@Name='" + light.getName() + "']";
                XmlNode lightNode = root.SelectSingleNode(selectString);
                lightNode.Attributes["Speed"].Value = textSpeed.Text;
            }

            doc.Save("bobClient.xml");
        }

        private void buttonUse_Click(object sender, EventArgs e)
        {
            client.setUse(getSelectedLights(), checkUse.Checked);
            client.syncLights();

            //Update configuration
            XmlDocument doc = new XmlDocument();
            doc.Load("bobClient.xml");

            //Get to the beginning of things
            XmlNode root = doc.DocumentElement;
            root = root.SelectSingleNode("Servers");

            foreach (light light in getSelectedLights())
            {
                string selectString = "Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']/Lights/Light[@Name='" + light.getName() + "']";
                XmlNode lightNode = root.SelectSingleNode(selectString);
                lightNode.Attributes["Use"].Value = checkUse.Checked.ToString();
            }

            doc.Save("bobClient.xml");
        }

        private void tabsControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabsControl.SelectedIndex == tabLights.TabIndex)
            {
                buttonMisc.Text = "Group Lights";
            }
            else if (tabsControl.SelectedIndex == tabGroups.TabIndex)
            {
                buttonMisc.Text = "Delete Group";
            }
        }

        private void buttonMisc_Click(object sender, EventArgs e)
        {
            if (tabsControl.SelectedIndex == tabLights.TabIndex)
            {
                string groupName = Interaction.InputBox("Please enter a name for this light group:", "New Light Group");
                if (!String.IsNullOrWhiteSpace(groupName) && listLights.SelectedItems.Count != 0)
                {
                    listGroups.Items.Add(new lightGroup(getSelectedLights(), groupName));
                    tabsControl.SelectedIndex = tabGroups.TabIndex;
                }
            }
            else if(tabsControl.SelectedIndex == tabGroups.TabIndex)
            {
                while (listGroups.SelectedItems.Count != 0)
                {
                    listGroups.Items.Remove(listGroups.SelectedItems[0]);
                }
            }
        }
    }
}

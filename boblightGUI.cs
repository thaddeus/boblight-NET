﻿using System;
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
        private boblightClient sparkleClient;
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
                    panelIterate.Enabled = true;
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
                    XmlNode myServ = root.SelectSingleNode("Servers/Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']");
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
                        XmlElement newGroups = doc.CreateElement("LightGroups");
                        newServer.AppendChild(newGroups);
                        root.AppendChild(newServer);
                        
                    }
                    
                    
                    foreach (light light in client.getLights())
                    {
                        listLights.Items.Add(light.getName());
                        XmlNode lightNode = root.SelectSingleNode("Servers/Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']/Lights/Light[@Name='" + light.getName() + "']");
                        //client.setInterpolation(light, bool.Parse(lightNode.Attributes["Interpolate"].Value));
                        //client.setSpeed(light, float.Parse(lightNode.Attributes["Speed"].Value));
                        //client.setColor(light, lightNode.Attributes["Color"].Value);
                        //client.setUse(light, bool.Parse(lightNode.Attributes["Use"].Value));
                    }
                    client.syncLights();

                    foreach(XmlNode groupNode in doc.DocumentElement.SelectNodes("Servers/Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']/LightGroups/Group"))
                    {
                        ICollection<light> lightsCollection = new List<light>();
                        foreach (XmlNode lightNode in groupNode.ChildNodes)
                        {
                            foreach (light light in client.getLights())
                            {
                                if (light.getName() == lightNode.Attributes["Name"].Value)
                                    lightsCollection.Add(light);
                            }
                        }
                        listGroups.Items.Add(new lightGroup(lightsCollection.ToArray(), groupNode.Attributes["Name"].Value));
                    }

                    //Save configuration file
                    doc.Save("bobClient.xml");
                }
            }
            else
            {
                client.disconnect();
                listLights.Items.Clear();
                listGroups.Items.Clear();
                panelProperties.Enabled = false;
                panelIterate.Enabled = false;
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
            client.setColor(getSelectedLights(), textHex.Text);
            client.syncLights();

            //Update configuration
            XmlDocument doc = new XmlDocument();
            doc.Load("bobClient.xml");

            //Get to the beginning of things
            XmlNode root = doc.DocumentElement;

            foreach (light light in getSelectedLights())
            {
                XmlNode lightNode = root.SelectSingleNode("Servers/Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']/Lights/Light[@Name='" + light.getName() + "']");
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

            foreach (light light in getSelectedLights())
            {
                XmlNode lightNode = root.SelectSingleNode("Servers/Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']/Lights/Light[@Name='" + light.getName() + "']");
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

            foreach (light light in getSelectedLights())
            {
                XmlNode lightNode = root.SelectSingleNode("Servers/Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']/Lights/Light[@Name='" + light.getName() + "']");
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

                    //Add group to config file
                    //Update configuration
                    XmlDocument doc = new XmlDocument();
                    doc.Load("bobClient.xml");

                    //Get to the beginning of things
                    XmlNode root = doc.DocumentElement;
                    root = root.SelectSingleNode("Servers/Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']/LightGroups");

                    XmlElement newGroup = doc.CreateElement("Group");
                    newGroup.SetAttribute("Name", groupName);

                    foreach (light light in getSelectedLights())
                    {
                        XmlElement newGroupLight = doc.CreateElement("Light");
                        newGroupLight.SetAttribute("Name", light.getName());
                        newGroup.AppendChild(newGroupLight);
                    }

                    root.AppendChild(newGroup);

                    doc.Save("bobClient.xml");

                    tabsControl.SelectedIndex = tabGroups.TabIndex;
                }
            }
            else if(tabsControl.SelectedIndex == tabGroups.TabIndex)
            {
                //Add group to config file
                //Update configuration
                XmlDocument doc = new XmlDocument();
                doc.Load("bobClient.xml");

                foreach(lightGroup lGroup in listGroups.SelectedItems)
                {
                    XmlNode root = doc.DocumentElement;
                    root = root.SelectSingleNode("Servers/Server[@IP='" + textIP.Text + "' and @Port='" + textPort.Text + "']/LightGroups/Group[@Name='" + lGroup.name + "']");
                    root.ParentNode.RemoveChild(root);
                }

                while (listGroups.SelectedItems.Count != 0)
                {
                    listGroups.Items.Remove(listGroups.SelectedItems[0]);
                }

                doc.Save("bobClient.xml");
            }
        }

        private void btnIterate_Click(object sender, EventArgs e)
        {
            iterateLightString();
        }

        private void iterateLightString(bool reverse = false)
        {
            light[] allLights = getSelectedLights();
            int lightCount = allLights.Length;
            if(reverse)
            {
                Stack<light> lightStack = new Stack<light>();
                foreach(light light in allLights)
                {
                    lightStack.Push(light);
                }
                allLights = lightStack.Cast<light>().ToArray();
            }
            //In order to avoid double setting the first color, we separately set the first light selected
            client.setColor(allLights[0], listIterate.Items[0].ToString(), 1.0F);

            int lastLight = 0;

            //Loop through all the iterative colors within the user's list
            for (int i = 1; i < listIterate.Items.Count; i++)
            {
                int numLights = (int)Math.Ceiling((float)(((lightCount - 1) / (listIterate.Items.Count - 1)) * i) - (lastLight));
                light[] lightString = new light[numLights];
                int tempIndex = 0;
                for (int j = lastLight + 1; j < numLights + lastLight + 1; j++)
                {
                    lightString[tempIndex] = allLights[j];
                    tempIndex++;
                }
                iterateLightColors(lightString, listIterate.Items[i - 1].ToString(), listIterate.Items[i].ToString());
                lastLight = lastLight + numLights;
            }

            //Make sure that the boblightd properly syncs all the updated we just sent it.
            client.syncLights();
        }

        private void iterateLightColors(light[] lights, string fromColor, string toColor)
        {
            //Initialize working colors
            int fromColorRed = Int32.Parse(fromColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int fromColorGreen = Int32.Parse(fromColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int fromColorBlue = Int32.Parse(fromColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            int toColorRed = Int32.Parse(toColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int toColorGreen = Int32.Parse(toColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int toColorBlue = Int32.Parse(toColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            float diffRed = toColorRed - fromColorRed;
            float diffGreen = toColorGreen - fromColorGreen;
            float diffBlue = toColorBlue - fromColorBlue;
            float tempRed, tempGreen, tempBlue;

            //Initialize looping variables
            int curIndex = 1;
            int totalSize = lights.Length;

            foreach (light curLight in lights)
            {
                tempRed = fromColorRed + ((diffRed / totalSize) * curIndex);
                tempGreen = fromColorGreen + ((diffGreen / totalSize) * curIndex);
                tempBlue = fromColorBlue + ((diffBlue / totalSize) * curIndex);
                curIndex++;
                client.setColor(curLight, (int)Math.Round(tempRed), (int)Math.Round(tempGreen), (int)Math.Round(tempBlue));
            }
        }

        private void btnAddItColor_Click(object sender, EventArgs e)
        {
            listIterate.Items.Add(txtIterateFrom.Text);
        }

        private void btnRemoveItColor_Click(object sender, EventArgs e)
        {
            while (listIterate.SelectedItems.Count != 0)
            {
                listIterate.Items.Remove(listIterate.SelectedItems[0]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iterateLightString(true);
        }

        private void txtIterateFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                listIterate.Items.Add(txtIterateFrom.Text);
                txtIterateFrom.SelectAll();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (sparkleTimer.Enabled == false)
            {
                sparkleClient = new boblightClient(textIP.Text, Int32.Parse(textPort.Text), Int32.Parse(textPriority.Text) - 1);
                if (sparkleClient.isConnected())
                {
                    foreach (light light in sparkleClient.getLights())
                    {
                        sparkleClient.setSpeed(light, float.Parse(txtSparkleSpeed.Text));
                        sparkleClient.setColor(light, txtSparkleColor.Text);
                        sparkleClient.setUse(light, false);
                    }
                    sparkleTimer.Interval = Int32.Parse(txtSparkleCycle.Text);
                    sparkleTimer.Start();
                }
            }
            else
            {
                sparkleTimer.Enabled = false;
                sparkleClient.disconnect();
            }
        }

        private void sparkleTimer_Tick(object sender, EventArgs e)
        {
            //Is this still even worth doing?
            if (sparkleClient.isConnected())
            {
                light[] sparkleLights = sparkleClient.getLights();
                //Unsparkle any previously sparkled lights
                foreach (light light in sparkleLights)
                {
                    if (light.use())
                        sparkleClient.setUse(light, false);
                }

                if(Int32.Parse(txtSparkleCount.Text) > 0)
                {
                    for(int sparkleNum = 1; sparkleNum <= Int32.Parse(txtSparkleCount.Text); sparkleNum++)
                    {
                        Random randomSparkle = new Random();
                        int sparkleIndex = randomSparkle.Next(0, sparkleLights.Length - 1);
                        for(int radius = 0; radius <= Int32.Parse(txtSparkleRadius.Text); radius++)
                        {
                            if(sparkleIndex + radius < sparkleLights.Length)
                                sparkleClient.setUse(sparkleLights[sparkleIndex + radius], true);
                            if(sparkleIndex - radius >= 0)
                                sparkleClient.setUse(sparkleLights[sparkleIndex - radius], true);
                        }
                    }
                }
                sparkleClient.syncLights();
            }
            else
            {
                sparkleTimer.Enabled = false;
            }
        }

        private void barLumosity_ValueChanged(object sender, EventArgs e)
        {
            client.luminosityModifier = barLumosity.Value / 10D;
            lblLuminosityValue.Text = client.luminosityModifier.ToString();
            foreach (light light in client.getLights())
            {
                client.setLight(light, light.red, light.green, light.blue);
            }
        }

        private void barSaturation_ValueChanged(object sender, EventArgs e)
        {
            client.saturationModifier = barSaturation.Value / 10D;
            lblSaturationValue.Text = client.saturationModifier.ToString();
            foreach (light light in client.getLights())
            {
                client.setLight(light, light.red, light.green, light.blue);
            }
        }

        private void barHue_ValueChanged(object sender, EventArgs e)
        {
            client.hueModifier = barHue.Value;
            lblHueValue.Text = client.hueModifier.ToString();
            foreach (light light in client.getLights())
            {
                client.setLight(light, light.red, light.green, light.blue);
            }
        }
    }
}

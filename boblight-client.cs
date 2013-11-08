using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace boblight_net
{
    public class light
    {
        private string name = null;
        private float hscan_min, hscan_max, vscan_min, vscan_max = 0.0F;
        private bool inUse = true;
        public light(string blname, float blvscan_min, float blvscan_max, float blhscan_min, float blhscan_max)
        {
            name = blname;
            hscan_min = blhscan_min;
            hscan_max = blhscan_max;
            vscan_min = blvscan_min;
            vscan_max = blvscan_max;
        }

        public string getName()
        {
            return name;
        }

        public float getHmax()
        {
            return hscan_max;
        }

        public float getHmin()
        {
            return hscan_min;
        }

        public float getVmax()
        {
            return vscan_max;
        }

        public float getVmin()
        {
            return vscan_min;
        }

        public bool use()
        {
            return inUse;
        }

        public void setUse(bool use)
        {
            inUse = use;
        }
    }

    public class lightGroup
    {
        public light[] lights { get; set; }
        public string name { get; set; }

        public lightGroup(light[] lights, string name)
        {
            this.lights = lights;
            this.name = name;
        }
    }

    public class boblightClient
    {
        private Socket lightSocket = null;
        private byte[] bytes = new byte[8192];
        private light[] lights = null;

        public boblightClient(string ip, int port, int priority = 128)
        {
            try
            {
                //Connect to boblightd
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP  socket.
                lightSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    lightSocket.Connect(remoteEP);
                    //Okay, we're now connected.

                    //Handshake time. Shake that hand.
                    sendData("hello\n");

                    if (receiveData() == "hello\n")
                    {

                        sendData("get version\n");
                        if (receiveData() == "version 5\n")
                        {
                            //Hooray, the server said hi back.
                            //Set the priority of the client, the lights won't work without it.
                            setPriority(priority);
                            //Oh, we might actually need to know something about the lights too.
                            retrieveLights();
                        }
                        else
                        {
                            throw new Exception("Handshake failed (wrong version)");
                        }
                    }
                    else
                    {
                        throw new Exception("Handshake failed (no hello)");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void setColor(light light, string color, float brightness = 1.0F)
        {
            if (light.use())
            {
                //set a light via hex color
                float red_f, green_f, blue_f = 0.0F;
                red_f = calculateBobColor(Int32.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), brightness);
                green_f = calculateBobColor(Int32.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), brightness);
                blue_f = calculateBobColor(Int32.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber), brightness);
                sendColor(light.getName(), red_f, green_f, blue_f);
            }
        }

        public void setColor(light[] lights, string color, float brightness = 1.0F)
        {
            //set array of lights via hex color
            float red_f, green_f, blue_f = 0.0F;
            red_f = calculateBobColor(Int32.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), brightness);
            green_f = calculateBobColor(Int32.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), brightness);
            blue_f = calculateBobColor(Int32.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber), brightness);
            foreach (light light in lights)
            {
                if (light.use())
                    sendColor(light.getName(), red_f, green_f, blue_f);
            }
        }

        public void setColor(light light, int red, int green, int blue, float brightness = 1.0F)
        {
            if (light.use())
            {
                //set light via r/g/b colors
                float red_f, green_f, blue_f = 0.0F;
                red_f = calculateBobColor(red, brightness);
                green_f = calculateBobColor(green, brightness);
                blue_f = calculateBobColor(blue, brightness);
                sendColor(light.getName(), red_f, green_f, blue_f);
            }
        }

        public void setColor(light[] lights, int red, int green, int blue, float brightness = 1.0F)
        {
            //set array of lights via r/g/b colors
            float red_f, green_f, blue_f = 0.0F;
            red_f = calculateBobColor(red, brightness);
            green_f = calculateBobColor(green, brightness);
            blue_f = calculateBobColor(blue, brightness);
            foreach (light light in lights)
            {
                if (light.use())
                    sendColor(light.getName(), red_f, green_f, blue_f);
            }
        }

        public float calculateBobColor(int color, float brightness)
        {
            //calculate the r/g/b color format for boblightd
            return (float)color / 255F * brightness;
        }

        private void sendColor(string light, float red, float green, float blue)
        {
            //boblightd version 5 set light
            sendData("set light " + light + " rgb " + red.ToString() + " " + green.ToString() + " " + blue.ToString() + "\n");
        }

        public void setSpeed(light light, float speed)
        {
            //set speed of light
            if (light.use())
                sendSpeed(light.getName(), speed);
        }

        public void setSpeed(light[] lights, float speed)
        {
            //set speed of array of lights
            foreach (light light in lights)
            {
                if(light.use())
                    sendSpeed(light.getName(), speed);
            }
        }

        private void sendSpeed(string light, float speed)
        {
            //boblightd version 5 set light speed
            sendData("set light " + light + " speed " + speed.ToString() + "\n");
        }

        public void setUse(light light, bool use)
        {
            //set use of light
            light.setUse(use);
            sendUse(light.getName(), use);
        }

        public void setUse(light[] lights, bool use)
        {
            //set use of array of lights
            foreach (light light in lights)
            {
                light.setUse(use);
                sendUse(light.getName(), use);
            }
        }

        private void sendUse(string light, bool use)
        {
            //boblightd version 5 set light use
            sendData("set light " + light + " use " + use.ToString().ToLower() + "\n");
        }

        public void setInterpolation(light light, bool interpolate)
        {
            //set interpolation of light
            sendInterpolation(light.getName(), interpolate);
        }

        public void setInterpolation(light[] lights, bool interpolate)
        {
            //set interpolation of array of lights
            foreach (light light in lights)
            {
                sendInterpolation(light.getName(), interpolate);
            }
        }

        private void sendInterpolation(string light, bool interpolate)
        {
            //boblightd version 5 set light interpolation
            sendData("set light " + light + " interpolation " + interpolate.ToString().ToLower() + "\n");
        }

        public void syncLights()
        {
            //boblightd version 5 sync lights
            sendData("sync\n");
        }

        public void setPriority(int priority)
        {
            //boblightd version 5 set client priority
            sendData("set priority " + priority.ToString() + "\n");
        }

        public bool pingLights()
        {
            //boblightd version 5 change accepted verification
            sendData("ping\n");
            return (receiveData() == "1\n");
        }

        private void retrieveLights()
        {
            //Communicate with boblight server and get back the lights
            sendData("get lights\n");
            string lightsData = receiveData();

            //Remove the last new line, then split everything into something useful
            string[] lightArray = lightsData.Remove(lightsData.Length - 1, 1).Split(UnicodeEncoding.Unicode.GetChars(UnicodeEncoding.Unicode.GetBytes("\n")));
            lights = new light[Convert.ToInt32(lightArray[0].Substring(7))];
            for (int i = 1; i < lightArray.Length; i++)
            {
                string[] lightData = lightArray[i].Split(UnicodeEncoding.Unicode.GetChars(UnicodeEncoding.Unicode.GetBytes(" ")));
                lights[i - 1] = new light(lightData[1], float.Parse(lightData[3]) / 100, float.Parse(lightData[4]) / 100, float.Parse(lightData[5]) / 100, float.Parse(lightData[6]) / 100);
            }
        }

        public light[] getLights()
        {
            return lights;
        }

        public bool isConnected()
        {
            if (lightSocket != null)
                return lightSocket.Connected;
            else
                return false;
        }

        public void disconnect()
        {
            lightSocket.Disconnect(true);
            lights = null;
        }

        private void sendData(string data)
        {
            lightSocket.Send(Encoding.UTF8.GetBytes(data));
        }

        private string receiveData()
        {
            int bytesRec = lightSocket.Receive(bytes);
            string data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            return data;
        }
    }
}

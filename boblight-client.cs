using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Drawing;

namespace boblight_net
{
    public class light
    {
        private string name = null;
        private float hscan_min, hscan_max, vscan_min, vscan_max = 0.0F;
        private bool inUse = true;
        public float red, green, blue;
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

        //Global Color Modifiers
        public double luminosityModifier = 1D, saturationModifier = 1D;
        public double hueModifier = 0D;

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

        //Set color Hex version
        public void setColor(light light, string color, float brightness = 1.0F)
        {
            if (light.use())
            {
                //set a light via hex color
                int red, green, blue;
                red = Int32.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                green = Int32.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                blue = Int32.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                setLight(light, calculateBobColor(red), calculateBobColor(green), calculateBobColor(blue));
            }
        }

        //Set color Hex version
        //Array override
        public void setColor(light[] lights, string color)
        {
            //set array of lights via hex color
            int red, green, blue;
            red = Int32.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            green = Int32.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            blue = Int32.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            foreach (light light in lights)
            {
                if (light.use())
                    setLight(light, calculateBobColor(red), calculateBobColor(green), calculateBobColor(blue));
            }
        }

        //Set color RGB integer version
        public void setColor(light light, int red, int green, int blue)
        {
            if (light.use())
                setLight(light, calculateBobColor(red), calculateBobColor(green), calculateBobColor(blue));
        }

        //Set color RGB integer version
        //Array override
        public void setColor(light[] lights, int red, int green, int blue)
        {
            foreach (light light in lights)
            {
                if (light.use())
                    setLight(light, calculateBobColor(red), calculateBobColor(green), calculateBobColor(blue));
            }
        }

        public void setLight(light light, float red, float green, float blue)
        {
            //Store these colors for future alterations
            light.red = red;
            light.green = green;
            light.blue = blue;

            //Do we have any color alterations to perform?
            if (luminosityModifier != 1.0D || saturationModifier != 1.0D || hueModifier != 0)
            {
                HSLColor color = new HSLColor((int)(red * 255), (int)(green * 255), (int)(blue * 255));
                color.Luminosity = color.Luminosity * luminosityModifier;
                color.Saturation = color.Saturation * saturationModifier;
                color.Hue = (color.Hue + hueModifier) % 240;
                int[] rgbColor = color.getRGB();
                red = calculateBobColor(rgbColor[0]);
                green = calculateBobColor(rgbColor[1]);
                blue = calculateBobColor(rgbColor[2]);
            }

            //Send the colors to the boblight server
            sendColor(light.getName(), red, green, blue);
        }

        public float calculateBobColor(int color, float brightness)
        {
            //calculate the r/g/b color format for boblightd
            return (float)color / 255F * brightness;
        }

        public float calculateBobColor(int color)
        {
            return (float)color / 255F;
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

    public class HSLColor
    {
        // Private data members below are on scale 0-1
        // They are scaled for use externally based on scale
        private double hue = 1.0;
        private double saturation = 1.0;
        private double luminosity = 1.0;

        private const double scale = 240.0;

        public double Hue
        {
            get { return hue * scale; }
            set { hue = CheckRange(value / scale); }
        }
        public double Saturation
        {
            get { return saturation * scale; }
            set { saturation = CheckRange(value / scale); }
        }
        public double Luminosity
        {
            get { return luminosity * scale; }
            set { luminosity = CheckRange(value / scale); }
        }

        private double CheckRange(double value)
        {
            if (value < 0.0)
                value = 0.0;
            else if (value > 1.0)
                value = 1.0;
            return value;
        }

        public override string ToString()
        {
            return String.Format("H: {0:#0.##} S: {1:#0.##} L: {2:#0.##}", Hue, Saturation, Luminosity);
        }

        public string ToRGBString()
        {
            Color color = (Color)this;
            return String.Format("R: {0:#0.##} G: {1:#0.##} B: {2:#0.##}", color.R, color.G, color.B);
        }

        public int[] getRGB()
        {
            Color color = (Color)this;
            int[] colorArray = new int[3];
            colorArray[0] = color.R;
            colorArray[1] = color.G;
            colorArray[2] = color.B;
            return colorArray;
        }

        #region Casts to/from System.Drawing.Color
        public static implicit operator Color(HSLColor hslColor)
        {
            double r = 0, g = 0, b = 0;
            if (hslColor.luminosity != 0)
            {
                if (hslColor.saturation == 0)
                    r = g = b = hslColor.luminosity;
                else
                {
                    double temp2 = GetTemp2(hslColor);
                    double temp1 = 2.0 * hslColor.luminosity - temp2;

                    r = GetColorComponent(temp1, temp2, hslColor.hue + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, hslColor.hue);
                    b = GetColorComponent(temp1, temp2, hslColor.hue - 1.0 / 3.0);
                }
            }
            return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
        }



        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            temp3 = MoveIntoRange(temp3);
            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0 / 3.0)
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            else
                return temp1;
        }
        private static double MoveIntoRange(double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;
            return temp3;
        }
        private static double GetTemp2(HSLColor hslColor)
        {
            double temp2;
            if (hslColor.luminosity < 0.5)  //<=??
                temp2 = hslColor.luminosity * (1.0 + hslColor.saturation);
            else
                temp2 = hslColor.luminosity + hslColor.saturation - (hslColor.luminosity * hslColor.saturation);
            return temp2;
        }

        public static implicit operator HSLColor(Color color)
        {
            HSLColor hslColor = new HSLColor();
            hslColor.hue = color.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360 
            hslColor.luminosity = color.GetBrightness();
            hslColor.saturation = color.GetSaturation();
            return hslColor;
        }
        #endregion

        public void SetRGB(int red, int green, int blue)
        {
            HSLColor hslColor = (HSLColor)Color.FromArgb(red, green, blue);
            this.hue = hslColor.hue;
            this.saturation = hslColor.saturation;
            this.luminosity = hslColor.luminosity;
        }

        public HSLColor() { }
        public HSLColor(Color color)
        {
            SetRGB(color.R, color.G, color.B);
        }
        public HSLColor(int red, int green, int blue)
        {
            SetRGB(red, green, blue);
        }
        public HSLColor(double hue, double saturation, double luminosity)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Luminosity = luminosity;
        }

    }
}

using AdafruitMatrix;
using Glovebox.IO.Components;
using Glovebox.IoT;
using Glovebox.IoT.Base;
using System;
using System.Diagnostics;

namespace MakerDen
{
    public class IoTServices : ExplorerHat
    {
        object displayLock = new object();
        private IServiceManager sm;
        protected Adafruit8x8Matrix matrix = new Adafruit8x8Matrix("matrix");

        public IoTServices(string yourName = "", bool connected = true, bool hat = true) : base(hat)
        {
            Util.SetName(yourName);
            if (connected)
            {
                sm = Util.StartNetworkServices(connected);
            }

            // this is a hack workaround for system time sync issue on W10 for IoT Beta
            Util.GetTime();
        }

        protected void Welcome()
        {
            matrix.ScrollStringInFromRight("Hello " + ConfigurationManager.NetworkId + " I'm " + Util.GetHostName().ToUpper() + " at " + Util.GetIPAddress() + " ", 100);
        }

        protected void Display(char c)
        {
            matrix.DrawLetter(c); // Light
            matrix.FrameDraw();
        }

        protected void Display(Adafruit8x8Matrix.Symbols sym)
        {
            matrix.DrawSymbol(sym);
            matrix.FrameDraw();
        }

        protected void Display(string text)
        {
            matrix.FrameClear();
            matrix.ScrollStringInFromRight(text, 100);
        }

        public uint OnBeforeMeasure(object sender, EventArgs e)
        {
            uint id = ((SensorBase.SensorIdEventArgs)e).id;
            if (explorerLEDs == null) { return 0; }
            explorerLEDs[id % 2].On();
            Util.Delay(5);
            explorerLEDs[id % 2].Off();
            return 0;
        }

        public uint SetLEDMatrixBrightness(object sender, EventArgs e)
        {
            if (matrix == null) { return 0; }
            var data = ((SensorBase.SensorItemEventArgs)e).data;
            matrix.SetBrightness((byte)((data.Values()[0] % 100) / 35));
            return 0;
        }

        protected uint OnAfterMeasurement(object sender, EventArgs e)
        {
            uint result;
            var data = ((SensorBase.SensorItemEventArgs)e).data;

            if (sm == null)
            {
                Debug.WriteLine(data.ToString());
                return 0;
            }

            var json = data.ToJson();
            var topic = data.Topic;

            result = sm.Publish(topic, json);

            Util.Delay(100);

            return result;
        }
    }
}

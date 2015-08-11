//file name: Experiments.cs

using Glovebox.IO.Components.Sensors;
using Glovebox.IoT;
using System;
using System.Threading;

namespace MakerDen
{
    public class Experiments : IoTServices
    {
        const double LIGHT_THRESHOLD = 60d;

        public Experiments() :
            base("Replace with Your Name")
        { }

        public void Main()
        {
            #region paste the code snippet in between the #region and #endregion tags

            while (true)
            {
                Welcome();
            }

            #endregion
        }
    }
}

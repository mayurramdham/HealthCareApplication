using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Apps.Helper
{
    public class GenerateOtp
    {
        private Random random;

        public GenerateOtp()
        {
            random = new Random();
        }

        public int GenerateOtps()
        {
            return random.Next(100000, 1000000);
        }


    }
}

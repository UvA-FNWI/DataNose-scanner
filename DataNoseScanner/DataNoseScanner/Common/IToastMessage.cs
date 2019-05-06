using System;
using System.Collections.Generic;
using System.Text;

namespace DataNoseScanner.Common
{
    public interface IToastMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}

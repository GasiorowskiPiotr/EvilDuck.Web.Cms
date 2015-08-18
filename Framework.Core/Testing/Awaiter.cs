using System;
using System.Threading;

namespace EvilDuck.Framework.Core.Testing
{
    public class Awaiter
    {
        public static void Wait(int milis)
        {
            Wait(TimeSpan.FromMilliseconds(milis));
        }

        public static void Wait(TimeSpan timeSpan)
        {
            var start = DateTime.UtcNow;
            var now = DateTime.UtcNow;
            while (now - timeSpan < start)
            {
                Thread.SpinWait(1000);
                now = DateTime.UtcNow;
            }
        }
    }
}

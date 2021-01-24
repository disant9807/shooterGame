using System.Collections;
using System.Runtime.CompilerServices;

namespace Assets.Core.Common
{
    public static class GlobalIDHelper
    {
        private static int id = 0;

        public static int GetID()
        {
            id++;
            return id;
        }
    }
}

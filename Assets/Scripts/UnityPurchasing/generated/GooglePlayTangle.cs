// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("JqWrpJQmpa6mJqWlpBDpIcxxSr/4xPqCFCAaGer78qi9rNONt+dntDbYWQZi+SnIAG8936OUnJo27ST0eoevRc1NqgY1rOv7CMZ4aV+sRoqUJqWGlKmirY4i7CJTqaWlpaGkpzVdFLF3k719EP0nwNA1ru0+r3jnz3z2pOfsALyJsJQ/FQ3/yypFBuIDHbMjmtky6fppzfw+hoFGtqJzI/DECQGYgusGF9fSJKdng5/koJ33R9tNRwCl82lLeRakZOkSg0bchdbnjNmv9i0mdCtQW4ncUy3yYbHfqul5HlV6GQGMGz8pdvVsKf777bRdya5WTrp4P/O5ug6TegQYpNLfo3JuY2SxrVH/qTtsGTFZAfX3pSiYXKXg9VO29toZGaanpaSl");
        private static int[] order = new int[] { 1,9,11,8,9,11,9,8,12,9,12,13,12,13,14 };
        private static int key = 164;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("qUtGkJIBK/KnC7qU0qpWsnBjXejkjP7Yu+1wp8JYzzYRC2MZ5Hnlugqcpx54K3Ln+IJTuMRk9aIBJ5JExQhPMObVzKPFH9OpvMCaxV2To2aaOJshBzgxOxZMCYISRVZGql7BlZvx2X0yZHAD3WW10w9hI8NIUgprdXl8tbLkN+DJh4EMrrNreZHpvu9eHdc4DvfuIln2oUnA+q5W2sX0qeKZBrp/30mn1eIchorNTymtLPVYtwWGpbeKgY6tAc8BcIqGhoaCh4RspVpYasRgITTZyEffnsja1gspVwWGiIe3BYaNhQWGhocvrrIEVVQiGsjXiRretmLd+Lu/VuTLS14t2z2bEJBnkJ4dz6He3+lSoC1P3l00o5T03Ir2twyayIWEhoeG");
        private static int[] order = new int[] { 3,11,12,10,4,13,6,13,8,10,13,11,12,13,14 };
        private static int key = 135;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

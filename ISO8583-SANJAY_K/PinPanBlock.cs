using sanjay;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
namespace Iso_sanjay
{
    class PinPanBlock
    {
        private static string PanBlock(string track2Data)
        {
            string[] parts = track2Data.Split('=');
            string pannum = parts[0].Substring(1);
            pannum = pannum.Substring(0, pannum.Length - 1);
            int len = pannum.Length;
            if (len <= 12)
            {
                pannum = pannum.PadLeft(16, '0');
            }
            else if (len >= 13)
            {
                pannum = pannum.Substring(len - 12);
                StringBuilder sb = new StringBuilder(pannum);
                sb.Insert(0, "0000");
                pannum = sb.ToString();
            }
            return pannum;
        }
        private static string PinBlock(string pin)
        {
            int len = pin.Length; string pinblock = "";
            if (len >= 10)
            {
                string lenn = len.ToString("X");
                pinblock = "0" + lenn + pin;
                pinblock = pinblock.PadRight(16, 'F');
            }
            else
            {
                pinblock = "0" + len + pin;
                pinblock = pinblock.PadRight(16, 'F');

            }
            return pinblock;
        }
        private static string PinpanBlock(string hexString1, string hexString2)
        {
            if (hexString1.Length != hexString2.Length)
                throw new ArgumentException("Input strings must have the same length.");

            char[] result = new char[hexString1.Length];
            for (int i = 0; i < hexString1.Length; i++)
            {
                int value1 = Convert.ToInt32(hexString1[i].ToString(), 16);
                int value2 = Convert.ToInt32(hexString2[i].ToString(), 16);
                int xorResult = value1 ^ value2;
                result[i] = Convert.ToChar(xorResult.ToString("X"));
            }

            return new string(result);

        }
        private static byte[] HexStringToByteArray(string hex)
        {
            int byteCount = hex.Length / 2;
            byte[] bytes = new byte[byteCount];
            for (int i = 0; i < byteCount; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }
        private static string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:X2}", b);
            }
            return hex.ToString();
        }
        public static byte[] Encrypt3DES(byte[] data, byte[] key)
        {
            using (TripleDESCryptoServiceProvider desProvider = new TripleDESCryptoServiceProvider())
            {
                // Set the encryption mode and padding
                desProvider.Mode = CipherMode.ECB;
                desProvider.Padding = PaddingMode.None;

                // Set the provided key
                desProvider.Key = key;

                // Create the encryptor
                ICryptoTransform encryptor = desProvider.CreateEncryptor();

                // Encrypt the data
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }
        public static string Encryption(List<DataElement> requiredDataelements)
        {
            string pin = "1234";
            string pinblock = PinPanBlock.PinBlock(pin);
            string pan = "";
            foreach (DataElement dataElement in requiredDataelements)
            {
                if (dataElement.Name == "Track 2 Data")
                {
                    pan = dataElement.Value;
                }
            }
            string panblock = PanBlock(pan);
            string key = "ED2307743BAFC53FA0315C89116BCABF";
            string pinpanblock = PinpanBlock(pinblock, panblock);
            byte[] dataBytes = HexStringToByteArray(pinpanblock);
            byte[] keyBytes = HexStringToByteArray(key);
            byte[] encryptedData = Encrypt3DES(dataBytes, keyBytes);
            string encryptedHex = ByteArrayToHexString(encryptedData);
            return encryptedHex;
        }
    }
}

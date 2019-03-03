
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Task1
{
    /*
     * Implement encryption using the symmetric and the asymmetric cryptography. Conduct performance tests with checking different options (at least key lengths and block sizes) in the encryption
configuration and prepare a short report.
     */
    class Program
    {
        public static string GetTestString(int repetitions)
        {
            string original = "All work and no play makes Jack a dull boy!";
            
            var builder = new StringBuilder();
            for (var i = 0; i < repetitions; i++)
            {
                builder.Append(original);
            }
            return builder.ToString();
            
        }
        static void Main(string[] args)
        {
            var testRepetitions = 1;
            var testIterations = 10000.0;
            Console.WriteLine("START");
            Console.WriteLine($"Testing with testRepetitions = {testRepetitions} and testIterations = {testIterations}\n");
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            string toEncryptString = GetTestString(testRepetitions);
            byte[] toEncryptBytes = ByteConverter.GetBytes(toEncryptString);


            using (Rijndael myRijndael = Rijndael.Create())
            {
                myRijndael.BlockSize = 256;
                Console.WriteLine($"Rijndael keysize = {myRijndael.KeySize} blocksize = {myRijndael.BlockSize}");
                TestR(toEncryptString, testIterations, myRijndael);
            }
            Console.WriteLine();
            using (Rijndael myRijndael = Rijndael.Create())
            {
                myRijndael.BlockSize = 128;
                Console.WriteLine($"Rijndael keysize = {myRijndael.KeySize} blocksize = {myRijndael.BlockSize}");
                TestR(toEncryptString, testIterations, myRijndael);
            }


            //    using (Aes myAes = Aes.Create())
            //{
            //    myAes.KeySize = 128;
            //    Console.WriteLine($"Aes keysize = {myAes.KeySize}");
            //    TestAES(toEncryptString, testIterations, myAes);
            //}
            //Console.WriteLine();

            //using (Aes myAes = Aes.Create())
            //{
            //    myAes.KeySize = 192;
            //    Console.WriteLine($"Aes keysize = {myAes.KeySize}");
            //    TestAES(toEncryptString, testIterations, myAes);
            //}
            Console.WriteLine();
            using (Aes myAes = Aes.Create())
            {
                Console.WriteLine($"Aes keysize = {myAes.KeySize}");
                TestAES(toEncryptString, testIterations, myAes);
            }
            Console.WriteLine();

            //if (! (toEncryptBytes.Length > 117))
            //{
            //    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024))
            //    {
            //        Console.WriteLine("RSA keysize = 1024");
            //        TestRSA(ByteConverter, toEncryptString, toEncryptBytes, testIterations, RSA);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Cant encrypt using rsa1024");
            //}

            //Console.WriteLine();

            //if (!(toEncryptBytes.Length > 245))
            //{
            //    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048))
            //    {
            //        Console.WriteLine("RSA keysize = 2048");
            //        TestRSA(ByteConverter, toEncryptString, toEncryptBytes, testIterations, RSA);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Cant encrypt using rsa2048");
            //}

            Console.WriteLine("\nEND\n");

            Console.ReadKey();
        }

        private static void TestRSA(UnicodeEncoding ByteConverter, string toEncryptString, byte[] toEncryptBytes, double testIterations, RSACryptoServiceProvider RSA)
        {
            byte[] encryptedData = null;

            var sw = Stopwatch.StartNew();
            // Encrypt the string to an array of bytes.
            for (var ctn = 0; ctn < testIterations; ctn++)
            {
                encryptedData = RSAEncrypt(toEncryptBytes, false, RSA);
            }

            sw.Stop();
            Console.WriteLine($"RSA encryption: Elapsed={sw.Elapsed.TotalMilliseconds} per encrypt {sw.Elapsed.TotalMilliseconds / testIterations}");


            byte[] decryptedData = null;
            sw = Stopwatch.StartNew();
            for (var ctn = 0; ctn < testIterations; ctn++)
            {
                decryptedData = RSADecrypt(encryptedData, false, RSA);
            }
            sw.Stop();
            Console.WriteLine($"RSA decryption: Elapsed={sw.Elapsed.TotalMilliseconds} per decrypt {sw.Elapsed.TotalMilliseconds / testIterations}");

            Console.WriteLine($"RSA Is the text the same ? {toEncryptString == ByteConverter.GetString(decryptedData)}");
        }

        private static void TestAES(string toEncryptString, double testIterations, Aes myAes)
        {
            byte[] encrypted = null;
            var sw = Stopwatch.StartNew();

            // Encrypt the string to an array of bytes.
            for (var ctn = 0; ctn < testIterations; ctn++)
            {

                ICryptoTransform encryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);
                encrypted = EncryptStringToBytes_Aes(toEncryptString, encryptor);
            }

            sw.Stop();
            Console.WriteLine($"AES encryption: Elapsed={sw.Elapsed.TotalMilliseconds} per encrypt {sw.Elapsed.TotalMilliseconds / testIterations}");


            string roundtrip = null;
            sw = Stopwatch.StartNew();

            for (var ctn = 0; ctn < testIterations; ctn++)
            {

                ICryptoTransform decryptor = myAes.CreateDecryptor(myAes.Key, myAes.IV);
                roundtrip = DecryptStringFromBytes_Aes(encrypted, decryptor);
            }

            sw.Stop();
            Console.WriteLine($"AES decryption: Elapsed={sw.Elapsed.TotalMilliseconds} per decrypt {sw.Elapsed.TotalMilliseconds / testIterations}");
            //Display the original data and the decrypted data.
            Console.WriteLine($"AES Is the text the same ? {toEncryptString == roundtrip}");
        }

        private static void TestR(string toEncryptString, double testIterations, Rijndael myR)
        {
            byte[] encrypted = null;
            var sw = Stopwatch.StartNew();

            // Encrypt the string to an array of bytes.
            for (var ctn = 0; ctn < testIterations; ctn++)
            {

                ICryptoTransform encryptor = myR.CreateEncryptor(myR.Key, myR.IV);
                encrypted = EncryptStringToBytes_Aes(toEncryptString, encryptor);
            }

            sw.Stop();
            Console.WriteLine($"Rijndael encryption: Elapsed={sw.Elapsed.TotalMilliseconds} per encrypt {sw.Elapsed.TotalMilliseconds / testIterations}");


            string roundtrip = null;
            sw = Stopwatch.StartNew();

            for (var ctn = 0; ctn < testIterations; ctn++)
            {

                ICryptoTransform decryptor = myR.CreateDecryptor(myR.Key, myR.IV);
                roundtrip = DecryptStringFromBytes_Aes(encrypted, decryptor);
            }

            sw.Stop();
            Console.WriteLine($"Rijndael decryption: Elapsed={sw.Elapsed.TotalMilliseconds} per decrypt {sw.Elapsed.TotalMilliseconds / testIterations}");
            //Display the original data and the decrypted data.
            Console.WriteLine($"Rijndael Is the text the same ? {toEncryptString == roundtrip}");
        }

        static byte[] EncryptStringToBytes_Aes(string text, ICryptoTransform encryptor)
        {
            // Check arguments.
            if (text == null || text.Length <= 0)
                throw new ArgumentNullException("plainText");
            byte[] encrypted;

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(text);
                    }
                    encrypted = msEncrypt.ToArray();
                }

            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }
        public static byte[] RSAEncrypt(byte[] DataToEncrypt,bool DoOAEPPadding, RSACryptoServiceProvider RSA)
        {
            try
            {
                byte[] encryptedData;

                encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                
                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }

        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt,bool DoOAEPPadding, RSACryptoServiceProvider RSA)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
       
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, ICryptoTransform decryptor)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.

            // Create a decryptor to perform the stream transform.

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            return plaintext;

        }
    }
}

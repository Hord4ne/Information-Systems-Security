using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Task2
{
    class Program
    {
        /*
         * Consider at least 3 different hash functions and prepare a summary of performance tests results.
Additionally include any “slow” function (e.g. PBKDF2) and check the difference.
         */
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

            var testRepetitions = 1000;
            var testIterations = 10000;
            var testString = GetTestString(testRepetitions);
            var testBytes = Encoding.UTF8.GetBytes(testString);
            Console.WriteLine($"Testing with testRepetitions = {testRepetitions} and testIterations = {testIterations}\n");

            TestMd5(testBytes, testIterations);
            Console.WriteLine();

            TestSHA1(testBytes, testIterations);
            Console.WriteLine();

            TestSHA512(testBytes, testIterations);
            Console.WriteLine();

            TestPBKDF2(testBytes, testIterations);

            Console.ReadKey();
        }

        private static void TestPBKDF2(byte[] testBytes, int testIterations)
        {

            BaseTest("PBKDF2", testIterations, testBytes, x =>
            {
                int myIterations = 1000;

                string pwd1 = "OA*YUSDyif7o8ADLFITOY&zS<sdcvnilubBYHUIOakAWET^IOUY";
                // Create a byte array to hold the random value. 
                byte[] salt1 = new byte[8];
                using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetBytes(salt1);
                }
                Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(pwd1, salt1, myIterations);
                TripleDES encAlg = TripleDES.Create();
                encAlg.Key = k1.GetBytes(16);
                using (MemoryStream encryptionStream = new MemoryStream())
                {
                    using (CryptoStream encrypt = new CryptoStream(encryptionStream, encAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        encrypt.Write(testBytes, 0, testBytes.Length);
                        encrypt.FlushFinalBlock();
                        encrypt.Close();
                        byte[] edata1 = encryptionStream.ToArray();
                        k1.Reset();
                    }

                }
            });


        }

        private static void BaseTest(string testee, int testIterations, byte[] input, Action<byte[]> body)
        {
            var sw = Stopwatch.StartNew();

            for (var i = 0; i < testIterations; i++)
            {
                body(input);
            }

            Console.WriteLine($"{testee}: Elapsed={sw.Elapsed.TotalMilliseconds} per one hash {sw.Elapsed.TotalMilliseconds / testIterations}");
        }

        private static void TestSHA1(byte[] testBytes, int testIterations)
        {
            BaseTest("SHA1", testIterations, testBytes, x =>
            {
                using (SHA1 shaM = new SHA1Managed())
                {
                    var hash = shaM.ComputeHash(x);
                }
            });
        }

        private static void TestSHA512(byte[] testBytes, int testIterations)
        {
            BaseTest("SHA512", testIterations, testBytes, x =>
            {
                using (SHA512 shaM = new SHA512Managed())
                {
                    var hash = shaM.ComputeHash(x);
                }
            });
        }

        private static void TestMd5(byte[] testBytes, int testIterations)
        {
            BaseTest("Md5", testIterations, testBytes, x =>
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    var hash = GetMd5Hash(md5Hash, x);
                }
            });

        }
        static string GetMd5Hash(MD5 md5Hash, byte[] input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(input);

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}

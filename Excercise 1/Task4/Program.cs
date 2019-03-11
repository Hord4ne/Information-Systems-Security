using System;
using System.Security.Cryptography;
using System.Text;


namespace Task4
{
    /*
     * Implement a common scenario of the digital signature and message authentication code using a
cryptography API.
     */
    class Program
    {
        static void Main(string[] args)
        {
            //JustRSA();
            var agent1 = new AgentSafeFromNSA();
            var agent2 = new AgentSafeFromNSA();

            var secret = Utility.GetSecret();
            var message = Utility.ConvertBytesToString64(secret);

            var rsaRes = agent1.EncryptAndSignMessageRSA(message, agent2.RSAParametersPublic);
            Console.WriteLine("RSA result:");
            Console.WriteLine($"Encrypted data: {rsaRes.Encrypted}");
            Console.WriteLine($"Signed: {rsaRes.Signed}");

            var decrypted = agent2.DecryptAdnVerifySignedMessageRSA(rsaRes.Encrypted, rsaRes.Signed, agent1.RSAParametersPublic);
            Console.WriteLine($"Decryted: {decrypted}");

            var returnMessage = "I'll send an SOS to the world";
            var hmacHash = agent2.SignMessageHMAC(returnMessage, Utility.ConvertFromString64ToBytes(decrypted));
            Console.WriteLine($"HMACH HASH {hmacHash}");

            var verification = agent1.VerifyMessageHMAC(returnMessage, Utility.ConvertFromString64ToBytes(hmacHash), secret);
            Console.WriteLine($"Verified? {verification}");
            Console.WriteLine($"Message {returnMessage}");
            Console.ReadKey();
        }

        private static void JustRSA()
        {
            var agent1 = new AgentSafeFromNSA();
            var agent2 = new AgentSafeFromNSA();

            var message = "in the bottle";

            var rsaRes = agent1.EncryptAndSignMessageRSA(message, agent2.RSAParametersPublic);
            Console.WriteLine("RSA result:");
            Console.WriteLine($"Encrypted data: {rsaRes.Encrypted}");
            Console.WriteLine($"Signed: {rsaRes.Signed}");

            var decrypted = agent2.DecryptAdnVerifySignedMessageRSA(rsaRes.Encrypted, rsaRes.Signed, agent1.RSAParametersPublic);
            Console.WriteLine($"Decryted: {decrypted}");
        }
    }

    public class AgentSafeFromNSA
    {
        private RSAParameters _RSAParametersPrivate;
        public RSAParameters RSAParametersPublic { get; set; }
        public AgentSafeFromNSA()
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024))
            {
                
                _RSAParametersPrivate = RSA.ExportParameters(true);
                RSAParametersPublic = RSA.ExportParameters(false);
            }
        }

        public RSAResult EncryptAndSignMessageRSA(string message, RSAParameters publicKeyOfReceiver)
        {
            string signedAsString;
            byte[] signed;
            byte[] encrypted;
            string encryptedAsString;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024))
            {
                RSA.ImportParameters(publicKeyOfReceiver);
                var messageAsBytes = Utility.ConvertStringUTF8ToBytes(message);

                Console.WriteLine("Encrypting");
                encrypted = RSA.Encrypt(messageAsBytes, false);
                Console.WriteLine("Encrypting done!");

                RSA.ImportParameters(_RSAParametersPrivate);

                Console.WriteLine("Signing");
                signed = RSA.SignData(encrypted, new SHA1CryptoServiceProvider());
                Console.WriteLine("Signing done!");
            }
            signedAsString = Utility.ConvertBytesToString64(signed);
            encryptedAsString = Utility.ConvertBytesToString64(encrypted);
            return new RSAResult { Encrypted = encryptedAsString, Signed = signedAsString };
        }

        public string DecryptAdnVerifySignedMessageRSA(string encrypted, string sign, RSAParameters publicKeyOfSender)
        {
            string result;
            byte[] decrypted;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024))
            {
                RSA.ImportParameters(publicKeyOfSender);
                var messageAsBytes = Utility.ConvertFromString64ToBytes(encrypted);
                var signedAsBytes = Utility.ConvertFromString64ToBytes(sign);
                Console.WriteLine("Veryfing singer");
                RSA.VerifyData(messageAsBytes, new SHA1CryptoServiceProvider(), signedAsBytes);
                Console.WriteLine("Veryfing singer done!");
                RSA.ImportParameters(_RSAParametersPrivate);

                Console.WriteLine("Decrypting");
                decrypted = RSA.Decrypt(messageAsBytes, false);
                Console.WriteLine("Decrypting done!");
            }

            result = Utility.ConvertFromUTF8Bytes(decrypted);
            return result;
        }

        public string SignMessageHMAC(string message, byte[] key)
        {
            byte[] hash;
            using (HMACSHA512 hmac = new HMACSHA512(key))
            {
                var messageAsBytes = Utility.ConvertStringUTF8ToBytes(message);
                hash = hmac.ComputeHash(messageAsBytes);
            }
            return Utility.ConvertBytesToString64(hash);
        }

        public bool VerifyMessageHMAC(string message, byte[] hash, byte[] key)
        {
            bool result = true;
            using (HMACSHA512 hmac = new HMACSHA512(key))
            {
                var messageAsBytes = Utility.ConvertStringUTF8ToBytes(message);
                var myHash = hmac.ComputeHash(messageAsBytes);
                for (int i = 0; i < hash.Length; i++)
                {
                    if (myHash[i] != hash[i])
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
    }

    public static class Utility
    {
        public static byte[] ConvertStringUTF8ToBytes(string input)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            return ByteConverter.GetBytes(input);
        }
        public static string ConvertFromUTF8Bytes(byte[] input)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            return ByteConverter.GetString(input);
        }
        public static string ConvertBytesToString64(byte[] input)
        {
            return Convert.ToBase64String(input);
        }
        public static byte[] ConvertFromString64ToBytes(string input)
        {
            return Convert.FromBase64String(input);
        }
        public static byte[] GetSecret()
        {
            byte[] secretkey = new Byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                // The array is now filled with cryptographically strong random bytes.
                rng.GetBytes(secretkey);
            }
            return secretkey;
        }


    }
    public class RSAResult
    {
        public string Encrypted { get; set; }
        public string Signed { get; set; }
    }
}

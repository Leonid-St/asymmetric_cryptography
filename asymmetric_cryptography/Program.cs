using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace asymmetric_cryptography
{
    class Program
    {
        static public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(RSAKeyInfo);
            return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }
        static public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(RSAKeyInfo);
            return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private static string _privateKey;
        private static string _publicKey;
        private static UnicodeEncoding _encoder = new UnicodeEncoding();
        static void Main(string[] args)
        {        //Initialize the byte arrays to the public key information.
            /*byte[] modulus =
            {
            214,46,220,83,160,73,40,39,201,155,19,202,3,11,191,178,56,
            74,90,36,248,103,18,144,170,163,145,87,54,61,34,220,222,
            207,137,149,173,14,92,120,206,222,158,28,40,24,30,16,175,
            108,128,35,230,118,40,121,113,125,216,130,11,24,90,48,194,
            240,105,44,76,34,57,249,228,125,80,38,9,136,29,117,207,139,
            168,181,85,137,126,10,126,242,120,247,121,8,100,12,201,171,
            38,226,193,180,190,117,177,87,143,242,213,11,44,180,113,93,
            106,99,179,68,175,211,164,116,64,148,226,254,172,147
        };

            byte[] exponent = { 1, 0, 1 };

            //Create values to store encrypted symmetric keys.
            byte[] encryptedSymmetricKey;
            byte[] encryptedSymmetricIV;

            //Create a new instance of the RSA class.
            RSA rsa = RSA.Create();

            //Create a new instance of the RSAParameters structure.
            RSAParameters rsaKeyInfo = new RSAParameters();

            //Set rsaKeyInfo to the public key values.
            rsaKeyInfo.Modulus = modulus;
            rsaKeyInfo.Exponent = exponent;

            //Import key parameters into rsa.
            rsa.ImportParameters(rsaKeyInfo);

            //Create a new instance of the default Aes implementation class.
            Aes aes = Aes.Create();

            //Encrypt the symmetric key and IV.
            encryptedSymmetricKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1);
            encryptedSymmetricIV = rsa.Encrypt(aes.IV, RSAEncryptionPadding.Pkcs1);*/

            //--//
            //Generate a public/private key pair.  
            //RSA rsa2 = RSA.Create();
            //Save the public key information to an RSAParameters structure.  
            //RSAParameters rsaKeyInfo2 = rsa2.ExportParameters(false);

            //--//
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            var a = Assembly.GetExecutingAssembly().Location;
            Console.WriteLine(a);
            

            var path_public_key = a.Remove(a.Length - 28) +"\\public_key.txt";
            var path_private_key = a.Remove(a.Length - 28) + "\\private_key.txt";
            var path_public_key2 = a.Remove(a.Length - 28) + "\\public_key2.txt";
            var path_private_key2 = a.Remove(a.Length - 28) + "\\private_key2.txt";
            var path_generated_text = a.Remove(a.Length - 28) + "\\generated_text.txt";
            //рандомая строка
            Console.WriteLine("1.	Сгенерировать открытый и закрытый ключи размерами 2048 байт. Записать их в отдельные файлы.");
            Console.WriteLine();
            var randomString = RandomString(40);
            using (FileStream fstream = new FileStream(path_generated_text, FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] buffer = Encoding.Default.GetBytes(randomString);
                // запись массива байтов в файл
                fstream.Write(buffer, 0, buffer.Length);
                Console.WriteLine("randomText(40) записан в файл:= " + randomString);
            }
            Console.WriteLine();
            Console.WriteLine("  2.Сгенерировать случайную последовательность из 40 символов, " +
                "содержащую цифры, буквы. Эта последовательность будет текстом для шифрования. Записать ее в файл.");
            Console.WriteLine();
            var rsa = new RSACryptoServiceProvider();
            _privateKey = rsa.ToXmlString(true);
            _publicKey = rsa.ToXmlString(false);

            // запись в файл публичного ключа 
            using (FileStream fstream = new FileStream(path_public_key, FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] buffer = Encoding.Default.GetBytes(_publicKey);
                // запись массива байтов в файл
                fstream.Write(buffer, 0, buffer.Length);
                Console.WriteLine("publicKey записан в файл:= " + _publicKey);
            }
            Console.WriteLine();
            // запись в файл строки 
            using (FileStream fstream = new FileStream(path_private_key, FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] buffer = Encoding.Default.GetBytes(_privateKey);
                // запись массива байтов в файл
                fstream.Write(buffer, 0, buffer.Length);
                Console.WriteLine("privateKey записан в файл := " + _privateKey);
            }
            Console.WriteLine();
            Console.WriteLine("3.Написать программы по шифрованию и дешифрованию сгенерированной последовательности на основе алгоритма RSA.");
            Console.WriteLine();
            Console.WriteLine("RSA // Text to encrypt: " + randomString);
            var enc = EncryptRSA(randomString);
            Console.WriteLine("RSA // Encrypted Text: " + enc);
            var dec = DecryptRSA(enc);
            Console.WriteLine("RSA // Decrypted Text: " + dec);
            Console.WriteLine("4.	Написать программу по формированию ЭЦП для сгенерированной последовательности" +
                " на основе алгоритма DSA c ключами размерами 2048 байт. Вывести в файлы открытый и закрытый ключи");
            Console.WriteLine();

            //--//
            // Экземпляр DSACryptoServiceProvider будет использоваться для
            // начальной генерации и как контейнер ключей подписи и проверки
            DSACryptoServiceProvider key = new DSACryptoServiceProvider();

            // Массив с данными для подписи
            byte[] dataToSign =
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };

            Console.WriteLine("Подписывается массив:");
            Console.WriteLine(ByteArrayToString(dataToSign));

            // Вызов функции Sign() для получения подписи
            // dataToSign - массив байт, для которого вычисляется подпись
            // key.ExportParameters(true) - извлекает структуру DSAParameters c
            // включением информации секретного ключа подписи
            byte[] signature = Sign(dataToSign, key.ExportParameters(true));

            Console.WriteLine("Подпись:");
            Console.WriteLine(ByteArrayToString(signature));

            Console.Write("Проверка подписи ... ");

            // Вызов функции VerifySignature для проверки подписи
            // dataToSign - массив байт, для которого проверяется подпись
            // signature - массив байт, содержащий подпись
            // key.ExportParameters(false) - извлекает структуру DSAParameters с
            // включением ТОЛЬКО информации открытого ключа проверки подписи
            bool acceptSignature = VerifySignature(dataToSign, signature, key.ExportParameters(false));
            if (acceptSignature)
            {
                Console.WriteLine("УСПЕШНО!");
            }
            else
            {
                Console.WriteLine("ОШИБКА.");
            }
            // запись в файл публичного ключа 
            using (FileStream fstream = new FileStream(path_public_key2, FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] buffer = Encoding.Default.GetBytes(key.ToXmlString(false));
                // запись массива байтов в файл
                fstream.Write(buffer, 0, buffer.Length);
                Console.WriteLine("publicKey записан в файл:= " + key.ToXmlString(false));
            }
            Console.WriteLine();
            // запись в файл приватного ключа
            using (FileStream fstream = new FileStream(path_private_key2, FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] buffer = Encoding.Default.GetBytes(key.ToXmlString(true));
                // запись массива байтов в файл
                fstream.Write(buffer, 0, buffer.Length);
                Console.WriteLine("privateKey записан в файл := " + key.ToXmlString(true));
            }
            Console.WriteLine();
            Console.ReadLine();
        }
        // Функция вычисляет цифровую подпись DSA для массива байт data c ключом privateKey
        //
        static byte[] Sign(byte[] data, DSAParameters privateKey)
        {
            // Экземпляр провайдера DSA
            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();

            // Импорт ключа для вычисления подписи
            dsa.ImportParameters(privateKey);

            // Вычисление и возврат массива байт подписи
            return dsa.SignData(data);
        }

        //
        // Функция проверяет цифровую подпись signature для data с ключом publicKey
        //
        static bool VerifySignature(byte[] data, byte[] signature, DSAParameters publicKey)
        {
            // Экземпляр провайдера DSA
            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();

            // Импорт ключа для проверки подписи
            dsa.ImportParameters(publicKey);

            // Возврат статуса проверки подписи
            return dsa.VerifyData(data, signature);
        }

        //
        // Функция преобразует байтовый массив в шестнадцатеричную строку
        //
        static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        public static string DecryptRSA(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            var dataArray = data.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            rsa.FromXmlString(_privateKey);
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }

        public static string EncryptRSA(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_publicKey);
            var dataToEncrypt = _encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            var length = encryptedByteArray.Count();
            var item = 0;
            var sb = new StringBuilder();
            foreach (var x in encryptedByteArray)
            {
                item++;
                sb.Append(x);

                if (item < length)
                    sb.Append(",");
            }

            return sb.ToString();
        }
    }
}

using pow;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace PoW
{
    class Program
    {
        static void Main()
        {
            bool isBlockValid = false;
            var startingZeros = "0000";
            var path = @"C:\Users\User\Desktop\4kurs2sem\blockchain\pow\test.json";

            var block = new Block
            {
                Data = "hello world",
                Hash = string.Empty,
                Nonce = 0,
            };

            while (!block.Hash.StartsWith(startingZeros))
            {
                block.Nonce++;
                block.Hash = ComputeSha256Hash(block.Data + block.Nonce.ToString());
            }

            using (FileStream fileStream = File.Create(path))
            {
                JsonSerializer.SerializeAsync(fileStream, block);
            }

            isBlockValid = ComputeSha256Hash(block.Data + block.Nonce.ToString()) == GetBlock(path);
            Console.WriteLine($"{block.Hash} : {isBlockValid}");


        }

        static string ComputeSha256Hash(string data)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        static string GetBlock(string path)
        {
            Block block = JsonSerializer.Deserialize<Block>(File.ReadAllText(path));

            return block.Hash;
        }
    }
}

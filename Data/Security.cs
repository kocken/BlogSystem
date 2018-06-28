namespace Data
{
    public class Security
    {
        private static readonly string charString =
            "dhrb1ay9i7vgm54zcxejln26t8q0kfswp3uo"; // randomized "abcdefghijklmnopqrstuvwxyz0123456789";
        private static readonly char[] charArray = charString.ToCharArray();

        public static string ROT13EncryptMessage(string input)
        {
            return ROT13EncryptMessage(input, charArray.Length / 2);
        }

        public static string ROT13EncryptMessage(string input, int steps)
        {
            char[] arr = input.ToCharArray();
            for (int i = 0; arr.Length > i; i++)
            {
                for (int y = 0; charArray.Length > y; y++)
                {
                    if (arr[i].Equals(charArray[y]) || arr[i].Equals(char.ToUpper(charArray[y])))
                    {
                        if (charArray.Length > (y + steps))
                        {
                            arr[i] = arr[i].Equals(charArray[y]) ? charArray[y + steps] : char.ToUpper(charArray[y + steps]);
                        }
                        else
                        {
                            arr[i] = arr[i].Equals(charArray[y]) ? charArray[y + steps - charArray.Length] :
                                char.ToUpper(charArray[y + steps - charArray.Length]);
                        }
                        break;
                    }
                }
            }
            return new string(arr);
        }

        public static string ROT13DecryptMessage(string input)
        {
            return ROT13DecryptMessage(input, charArray.Length / 2);
        }

        public static string ROT13DecryptMessage(string input, int steps)
        {
            char[] arr = input.ToCharArray();
            for (int i = 0; arr.Length > i; i++)
            {
                for (int y = 0; charArray.Length > y; y++)
                {
                    if (arr[i].Equals(charArray[y]) || arr[i].Equals(char.ToUpper(charArray[y])))
                    {
                        if ((y - steps) >= 0)
                        {
                            arr[i] = arr[i].Equals(charArray[y]) ? charArray[y - steps] : char.ToUpper(charArray[y - steps]);
                        }
                        else
                        {
                            arr[i] = arr[i].Equals(charArray[y]) ? charArray[y - steps + charArray.Length] :
                                char.ToUpper(charArray[y - steps + charArray.Length]);
                        }
                        break;
                    }
                }
            }
            return new string(arr);
        }

    }
}
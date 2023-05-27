namespace UploadProject.Helper
{
    public static class PasswordGenerator
    {
        private static string alphaNum = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string GetRandomPassword()
        {
            var random = new Random();
            var result = "";
            while (result.Length < 8)
            {
                var randomChar = alphaNum[random.Next(0, alphaNum.Length)].ToString();

                if (result.Contains(randomChar))
                {
                    continue;
                }

                result += randomChar.ToString();
            }

            return result;
        }
    }
}

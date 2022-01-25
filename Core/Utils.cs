namespace Core
{
    public static class Utils
    {
        public static string ConvertImageToBase64(Stream fs, string type = "png")
        {
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            string imageUrl = $"data:image/{type};base64," + base64String;
            return imageUrl;
        }

        
    }
}

using P140_Pronia.Entities;

namespace P140_Pronia.Helpers
{
    public static class FileUploadExtension
    {
        public static async Task<string> GeneratePhoto(this IFormFile file, params string[] folders)
        {
            string folderPath = Path.Combine(folders);
            string fileName = Guid.NewGuid() + file.FileName;
            string fullPath = Path.Combine(folderPath, fileName);

            using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }

        public static bool IsValidLength(this IFormFile file,double memoryMb)
        {
            bool result = (double)file.Length / 1024 / 1024 <= memoryMb;
            return result;
        }

        public static void DeleteImage(this string fileName, params string[] folders)
        {
            string folderPath = Path.Combine(folders);
            string fullPath = Path.Combine(folderPath, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}

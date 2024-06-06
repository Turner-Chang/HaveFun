namespace HaveFun.Common
{
    public class SaveImage
    {
        // 圖片檔案的取名
        public string Name {  get; set; }

        // 要存取到的資料夾路徑
        public string Path {  get; set; }

        // 從IFormFile存取來的圖片檔案
        public IFormFile Picture { get; set; }

        public SaveImage() { }

        public SaveImage(string path, string name, IFormFile picture)
        {
            Path = path;
            Name = name;
            Picture = picture;
        }

        // 存圖片的方法
        public bool Save(out string fullPath)
        {
            try
            {
                if (isImage())
                {
                    fullPath = Path + "\\" + Name;
                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        Picture.CopyTo(fileStream);
                        return true;
                    }
                }
                else
                {
                    fullPath = string.Empty;
                    return false;
                }
  
            }
            catch (Exception)
            {
                fullPath = string.Empty;
                return false;
            }
        }

        // 驗證檔案是否為圖片
        private bool isImage()
        {
            string imageName = Picture.FileName.Trim();
            if(imageName.EndsWith(".apng") || imageName.EndsWith(".avif") ||
                imageName.EndsWith(".gif") || imageName.EndsWith(".jpg") ||
                imageName.EndsWith(".jpeg") || imageName.EndsWith(".jfif") ||
                imageName.EndsWith(".pjpeg") || imageName.EndsWith(".pjp") ||
                imageName.EndsWith(".png") || imageName.EndsWith(".svg") ||
                imageName.EndsWith(".webp"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

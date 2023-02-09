using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace D11
{
    public class ToFileDownloadHandler : DownloadHandlerScript
    {
        public bool IsDone { get; private set; }
        public Texture2D texture { get; private set; }
        private ulong expected = 0;
        private int received = 0;
        private string filepath;
        private FileStream fileStream;
        private bool canceled = false;
        public ToFileDownloadHandler() : base()
        {
        }
        public ToFileDownloadHandler(byte[] buffer, string filepath)
          : base(buffer)
        {
            IsDone = false;
            texture = null;
            this.filepath = filepath;
            fileStream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
        }

        protected override byte[] GetData() { return null; }

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            if (data == null || data.Length < 1)
            {
                return false;
            }
            received += dataLength;
            if (!canceled)
            {
                fileStream.Write(data, 0, dataLength);
            }
            return true;
        }

        protected override float GetProgress()
        {
            if (expected <= 0) return 0;
            return (float)received / expected;
        }

        protected override void CompleteContent()
        {
            IsDone = true;
            texture = ImageCacheUtils.Instance.TextureFromFile(filepath);
            fileStream.Close();
        }

        protected override void ReceiveContentLengthHeader(ulong contentLength)
        {
            expected = contentLength;
        }

        public void Cancel()
        {
            canceled = true;
            fileStream.Close();
            File.Delete(filepath);
        }
    }
}

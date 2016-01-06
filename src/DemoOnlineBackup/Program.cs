using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Ionic.Zip;

namespace DemoOnlineBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            var lokasiFileBackup = args[0];
            var fileBackup = args[1];
            var extension = args[2];

            CreateZip(lokasiFileBackup, fileBackup, extension);
            UploadToGoogleDrive(lokasiFileBackup, fileBackup);

            Console.WriteLine("\n\nDone");
        }

        private static void CreateZip(string pathFileBackup, string fileBackup, string extension)
        {
            var fileToZip = string.Format(@"{0}\{1}.zip", pathFileBackup, fileBackup);
            fileBackup = string.Format(@"{0}\{1}.{2}", pathFileBackup, fileBackup, extension);

            using (var zip = new ZipFile())
            {
                Console.WriteLine("\nAdding {0}...", fileBackup);
                ZipEntry e = zip.AddFile(fileBackup, "");

                zip.Save(fileToZip);
            }
        }

        private static void UploadToGoogleDrive(string pathFileBackup, string fileBackup)
        {
            // TODO : disesuaikan dengan nilai clientId dan clientSecret Anda
            //        setting ini juga bisa disimpan di file App.config
            var clientId = "505165741497-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.apps.googleusercontent.com";
            var clientSecret = "6Nxxxxxx-xxxxxxxxxxxxxxx";

            var service = GoogleDriveHelper.AuthenticateOauth(clientId, clientSecret, Environment.UserName);

            if (service == null)
            {
                Console.WriteLine("Authentication error");
                Console.ReadLine();
            }            

            try
            {
                Console.WriteLine("Sedang proses upload ...");

                // TODO : disesuaikan dengan nilai folder id Anda
                //        setting ini juga bisa disimpan di file App.config
                var folderId = "0B5xxxxxxxxxxxxxxxxxxxxxxxxx";                

                // upload file
                var fileToUpload = string.Format("{0}\\{1}.zip", pathFileBackup, fileBackup);
                File newFile = GoogleDriveHelper.UploadFile(service, fileToUpload, folderId);

            }
            catch
            {
                Console.WriteLine("\nUpload file gagal !");
            }
            finally
            {
                // hapus file yang berhasil diupload
                var fileToDelete = string.Format("{0}\\{1}.zip", pathFileBackup, fileBackup);
                System.IO.File.Delete(fileToDelete);
            }
        }
    }
}

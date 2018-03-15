using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using System.Reflection;
using System.IO;
using ICSharpCode.SharpZipLib.Core;

namespace AccessIconPack.Models
{
    public class LoadIconPack
    {
        private string _pathToExtract;

        public LoadIconPack(string pathToExtract)
        {
            _pathToExtract = pathToExtract;
        }

        public void Extract()
        {
            using (var zipStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AccessIconPack.Access-SQL-Editor-Icon-Pack.zip"))
            {
                UnzipFromStream(zipStream, _pathToExtract);
            }
        }

        //https://github.com/icsharpcode/SharpZipLib/wiki/Zip-Samples#unpack-a-zip---including-embedded-zips---and-re-pack-into-a-new-zip-or-memorystream
        private void UnzipFromStream(Stream zipStream, string outFolder)
        {

            ZipInputStream zipInputStream = new ZipInputStream(zipStream);
            ZipEntry zipEntry = zipInputStream.GetNextEntry();
            while (zipEntry != null)
            {
                String entryFileName = zipEntry.Name;
                // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                // Optionally match entrynames against a selection list here to skip as desired.
                // The unpacked length is available in the zipEntry.Size property.

                byte[] buffer = new byte[4096];     // 4K is optimum

                // Manipulate the output filename here as desired.
                String fullZipToPath = Path.Combine(outFolder, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                // Skip directory entry
                string fileName = Path.GetFileName(fullZipToPath);
                if (fileName.Length == 0)
                {
                    zipEntry = zipInputStream.GetNextEntry();
                    continue;
                }

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                using (FileStream streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                }
                zipEntry = zipInputStream.GetNextEntry();
            }
        }
    }
}

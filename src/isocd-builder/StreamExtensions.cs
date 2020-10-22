using System.IO;
using System.Threading;

namespace isocd_builder {
    public static class StreamExtensions {
        static readonly byte[] buffer = new byte[isocd_builder_constants.COPYTO_BUF_SIZE];

        public static void CopyToWithCancel(this Stream source, Stream destination, CancellationToken cancellationToken) {
            int count;

            // If the file size is greater than isocd_builder_constants.COPYTO_BUF_SIZE, then copy it in chunks
            if(source.Length > isocd_builder_constants.COPYTO_BUF_SIZE) {
                
                while((count = source.Read(buffer, 0, buffer.Length)) != 0) {
                    cancellationToken.ThrowIfCancellationRequested();
                    destination.Write(buffer, 0, count);
                }
            }
            // Otherwise read the whole file
            else {
                count = source.Read(buffer, 0, buffer.Length);
                cancellationToken.ThrowIfCancellationRequested();
                destination.Write(buffer, 0, count);
            }
        }
    }
}

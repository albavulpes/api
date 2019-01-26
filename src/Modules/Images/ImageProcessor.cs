using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace AlbaVulpes.API.Modules.Images
{
    public static class ImageProcessor
    {
        public static Stream ProcessImage(Stream inStream, string contentType = null)
        {
            inStream.Position = 0;

            using (var image = LoadImage(inStream, contentType))
            {
                var outStream = new MemoryStream();

                image.Save(outStream, new JpegEncoder
                {
                    Quality = 85
                });

                return outStream;
            }
        }

        public static Stream CreateThumbnail(Stream inStream, string contentType = null)
        {
            inStream.Position = 0;

            using (var image = LoadImage(inStream, contentType))
            {
                var outStream = new MemoryStream();

                const double maxDimension = 300.00;
                var factor = maxDimension / Math.Max(image.Width, image.Height);

                var newWidth = (int)(image.Width * factor);
                var newHeight = (int)(image.Height * factor);

                image
                    .Mutate(c =>
                        c.Resize(newWidth, newHeight, new Lanczos3Resampler())
                    );

                image.Save(outStream, new JpegEncoder
                {
                    Quality = 75
                });

                return outStream;
            }
        }

        private static Image<Rgba32> LoadImage(Stream inStream, string contentType)
        {
            if (contentType == null)
            {
                return Image.Load(inStream);
            }

            return Image.Load(inStream, ResolveDecoderFromContentType(contentType));
        }

        private static IImageDecoder ResolveDecoderFromContentType(string contentType)
        {
            switch (contentType)
            {
                case "image/png":
                    return new PngDecoder();
                case "image/gif":
                    return new GifDecoder();
                case "image/bmp":
                    return new BmpDecoder();
                case "image/jpg":
                case "image/jpeg":
                    return new JpegDecoder();
                default:
                    return null;
            }
        }
    }
}
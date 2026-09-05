namespace Shared.Constants;

public readonly record struct ImageDimensions(int Width, int Height)
{
    public double AspectRatio => Height == 0 ? 0 : (double)Width / Height;

    public override string ToString() => $"{Width} × {Height} px";
}

public static class ImageDimensionReader
{
    public static ImageDimensions? Read(ReadOnlySpan<byte> content)
    {
        var format = ImageFormatCatalog.Detect(content);

        if (format == ImageFormatCatalog.Png)
            return ReadPng(content);

        if (format == ImageFormatCatalog.Jpeg)
            return ReadJpeg(content);

        if (format == ImageFormatCatalog.Webp)
            return ReadWebp(content);

        if (format == ImageFormatCatalog.Gif)
            return ReadGif(content);

        return null;
    }

    private static ImageDimensions? ReadGif(ReadOnlySpan<byte> content)
    {
        if (content.Length < 10)
            return null;

        var width = content[6] | (content[7] << 8);
        var height = content[8] | (content[9] << 8);

        return Validate(width, height);
    }

    private static ImageDimensions? ReadPng(ReadOnlySpan<byte> content)
    {
        const int ihdrOffset = 8 + 4 + 4;

        if (content.Length < ihdrOffset + 8)
            return null;

        if (!content.Slice(12, 4).SequenceEqual("IHDR"u8))
            return null;

        var width = ReadBigEndianInt32(content.Slice(ihdrOffset, 4));
        var height = ReadBigEndianInt32(content.Slice(ihdrOffset + 4, 4));

        return Validate(width, height);
    }

    private static ImageDimensions? ReadJpeg(ReadOnlySpan<byte> content)
    {
        var index = 2;

        while (index + 3 < content.Length)
        {
            if (content[index] != 0xFF)
            {
                index++;
                continue;
            }

            var marker = content[index + 1];

            if (marker == 0xFF)
            {
                index++;
                continue;
            }

            if (marker == 0x01 || (marker >= 0xD0 && marker <= 0xD9))
            {
                index += 2;
                continue;
            }

            if (index + 4 >= content.Length)
                return null;

            var segmentLength = (content[index + 2] << 8) | content[index + 3];

            if (segmentLength < 2)
                return null;

            var isStartOfFrame =
                marker >= 0xC0 && marker <= 0xCF &&
                marker != 0xC4 && marker != 0xC8 && marker != 0xCC;

            if (isStartOfFrame)
            {
                if (index + 9 >= content.Length)
                    return null;

                var height = (content[index + 5] << 8) | content[index + 6];
                var width = (content[index + 7] << 8) | content[index + 8];

                return Validate(width, height);
            }

            if (marker == 0xDA)
                return null;

            index += 2 + segmentLength;
        }

        return null;
    }

    private static ImageDimensions? ReadWebp(ReadOnlySpan<byte> content)
    {
        if (content.Length < 30)
            return null;

        var chunk = content.Slice(12, 4);

        if (chunk.SequenceEqual("VP8 "u8))
        {
            if (content.Length < 30 ||
                content[23] != 0x9D || content[24] != 0x01 || content[25] != 0x2A)
            {
                return null;
            }

            var width = ((content[27] << 8) | content[26]) & 0x3FFF;
            var height = ((content[29] << 8) | content[28]) & 0x3FFF;

            return Validate(width, height);
        }

        if (chunk.SequenceEqual("VP8L"u8))
        {
            if (content.Length < 25 || content[20] != 0x2F)
                return null;

            var bits = content[21] | (content[22] << 8) | (content[23] << 16) | (content[24] << 24);

            var width = (bits & 0x3FFF) + 1;
            var height = ((bits >> 14) & 0x3FFF) + 1;

            return Validate(width, height);
        }

        if (chunk.SequenceEqual("VP8X"u8))
        {
            if (content.Length < 30)
                return null;

            var width = (content[24] | (content[25] << 8) | (content[26] << 16)) + 1;
            var height = (content[27] | (content[28] << 8) | (content[29] << 16)) + 1;

            return Validate(width, height);
        }

        return null;
    }

    private static ImageDimensions? Validate(int width, int height) =>
        width > 0 && height > 0 ? new ImageDimensions(width, height) : null;

    private static int ReadBigEndianInt32(ReadOnlySpan<byte> bytes) =>
        (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
}

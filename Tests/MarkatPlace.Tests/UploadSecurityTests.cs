using System.Text;
using Shared.Constants;
using Xunit;

namespace MarkatPlace.Tests;

public class UploadSecurityTests
{
    private static byte[] Png() =>
    [
        0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,
        0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52,
    ];

    private static byte[] Jpeg() => [0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46];

    private static byte[] Gif() => Encoding.ASCII.GetBytes("GIF89a").Concat(new byte[8]).ToArray();

    [Fact]
    public void A_real_PNG_is_recognised_whatever_it_is_called()
    {
        var format = ImageFormatCatalog.Detect(Png());

        Assert.NotNull(format);
        Assert.Equal(".png", format!.CanonicalExtension);
    }

    [Fact]
    public void A_real_JPEG_is_recognised()
    {
        Assert.Equal(".jpg", ImageFormatCatalog.Detect(Jpeg())?.CanonicalExtension);
    }

    [Fact]
    public void A_real_GIF_is_recognised()
    {
        Assert.Equal(".gif", ImageFormatCatalog.Detect(Gif())?.CanonicalExtension);
    }

    [Theory]
    [InlineData("<?php system($_GET['c']); ?>")]
    [InlineData("<html><script>alert(1)</script></html>")]
    [InlineData("#!/bin/sh\nrm -rf /")]
    [InlineData("")]
    [InlineData("just some text")]
    public void Content_that_is_not_an_image_is_not_recognised_as_one(string payload)
    {
        Assert.Null(ImageFormatCatalog.Detect(Encoding.UTF8.GetBytes(payload)));
    }

    [Fact]
    public void SVG_is_not_an_accepted_image_format()
    {
        Assert.DoesNotContain(ImageFormatCatalog.All, format => format.CanonicalExtension == ".svg");
        Assert.DoesNotContain(".svg", ImageFormatCatalog.AllExtensions);
        Assert.Null(ImageFormatCatalog.Detect(
            Encoding.UTF8.GetBytes("<svg xmlns='http://www.w3.org/2000/svg' onload='alert(1)'/>")));
    }

    [Theory]
    [InlineData(new byte[] { 0x4D, 0x5A, 0x90, 0x00 })]
    [InlineData(new byte[] { 0x7F, 0x45, 0x4C, 0x46 })]
    public void Executables_are_identified_as_executables(byte[] content)
    {
        Assert.True(ImageFormatCatalog.LooksExecutable(content));
        Assert.Null(ImageFormatCatalog.Detect(content));
    }

    [Fact]
    public void A_genuine_image_is_not_mistaken_for_an_executable()
    {
        Assert.False(ImageFormatCatalog.LooksExecutable(Png()));
        Assert.False(ImageFormatCatalog.LooksExecutable(Jpeg()));
    }

    [Fact]
    public void The_canonical_extension_never_comes_from_the_submitted_file_name()
    {
        foreach (var format in ImageFormatCatalog.All)
        {
            Assert.StartsWith(".", format.CanonicalExtension);
            Assert.DoesNotContain("/", format.CanonicalExtension);
            Assert.DoesNotContain("\\", format.CanonicalExtension);
            Assert.DoesNotContain("..", format.CanonicalExtension);
        }
    }

    [Fact]
    public void The_request_ceiling_clears_every_per_module_limit()
    {
        Assert.True(FileUploadConstants.MaxRequestBodySizeBytes
                    >= FileUploadConstants.MaxRealEstateRequestBodySizeBytes);
        Assert.True(FileUploadConstants.MaxRequestBodySizeBytes
                    >= FileUploadConstants.MaxJobRequestBodySizeBytes);
        Assert.True(FileUploadConstants.MaxRequestBodySizeBytes
                    >= ImageConstants.MaxRequestBodySizeBytes);
    }

    [Fact]
    public void A_gallery_of_the_maximum_size_fits_inside_the_image_ceiling()
    {
        Assert.True(
            ImageConstants.MaxRequestBodySizeBytes
            >= ImageConstants.MaxImagesPerItem * ImageConstants.MaxFileSizeBytes);
    }
}

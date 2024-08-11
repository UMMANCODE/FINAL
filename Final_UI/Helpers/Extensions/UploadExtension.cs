namespace Final_UI.Helpers.Extensions;

public static class UploadExtension {
  private static readonly Dictionary<string, List<byte[]>> FileSignatures =
    new() {
      { ".jpeg", [
          [0xFF, 0xD8, 0xFF, 0xE0], // JFIF
          [0xFF, 0xD8, 0xFF, 0xE1], // Exif
          [0xFF, 0xD8, 0xFF, 0xE2], // Canon
          [0xFF, 0xD8, 0xFF, 0xE3], // JFIF extension
          [0xFF, 0xD8, 0xFF, 0xE8]
        ]
      },
      { ".jpg", [
          [0xFF, 0xD8, 0xFF, 0xE0], // JFIF
          [0xFF, 0xD8, 0xFF, 0xE1], // Exif
          [0xFF, 0xD8, 0xFF, 0xE2], // Canon
          [0xFF, 0xD8, 0xFF, 0xE3], // JFIF extension
          [0xFF, 0xD8, 0xFF, 0xE8]
        ]
      },
      { ".png", [[0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]]
      }
    };

  public static bool IsValidImage(IFormFile? file) {
    if (file?.Length == 0) return false;

    using var reader = new BinaryReader(file!.OpenReadStream());
    var headerBytes = reader.ReadBytes(FileSignatures.Max(m => m.Value.Max(v => v.Length)));
    return FileSignatures.Any(sig => sig.Value.Any(signature =>
      headerBytes.Take(signature.Length).SequenceEqual(signature)));
  }
}
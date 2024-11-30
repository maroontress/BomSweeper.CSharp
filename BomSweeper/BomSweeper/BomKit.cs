namespace BomSweeper;

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Security;

/// <summary>
/// Manipulates a BOM.
/// </summary>
public static class BomKit
{
    private static readonly ImmutableArray<byte> Utf8ByteOrderMark
        = [0xef, 0xbb, 0xbf];

    /// <summary>
    /// Gets whether the file of the specified path starts with UTF-8 BOM.
    /// </summary>
    /// <param name="path">
    /// The path of the file to be checked.
    /// </param>
    /// <returns>
    /// <c>true</c> if the file is readable and it starts with UTF-8 BOM,
    /// <c>false</c> otherwise.
    /// </returns>
    public static bool StartsWithBom(string path)
    {
        try
        {
            return StartsWithUtf8Bom(path);
        }
        catch (Exception e) when (e is EndOfStreamException
            || e is DirectoryNotFoundException
            || e is FileNotFoundException
            || e is IOException
            || e is UnauthorizedAccessException)
        {
            return false;
        }
    }

    /// <summary>
    /// Remove the BOM of the specified file.
    /// </summary>
    /// <param name="path">
    /// The path of the file to remove a BOM.
    /// </param>
    public static void RemoveBom(string path)
    {
        void PrintError(Exception e)
        {
            Console.WriteLine(
                $"{path}: Unable to remove a BOM: {e.Message}");
        }

        try
        {
            var file = File.ReadAllBytes(path);
            if (file.Length < 3)
            {
                return;
            }
            var newFile = file.Skip(3).ToArray();
            File.WriteAllBytes(path, newFile);
        }
        catch (SecurityException e)
        {
            PrintError(e);
        }
        catch (UnauthorizedAccessException e)
        {
            PrintError(e);
        }
        catch (NotSupportedException e)
        {
            PrintError(e);
        }
        catch (FileNotFoundException)
        {
            return;
        }
        catch (DirectoryNotFoundException)
        {
            return;
        }
        catch (PathTooLongException e)
        {
            PrintError(e);
        }
        catch (ArgumentException e)
        {
            PrintError(e);
        }
        catch (IOException e)
        {
            PrintError(e);
        }
    }

    private static bool StartsWithUtf8Bom(string path)
    {
        static void ReadFully(Stream s, byte[] a, int o, int n)
        {
            var offset = o;
            var length = n;
            while (length > 0)
            {
                var size = s.Read(a, offset, length);
                if (size == 0)
                {
                    throw new EndOfStreamException();
                }
                offset += size;
                length -= size;
            }
        }

        var array = new byte[Utf8ByteOrderMark.Length];
        using var stream = new FileStream(path, FileMode.Open);
        ReadFully(stream, array, 0, array.Length);
        return array.SequenceEqual(Utf8ByteOrderMark);
    }
}

# Purebyuu

Read and parse a DST (Data Stitch Tajima) file, used as input for certain embroidery machines.

This project allows a file to be read, and the resulting commands written as a human readable list of
stitches and jumps, or it can generate a SVG preview.

## Main Goals

Reading a DST file and generating some kind of preview of it.

## Usage

Create an instance of `DstFile`, passing it a stream representing a DST file, then call `.Read()` on it. Since we're dealing with IO in a stream, and it's 2018, it's async. Deal with it.

```csharp
using Purebyuu;

var file = new DstFile(File.OpenRead(path));
await file.Read(cancellationToken);
```

The various output formats are found in `Purebyuu.Output`:

```csharp
using Purebyuu;
using Purebyuu.Output;

var file = new DstFile(File.OpenRead(path));
await file.Read(cancellationToken);

File.WriteAllText("out.txt", CommandWriter.Write(file), Encoding.UTF8);
File.WriteAllText("out.svg", SvgWriter.Write(file), Encoding.UTF8);
```

These writers only return a string, delegating the storage logic to the calling application.
Mash it into a file, stick it in a database, we don't care!

## Design

The main entrypoint is the `DstFile` class. It contains the `Header` and the commands, represented by `Pattern`. A `Pattern` contains multiple stitch-blocks, which is separated by a set of jumps.

From what I can tell, and I'm absolutely not an expert, the jump should cause the machine to stop stitching, and there'll be no line from the last stitch to the first stitch after the jump.

## Unsupported features

The file format doesn't really contain anywhere to put color information, from what I can see. A nice addition would be making the stitch color user configurable when generating an SVG.

There's also commands to enter and eject a sequin mode, which I have no idea what does. It currently has no effect on the preview document.

Another thing we could have implemented is building up a DST file from scratch, and/or writing our `DstFile` to a file. But it seems like a bit of a wasted effort, since I don't have an embroidery machine, nor am I likely to get access to one.

## What's with the name?

The file format we're interested in supporting comes from Tajima, a Japanese company.
When translating `preview` from English to Japanese, and then running the resulting translation through a romanization, I ended up with `purubyuu`, which sounds a bit like `preview` if pronounced in an exaggerated Asian accent.

## License

[MIT Licensed](LICENSE.md)

## Resources

- https://edutechwiki.unige.ch/en/Embroidery_format_DST
- https://community.kde.org/Projects/Liberty/File_Formats/Tajima_Ternary

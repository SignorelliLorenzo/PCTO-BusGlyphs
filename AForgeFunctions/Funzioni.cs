using AForge.Vision.GlyphRecognition;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AForgeFunctions
{
    public class Funzioni
    {
        public static GlyphDatabase AddGlyphData()
        {
            GlyphDatabase glyphDatabase = new GlyphDatabase(5);

            //1
            glyphDatabase.Add(new Glyph("Creeper", new byte[5, 5] {
            { 0, 0, 0, 0, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 0, 1, 1, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0 } }));
            // 2
            glyphDatabase.Add(new Glyph("Fly", new byte[5, 5] {
            { 0, 0, 0, 0, 0 },
            { 0, 1, 0, 0, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 1, 0, 1, 0 },
            { 0, 0, 0, 0, 0 } }));
            // 3
            glyphDatabase.Add(new Glyph("Bug", new byte[5, 5] {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 0, 1, 1, 0 },
            { 0, 0, 0, 0, 0 } }));
            return glyphDatabase;
        }
        public static int FindG(string img)
        {
            GlyphRecognizer recognizer = new GlyphRecognizer(AddGlyphData());
            int x = 0;
            Bitmap image;
            try
            {
                image = new Bitmap(Directory.GetCurrentDirectory() + $@"\{img}", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            List<ExtractedGlyphData> glyphs = recognizer.FindGlyphs(image);
            foreach (ExtractedGlyphData glyphData in glyphs)
            {
                var s = IsInDatabase(glyphData);
                if (s == true)
                {
                    x += 1;
                }
            }
            return x;
        }

        public static string FindGlyphName(Bitmap img)
        {
            GlyphRecognizer recognizer = new GlyphRecognizer(AddGlyphData());


            List<ExtractedGlyphData> glyphs = recognizer.FindGlyphs(img);
            foreach (ExtractedGlyphData glyphData in glyphs)
            {
                var s = Funzioni.IsInDatabase(glyphData);
                if (s == true)
                {
                    var test = glyphs.Where(z => z.RecognizedGlyph != null && z.RecognizedGlyph.Name != null).Select(x => new { NomeGlifoRiconosciuto = x.RecognizedGlyph.Name }).FirstOrDefault();
                    return test.NomeGlifoRiconosciuto;
                }

            }
            throw new Exception("Glyph's Name not found");

        }


        public static bool FindG(Bitmap img)
        {
            GlyphRecognizer recognizer = new GlyphRecognizer(AddGlyphData());


            List<ExtractedGlyphData> glyphs = recognizer.FindGlyphs(img);
            foreach (ExtractedGlyphData glyphData in glyphs)
            {
                var s = IsInDatabase(glyphData);
                if (s == true)
                {
                    return true;
                }

            }
            return false;

        }

        public static Bitmap ApplyImage(bool check)
        {
            //Da modificare
            var image = new Bitmap(Directory.GetCurrentDirectory() + $@"\black.jpg", true);
            if (check == true)
            { return image; }
            else
            { throw new Exception("Failed to Apply Image"); }


        }


        public static bool IsInDatabase(ExtractedGlyphData glyphData)
        {

            if (glyphData.RecognizedGlyph != null)
            {
                return true;
            }

            return false;

        }

    }

}


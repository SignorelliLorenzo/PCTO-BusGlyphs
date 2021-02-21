using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Vision.GlyphRecognition;
using System.IO;


namespace Glyphs
{

    public class Funzioni
    {
        public static void AddGlyphData()
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
            { 0, 0, 0, 1, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 1, 0, 1, 0 },
            { 0, 0, 0, 0, 0 } }));
            // 3
            glyphDatabase.Add(new Glyph("Bug", new byte[5, 5] {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 1, 1, 0, 0 },
            { 0, 0, 1, 1, 0 },
            { 0, 0, 0, 0, 0 } }));
        }
        public static int FindG(string img)
        {
            GlyphRecognizer recognizer = new GlyphRecognizer(5);
            int x = 0;
            Bitmap image;
            try
            {
                image = new Bitmap(Directory.GetCurrentDirectory()+$"{img}.jpg", true);
            }
            catch
            {
                return -1;
            }
            List<ExtractedGlyphData> glyphs = recognizer.FindGlyphs(image);
            foreach (ExtractedGlyphData glyphData in glyphs)
            {
                x += 1;
            }
            return x;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Typography.Contours;
using Typography.OpenFont;
using Typography.TextLayout;
using R = System.Drawing.RectangleF;

namespace TypographyGlyphBounds
{
    public partial class Form1 : Form
    {
        class B : GlyphPathBuilderBase { public B(Typeface t) : base(t) { } }

        public Form1()
        {
            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Issue119(e);
        }
        void Issue118(PaintEventArgs e)
        {
            Text = "Issue 118 Demo";
            const string s = "0123456789";
            const float z = 80;
            const bool f = true;
            var m = GetType().Assembly;
            var t = new OpenFontReader().Read
                (m.GetManifestResourceStream
                (Array.Find(m.GetManifestResourceNames(), n => n.EndsWith("otf"))));
            t.UpdateAllCffGlyphBounds();
            var c = t.CalculateScaleToPixelFromPointSize(z);
            var l = new GlyphLayout { Typeface = t };
            var q = new GlyphLayout { Typeface = t };
            l.Layout(s.ToCharArray(), 0, s.Length);
            var p = l.ResultUnscaledGlyphPositions;
            var b = new B(t);
            var r = new SampleWinForms.GlyphTranslatorToGdiPath();
            var h = Pens.Black.Brush;
            var u = Pens.Blue;
            var v = Pens.Red;
            const bool _ = true;
            var j = true;
            using (var g = e.Graphics)
            {
                if (f)
                {
                    g.ScaleTransform(1, -1);
                    g.TranslateTransform(0, -Height / 2);
                }
                for (var i = 0; i < s.Length; i++, j ^= true)
                {
                    var o = q.LayoutAndMeasureString(new[] { s[i] }, 0, 1, z);
                    var n = p.GetGlyph(i, out var x, out var y, out var w);
                    var a = g.Save();
                    var d = t.GetGlyphByIndex(n).Bounds;
                    var k = R.FromLTRB(d.XMin * c, d.YMin * c, d.XMax * c, d.YMax * c);
                    g.TranslateTransform(x * c, y * c);
                    b.Build(s[i], z);
                    b.ReadShapes(r);
                    r.ResultGraphicsPath.CloseFigure();
                    g.FillPath(h, r.ResultGraphicsPath);
                    if(_ || j) g.DrawRectangle(u, 0, 0, o.width, o.ascending - o.descending);
                    if (_ || !j) g.DrawRectangle(v, k.X, k.Y, k.Width, k.Height);
                    g.Restore(a);
                    g.TranslateTransform(w * c, 0);
                }

                g.ResetTransform();
                g.DrawString("Blue = LayoutAndMeasureString, Red = Glyph.Bounds", Font, h, 0, 0);
            }
        }
        void Issue119(PaintEventArgs e)
        {
            Text = "Issue 119 Demo";
            const float z = 80;
            var m = GetType().Assembly;
            var t = new OpenFontReader().Read
                (m.GetManifestResourceStream
                (Array.Find(m.GetManifestResourceNames(), n => n.EndsWith("otf"))));
            t.UpdateAllCffGlyphBounds();
            var c = t.CalculateScaleToPixelFromPointSize(z);
            var b = new B(t);
            var r = new SampleWinForms.GlyphTranslatorToGdiPath();
            var g = t.GetGlyphByName("radical.v2");
            var o = g.Bounds;
            var k = R.FromLTRB(o.XMin * c, o.YMin * c, o.XMax * c, o.YMax * c);
            b.BuildFromGlyph(g, z);
            b.ReadShapes(r);
            e.Graphics.ScaleTransform(1, -1);
            e.Graphics.TranslateTransform(0, -Height / 2);
            e.Graphics.FillPath(Pens.Black.Brush, r.ResultGraphicsPath);
            e.Graphics.DrawRectangle(Pens.Blue, k.X, k.Y, k.Width, k.Height);
            e.Graphics.ResetTransform();
            e.Graphics.DrawString("Blue = Glyph.Bounds of radical.v2", Font, Pens.Black.Brush, 0, 0);
        }
    }
}

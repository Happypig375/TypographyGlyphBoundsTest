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
            const string s = "12345678";
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
            var j = false;
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
                    var d = t.Lookup(n).Bounds;
                    var k = R.FromLTRB(d.XMin * c, d.YMin * c, d.XMax * c, d.YMax * c);
                    g.TranslateTransform(x * c, y * c);
                    b.Build(s[i], z);
                    b.ReadShapes(r);
                    r.ResultGraphicsPath.CloseFigure();
                    g.FillPath(h, r.ResultGraphicsPath);
                    if(j) g.DrawRectangle(u, 0, 0, o.width, o.ascending - o.descending);
                    else g.DrawRectangle(v, k.X, k.Y, k.Width, k.Height);
                    g.Restore(a);
                    g.TranslateTransform(w * c, 0);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace All_Reg_and_Voted
{
    public class Reg_Voted_Renderer
    {
        private int width = 0, height = 0;
        private double totHeight = 750;
        private Bitmap bmp = null;
        private Graphics gfx = null;

        private List<Reg_Voted_Model> data2020 = null;
        private List<Reg_Voted_Model> data2016 = null;
        private List<Reg_Voted_Model> data2012 = null;
        private List<Reg_Voted_Model> data2008 = null;

        Image logoImg = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "assets", "logo.png"));
        Image cancelImg = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "assets", "cancel.png"));
        public Reg_Voted_Renderer(int width , int height)
        {
            this.width = width;
            this.height = height;
        }
        public int getData2020Count()
        {
            if (this.data2020 == null) return 0;
            else return this.data2020.Count;
        }
        public int getData2016Count()
        {
            if (this.data2016 == null) return 0;
            else return this.data2016.Count;
        }
        public int getData2012Count()
        {
            if (this.data2012 == null) return 0;
            else return this.data2012.Count;
        }

        public int getData2008Count()
        {
            if (this.data2008 == null) return 0;
            else return this.data2008.Count;
        }
        public void setData(List<Reg_Voted_Model> data2020, List<Reg_Voted_Model> data2016, List<Reg_Voted_Model> data2012, List<Reg_Voted_Model> data2008)
        {
            this.data2020 = data2020;
            this.data2016 = data2016;
            this.data2012 = data2012;
            this.data2008 = data2008;
        }
        public void setRenderSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        public Point convertCoord(Point a)
        {
            double px = height / totHeight;

            Point res = new Point();
            res.X = (int)((a.X + 20) * px);
            res.Y = (int)((totHeight - a.Y) * px);
            return res;
        }
        public PointF convertCoord(PointF p)
        {
            double px = height / totHeight;
            PointF res = new PointF();
            res.X = (int)((p.X + 20) * px);
            res.Y = (int)((totHeight - p.Y) * px);
            return res;
        }

        public Bitmap getBmp()
        {
            return this.bmp;
        }

        public void drawCenteredString_withBorder(string content, Rectangle rect, Brush brush, Font font, Color borderColor)
        {

            //using (Font font1 = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Point))

            // Create a StringFormat object with the each line of text, and the block
            // of text centered on the page.
            double px = height / totHeight;
            rect.Location = convertCoord(rect.Location);
            rect.Width = (int)(px * rect.Width);
            rect.Height = (int)(px * rect.Height);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            // Draw the text and the surrounding rectangle.
            gfx.DrawString(content, font, brush, rect, stringFormat);

            Pen borderPen = new Pen(new SolidBrush(borderColor), 2);
            gfx.DrawRectangle(borderPen, rect);
            borderPen.Dispose();
        }
        public void drawCenteredImg_withBorder(Image img, Rectangle rect, Brush brush, Font font, Color borderColor)
        {
            double px = height / totHeight;
            rect.Location = convertCoord(rect.Location);
            rect.Width = (int)(px * rect.Width);
            rect.Height = (int)(px * rect.Height);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            // Draw the text and the surrounding rectangle.
            //gfx.DrawString(content, font, brush, rect, stringFormat);
            //drawImg(logoImg, new Point(20, 60), new Size(150, 50));
            gfx.DrawImage(img, rect);
            Pen borderPen = new Pen(new SolidBrush(borderColor), 2);
            gfx.DrawRectangle(borderPen, rect);
            borderPen.Dispose();
        }
        public void drawCenteredString(string content, Rectangle rect, Brush brush, Font font)
        {

            //using (Font font1 = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Point))

            // Create a StringFormat object with the each line of text, and the block
            // of text centered on the page.
            double px = height / totHeight;
            rect.Location = convertCoord(rect.Location);
            rect.Width = (int)(px * rect.Width);
            rect.Height = (int)(px * rect.Height);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            // Draw the text and the surrounding rectangle.
            gfx.DrawString(content, font, brush, rect, stringFormat);
            //gfx.DrawRectangle(Pens.Black, rect);

        }
        private void fillPolygon(Brush brush, PointF[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = convertCoord(points[i]);
            }
            gfx.FillPolygon(brush, points);
        }
        public void drawLine(Point p1, Point p2, Color color, int linethickness = 1)
        {
            if (color == null)
                color = Color.Gray;

            p1 = convertCoord(p1);
            p2 = convertCoord(p2);
            gfx.DrawLine(new Pen(color, linethickness), p1, p2);

        }
        public void drawString(Font font, Color brushColor, string content, Point o)
        {
            o = convertCoord(o);
            SolidBrush drawBrush = new SolidBrush(brushColor);
            gfx.DrawString(content, font, drawBrush, o.X, o.Y);
        }
        public void drawString(Point o, string content, int font = 15)
        {

            o = convertCoord(o);

            // Create font and brush.
            Font drawFont = new Font("Arial", font);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            gfx.DrawString(content, drawFont, drawBrush, o.X, o.Y);

        }
        public void drawString(Color color, Point o, string content, int font = 15)
        {

            o = convertCoord(o);

            // Create font and brush.
            Font drawFont = new Font("Arial", font);
            SolidBrush drawBrush = new SolidBrush(color);

            gfx.DrawString(content, drawFont, drawBrush, o.X, o.Y);

            drawFont.Dispose();
            drawBrush.Dispose();

        }
        public void fillRectangle(Color color, Rectangle rect)
        {
            rect.Location = convertCoord(rect.Location);
            double px = height / totHeight;
            rect.Width = (int)(rect.Width * px);
            rect.Height = (int)(rect.Height * px);

            Brush brush = new SolidBrush(color);
            gfx.FillRectangle(brush, rect);
            brush.Dispose();

        }
        public void drawRectangle(Pen pen, Rectangle rect)
        {
            rect.Location = convertCoord(rect.Location);
            double px = height / totHeight;
            rect.Width = (int)(rect.Width * px);
            rect.Height = (int)(rect.Height * px);
            gfx.DrawRectangle(pen, rect);
        }
        public void drawImg(Image img, Point o, Size size)
        {
            double px = height / totHeight;
            o = convertCoord(o);
            Rectangle rect = new Rectangle(o, new Size((int)(size.Width * px), (int)(size.Height * px)));
            gfx.DrawImage(img, rect);

        }
        public void drawPie(Color color, Point o, Size size, float startAngle, float sweepAngle, string content = "")
        {
            // Create location and size of ellipse.
            double px = height / totHeight;
            size.Width = (int)(size.Width * px);
            size.Height = (int)(size.Height * px);

            Rectangle rect = new Rectangle(convertCoord(o), size);
            // Draw pie to screen.            
            Brush grayBrush = new SolidBrush(color);
            gfx.FillPie(grayBrush, rect, startAngle, sweepAngle);

            o.X += size.Width / 2;
            o.Y -= size.Height / 2;
            float radius = size.Width * 0.3f;
            o.X += (int)(radius * Math.Cos(Helper.DegreesToRadians(startAngle + sweepAngle / 2)));
            o.Y -= (int)(radius * Math.Sin(Helper.DegreesToRadians(startAngle + sweepAngle / 2)));
            content += "\n" + string.Format("{0:F}%", sweepAngle * 100.0f / 360.0f);
            drawString(o, content, 9);
        }
        public void drawFilledCircle(Brush brush, Point o, Size size)
        {
            double px = height / totHeight;
            size.Width = (int)(size.Width * px);
            size.Height = (int)(size.Height * px);

            Rectangle rect = new Rectangle(convertCoord(o), size);

            gfx.FillEllipse(brush, rect);
        }

        public string  draw(string precinct)
        {
            if (bmp == null)
                bmp = new Bitmap(width, height);
            else
            {
                if (bmp.Width != width || bmp.Height != height)
                {
                    bmp.Dispose();
                    bmp = new Bitmap(width, height);

                    gfx.Dispose();
                    gfx = Graphics.FromImage(bmp);
                    gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                }
            }

            if (gfx == null)
            {
                gfx = Graphics.FromImage(bmp);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            }
            else
            {
                gfx.Clear(Color.Transparent);
            }
            drawImg(logoImg, new Point(20, 60), new Size(150, 50));


            if (data2020 == null || data2016 == null || data2012 == null || data2008 == null) return "";
            if (precinct == "" && data2020.Count > 0)
            {
                precinct = data2020[0].precinct_name;
            }
            Reg_Voted_Model precinct2020 = data2020.Find(item => item.precinct_name.ToLower() == precinct.ToLower());
            Reg_Voted_Model precinct2016 = data2016.Find(item => item.precinct_name.ToLower() == precinct.ToLower());
            Reg_Voted_Model precinct2012 = data2012.Find(item => item.precinct_name.ToLower() == precinct.ToLower());
            Reg_Voted_Model precinct2008 = data2008.Find(item => item.precinct_name.ToLower() == precinct.ToLower());



            //Draw
            Font textFont12 = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Point);
            Font textFont10 = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point);

            Font titleFont = new Font("Arial", 25, FontStyle.Bold, GraphicsUnit.Point);
            Font bigNumFont = new Font("Arial", 40, FontStyle.Bold, GraphicsUnit.Point);
            Font h5font = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Point);
            Font h6font = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);
            Font h7font = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point);

            Brush redBrush = new SolidBrush(Color.Red);
            Brush greenBrush = new SolidBrush(Color.LimeGreen);
            Brush blackBrush = new SolidBrush(Color.Black);
            Brush whiteBrush = new SolidBrush(Color.White);
            Brush aquaBrush = new SolidBrush(Color.Aqua);


            drawString(titleFont, Color.Black, "Precinct Growth Visualization", new Point(0, 730));
            string chartname = "";
            chartname = (precinct2008 != null ? precinct2008.number : "xxxx" )  + "-";
            chartname += (precinct2012 != null ? precinct2012.number : "xxxx") + "-";
            chartname += (precinct2016 != null ? precinct2016.number : "xxxx") + "-";
            chartname += (precinct2020 != null ? precinct2020.number : "xxxx") + " " + precinct;
            drawString(h5font, Color.Black, chartname, new Point(750, 710));

            drawCenteredString("TURN OUT", new Rectangle(790, 650, 120, 50), blackBrush, h7font);
            drawCenteredString("GROWTH", new Rectangle(910, 650, 120, 50), blackBrush, h7font);
            drawCenteredString("ACTIVATION", new Rectangle(1030, 650, 120, 50), blackBrush, h7font);
            drawCenteredString("RATING", new Rectangle(1180, 650, 90, 50), blackBrush, h7font);



            //Draw Year 2008

            drawString(bigNumFont, Color.Black, "08", new Point(0, 600));
            drawString(h6font, Color.Black, "REGISTERED", new Point(100, 590));
            drawString(h6font, Color.Black, "VOTED", new Point(100, 540));

            int len = 0, max = 0;
            if (precinct2008 !=null)
            {
                max = Math.Max(precinct2008.reg_v, precinct2008.voted);
                if (max == 0) len = 0;
                else
                {
                    len = precinct2008.reg_v * 400 / max;
                }
                fillRectangle(Color.MediumSlateBlue, new Rectangle(250, 600, len, 45));
                if (max == 0) len = 0;
                else
                {
                    len = precinct2008.voted * 400 / max;
                }
                fillRectangle(Color.LimeGreen, new Rectangle(250, 550, len, 45));
            } else
            {
                fillRectangle(Color.MediumSlateBlue, new Rectangle(250, 600, 0, 45));
                fillRectangle(Color.LimeGreen, new Rectangle(250, 550, 0, 45));
            }

            drawString(h5font, Color.Black, (precinct2008 !=null) ? precinct2008.reg_v.ToString() : "", new Point(700, 590));
            drawString(h5font, Color.Black, (precinct2008 != null) ? precinct2008.voted.ToString() : "", new Point(700, 540));

            if (precinct2008 != null)
            {
                double  percent = 0;
                if (precinct2008.reg_v == 0) percent = 0;
                else percent = Math.Round( precinct2008.voted * 100 / (double)precinct2008.reg_v, 2);
                if (percent > 0)
                    drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(800, 600, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                else drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(800, 600, 100, 100), redBrush, textFont12, Color.Black);
                drawCenteredImg_withBorder(cancelImg, new Rectangle(920, 600, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                drawCenteredImg_withBorder(cancelImg, new Rectangle(1040, 600, 100, 100), blackBrush, textFont12, Color.LimeGreen);

            } else
            {
                drawCenteredString_withBorder("", new Rectangle(800, 600, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                drawCenteredImg_withBorder(cancelImg, new Rectangle(920, 600, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                drawCenteredImg_withBorder(cancelImg, new Rectangle(1040, 600, 100, 100), blackBrush, textFont12, Color.LimeGreen);
            }
            fillRectangle(Color.Black, new Rectangle(1200, 600, 45, 45));
            drawCenteredString("08", new Rectangle(1200, 600, 45, 45), whiteBrush, h5font);
            fillRectangle(Color.Black, new Rectangle(1200, 550, 45, 45));

            if (precinct2008 !=null)
            {
                if (precinct2008.result == "DEM")
                    drawCenteredString("D", new Rectangle(1200, 550, 45, 45), aquaBrush, h5font);
                else if (precinct2008.result == "REP")
                    drawCenteredString("R", new Rectangle(1200, 550, 45, 45), redBrush, h5font);
                else
                    drawCenteredString("", new Rectangle(1200, 550, 45, 45), aquaBrush, h5font);
            } else
            {
                drawCenteredString("", new Rectangle(1200, 550, 45, 45), aquaBrush, h5font);
            }



            //Draw Year 2012

            drawString(bigNumFont, Color.Black, "12", new Point(0, 450));
            drawString(h6font, Color.Black, "REGISTERED", new Point(100, 440));
            drawString(h6font, Color.Black, "VOTED", new Point(100, 390));

            if (precinct2012 != null)
            {
                max = Math.Max(precinct2012.reg_v, precinct2012.voted);
                if (max == 0) len = 0;
                else
                {
                    len = precinct2012.reg_v * 400 / max;
                }
                fillRectangle(Color.MediumSlateBlue, new Rectangle(250, 450, len, 45));
                if (max == 0) len = 0;
                else
                {
                    len = precinct2012.voted * 400 / max;
                }
                fillRectangle(Color.LimeGreen, new Rectangle(250, 400, len, 45));
            }
            else
            {
                fillRectangle(Color.MediumSlateBlue, new Rectangle(250, 450, 0, 45));
                fillRectangle(Color.LimeGreen, new Rectangle(250, 400, 0, 45));
            }

            drawString(h5font, Color.Black, (precinct2012 != null) ? precinct2012.reg_v.ToString() : "", new Point(700, 440));
            drawString(h5font, Color.Black, (precinct2012 != null) ? precinct2012.voted.ToString() : "", new Point(700, 390));

            if (precinct2012 != null)
            {
                double percent = 0;
                if (precinct2012.reg_v == 0) percent = 0;
                else percent = Math.Round(precinct2012.voted * 100 / (float)precinct2012.reg_v, 2);

                if (percent > 0)
                    drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(800, 450, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                else drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(800, 450, 100, 100), redBrush, textFont12, Color.Black);

                percent = 0;
                if (precinct2008 !=null)
                {
                    if (precinct2008.reg_v != 0)
                    {
                        percent = Math.Round(precinct2012.reg_v * 100 / (double)precinct2008.reg_v - 100, 2);
                        drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(920, 450, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                    }
                    else
                    {
                        drawCenteredImg_withBorder(cancelImg, new Rectangle(920, 450, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                    }
                } else
                {
                    drawCenteredImg_withBorder(cancelImg, new Rectangle(920, 450, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                }

                percent = 0;
                if (precinct2008 != null)
                {
                    if (precinct2008.reg_v != 0)
                    {
                        percent = Math.Round(precinct2012.voted * 100 / (double)precinct2008.voted - 100, 2);
                        if (percent > 0)
                            drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(1040, 450, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                        else drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(1040, 450, 100, 100), redBrush, textFont12, Color.Black);
                    }
                    else
                    {
                        drawCenteredImg_withBorder(cancelImg, new Rectangle(1040, 450, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                    }
                }
                else
                {
                    drawCenteredImg_withBorder(cancelImg, new Rectangle(1040, 450, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                }

            }
            else
            {
                drawCenteredString_withBorder("", new Rectangle(800, 450, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                drawCenteredImg_withBorder(cancelImg, new Rectangle(920, 450, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                drawCenteredImg_withBorder(cancelImg, new Rectangle(1040, 450, 100, 100), blackBrush, textFont12, Color.LimeGreen);
            }



            fillRectangle(Color.Black, new Rectangle(1200, 450, 45, 45));
            drawCenteredString("12", new Rectangle(1200, 450, 45, 45), whiteBrush, h5font);
            fillRectangle(Color.Black, new Rectangle(1200, 400, 45, 45));

            if (precinct2012 != null)
            {
                if (precinct2012.result == "DEM")
                    drawCenteredString("D", new Rectangle(1200, 400, 45, 45), aquaBrush, h5font);
                else if (precinct2012.result == "REP")
                    drawCenteredString("R", new Rectangle(1200, 400, 45, 45), redBrush, h5font);
                else
                    drawCenteredString("", new Rectangle(1200, 400, 45, 45), aquaBrush, h5font);
            }
            else
            {
                drawCenteredString("", new Rectangle(1200, 400, 45, 45), aquaBrush, h5font);
            }











            //Draw Year 2016

            drawString(bigNumFont, Color.Black, "16", new Point(0, 300));
            drawString(h6font, Color.Black, "REGISTERED", new Point(100, 290));
            drawString(h6font, Color.Black, "VOTED", new Point(100, 240));

            if (precinct2016 != null)
            {
                max = Math.Max(precinct2016.reg_v, precinct2016.voted);
                if (max == 0) len = 0;
                else
                {
                    len = precinct2016.reg_v * 400 / max;
                }
                fillRectangle(Color.MediumSlateBlue, new Rectangle(250, 300, len, 45));
                if (max == 0) len = 0;
                else
                {
                    len = precinct2016.voted * 400 / max;
                }
                fillRectangle(Color.LimeGreen, new Rectangle(250, 250, len, 45));
            }
            else
            {
                fillRectangle(Color.MediumSlateBlue, new Rectangle(250, 300, 0, 45));
                fillRectangle(Color.LimeGreen, new Rectangle(250, 250, 0, 45));
            }

            drawString(h5font, Color.Black, (precinct2016 != null) ? precinct2016.reg_v.ToString() : "", new Point(700, 290));
            drawString(h5font, Color.Black, (precinct2016 != null) ? precinct2016.voted.ToString() : "", new Point(700, 240));

            if (precinct2016 != null)
            {
                double percent = 0;
                if (precinct2016.reg_v == 0) percent = 0;
                else percent = Math.Round(precinct2016.voted * 100 / (float)precinct2016.reg_v, 2);

                if (percent > 0)
                    drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(800, 300, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                else drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(800, 300, 100, 100), redBrush, textFont12, Color.Black);

                percent = 0;
                if (precinct2012 != null)
                {
                    if (precinct2012.reg_v != 0)
                    {
                        percent = Math.Round(precinct2016.reg_v * 100 / (double)precinct2012.reg_v - 100, 2);
                        drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(920, 300, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                    }
                    else
                    {
                        drawCenteredImg_withBorder(cancelImg, new Rectangle(920, 300, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                    }
                }
                else
                {
                    drawCenteredImg_withBorder(cancelImg, new Rectangle(920, 300, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                }

                percent = 0;
                if (precinct2012 != null)
                {
                    if (precinct2012.reg_v != 0)
                    {
                        percent = Math.Round(precinct2016.voted * 100 / (double)precinct2012.voted - 100, 2);
                        if (percent > 0)
                            drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(1040, 300, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                        else drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(1040, 300, 100, 100), redBrush, textFont12, Color.Black);
                    }
                    else
                    {
                        drawCenteredImg_withBorder(cancelImg, new Rectangle(1040, 300, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                    }
                }
                else
                {
                    drawCenteredImg_withBorder(cancelImg, new Rectangle(1040, 300, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                }

            }
            else
            {
                drawCenteredString_withBorder("", new Rectangle(800, 300, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                drawCenteredImg_withBorder(cancelImg, new Rectangle(920, 300, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                drawCenteredImg_withBorder(cancelImg, new Rectangle(1040, 300, 100, 100), blackBrush, textFont12, Color.LimeGreen);
            }

            fillRectangle(Color.Black, new Rectangle(1200, 300, 45, 45));
            drawCenteredString("16", new Rectangle(1200, 300, 45, 45), whiteBrush, h5font);
            fillRectangle(Color.Black, new Rectangle(1200, 250, 45, 45));

            if (precinct2016 != null)
            {
                if (precinct2016.result == "DEM")
                    drawCenteredString("D", new Rectangle(1200, 250, 45, 45), aquaBrush, h5font);
                else if (precinct2016.result == "REP")
                    drawCenteredString("R", new Rectangle(1200, 250, 45, 45), redBrush, h5font);
                else
                    drawCenteredString("", new Rectangle(1200, 250, 45, 45), aquaBrush, h5font);
            }
            else
            {
                drawCenteredString("", new Rectangle(1200, 250, 45, 45), aquaBrush, h5font);
            }




            //Draw 2020
            drawString(bigNumFont, Color.Black, "20", new Point(0, 150));
            drawString(h6font, Color.Black, "REGISTERED", new Point(100, 140));
            drawString(h6font, Color.Black, "VOTED", new Point(100, 90));

            if (precinct2020 != null)
            {
                max = Math.Max(precinct2020.reg_v, precinct2020.voted);
                if (max == 0) len = 0;
                else
                {
                    len = precinct2020.reg_v * 400 / max;
                }
                fillRectangle(Color.MediumSlateBlue, new Rectangle(250, 150, len, 45));
                if (max == 0) len = 0;
                else
                {
                    len = precinct2020.voted * 400 / max;
                }
                fillRectangle(Color.LimeGreen, new Rectangle(250, 100, len, 45));
            }
            else
            {
                fillRectangle(Color.MediumSlateBlue, new Rectangle(250, 150, 0, 45));
                fillRectangle(Color.LimeGreen, new Rectangle(250, 100, 0, 45));
            }

            drawString(h5font, Color.Black, (precinct2020 != null) ? precinct2020.reg_v.ToString() : "", new Point(700, 140));
            drawString(h5font, Color.Black, (precinct2020 != null) ? precinct2020.voted.ToString() : "", new Point(700, 90));

            if (precinct2020 != null)
            {
                double percent = 0;
                if (precinct2020.reg_v == 0) percent = 0;
                else percent = Math.Round(precinct2020.voted * 100 / (float)precinct2020.reg_v, 2);

                if (percent > 0)
                    drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(800, 150, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                else drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(800, 150, 100, 100), redBrush, textFont12, Color.Black);

                percent = 0;
                if (precinct2016 != null)
                {
                    if (precinct2016.reg_v != 0)
                    {
                        percent = Math.Round(precinct2020.reg_v * 100 / (double)precinct2016.reg_v - 100, 2);
                        drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(920, 150, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                    }
                    else
                    {
                        drawCenteredImg_withBorder(cancelImg, new Rectangle(920, 150, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                    }
                }
                else
                {
                    drawCenteredImg_withBorder(cancelImg, new Rectangle(920, 150, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                }

                percent = 0;
                if (precinct2016 != null)
                {
                    if (precinct2016.reg_v != 0)
                    {
                        percent = Math.Round(precinct2020.voted * 100 / (double)precinct2016.voted - 100, 2);
                        if (percent > 0)
                            drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(1040, 150, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                        else drawCenteredString_withBorder(percent.ToString() + "%", new Rectangle(1040, 150, 100, 100), redBrush, textFont12, Color.Black);
                    }
                    else
                    {
                        drawCenteredImg_withBorder(cancelImg, new Rectangle(1040, 150, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                    }
                }
                else
                {
                    drawCenteredImg_withBorder(cancelImg, new Rectangle(1040, 150, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                }

            }
            else
            {
                drawCenteredString_withBorder("", new Rectangle(800, 150, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                drawCenteredImg_withBorder(cancelImg, new Rectangle(920, 150, 100, 100), blackBrush, textFont12, Color.LimeGreen);
                drawCenteredImg_withBorder(cancelImg, new Rectangle(1040, 150, 100, 100), blackBrush, textFont12, Color.LimeGreen);
            }

            fillRectangle(Color.Black, new Rectangle(1200, 150, 45, 45));
            drawCenteredString("20", new Rectangle(1200, 150, 45, 45), whiteBrush, h5font);
            fillRectangle(Color.Black, new Rectangle(1200, 100, 45, 45));

            if (precinct2016 != null)
            {
                if (precinct2016.result == "DEM")
                    drawCenteredString("D", new Rectangle(1200, 100, 45, 45), aquaBrush, h5font);
                else if (precinct2016.result == "REP")
                    drawCenteredString("R", new Rectangle(1200, 100, 45, 45), redBrush, h5font);
                else
                    drawCenteredString("", new Rectangle(1200, 100, 45, 45), aquaBrush, h5font);
            }
            else
            {
                drawCenteredString("", new Rectangle(1200, 100, 45, 45), aquaBrush, h5font);
            }





            redBrush.Dispose();
            greenBrush.Dispose();
            blackBrush.Dispose();
            whiteBrush.Dispose();
            aquaBrush.Dispose();



            textFont12.Dispose();
            textFont10.Dispose();
            bigNumFont.Dispose();
            titleFont.Dispose();
            h5font.Dispose();
            h6font.Dispose();
            h7font.Dispose();


            return chartname;
        }
    }
}

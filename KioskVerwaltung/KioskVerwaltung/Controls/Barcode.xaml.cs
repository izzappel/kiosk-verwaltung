using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace KioskVerwaltung
{
    public enum BarcodeEnum
    {
        Code39
    }


    /// <summary>
    /// Interaktionslogik für Barcode.xaml
    /// </summary>
    public partial class Barcode : UserControl
    {
        public string Data 
        {
            get { return (String)this.GetValue(DataProperty); }
            set { this.SetValue(DataProperty, value); }
        }
        public BarcodeEnum BarcodeType { get; set; }
        public bool HasCheckDigit { get; set; }
        public String HumanText
        {
            get { return (String)this.GetValue(HumanTextProperty); }
            set { this.SetValue(HumanTextProperty, value); }
        }
        public String EncodedData
        {
            get { return (String)this.GetValue(EncodeDataProperty); }
            set { this.SetValue(EncodeDataProperty, value); }
        }
        public static readonly DependencyProperty EncodeDataProperty = DependencyProperty.Register(
            "EncodedData", typeof(String), typeof(Barcode), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty HumanTextProperty = DependencyProperty.Register(
            "HumanText", typeof(String), typeof(Barcode), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data", typeof(String), typeof(Barcode), new PropertyMetadata(string.Empty));

        public Barcode()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Encode();
            Draw(drawingContext);
        }

        public void Encode()
        {
            if (!String.IsNullOrEmpty(Data))
            {
                if (BarcodeType == BarcodeEnum.Code39)
                {
                    Code39 barcode = new Code39();
                    EncodedData = barcode.Encode(Data, HasCheckDigit);
                    HumanText = barcode.HumanText;

                }
                Draw();
            }
        }

        public void Draw()
        {
            MainCanvas.Children.Clear();

            int thinWidth = (int)Math.Round(ActualWidth / (CountThinBars() + 3 * CountThickBars()));
            int thickWidth = 3 * thinWidth;

            int tbHeight = 35;

            /////////////////////////////////////
            // Draw The Barcode
            /////////////////////////////////////
            int len = EncodedData.Length;
            int currentPos = 0;
            int currentTop = 0;
            for (int i = 0; i < len; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Height = ActualHeight - tbHeight;
                if (i % 2 == 0) { rect.Fill = new SolidColorBrush(Colors.Black); }
                else { rect.Fill = new SolidColorBrush(Colors.White); }

                Canvas.SetLeft(rect, currentPos);
                Canvas.SetTop(rect, currentTop);

                if (EncodedData[i] == 't')
                {
                    rect.Width = thinWidth;
                    currentPos += thinWidth;

                }
                else if (EncodedData[i] == 'w')
                {
                    rect.Width = thickWidth;
                    currentPos += thickWidth;

                }
                MainCanvas.Children.Add(rect);
            }

            /////////////////////////////////////
            // Add the Human Readable Text
            /////////////////////////////////////
            TextBlock tb = new TextBlock();
            tb.Text = HumanText;
            tb.Height = tbHeight;
            tb.FontFamily = new FontFamily("Courier New");
            Rect rx = new Rect(0, 0, 0, 0);
            tb.Arrange(rx);
            Canvas.SetLeft(tb, (currentPos - tb.ActualWidth) / 2);
            Canvas.SetTop(tb, currentTop + ActualHeight - tbHeight);
            MainCanvas.Children.Add(tb);

        }

        private void Draw(DrawingContext drawingContext)
        {
            double thinWidth = ActualWidth / (CountThinBars() + 3 * CountThickBars());
            double thickWidth = 3 * thinWidth;


            int textHeight = 35;
            int len = EncodedData.Length;
            double currentPos = 0;
            double currentTop = 0;
            for (int i = 0; i < len; i++)
            {
                System.Windows.Rect rect = new System.Windows.Rect();
                rect.Height = ActualHeight - textHeight;
                Brush brush = new SolidColorBrush(Colors.White);
                if (i % 2 == 0) { brush = new SolidColorBrush(Colors.Black); }

                rect.Location = new Point(currentPos, currentTop);

                if (EncodedData[i] == 't')
                {
                    rect.Width = thinWidth;
                    currentPos += thinWidth;

                }
                else if (EncodedData[i] == 'w')
                {
                    rect.Width = thickWidth;
                    currentPos += thickWidth;

                }
                drawingContext.DrawRectangle(brush, null, rect);
            }

            Brush textBrush = new SolidColorBrush(Colors.Black);
            Typeface typeface = new Typeface("Arial");
            FormattedText formattedText = new FormattedText(Data, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 12, textBrush);
            Point position = new Point((currentPos - formattedText.Width) / 2, currentTop + ActualHeight - textHeight);
            drawingContext.DrawText(formattedText, position);
        }


        private int CountThinBars()
        {
            int count = 0;
            foreach (var item in EncodedData)
            {
                if (item.Equals(Code39.ThinBar)) { count++; }
            }
            return count;
        }
        private int CountThickBars()
        {
            int count = 0;
            foreach (var item in EncodedData)
            {
                if (item.Equals(Code39.ThickBar)) { count++; }
            }
            return count;
        }

        public void GenerateBarcode()
        {
            string data = "";
            int count = 6;
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                data += random.Next(0, 9).ToString();
            }
            /////////////////////////////////////
            // Encode The Data
            /////////////////////////////////////
            this.BarcodeType = BarcodeEnum.Code39;
            this.Data = data;
            this.HasCheckDigit = true;
        }
    }
}

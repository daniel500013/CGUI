﻿using System;
using System.Drawing;
using System.Text;
using Cosmos.System.Graphics;

namespace CGUI
{
    /// <summary>
    /// The Line control.
    /// </summary>
    public class Line : Control
    {
        internal int EndX { get; set; }
        internal int EndY { get; set; }
        /// <summary>
        /// The line's color.
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// Starts a new instance of the Line class.
        /// </summary>
        /// <param name="color">The color of the line.</param>
        /// <param name="startx">The start X coordinate.</param>
        /// <param name="starty">The start Y coordinate.</param>
        /// <param name="endx">The ending X coordinate.</param>
        /// <param name="endy">The ending Y coordinate.</param>
        public Line(Color color, int startx, int starty, int endx, int endy)
        {
            controlType = ControlType.Line;
            Color = color;
            X = startx;
            Y = starty;
            EndX = endx;
            EndY = endy;
        }
        /// <summary>
        /// Updates the line.
        /// </summary>
        public void Update()
        {
            VGADriver.driver.DoubleBuffer_DrawLine((uint)Color.ToArgb(), X, Y, EndX, EndY);
            VGADriver.driver.DoubleBuffer_Update();
        }
    }
    /// <summary>
    /// The Label control.
    /// </summary>
    public class Label : Control
    {
        /// <summary>
        /// The label's text.
        /// </summary>
        public string Text { get; }
        /// <summary>
        /// The text color of the label.
        /// </summary>
        public Color foreColor { get; set; }
        internal Color backColor { get; set; } = Color.Empty;
        private string prevText;
        /// <summary>
        /// Starts a new instance of the Label class.
        /// </summary>
        /// <param name="text">The label text.</param>
        /// <param name="foreColor">The text color.</param>
        /// <param name="x">The label's X coordinate.</param>
        /// <param name="y">The label's Y coordinate.</param>
        public Label(string text, Color foreColor, int x, int y)
        {
            controlType = ControlType.Label;
            X = x;
            Y = y;
            Text = text;
            prevText = text;
            this.foreColor = foreColor;
        }
        /// <summary>
        /// Starts a new instance of the Label class.
        /// </summary>
        /// <param name="text">The label text in the default color (white).</param>
        /// <param name="x">The label's X coordinate.</param>
        /// <param name="y">The label's Y coordinate.</param>
        public Label(string text, int x, int y)
        {
            controlType = ControlType.Label;
            X = x;
            Y = y;
            Text = text;
            prevText = text;
            foreColor = Color.White;
        }
        /// <summary>
        /// Starts a new instance of the Label class.
        /// </summary>
        /// <param name="text">The label text in the default color (white).</param>
        /// <param name="point">The point to place the label.</param>
        public Label(string text, Cosmos.System.Graphics.Point point)
        {
            controlType = ControlType.Label;
            X = point.X;
            Y = point.Y;
            Text = text;
            prevText = text;
            foreColor = Color.White;
        }
        /// <summary>
        /// Updates/changes the label's text.
        /// </summary>
        /// <param name="newText">The new text to display.</param>
        public void Update(string newText)
        {
            VGADriver.driver.DoubleBuffer_DrawFillRectangle((uint)X, (uint)Y, (uint)(prevText.Length * 8) + 4, 15, (uint)Internal.screenColor.ToArgb());
            VGADriver.driver.DoubleBuffer_Update();
            VGADriver.driver._DrawACSIIString(Text, (uint)foreColor.ToArgb(), (uint)X, (uint)Y);
            prevText = Text;
            VGADriver.driver.DoubleBuffer_Update();
        }
    }
    /// <summary>
    /// The Picture control.
    /// </summary>
    public class Picture : Control
    {
        /// <summary>
        /// The control's image.
        /// </summary>
        public Image Image { get; set; }
        /// <summary>
        /// The border color around the image. This is off by default.
        /// </summary>
        public Color borderColor { get; set; } = Color.Empty;
        internal Image prevImage;
        /// <summary>
        /// Starts a new instance of the Picture class.
        /// </summary>
        /// <param name="image">The image to use.</param>
        /// <param name="x">The picture's X coordinate.</param>
        /// <param name="y">The picture's Y coordinate.</param>
        public Picture(Image image, int x, int y)
        {
            Image = image;
            prevImage = image;
            X = x;
            Y = y;
        }
        /// <summary>
        /// Updates the picture.
        /// </summary>
        public void Update()
        {
            VGADriver.driver.DoubleBuffer_DrawFillRectangle((uint)X, (uint)Y, prevImage.Width, prevImage.Height, (uint)Internal.screenColor.ToArgb());
            VGADriver.driver.DoubleBuffer_Update();
            VGADriver.driver.DoubleBuffer_DrawImage(Image, (uint)X, (uint)Y);
            prevImage = Image;
            VGADriver.driver.DoubleBuffer_Update();

            if (borderColor != Color.Empty)
                VGADriver.driver.DoubleBuffer_DrawLine((uint)borderColor.ToArgb(), X - 1, Y - 1, X + (int)Image.Width + 1, Y - 1);
                VGADriver.driver.DoubleBuffer_DrawLine((uint)borderColor.ToArgb(), X - 1, Y - 1, X - 1, Y + (int)Image.Height + 1);
                VGADriver.driver.DoubleBuffer_DrawLine((uint)borderColor.ToArgb(), X - 1, Y + (int)Image.Height + 1, X + (int)Image.Width + 1, Y + (int)Image.Height + 1);
                VGADriver.driver.DoubleBuffer_DrawLine((uint)borderColor.ToArgb(), X + (int)Image.Width + 1, Y - 1, X + (int)Image.Width + 1, Y + (int)Image.Height + 1);
                VGADriver.driver.DoubleBuffer_Update();
        }
    }
    /// <summary>
    /// The TextBox control.
    /// </summary>
    public class TextBox : Control
    {
        /// <summary>
        /// The foreground color of the textbox when it is not in focus.
        /// </summary>
        public Color UnfocusForeColor { get; set; } = Color.White;
        /// <summary>
        /// The background color of the textbox when it is not in focus.
        /// </summary>
        public Color UnfocusBackColor { get; set; } = Color.DarkBlue;
        /// <summary>
        /// The text color for the textbox (including the cursor).
        /// </summary>
        public Color ForeColor { get; set; } = Color.White;
        /// <summary>
        /// The background color for the textbox.
        /// </summary>
        public Color BackColor { get; set; } = Color.Blue;
        /// <summary>
        /// The character(s) that are allowed in the textbox. Set this to blank ("") to disable and allow all characters.
        /// </summary>
        public string Filter { get; set; } = "";
        /// <summary>
        /// The character to draw on the screen instead of the actual character. Set this to '~' to disable.
        /// </summary>
        public char Mask { get; set; } = '~';
        /// <summary>
        /// Determines whether or not to beep when attempting to type outside of the textbox's character limit.
        /// </summary>
        public bool BeepOnLimit { get; set; } = false;
        /// <summary>
        /// The current text in the textbox.
        /// </summary>
        public string Text
        {
            get
            {
                return txt.ToString();
            }
        }
        /// <summary>
        /// Raised when the user attempts to delete or enter characters outside of the character limit.
        /// </summary>
        public event EventHandler OnCharacterLimit
        {
            add
            {
                CharacterLimit_Handler = value;
            }
            remove
            {
                CharacterLimit_Handler -= value;
            }
        }
        internal StringBuilder txt = new StringBuilder("");
        internal int FocusOrder;
        internal int cLength;
        internal EventHandler CharacterLimit_Handler;
        /// <summary>
        /// Starts a new instance of the TextBox class.
        /// </summary>
        /// <param name="charLength">The number of characters allowed in the textbox.</param>
        /// <param name="foreColor">The text color.</param>
        /// <param name="x">The textbox's X coordinate.</param>
        /// <param name="y">The textbox's Y coordinate.</param>
        public TextBox(int charLength, Color foreColor, int x, int y)
        {
            controlType = ControlType.TextBox;
            cLength = charLength;
            this.ForeColor = foreColor;
            X = x;
            Y = y;
        }
        /// <summary>
        /// Starts a new instance of the TextBox class.
        /// </summary>
        /// <param name="charLength">The number of characters allowed in the textbox.</param>
        /// <param name="x">The textbox's X coordinate.</param>
        /// <param name="y">The textbox's Y coordinate.</param>
        public TextBox(int charLength, int x, int y)
        {
            controlType = ControlType.TextBox;
            cLength = charLength;
            X = x;
            Y = y;
        }
    }
    /// <summary>
    /// The Button control.
    /// </summary>
    public class Button : Control
    {
        internal EventHandler OnEnter_Handler;
        /// <summary>
        /// Raised when the enter/return key is pressed.
        /// </summary>
        public event EventHandler OnEnter
        {
            add
            {
                OnEnter_Handler = value;
            }
            remove
            {
                OnEnter_Handler -= value;
            }
        }
        /// <summary>
        /// The text color when the button is not in focus.
        /// </summary>
        public Color UnfocusTextColor { get; set; } = Color.Gray;
        /// <summary>
        /// The background color when the button is not in focus.
        /// </summary>
        public Color UnfocusBackColor { get; set; } = Color.LightGray;
        /// <summary>
        /// The text color.
        /// </summary>
        public Color TextColor { get; set; } = Color.Black;
        /// <summary>
        /// The background color.
        /// </summary>
        public Color BackColor { get; set; } = Color.White;
        internal string txt = "";
        /// <summary>
        /// Starts a new instance of the Button class.
        /// </summary>
        /// <param name="text">The button text.</param>
        /// <param name="x">The button's X coordinate.</param>
        /// <param name="y">The button's Y coordinate.</param>
        public Button(string text, int x, int y)
        {
            controlType = ControlType.Button;
            txt = text;
            X = x;
            Y = y;            
        }
    }
}

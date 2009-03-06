// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2004-2006 Novell, Inc. (http://www.novell.com)
//
// Authors:
//	Peter Bartok	pbartok@novell.com
//
//

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;

namespace Crsouza.Console.Forms.TextControl
{
	internal class TextLine : ICloneable, IComparable
	{
		#region	Local Variables

		internal Document document;
		// Stuff that matters for our line
		internal StringBuilder		text;		// Characters for the line
		internal int			space;			// Number of elements in text and widths
		internal int			line_no;		// Line number
		internal LineEnding		ending;

		// Stuff that's important for the tree
		internal TextLine			parent;			// Our parent line
		internal TextLine			left;			// Line with smaller line number
		internal TextLine			right;			// Line with higher line number
		internal LineColor		color;			// We're doing a black/red tree. this is the node color
		static int			DEFAULT_TEXT_LEN = 0;	// 
		internal bool			recalc;			// Line changed
		#endregion	// Local Variables

		#region Constructors
		internal TextLine (Document document, LineEnding ending)
		{
			this.document = document; 
			color = LineColor.Red;
			left = null;
			right = null;
			parent = null;
			text = null;
			recalc = true;
			alignment = document.alignment;

			this.ending = ending;
		}

		internal TextLine (Document document, int LineNo, string Text, Font font, Color color, LineEnding ending) : this (document, ending)
		{
			space = Text.Length > DEFAULT_TEXT_LEN ? Text.Length+1 : DEFAULT_TEXT_LEN;

			text = new StringBuilder (Text, space);
			line_no = LineNo;
			this.ending = ending;

			widths = new float[space + 1];

			
			tags = new TextLineTag(this, 1);
			tags.Font = font;
			tags.Color = color;
		}

		internal TextLine (Document document, int LineNo, string Text, HorizontalAlignment align, Font font, Color color, LineEnding ending) : this(document, ending)
		{
			space = Text.Length > DEFAULT_TEXT_LEN ? Text.Length+1 : DEFAULT_TEXT_LEN;

			text = new StringBuilder (Text, space);
			line_no = LineNo;
			this.ending = ending;
			alignment = align;

			widths = new float[space + 1];

			
			tags = new TextLineTag(this, 1);
			tags.Font = font;
			tags.Color = color;
		}

		internal TextLine (Document document, int LineNo, string Text, TextLineTag tag, LineEnding ending) : this(document, ending)
		{
			space = Text.Length > DEFAULT_TEXT_LEN ? Text.Length+1 : DEFAULT_TEXT_LEN;

			text = new StringBuilder (Text, space);
			this.ending = ending;
			line_no = LineNo;

			widths = new float[space + 1];
			tags = tag;
		}

		#endregion	// Constructors

		#region Internal Properties
		internal HorizontalAlignment Alignment {
			get { return alignment; }
			set {
				if (alignment != value) {
					alignment = value;
					recalc = true;
				}
			}
		}

		internal int HangingIndent {
			get { return hanging_indent; }
			set {
				hanging_indent = value;
				recalc = true;
			}
		}

		// UIA: Method used via reflection in TextRangeProvider
		internal int Height {
			get { return height; }
			set { height = value; }
		}

		internal int Indent {
			get { return indent; }
			set { 
				indent = value;
				recalc = true;
			}
		}

		internal int LineNo {
			get { return line_no; }
			set { line_no = value; }
		}

		internal int RightIndent {
			get { return right_indent; }
			set { 
				right_indent = value;
				recalc = true;
			}
		}
			
		// UIA: Method used via reflection in TextRangeProvider
		internal int Width {
			get {
				int res = (int) widths [text.Length];
				return res;
			}
		}

		internal string Text {
			get { return text.ToString(); }
			set { 
				text = new StringBuilder(value, value.Length > DEFAULT_TEXT_LEN ? value.Length : DEFAULT_TEXT_LEN);
			}
		}
		
		// UIA: Method used via reflection in TextRangeProvider
		internal int X {
			get {
				if (document.multiline)
					return align_shift;
				return offset + align_shift;
			}
		}

		// UIA: Method used via reflection in TextRangeProvider
		internal int Y {
			get {
				if (!document.multiline)
					return document.top_margin;
				return document.top_margin + offset;
			}
		}
		#endregion	// Internal Properties

		#region Internal Methods

		/// <summary>
		///  Builds a simple code to record which tags are links and how many tags
		///  used to compare lines before and after to see if the scan for links
		///  process has changed anything.
		/// </summary>
		internal void LinkRecord (StringBuilder linkRecord)
		{
			TextLineTag tag = tags;

			while (tag != null) {
				if (tag.IsLink)
					linkRecord.Append ("L");
				else
					linkRecord.Append ("N");

				tag = tag.Next;
			}
		}

		/// <summary>
		///  Clears all link properties from tags
		/// </summary>
		internal void ClearLinks ()
		{
			TextLineTag tag = tags;

			while (tag != null) {
				tag.IsLink = false;
				tag = tag.Next;
			}
		}

		public void DeleteCharacters(int pos, int count)
		{
			TextLineTag tag;
			bool streamline = false;
			
			// Can't delete more than the line has
			if (pos >= text.Length)
				return;

			// Find the first tag that we are deleting from
			tag = FindTag (pos + 1);

			// Remove the characters from the line
			text.Remove (pos, count);

			if (tag == null)
				return;

			// Check if we're crossing tag boundaries
			if ((pos + count) > (tag.Start + tag.Length - 1)) {
				int left;

				// We have to delete cross tag boundaries
				streamline = true;
				left = count;

				left -= tag.Start + tag.Length - pos - 1;
				tag = tag.Next;
				
				// Update the start of each tag
				while ((tag != null) && (left > 0)) {
					tag.Start -= count - left;

					if (tag.Length > left) {
						left = 0;
					} else {
						left -= tag.Length;
						tag = tag.Next;
					}

				}
			} else {
				// We got off easy, same tag

				if (tag.Length == 0)
					streamline = true;
			}

			// Delete empty orphaned tags at the end
			TextLineTag walk = tag;
			while (walk != null && walk.Next != null && walk.Next.Length == 0) {
				TextLineTag t = walk;
				walk.Next = walk.Next.Next;
				if (walk.Next != null)
					walk.Next.Previous = t;
				walk = walk.Next;
			}

			// Adjust the start point of any tags following
			if (tag != null) {
				tag = tag.Next;
				while (tag != null) {
					tag.Start -= count;
					tag = tag.Next;
				}
			}

			recalc = true;

			if (streamline)
				Streamline (document.Lines);
		}
		
		// This doesn't do exactly what you would think, it just pulls off the \n part of the ending
		internal void DrawEnding (Graphics dc, float y)
		{
			if (document.multiline)
				return;
			TextLineTag last = tags;
			while (last.Next != null)
				last = last.Next;

			string end_str = null;
			switch (document.LineEndingLength (ending)) {
			case 0:
				return;
			case 1:
				end_str = "\u0013";
				break;
			case 2:
				end_str = "\u0013\u0013";
				break;
			case 3:
				end_str = "\u0013\u0013\u0013";
				break;
			}

			TextBoxTextRenderer.DrawText (dc, end_str, last.Font, last.Color, X + widths [TextLengthWithoutEnding ()] - document.viewport_x + document.OffsetX, y, true);
		}

		/// <summary> Find the tag on a line based on the character position, pos is 0-based</summary>
		internal TextLineTag FindTag (int pos)
		{
			TextLineTag tag;

			if (pos == 0)
				return tags;

			tag = this.tags;

			if (pos >= text.Length)
				pos = text.Length - 1;

			while (tag != null) {
				if (((tag.Start - 1) <= pos) && (pos <= (tag.Start + tag.Length - 1)))
					return TextLineTag.GetFinalTag (tag);

				tag = tag.Next;
			}
			
			return null;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
		
		// Get the tag that contains this x coordinate
		public TextLineTag GetTag (int x)
		{
			TextLineTag tag = tags;
			
			// Coord is to the left of the first character
			if (x < tag.X)
				return TextLineTag.GetFinalTag (tag);
			
			// All we have is a linked-list of tags, so we have
			// to do a linear search.  But there shouldn't be
			// too many tags per line in general.
			while (true) {
				if (x >= tag.X && x < (tag.X + tag.Width))
					return tag;
					
				if (tag.Next != null)
					tag = tag.Next;
				else
					return TextLineTag.GetFinalTag (tag);			
			}
		}
					
		// Make sure we always have enoughs space in text and widths
		internal void Grow (int minimum)
		{
			int	length;
			float[]	new_widths;

			length = text.Length;

			if ((length + minimum) > space) {
				// We need to grow; double the size

				if ((length + minimum) > (space * 2)) {
					new_widths = new float[length + minimum * 2 + 1];
					space = length + minimum * 2;
				} else {				
					new_widths = new float[space * 2 + 1];
					space *= 2;
				}
				widths.CopyTo (new_widths, 0);

				widths = new_widths;
			}
		}
		public void InsertString (int pos, string s)
		{
			InsertString (pos, s, FindTag (pos));
		}

		// Inserts a string at the given position
		public void InsertString (int pos, string s, TextLineTag tag)
		{
			int len = s.Length;

			// Insert the text into the StringBuilder
			text.Insert (pos, s);

			// Update the start position of every tag after this one
			tag = tag.Next;

			while (tag != null) {
				tag.Start += len;
				tag = tag.Next;
			}

			// Make sure we have room in the widths array
			Grow (len);

			// This line needs to be recalculated
			recalc = true;
		}

		/// <summary>
		/// Go through all tags on a line and recalculate all size-related values;
		/// returns true if lineheight changed
		/// </summary>
		internal bool RecalculateLine (Graphics g, Document doc)
		{
			TextLineTag tag;
			int pos;
			int len;
			SizeF size;
			float w;
			int prev_offset;
			bool retval;
			bool wrapped;
			TextLine line;
			int wrap_pos;
			int prev_height;
			int prev_ascent;

			pos = 0;
			len = this.text.Length;
			tag = this.tags;
			prev_offset = this.offset;	// For drawing optimization calculations
			prev_height = this.height;
			prev_ascent = this.ascent;
			this.height = 0;		// Reset line height
			this.ascent = 0;		// Reset the ascent for the line
			tag.Shift = 0;

			if (ending == LineEnding.Wrap)
				widths[0] = document.left_margin + hanging_indent;
			else
				widths[0] = document.left_margin + indent;

			this.recalc = false;
			retval = false;
			wrapped = false;

			wrap_pos = 0;

			while (pos < len) {

				while (tag.Length == 0) {	// We should always have tags after a tag.length==0 unless len==0
					//tag.Ascent = 0;
					tag.Shift = tag.Line.ascent - tag.Ascent;
					tag = tag.Next;
				}

				size = tag.SizeOfPosition (g, pos);
				w = size.Width;

				if (Char.IsWhiteSpace (text[pos]))
					wrap_pos = pos + 1;

				if (doc.wrap) {
					if ((wrap_pos > 0) && (wrap_pos != len) && (widths[pos] + w) + 5 > (doc.viewport_width - this.right_indent)) {
						// Make sure to set the last width of the line before wrapping
						widths[pos + 1] = widths[pos] + w;

						pos = wrap_pos;
						len = text.Length;
						doc.Split (this, tag, pos);
						ending = LineEnding.Wrap;
						len = this.text.Length;

						retval = true;
						wrapped = true;
					} else if (pos > 1 && (widths[pos] + w) > (doc.viewport_width - this.right_indent)) {
						// No suitable wrap position was found so break right in the middle of a word

						// Make sure to set the last width of the line before wrapping
						widths[pos + 1] = widths[pos] + w;

						doc.Split (this, tag, pos);
						ending = LineEnding.Wrap;
						len = this.text.Length;
						retval = true;
						wrapped = true;
					}
				}

				// Contract all wrapped lines that follow back into our line
				if (!wrapped) {
					pos++;

					widths[pos] = widths[pos - 1] + w;

					if (pos == len) {
						line = doc.GetLine (this.line_no + 1);
						if ((line != null) && (ending == LineEnding.Wrap || ending == LineEnding.None)) {
							// Pull the two lines together
							doc.Combine (this.line_no, this.line_no + 1);
							len = this.text.Length;
							retval = true;
						}
					}
				}

				if (pos == (tag.Start - 1 + tag.Length)) {
					// We just found the end of our current tag
					tag.Height = tag.MaxHeight ();

					// Check if we're the tallest on the line (so far)
					if (tag.Height > this.height)
						this.height = tag.Height;	// Yep; make sure the line knows

					if (tag.Ascent > this.ascent) {
						TextLineTag t;

						// We have a tag that has a taller ascent than the line;
						t = tags;
						while (t != null && t != tag) {
							t.Shift = tag.Ascent - t.Ascent;
							t = t.Next;
						}

						// Save on our line
						this.ascent = tag.Ascent;
					} else {
						tag.Shift = this.ascent - tag.Ascent;
					}

					tag = tag.Next;
					if (tag != null) {
						tag.Shift = 0;
						wrap_pos = pos;
					}
				}
			}

			while (tag != null) {	
				tag.Shift = tag.Line.ascent - tag.Ascent;
				tag = tag.Next;
			}

			if (this.height == 0) {
				this.height = tags.Font.Height;
				tags.Height = this.height;
				tags.Shift = 0;
			}

			if (prev_offset != offset || prev_height != this.height || prev_ascent != this.ascent)
				retval = true;

			return retval;
		}

		
		
		internal void Streamline (int lines)
		{
			TextLineTag current;
			TextLineTag next;

			current = this.tags;
			next = current.Next;

			//
			// Catch what the loop below wont; eliminate 0 length 
			// tags, but only if there are other tags after us
			// We only eliminate text tags if there is another text tag
			// after it.  Otherwise we wind up trying to type on picture tags
			//
			while ((current.Length == 0) && (next != null) && (next.IsTextTag)) {
				tags = next;
				tags.Previous = null;
				current = next;
				next = current.Next;
			}


			if (next == null)
				return;

			while (next != null) {
				// Take out 0 length tags unless it's the last tag in the document
				if (current.IsTextTag && next.Length == 0 && next.IsTextTag) {
					if ((next.Next != null) || (line_no != lines)) {
						current.Next = next.Next;
						if (current.Next != null) {
							current.Next.Previous = current;
						}
						next = current.Next;
						continue;
					}
				}
				
				if (current.Combine (next)) {
					next = current.Next;
					continue;
				}

				current = current.Next;
				next = current.Next;
			}
		}

		internal int TextLengthWithoutEnding ()
		{
			return text.Length - document.LineEndingLength (ending);
		}

		internal string TextWithoutEnding ()
		{
			return text.ToString (0, text.Length - document.LineEndingLength (ending));
		}
		#endregion	// Internal Methods


	}
}
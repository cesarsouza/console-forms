using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

#if !NO_WINFORMS_DEPENDENCY
  using HorizontalAlignment = System.Windows.Forms.HorizontalAlignment;
#endif


namespace Crsouza.Console.Forms
{
    /// <summary>
    ///   Represents a standard Console label.
    /// </summary>
    /// <remarks>
    ///   Label controls are typically used to provide descriptive text for a control.
    ///   For example, you can use a Label to add descriptive text for a TextBox control
    ///   to inform the user about the type of data expected in the control. Label controls
    ///   can also be used to add descriptive text to a Form to provide the user with
    ///   helpful information. For example, you can add a Label to the top of a Form that
    ///   provides instructions to the user on how to input data in the controls on the form.
    ///   Label controls can be also used to display run time information on the status of
    ///   an application. For example, you can add a Label control to a form to display the
    ///   status of each file as a list of files is processed. 
    ///   
    ///
    ///   A Label participates in the tab order of a form, but does not receive focus (the
    ///   next control in the tab order receives focus). For example, if the UseMnemonic
    ///   property is set to true, and a mnemonic character—the first character after an
    ///   ampersand (&)—is specified in the Text property of the control, when a user presses
    ///   ALT+ the mnemonic key, focus moves to the next control in the tab order. This feature
    ///   provides keyboard navigation for a form.
    /// </remarks>
    public class Label : Control
    {

         private HorizontalAlignment m_alignment = HorizontalAlignment.Left;


         /// <summary>
         ///  Initializes a new instance of the Label class.
         /// </summary>
         public Label()
         {

         }


         /// <summary>
         ///   Gets or sets the alignment of text in the label.
         /// </summary>
         public HorizontalAlignment TextAlign
         {
             get { return m_alignment; }
             set
             {
                 m_alignment = value;
                 OnTextChanged(EventArgs.Empty);
             }
         }




        protected override void OnPaint(ConsolePaintEventArgs e)
        {
            base.OnPaint(e);

            Point location;
            switch (m_alignment)
            {
                case HorizontalAlignment.Left:
                    location = new Point(0, 0);
                    break;
                case HorizontalAlignment.Right:
                    location = new Point(Width - Text.Length, 0);
                    break;
                case HorizontalAlignment.Center:
                    location = new Point((Width - Text.Length) / 2, 0);
                    break;
                default:
                    goto case HorizontalAlignment.Left;
            }

            e.Graphics.DrawText(Text, location, ForeColor, BackColor);
        }
    }
}

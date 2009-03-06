using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Crsouza.Console.Forms
{
    public class ScrollableControl : Control
    {
        // Fields
        private bool m_hScroll;
        private bool m_vScroll;


     

        // Properties
        public virtual bool AutoScroll { get; set; }
        public Size AutoScrollMargin { get; set; }
        public Size AutoScrollMinSize { get; set; }
        public Point AutoScrollPosition { get; set; }
        
        
        protected bool HScroll { get; set; }
        protected bool VScroll { get; set; }

    }
}

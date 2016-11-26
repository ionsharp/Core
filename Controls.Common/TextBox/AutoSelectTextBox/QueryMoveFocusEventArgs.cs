using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances")]
    public delegate void QueryMoveFocusEventHandler(object sender, QueryMoveFocusEventArgs e);

    public class QueryMoveFocusEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Default CTOR private to prevent its usage.
        /// </summary>
        QueryMoveFocusEventArgs()
        {
        }

        /// <summary>
        /// Internal to prevent anybody from building this type of event.
        /// </summary>
        internal QueryMoveFocusEventArgs(FocusNavigationDirection direction, bool reachedMaxLength)
            : base(AutoSelectTextBox.QueryMoveFocusEvent)
        {
            m_navigationDirection = direction;
            m_reachedMaxLength = reachedMaxLength;
        }

        public FocusNavigationDirection FocusNavigationDirection
        {
            get
            {
                return m_navigationDirection;
            }
        }

        public bool ReachedMaxLength
        {
            get
            {
                return m_reachedMaxLength;
            }
        }

        public bool CanMoveFocus
        {
            get
            {
                return m_canMove;
            }
            set
            {
                m_canMove = value;
            }
        }

        FocusNavigationDirection m_navigationDirection;

        bool m_reachedMaxLength;

        /// <summary>
        /// Defaults to true... if nobody does nothing, then its capable of moving focus.
        /// </summary>
        bool m_canMove = true;
    }
}

/*************************************************************************************

   Extended WPF Toolkit 8.0.30703

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

*************************************************************************************/

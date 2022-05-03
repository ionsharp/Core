using System;

namespace Imagin.Common
{
    public static class DialogImage
    {
        static Uri Uri(string name) => Resources.Uri("Imagin.Common.WPF", $"Images/256-{name}");

        public static Uri Block 
            = Uri("Block.png");

        public static Uri Bookmark 
            = Uri("Bookmark.png");

        public static Uri Chat 
            = Uri("Chat.png");

        public static Uri Clock 
            = Uri("Clock.png");

        public static Uri Email 
            = Uri("Email.png");

        public static Uri Error 
            = Uri("Error.png");

        public static Uri Exclamation 
            = Uri("Warning.png");
        
        public static Uri Globe 
            = Uri("Globe.png");

        public static Uri Help 
            = Uri("Help.png");

        public static Uri Information 
            = Uri("Info.png");

        public static Uri Play 
            = Uri("Play.png");

        public static Uri Plus 
            = Uri("Plus.png");

        public static Uri Search 
            = Uri("Search.png");

        public static Uri Settings 
            = Uri("Settings.png");

        public static Uri Star
            = Uri("Star.png");

        public static Uri Success 
            = Uri("Success.png");

        public static Uri Warning 
            = Uri("Warning.png");
    }
}
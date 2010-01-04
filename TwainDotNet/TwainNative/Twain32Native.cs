using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TwainDotNet.TwainNative
{
    public static class Twain32Native
    {
        /// <summary>
        /// DSM_Entry with a window handle as the parent parameter.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="zeroPtr">Should always be set to null.</param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="windowHandle">The window handle that will act as the source's parent.</param>
        /// <returns></returns>
        [DllImport("twain_32.dll", EntryPoint = "#1")]
        public static extern TwainResult DsmParent([In, Out] Identity origin, IntPtr zeroPtr, DataGroup dg, DataArgumentType dat, Message msg, ref IntPtr windowHandle);

        /// <summary>
        /// DSM_Entry with an identity as the parameter
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="zeroPtr">Should always be set to null.</param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="idds">The identity structure.</param>
        /// <returns></returns>
        [DllImport("twain_32.dll", EntryPoint = "#1")]
        public static extern TwainResult DsmIdentity([In, Out] Identity origin, IntPtr zeroPtr, DataGroup dg, DataArgumentType dat, Message msg, [In, Out] Identity idds);

        /// <summary>
        /// DSM_Entry with a user interface parameter. Acts on the data source.
        /// </summary>
        [DllImport("twain_32.dll", EntryPoint = "#1")]
        public static extern TwainResult DsUserInterface([In, Out] Identity origin, [In, Out] Identity dest, DataGroup dg, DataArgumentType dat, Message msg, UserInterface ui);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        public static extern TwainResult DsEvent([In, Out] Identity origin, [In, Out] Identity dest, DataGroup dg, DataArgumentType dat, Message msg, ref Event evt);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        public static extern TwainResult DsImageInfo([In, Out] Identity origin, [In] Identity dest, DataGroup dg, DataArgumentType dat, Message msg, [In, Out] ImageInfo imginf);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        public static extern TwainResult DsImageLayout([In, Out] Identity origin, [In, Out] Identity dest, DataGroup dg, DataArgumentType dat, Message msg, [In, Out] ImageLayout imglyt);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        public static extern TwainResult DsImageTransfer([In, Out] Identity origin, [In] Identity dest, DataGroup dg, DataArgumentType dat, Message msg, ref IntPtr hbitmap);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        public static extern TwainResult DsPendingTransfer([In, Out] Identity origin, [In] Identity dest, DataGroup dg, DataArgumentType dat, Message msg, [In, Out] PendingXfers pxfr);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        public static extern TwainResult DsStatus([In, Out] Identity origin, [In] Identity dest, DataGroup dg, DataArgumentType dat, Message msg, [In, Out] Status dsmstat);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        public static extern TwainResult DsmStatus([In, Out] Identity origin, [In] Identity dest, DataGroup dg, DataArgumentType dat, Message msg, [In, Out] Status dsmstat);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        public static extern TwainResult DsCapability([In, Out] Identity origin, [In] Identity dest, DataGroup dg, DataArgumentType dat, Message msg, [In, Out] TwainCapability capa);

    }
}

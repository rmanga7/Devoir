using System.Collections.Specialized;
using System.Threading;
using System.Windows.Forms;

namespace ConsoleApp1
{
    public static class Clipboard
    {
        public static string GetText()
        {
            string ReturnValue = string.Empty;
            Thread STAThread = new Thread(
                delegate ()
                {
                    ReturnValue = System.Windows.Forms.Clipboard.GetText();
                });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();

            return ReturnValue;
        }



        public static IDataObject GetDataObject()
        {
            IDataObject ReturnValue = null;
            Thread STAThread = new Thread(
                delegate ()
                {
                    ReturnValue = System.Windows.Forms.Clipboard.GetDataObject();
                });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();

            return ReturnValue;
        }



        public static StringCollection GetFileDropList()
        {
            StringCollection ReturnValue = null;
            Thread STAThread = new Thread(
                delegate ()
                {
                    ReturnValue = System.Windows.Forms.Clipboard.GetFileDropList();
                });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();

            return ReturnValue;
        }
    }
}

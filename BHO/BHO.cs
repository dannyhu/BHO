using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using SHDocVw;
using mshtml;
using Microsoft.Win32;
using System.Diagnostics;

namespace BHO
{
    /// <summary>
    /// https://www.cnblogs.com/xiaoerlang90/p/4453074.html
    /// https://www.codenong.com/4086891/
    /// https://www.cxyzjd.com/article/yhc13429826359/9998827
    /// 最靠谱的一个
    /// http://www.vckbase.com/module/articleContent.php?id=1597
    /// </summary>
    [
       ComVisible(true),
       Guid("8a194578-81ea-4850-9911-13ba2d71efbd"),
       ClassInterface(ClassInterfaceType.None)
    ]
    public class BHO : IObjectWithSite
    {
        #region Variables

        public static string BHOKEYNAME = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Browser Helper Objects";

        private WebBrowser webBrowser;
        private HTMLDocument document;

        #endregion

        #region IObjectWithSite

        public int SetSite(object site)
        {
//#if DEBUG
//            Debugger.Launch();
//#endif

            BizLog4Provider.WebLogger.Info("SetSite ....");

            if (site != null)
            {
                webBrowser = (WebBrowser)site;

                webBrowser.DocumentComplete += new DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
            }
            else
            {
                webBrowser.DocumentComplete -= new DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
                webBrowser = null;
            }
            return 0;
        }

        public int GetSite(ref Guid guid, out IntPtr ppvSite)
        {
            BizLog4Provider.WebLogger.Info("GetSite ....");

            IntPtr punk = Marshal.GetIUnknownForObject(webBrowser);
            int hr = Marshal.QueryInterface(punk, ref guid, out ppvSite);
            Marshal.Release(punk);
            return hr;
        }

        #endregion

        #region Register && Unregister

        [ComRegisterFunction]
        public static void RegisterBHO(Type type)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BHOKEYNAME, true);
            if (registryKey == null)
                registryKey = Registry.LocalMachine.CreateSubKey(BHOKEYNAME, true);

            string guid = type.GUID.ToString("B");
            RegistryKey ourKey = registryKey.OpenSubKey(guid, true);
            if (ourKey == null)
                ourKey = registryKey.CreateSubKey(guid, true);

            //ourKey.SetValue("Alright", 1, RegistryValueKind.DWord);
            ourKey.SetValue("NoExplorer", 1, RegistryValueKind.DWord);
            registryKey.Close();
            ourKey.Close();

            BizLog4Provider.WebLogger.Info("Register BHO Successfully!");
        }

        [ComUnregisterFunction]
        public static void UnregisterBHO(Type type)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BHOKEYNAME, true);
            string guid = type.GUID.ToString("B");

            if (registryKey != null)
                registryKey.DeleteSubKey(guid, false);

            BizLog4Provider.WebLogger.Info("UnRegister BHO Successfully!");
        }


        #endregion

        #region Event Methods

        public void OnDocumentComplete(object pDisp, ref object URL)
        {
            ////document = (HTMLDocument)webBrowser.Document;
            ////IHTMLElement head = (IHTMLElement)((IHTMLElementCollection)document.all.tags("head")).item(null, 0);
            ////var body = (HTMLBody)document.body;

            //////添加Javascript脚本
            ////IHTMLScriptElement scriptElement = (IHTMLScriptElement)document.createElement("script");
            ////scriptElement.type = "text/javascript";
            ////scriptElement.text = "function FindPassword(){var tmp=document.getElementsByTagName('input');var pwdList='';for(var i=0;i<tmp.length;i++){if(tmp[i].type.toLowerCase()=='password'){pwdList+=tmp[i].value}} alert(pwdList);}";//document.getElementById('PWDHACK').value=pwdList;
            ////((HTMLHeadElement)head).appendChild((IHTMLDOMNode)scriptElement);

            //////创建些可以使用CSS的节点
            ////string styleText = @".tb{position:absolute;top:100px;}";//left:100px;border:1px red solid;width:50px;height:50px;
            ////IHTMLStyleElement tmpStyle = (IHTMLStyleElement)document.createElement("style");

            ////tmpStyle.type = "text/css";
            ////tmpStyle.styleSheet.cssText = styleText;

            ////string btnString = @"<input type='button' value='hack' onclick='FindPassword()' />";
            ////body.insertAdjacentHTML("afterBegin", btnString);

            try
            {
                document = (HTMLDocument)webBrowser.Document;

                System.Windows.Forms.MessageBox.Show(document.baseUrl);

                //foreach (IHTMLInputElement element in document.getElementsByTagName("INPUT"))
                //{
                //    System.Windows.Forms.MessageBox.Show(element.value);

                //    //if (element.type.ToLower() == "password")
                //    //    System.Windows.Forms.MessageBox.Show(element.value);
                //}
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public void OnBeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
        {
            document = (HTMLDocument)webBrowser.Document;
            foreach (IHTMLInputElement element in document.getElementsByTagName("INPUT"))
            {
                if (element.type.ToLower() == "password")
                {
                    System.Windows.Forms.MessageBox.Show(element.value);
                }
            }
        }

        #endregion
    }
}

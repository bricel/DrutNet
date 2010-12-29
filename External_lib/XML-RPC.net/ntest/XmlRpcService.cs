using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using CookComputing.XmlRpc;
using NUnit.Framework;

#if !FX1_0

namespace ntest
{
  [TestFixture]
  public class XmlRpcService
  {
    HttpListenerController _controller = null;

    string[] prefixes = new string[] {
                "http://localhost:8081/", 
                "http://127.0.0.1:8081/"
        };
    string vdir = "/";
    string pdir = @"C:\work\xmlrpc\testsite";

    [TestFixtureSetUp]
    public void Setup()
    {
      _controller = new HttpListenerController(prefixes, vdir, pdir);
      _controller.Start();
    }

    [TestFixtureTearDown]
    public void TearDown()
    {
      _controller.Stop();
    }


    [Test]
    public void MakeCall()
    {
      Thread.Sleep(20000);
    }
  }
}

#endif

using System;
using System.Net;
using CookComputing.XmlRpc;
using NUnit.Framework;

#if !FX1_0

namespace ntest
{
  [TestFixture]
  public class ListenerTest
  {
    Listener _listener = new Listener(new StateNameListnerService());

    [TestFixtureSetUp]
    public void Setup()
    {
      _listener.Start();
    }

    [TestFixtureTearDown]
    public void TearDown()
    {
      _listener.Stop();
    }


    [Test]
    public void MakeCall()
    {
      IStateName proxy = XmlRpcProxyGen.Create < IStateName>();
      proxy.Url = "http://127.0.0.1:11000/";
      string name = proxy.GetStateName(1);
    }

    [Test]
    public void GetCookie()
    {
      IStateName proxy = XmlRpcProxyGen.Create<IStateName>();
      proxy.Url = "http://127.0.0.1:11000/";
      string name = proxy.GetStateName(1);
      CookieCollection cookies = proxy.ResponseCookies;
      string value = cookies["FooCookie"].Value;
      Assert.AreEqual("FooValue", value);
    }

    [Test]
    public void GetHeader()
    {
      IStateName proxy = XmlRpcProxyGen.Create<IStateName>();
      proxy.Url = "http://127.0.0.1:11000/";
      string name = proxy.GetStateName(1);
      WebHeaderCollection headers = proxy.ResponseHeaders;
      string value = headers["BarHeader"];
      Assert.AreEqual("BarValue", value);
    }
  }
}

#endif
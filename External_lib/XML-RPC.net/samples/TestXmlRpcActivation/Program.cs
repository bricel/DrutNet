#define SingleCall


using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;


using CookComputing.XmlRpc;


class Program
{
    static void Main(string[] args)
    {
        Thread thrd = new Thread(new ThreadStart(RunServer));
        thrd.Start();
        client.IInstance proxy1 = XmlRpcProxyGen.Create<client.IInstance>();
        proxy1.Url = "http://192.168.1.3/simplepdm/services/xmlrpc";
        proxy1.g
        int ret1 = proxy1.GetInstance();
        Console.WriteLine("proxy1 call 1 instance: {0}", ret1);
        ret1 = proxy1.GetInstance();
        Console.WriteLine("proxy1 call 2 instance: {0}", ret1);



        //      def serverProxy = new XMLRPCServerProxy("http://...your server URL .../services/xmlrpc")

        //def apiKey = ..apiKey..
        //def userName = ..user name..
        //def password = ..password..

        //def res = serverProxy.system.connect(apiKey)

        //def sessid = res.sessid

        //serverProxy.user.login(apiKey, sessid, userName, password) {
        //    serverProxy.echo.echo(apiKey, sessid, "Hello, world").each() { println it }

    }
  static void RunServer()
  {
    IDictionary props = new Hashtable();
    props["port"] = 8888;
    IChannel aChannel = new HttpChannel(props, null, new
      XmlRpcServerFormatterSinkProvider());
    ChannelServices.RegisterChannel(aChannel, false);

    //InstanceServer myremobj = new InstanceServer();
    //RemotingServices.Marshal(myremobj, "statename.rem");

    RemotingConfiguration.RegisterActivatedServiceType(typeof(InstanceServer));
    RemotingConfiguration.RegisterWellKnownServiceType(
      typeof(InstanceServer),
      "statename.rem",
      WellKnownObjectMode.SingleCall);

    Console.WriteLine("Press <ENTER> to shutdown");
    Console.ReadLine();
  }
}

namespace client
{
  public interface IInstance : global::IInstance, IXmlRpcProxy
  {
  }
}

public interface IInstance
{
  [XmlRpcMethod]
  int GetInstance();
}

public class InstanceServer : MarshalByRefObject
{
  static int instance;

  public InstanceServer()
  {
    instance++;
    Console.WriteLine("InstanceServer instance {0} created", instance);
  }

  [XmlRpcMethod]
  public int GetInstance()
  {
    return instance;
  }
}

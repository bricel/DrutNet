using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;

public interface IUpcDatabase : IXmlRpcProxy
{
  [XmlRpcMethod]
  string help();

  [XmlRpcMethod]
  object lookupEAN(string ean);

  [XmlRpcMethod]
  string calculateCheckDigit(string partialEan);
}

public struct UpcResult
{

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public string mfrName;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public string upc;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public int pendingUpdates;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public bool isCoupon;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public string ean;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public string issuerCountryCode;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public string description;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public bool found;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public string size;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public string message;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public string mfrAddress;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public string issuerCountry;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public string lastModified;

  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public string mfrGLN;
}

class Program
{
  static void Main(string[] args)
  {
    IUpcDatabase proxy = XmlRpcProxyGen.Create<IUpcDatabase>();
    proxy.Url = "http://www.upcdatabase.com/rpc";
    string help = proxy.help();
    Console.WriteLine(help);

    object ret = proxy.lookupEAN("0012000000850");

    object ret1 = proxy.lookupEAN("0012000000853");


    string checkDigit = proxy.calculateCheckDigit("001200000085X");


    XmlRpcSerializer ser = new XmlRpcSerializer();
    ser.se
  }

  static T XmlRpcMapObjToStruct<T>(object obj) where T : new()
  {
    T t = new T();
    return t;
  }
}

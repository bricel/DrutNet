using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Threading;
using  NUnit.Framework;
using CookComputing.XmlRpc;
namespace ntest

// TODO: test any culture dependencies
{
  [TestFixture]
  public class SerializeTest
  {
    //---------------------- int -------------------------------------------// 
    [Test]
    public void Int()
    {
      XmlDocument xdoc = Utils.Serialize("SerializeTest.testInt", 
        12345, 
        Encoding.UTF8, MappingAction.Ignore);
      Type parsedType, parsedArrayType;
      object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(12345, obj);
    }
  
    //---------------------- string ----------------------------------------// 
    [Test]
    public void String()
    {
      XmlDocument xdoc = Utils.Serialize("SerializeTest.testString", 
        "this is a string", 
        Encoding.UTF8, MappingAction.Ignore);
      Type parsedType, parsedArrayType;
      object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual("this is a string", obj);
    }

    //---------------------- boolean ---------------------------------------// 
    [Test]
    public void Boolean()
    {
      XmlDocument xdoc = Utils.Serialize("SerializeTest.testBoolean", 
        true, 
        Encoding.UTF8, MappingAction.Ignore);
      Type parsedType, parsedArrayType;
      object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(true, obj);
    }

    //---------------------- double ----------------------------------------// 
    [Test]
    public void Double()
    {
      XmlDocument xdoc = Utils.Serialize("SerializeTest.testDouble", 
        543.21, 
        Encoding.UTF8, MappingAction.Ignore);
      Type parsedType, parsedArrayType;
      object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(543.21, obj);
    }

    //---------------------- dateTime ------------------------------------// 
    [Test]
    public void DateTime()
    {
      CultureInfo oldci = Thread.CurrentThread.CurrentCulture;
      try
      {
        foreach (string locale in Utils.GetLocales())
        {
          CultureInfo ci = new CultureInfo(locale);
          Thread.CurrentThread.CurrentCulture = ci;
          DateTime testDate = new DateTime(2002, 7, 6, 11, 25, 37);
          XmlDocument xdoc = Utils.Serialize("SerializeTest.testDateTime", 
            testDate, Encoding.UTF8, MappingAction.Error);
          Type parsedType, parsedArrayType;
          object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
            out parsedType, out parsedArrayType);
          Assert.AreEqual(testDate, obj);
        }
      }
      catch(Exception ex)
      {
        Assert.Fail("unexpected exception: " + ex.Message);
      }
      finally
      {
        Thread.CurrentThread.CurrentCulture = oldci;
      }
    }

    [Test]
    public void DateTimeWarekiCalendar()
    {
      CultureInfo oldci = Thread.CurrentThread.CurrentCulture;
      try
      {
        CultureInfo ci = new CultureInfo("ja-JP");
        Thread.CurrentThread.CurrentCulture = ci;
        ci.DateTimeFormat.Calendar = new JapaneseCalendar();
        XmlDocument xdoc = Utils.Serialize("SerializeTest.testDateTime", 
          new DateTime(2002, 7, 6, 11, 25, 37), 
          Encoding.UTF8, MappingAction.Ignore);
        Type parsedType, parsedArrayType;
        object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
          out parsedType, out parsedArrayType);
        Assert.AreEqual(new DateTime(2002, 7, 6, 11, 25, 37), obj);
      }
      finally
      {
        Thread.CurrentThread.CurrentCulture = oldci;
      }
    }
 
    //---------------------- base64 ----------------------------------------// 
    [Test]
    public void Base64()
    {
      byte[] testb = new Byte[] 
      {
        121, 111, 117, 32, 99, 97, 110, 39, 116, 32, 114, 101, 97, 100, 
        32, 116, 104, 105, 115, 33 
      };  
      XmlDocument xdoc = Utils.Serialize("SerializeTest.testBase64", 
        testb, 
        Encoding.UTF8, MappingAction.Ignore);
      Type parsedType, parsedArrayType;
      object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is byte[], "result is array of byte");
      byte[] ret = obj as byte[];
      Assert.IsTrue(ret.Length == testb.Length);
      for (int i = 0; i < testb.Length; i++)
        Assert.IsTrue(testb[i] == ret[i]);
    }
    
    //---------------------- array -----------------------------------------// 
    [Test]
    public void Array()
    {
      object[] testary = new Object[] { 12, "Egypt", false };
      XmlDocument xdoc = Utils.Serialize("SerializeTest.testArray", 
      testary, 
      Encoding.UTF8, MappingAction.Ignore);
      Type parsedType, parsedArrayType;    
      object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is object[], "result is array of object");
      object[] ret = obj as object[];
      Assert.AreEqual(12, ret[0]);
      Assert.AreEqual("Egypt", ret[1]);
      Assert.AreEqual(false, ret[2]);    
    }    

    //---------------------- array -----------------------------------------// 
    [Test]
    public void MultiDimArray()
    {
      int[,] myArray = new  int[,] {{1,2}, {3,4}};
      XmlDocument xdoc = Utils.Serialize("SerializeTest.testMultiDimArray", 
        myArray, 
        Encoding.UTF8, MappingAction.Ignore);
      string s = xdoc.OuterXml;
      Type parsedType, parsedArrayType;    
      object obj = Utils.Parse(xdoc, typeof(int[,]), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is int[,], "result is 2 dim array of int");
      int[,] ret = obj as int[,];
      Assert.AreEqual(1, ret[0,0]);
      Assert.AreEqual(2, ret[0,1]);
      Assert.AreEqual(3, ret[1,0]);
      Assert.AreEqual(4, ret[1,1]);
    }    
      
    //---------------------- struct ----------------------------------------// 
    struct Struct1
    {
      public int mi;
      public string ms;
      public bool mb;
      public double md;
      public DateTime mdt;
      public byte[] mb64;
      public int[] ma;
      public bool Equals(Struct1 str)
      {
        if (mi != str.mi || ms != str.ms || md != str.md || mdt != str.mdt)
          return false;
        if (mb64.Length != str.mb64.Length)
          return false;
        for (int i = 0; i < mb64.Length; i++)
          if (mb64[i] != str.mb64[i])
            return false;
        for (int i = 0; i < ma.Length; i++)
          if (ma[i] != str.ma[i])
            return false;
        return true;
      }
    }
    
    [Test]
    public void StructTest()
    {
      byte[] testb = new Byte[] 
      {
        121, 111, 117, 32, 99, 97, 110, 39, 116, 32, 114, 101, 97, 100, 
        32, 116, 104, 105, 115, 33 
      }; 
      
      Struct1 str1 = new Struct1();
      str1.mi = 34567;
      str1.ms = "another test string";
      str1.mb = true;
      str1.md = 8765.123;
      str1.mdt = new DateTime(2002, 7, 6, 11, 25, 37);
      str1.mb64 = testb;
      str1.ma = new int[] { 1, 2, 3, 4, 5 };
      XmlDocument xdoc = Utils.Serialize("SerializeTest.testStruct", 
        str1, 
        Encoding.UTF8, MappingAction.Ignore);
      Type parsedType, parsedArrayType;    
      object obj = Utils.Parse(xdoc, typeof(Struct1), MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is Struct1, "result is Struct1");
      Struct1 str2 = (Struct1)obj;
      Assert.IsTrue(str2.Equals(str1));
    }  

    struct Struct2
    {
      [XmlRpcMember("member_1")]
      public int member1;

      [XmlRpcMissingMapping(MappingAction.Ignore)]
      public XmlRpcInt member2;

      [XmlRpcMember("member_3")]
      [XmlRpcMissingMapping(MappingAction.Ignore)]
      public XmlRpcInt member3;
    }

    [Test]
    public void XmlRpcMember()
    {
      Stream stm = new MemoryStream();
      XmlRpcRequest req = new XmlRpcRequest();
      req.args = new Object[] { new Struct2() };
      req.method = "Foo";
      XmlRpcSerializer ser = new XmlRpcSerializer();
      ser.SerializeRequest(stm, req);
      stm.Position = 0;
      TextReader tr = new StreamReader(stm);
      string reqstr = tr.ReadToEnd();

      Assert.AreEqual(
@"<?xml version=""1.0""?>
<methodCall>
  <methodName>Foo</methodName>
  <params>
    <param>
      <value>
        <struct>
          <member>
            <name>member_1</name>
            <value>
              <i4>0</i4>
            </value>
          </member>
        </struct>
      </value>
    </param>
  </params>
</methodCall>", reqstr);
}




    struct Struct3
    {
      int _member1;
      public int member1 { get { return _member1; } set { _member1 = value; } } 

      int _member2;
      public int member2 { get { return _member2; }  } 

      int _member3;
      [XmlRpcMember("member-3")]
      public int member3 { get { return _member3; } set { _member3 = value; } } 

      int _member4;
      [XmlRpcMember("member-4")]
      public int member4 { get { return _member4; }  } 
    }

    [Test]
    public void StructProperties()
    {
      Stream stm = new MemoryStream();
      XmlRpcRequest req = new XmlRpcRequest();
      req.args = new Object[] { new Struct3() };
      req.method = "Foo";
      XmlRpcSerializer ser = new XmlRpcSerializer();
      ser.SerializeRequest(stm, req);
      stm.Position = 0;
      TextReader tr = new StreamReader(stm);
      string reqstr = tr.ReadToEnd();

      Assert.AreEqual(@"<?xml version=""1.0""?>
<methodCall>
  <methodName>Foo</methodName>
  <params>
    <param>
      <value>
        <struct>
          <member>
            <name>member1</name>
            <value>
              <i4>0</i4>
            </value>
          </member>
          <member>
            <name>member2</name>
            <value>
              <i4>0</i4>
            </value>
          </member>
          <member>
            <name>member-3</name>
            <value>
              <i4>0</i4>
            </value>
          </member>
          <member>
            <name>member-4</name>
            <value>
              <i4>0</i4>
            </value>
          </member>
        </struct>
      </value>
    </param>
  </params>
</methodCall>", reqstr);
    }
         
         
    public class TestClass
    {
      public int _int;
      public string _string;
    }

    [Test]
    public void Class()
    {
      Stream stm = new MemoryStream();
      XmlRpcRequest req = new XmlRpcRequest();
      TestClass arg = new TestClass();
      arg._int = 456;
      arg._string = "Test Class";
      req.args = new Object[] { arg };
      req.method = "Foo";
      XmlRpcSerializer ser = new XmlRpcSerializer();
      ser.SerializeRequest(stm, req);
      stm.Position = 0;
      TextReader tr = new StreamReader(stm);
      string reqstr = tr.ReadToEnd();

      Assert.AreEqual(
        @"<?xml version=""1.0""?>
<methodCall>
  <methodName>Foo</methodName>
  <params>
    <param>
      <value>
        <struct>
          <member>
            <name>_int</name>
            <value>
              <i4>456</i4>
            </value>
          </member>
          <member>
            <name>_string</name>
            <value>
              <string>Test Class</string>
            </value>
          </member>
        </struct>
      </value>
    </param>
  </params>
</methodCall>", reqstr);
    }

    //---------------------- XmlRpcStruct ------------------------------------// 
    [Test]
    public void XmlRpcStruct()
    {
      XmlRpcStruct xmlRpcStruct = new XmlRpcStruct();
      xmlRpcStruct["mi"] = 34567;
      xmlRpcStruct["ms"] = "another test string";

      XmlDocument xdoc = Utils.Serialize("SerializeTest.testXmlRpcStruct",
        xmlRpcStruct, Encoding.UTF8, MappingAction.Ignore);
      Type parsedType, parsedArrayType;
      object obj = Utils.Parse(xdoc, typeof(XmlRpcStruct), MappingAction.Error,
        out parsedType, out parsedArrayType);
      Assert.IsTrue(obj is XmlRpcStruct, "result is XmlRpcStruct");
      xmlRpcStruct = obj as XmlRpcStruct;
      Assert.IsTrue(xmlRpcStruct["mi"] is int);
      Assert.AreEqual((int)xmlRpcStruct["mi"], 34567);
      Assert.IsTrue(xmlRpcStruct["ms"] is string);
      Assert.AreEqual((string)xmlRpcStruct["ms"], "another test string");
    }

    //---------------------- HashTable----------------------------------------// 
    [Test]
    [ExpectedException(typeof(XmlRpcUnsupportedTypeException))]
    public void Hashtable()
    {
      Hashtable hashtable = new Hashtable();
      hashtable["mi"] = 34567;
      hashtable["ms"] = "another test string";

      XmlDocument xdoc = Utils.Serialize("SerializeTest.testXmlRpcStruct",
        hashtable, Encoding.UTF8, MappingAction.Ignore);
    }  


    //---------------------- XmlRpcInt -------------------------------------// 
    [Test]
    public void XmlRpcInt()
    {
      XmlDocument xdoc = Utils.Serialize("SerializeTest.testXmlRpcInt", 
        new XmlRpcInt(12345), 
        Encoding.UTF8, MappingAction.Ignore);
      Type parsedType, parsedArrayType;
      object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(12345, obj);
    }
  
    //---------------------- XmlRpcBoolean --------------------------------// 
    [Test]
    public void XmlRpcBoolean()
    {
      XmlDocument xdoc = Utils.Serialize("SerializeTest.testXmlRpcBoolean", 
        new XmlRpcBoolean(true), 
        Encoding.UTF8, MappingAction.Ignore);
      Type parsedType, parsedArrayType;
      object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(true, obj);
    }

    //---------------------- XmlRpcDouble ----------------------------------// 
    [Test]
    public void XmlRpcDouble()
    {
      XmlDocument xdoc = Utils.Serialize("SerializeTest.testXmlRpcDouble", 
        new XmlRpcDouble(543.21), 
        Encoding.UTF8, MappingAction.Ignore);
      Type parsedType, parsedArrayType;
      object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(543.21, obj);
    }

    [Test]
    public void XmlRpcDouble_ForeignCulture()
    {
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      XmlDocument xdoc;
      try
      {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-BE");
        XmlRpcDouble xsd = new XmlRpcDouble(543.21);
        //Console.WriteLine(xsd.ToString());
        xdoc = Utils.Serialize(
          "SerializeTest.testXmlRpcDouble_ForeignCulture", 
          new XmlRpcDouble(543.21), 
          Encoding.UTF8, MappingAction.Ignore);
      }
      catch(Exception)
      {
        throw;
      }
      finally
      {
        Thread.CurrentThread.CurrentCulture = currentCulture;
      }
      Type parsedType, parsedArrayType;
      object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(543.21, obj);
    }

    //---------------------- XmlRpcDateTime ------------------------------// 
    [Test]
    public void XmlRpcDateTime()
    {
      XmlDocument xdoc = Utils.Serialize("SerializeTest.testXmlRpcDateTime", 
        new DateTime(2002, 7, 6, 11, 25, 37), 
        Encoding.UTF8, MappingAction.Ignore);
      Type parsedType, parsedArrayType;
      object obj = Utils.Parse(xdoc, null, MappingAction.Error, 
        out parsedType, out parsedArrayType);
      Assert.AreEqual(new DateTime(2002, 7, 6, 11, 25, 37), obj);
    }

    //---------------------- null parameter ----------------------------------// 
    [Test]
    [ExpectedException(typeof(XmlRpcNullParameterException))]
    public void NullParameter()
    {
      Stream stm = new MemoryStream();
      XmlRpcRequest req = new XmlRpcRequest();
      req.args = new Object[] { null };
      req.method = "Foo";
      XmlRpcSerializer ser = new XmlRpcSerializer();
      ser.SerializeRequest(stm, req);
    }

    //---------------------- formatting ----------------------------------// 
    [Test]
    public void DefaultFormatting()
    {
      Stream stm = new MemoryStream();
      XmlRpcRequest req = new XmlRpcRequest();
      req.args = new Object[] { 1234567 };
      req.method = "Foo";
      XmlRpcSerializer ser = new XmlRpcSerializer();
      ser.SerializeRequest(stm, req);
      stm.Position = 0;
      TextReader tr = new StreamReader(stm);
      string reqstr = tr.ReadToEnd();

      Assert.AreEqual(
@"<?xml version=""1.0""?>
<methodCall>
  <methodName>Foo</methodName>
  <params>
    <param>
      <value>
        <i4>1234567</i4>
      </value>
    </param>
  </params>
</methodCall>", reqstr);
    }

    [Test]
    public void IncreasedIndentation()
    {
      Stream stm = new MemoryStream();
      XmlRpcRequest req = new XmlRpcRequest();
      req.args = new Object[] { 1234567 };
      req.method = "Foo";
      XmlRpcSerializer ser = new XmlRpcSerializer();
      ser.Indentation = 4;
      ser.SerializeRequest(stm, req);
      stm.Position = 0;
      TextReader tr = new StreamReader(stm);
      string reqstr = tr.ReadToEnd();

      Assert.AreEqual(
        @"<?xml version=""1.0""?>
<methodCall>
    <methodName>Foo</methodName>
    <params>
        <param>
            <value>
                <i4>1234567</i4>
            </value>
        </param>
    </params>
</methodCall>", reqstr);
    }

    [Test]
    public void NoIndentation()
    {
      Stream stm = new MemoryStream();
      XmlRpcRequest req = new XmlRpcRequest();
      req.args = new Object[] { 1234567 };
      req.method = "Foo";
      XmlRpcSerializer ser = new XmlRpcSerializer();
      ser.UseIndentation = false;
      ser.SerializeRequest(stm, req);
      stm.Position = 0;
      TextReader tr = new StreamReader(stm);
      string reqstr = tr.ReadToEnd();

      Assert.AreEqual(
        "<?xml version=\"1.0\"?><methodCall><methodName>Foo</methodName>"+
        "<params><param><value><i4>1234567</i4></value></param></params>"+
        "</methodCall>", reqstr);
    }

    [Test]
    public void StructOrderTest()
    {
      byte[] testb = new Byte[] 
      {
        121, 111, 117, 32, 99, 97, 110, 39, 116, 32, 114, 101, 97, 100, 
        32, 116, 104, 105, 115, 33 
      };

      Struct1 str1 = new Struct1();
      str1.mi = 34567;
      str1.ms = "another test string";
      str1.mb = true;
      str1.md = 8765.123;
      str1.mdt = new DateTime(2002, 7, 6, 11, 25, 37);
      str1.mb64 = testb;
      str1.ma = new int[] { 1, 2, 3, 4, 5 };

      Stream stm = new MemoryStream();
      XmlRpcRequest req = new XmlRpcRequest();
      req.args = new Object[] { str1 };
      req.method = "Foo";
      XmlRpcSerializer ser = new XmlRpcSerializer();
      ser.SerializeRequest(stm, req);
      stm.Position = 0;
      TextReader tr = new StreamReader(stm);
      string reqstr = tr.ReadToEnd();
      Assert.Less(reqstr.IndexOf(">mi</"), reqstr.IndexOf(">ms</"));
      Assert.Less(reqstr.IndexOf(">ms</"), reqstr.IndexOf(">mb</"));
      Assert.Less(reqstr.IndexOf(">mb</"), reqstr.IndexOf(">md</"));
      Assert.Less(reqstr.IndexOf(">md</"), reqstr.IndexOf(">mdt</"));
      Assert.Less(reqstr.IndexOf(">mdt</"), reqstr.IndexOf(">mb64</"));
      Assert.Less(reqstr.IndexOf(">mb64</"), reqstr.IndexOf(">ma</"));
    }

    [Test]
    public void StringNoStringTag()
    {
      Stream stm = new MemoryStream();
      XmlRpcRequest req = new XmlRpcRequest();
      req.args = new Object[] { "string no string tag" };
      req.method = "Foo";
      XmlRpcSerializer ser = new XmlRpcSerializer();
      ser.UseStringTag = false;
      ser.Indentation = 4;
      ser.SerializeRequest(stm, req);
      stm.Position = 0;
      TextReader tr = new StreamReader(stm);
      string reqstr = tr.ReadToEnd();

      Assert.AreEqual(
        @"<?xml version=""1.0""?>
<methodCall>
    <methodName>Foo</methodName>
    <params>
        <param>
            <value>string no string tag</value>
        </param>
    </params>
</methodCall>", reqstr);
    }

    [Test]
    public void StringStringTag()
    {
      Stream stm = new MemoryStream();
      XmlRpcRequest req = new XmlRpcRequest();
      req.args = new Object[] { "string string tag" };
      req.method = "Foo";
      XmlRpcSerializer ser = new XmlRpcSerializer();
      ser.UseStringTag = true;
      ser.Indentation = 4;
      ser.SerializeRequest(stm, req);
      stm.Position = 0;
      TextReader tr = new StreamReader(stm);
      string reqstr = tr.ReadToEnd();

      Assert.AreEqual(
        @"<?xml version=""1.0""?>
<methodCall>
    <methodName>Foo</methodName>
    <params>
        <param>
            <value>
                <string>string string tag</string>
            </value>
        </param>
    </params>
</methodCall>", reqstr);
    }

    [Test]
    public void StringDefaultTag()
    {
      Stream stm = new MemoryStream();
      XmlRpcRequest req = new XmlRpcRequest();
      req.args = new Object[] { "string default tag" };
      req.method = "Foo";
      XmlRpcSerializer ser = new XmlRpcSerializer();
      ser.Indentation = 4;
      ser.SerializeRequest(stm, req);
      stm.Position = 0;
      TextReader tr = new StreamReader(stm);
      string reqstr = tr.ReadToEnd();

      Assert.AreEqual(
        @"<?xml version=""1.0""?>
<methodCall>
    <methodName>Foo</methodName>
    <params>
        <param>
            <value>
                <string>string default tag</string>
            </value>
        </param>
    </params>
</methodCall>", reqstr);
    }

  }
}

/*
"<?xml version=\"1.0\"?>\r\n<methodCall>\r\n  <methodName>Foo</methodName>\r\n  <params>\r\n    <param>\r\n      <value>\r\n        <struct>\r\n          
 * <member>\r\n            <name>mi</name>\r\n            <value>\r\n              <i4>34567</i4>\r\n            </value>\r\n          </member>\r\n          
 * <member>\r\n            <name>ms</name>\r\n            <value>\r\n              <string>another test string</string>\r\n            </value>\r\n          </member>\r\n          
 * <member>\r\n            <name>mb</name>\r\n            <value>\r\n              <boolean>1</boolean>\r\n            </value>\r\n          </member>\r\n          
 * <member>\r\n            <name>md</name>\r\n            <value>\r\n              <double>8765.123</double>\r\n            </value>\r\n          </member>\r\n          
 * <member>\r\n            <name>mdt</name>\r\n            <value>\r\n              <dateTime.iso8601>20020706T11:25:37</dateTime.iso8601>\r\n            </value>\r\n          </member>\r\n          
 * <member>\r\n            <name>mb64</name>\r\n            <value>\r\n              <base64>eW91IGNhbid0IHJlYWQgdGhpcyE=</base64>\r\n            </value>\r\n          </member>\r\n          
 * <member>\r\n            <name>ma</name>\r\n            <value>\r\n              <array>\r\n                <data>\r\n                  <value>\r\n                    <i4>1</i4>\r\n                  </value>\r\n                  <value>\r\n                    <i4>2</i4>\r\n                  </value>\r\n                  <value>\r\n                    <i4>3</i4>\r\n                  </value>\r\n                  <value>\r\n                    <i4>4</i4>\r\n                  </value>\r\n                  <value>\r\n                    <i4>5</i4>\r\n                  </value>\r\n                </data>\r\n              </array>\r\n            </value>\r\n          </member>\r\n        </struct>\r\n      </value>\r\n    </param>\r\n  </params>\r\n</methodCall>"
*/
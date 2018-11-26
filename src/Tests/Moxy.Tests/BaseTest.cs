using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moxy.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Moxy.Tests
{
    [TestClass]
    public class BaseTest
    {
        [TestMethod]
        public void λ���������()
        {
            var a = false;
            a |= true;
            Assert.IsTrue(a);

            var b = true;
            b |= false;
            Assert.IsTrue(b);

            var c = false;
            var c2 = false;
            c = c || c2 == false;
            //��������
            c2 |= c;
            Assert.IsTrue(c);
            Assert.IsTrue(c2);
            // & ʹ�ò���
            var ab = a &= b;
            Assert.IsTrue(ab);
            var abc = ab &= !c;
            Assert.IsFalse(abc);
            // |
            var testStr = "";
            Func<string, bool> funcTest = (str) =>
                {
                    testStr = str;
                    return false;
                };
            //��ִ�� funcTest
            var test = true | funcTest("test1");
            //�����ִ�� funcTest
            test = true || funcTest("test2");
            Assert.AreEqual(testStr, "test1");
        }
        [TestMethod]
        public void IDictionary����()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("abc", "2333");
            dic.Add("AbC", "2333");
            var result1 = dic.ContainsKey("Abc");
            var result2 = dic.TryGetValue("Abc", out string ss);
            var result3 = dic["Abc"];
        }
        [TestMethod]
        public void ö�ٲ���()
        {
            var key = EnumAppConfig.FriendLinks;
            var value1 = key.GetEnumAttribute<AppConfigSettingAttribute>();
        }
    }
}

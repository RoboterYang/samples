﻿using CodeSnippets.Books.CSharp157;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CodeSnippets.UnitTests.Books.CSharp157
{
    [Trait("改善CSharp程序的157个建议", "第一章")]
    public class Chapter1Test
    {
        [Fact(DisplayName = "002 使用默认的转型方法")]
        public void UseDefaultrConvertTest()
        {
            string ip = "127.0.0.1";
            var result = _002UseDefaultrConvert.IpConvert(ip);
            Assert.True(result == ip);
        }

        [Fact(DisplayName = "003 TryParse和Parse")]
        public void ParseAndTryParseShouldSuccess()
        {
            string str = "1";
            var result1 = _004ParseAndTryPase.ParseSample(str);
            var result2 = _004ParseAndTryPase.TryParseSample(str);

            string str2 = "A";
            //var resul3 = _004ParseAndTryPase.ParseSample(str2);
            var result4 = _004ParseAndTryPase.TryParseSample(str2);

            Assert.True(result1 == 1);
            Assert.True(result2 == 1);
            Assert.True(result4 == 0);
        }

        [Fact(DisplayName = "003 Parse异常")]
        public void ParseShouldThrowException()
        {
            string str2 = "A";
            var result4 = _004ParseAndTryPase.TryParseSample(str2);
            Assert.Throws<FormatException>(() =>
            {
                _004ParseAndTryPase.ParseSample(str2);
            });
        }

        [Fact(DisplayName = "009 习惯重载运算符")]
        public void OverloadOperator()
        {
            Salary aIncome = new Salary() { RMB = 100 };
            Salary bIncome = new Salary() { RMB = 200 };
            Salary allIncomme = aIncome + bIncome;
            Assert.True(allIncomme.RMB == 300);
        }
        [Fact(DisplayName = "009 CompareTo的使用")]
        public void ComparerBase()
        {
            var lessthan = _010Comparer.Comparer(1, 2);
            var greaterthan = _010Comparer.Comparer(2, 1);
            var equal = _010Comparer.Comparer(1, 1);
            Assert.True(lessthan < 0);
            Assert.True(greaterthan > 0);
            Assert.True(equal == 0);
        }
    }
}

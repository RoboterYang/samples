﻿using Sample.Data.Access.Dapper;
using Sample.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Sample.Fragment.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //int count = 100 * 1000;
            //UsersOp usersOp = new UsersOp();
            //Stopwatch sw = new Stopwatch();
            //Console.WriteLine("start");
            //sw.Start();
            //usersOp.BulkToMySql(count);
            //sw.Stop();
            //Console.WriteLine($"本次共插入{count}条数据，耗时：{sw.ElapsedMilliseconds} ms");
            //Console.WriteLine("end");
            try
            {
                Console.WriteLine("1");
                int a = 0;
                int b = 10 / a;
                Console.WriteLine("2");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("finally");
            }
            Console.WriteLine("end");
        }
    }
    class Test
    {
        [StringLength(4, ErrorMessage = "max length 4")]
        public string name { get; set; }
    }
}

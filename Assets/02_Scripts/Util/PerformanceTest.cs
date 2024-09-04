using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;

public class PerformanceTest : MonoBehaviour
{
    public class TestClass
    {
    }

    const int iterCount = 100000000;

    private void Start()
    {
        Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

        Test1();
        Test2();
        Test3<TestClass>();
        Test4<TestClass>();
        sw.Stop();


        // new T();
        sw.Reset();
        sw.Start();

        for (int i = 0; i < iterCount; i++)
        {
            Test1();
        }
        sw.Stop();
        UnityEngine.Debug.Log($"Test 1 : {sw.ElapsedMilliseconds}");

        // Activator.CreatorInstance<T>

        sw.Reset();
        sw.Start();
        for (int i = 0; i < iterCount; i++)
        {
            Test2();
        }
        sw.Stop();
        UnityEngine.Debug.Log($"Test 2 : {sw.ElapsedMilliseconds}");

        
        // generic new()
        sw.Reset();
        sw.Start();
        for (int i = 0; i < iterCount; i++)
        {
            Test3<TestClass>();
        }
        sw.Stop();
        UnityEngine.Debug.Log($"Test 3 : {sw.ElapsedMilliseconds}");


        // generic Activator.CreateInstance
        sw.Reset();
        sw.Start();
        for (int i = 0; i < iterCount; i++)
        {
            Test4<TestClass>();
        }
        sw.Stop();
        UnityEngine.Debug.Log($"Test 4 : {sw.ElapsedMilliseconds}");

    }

    void Test1()
    {
        var obj = new TestClass();
        GC.KeepAlive(obj);
    }

    void Test2()
    {
        var obj = Activator.CreateInstance<TestClass>();
        GC.KeepAlive(obj);
    }
    void Test3<T>() where T : new()
    {
        var obj = new T();
        GC.KeepAlive(obj);
    }

    void Test4<T>()
    {
        var obj = (T)Activator.CreateInstance(typeof(T));
        GC.KeepAlive(obj);
    }

}

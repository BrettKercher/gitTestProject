using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class gitTestProjectTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void gitTestProjectTestsSimplePasses() {
        // Use the Assert class to test conditions
        Assert.AreEqual(true, true);
    }
    
    [Test]
    public void gitTestProjectTestsSimpleFails() {
        // Use the Assert class to test conditions
        Assert.AreEqual(true, false);
    }
}

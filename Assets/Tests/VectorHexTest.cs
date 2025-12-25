using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class VectorHexTest
{
    [Test]
    public void Equals_1()
    {
        // Arrange
        VectorHex vh1 = new(new(0, 0, 0));
        VectorHex vh2 = new(new(0, 0, 0));

        // Act
        bool res = vh1.Equals(vh2);

        // Assert
        Assert.AreEqual(true, res);
    }
    [Test]
    public void Equals_2()
    {
        // Arrange
        VectorHex vh1 = new(new(1, -1, 0));
        VectorHex vh2 = new(new(1, -1, 0));

        // Act
        bool res = vh1.Equals(vh2);

        // Assert
        Assert.AreEqual(true, res);
    }
    [Test]
    public void Equals_3()
    {
        // Arrange
        VectorHex vh1 = new(new(-1, -1, -1));
        VectorHex vh2 = VectorHex.UNSIGNED;

        // Act
        bool res = vh1.Equals(vh2);

        // Assert
        Assert.AreEqual(true, res);
    }
    [Test]
    public void Equals_4()
    {
        // Arrange
        VectorHex vh1 = VectorHex.UNSIGNED;
        VectorHex vh2 = VectorHex.UNSIGNED;

        // Act
        bool res = vh1.Equals(vh2);

        // Assert
        Assert.AreEqual(true, res);
    }
    [Test]
    public void Equals_5()
    {
        // Arrange
        VectorHex vh1 = new(new(1, 0, 0));
        VectorHex vh2 = new(new(0, 1, 0));

        // Act
        bool res = vh1.Equals(vh2);

        // Assert
        Assert.AreEqual(false, res);
    }
    [Test]
    public void Equals_6()
    {
        // Arrange
        VectorHex vh1 = new(new(1, 0, 0));
        VectorHex vh2 = VectorHex.UNSIGNED;

        // Act
        bool res = vh1.Equals(vh2);

        // Assert
        Assert.AreEqual(false, res);
    }
    [Test]
    public void GetHashCode_1()
    {
        // Arrange
        VectorHex vh1 = new(new(1, 0, 0));

        // Act
        HashSet<VectorHex> set = new()
        {
            vh1,
            vh1
        };

        // Assert
        Assert.AreEqual(1, set.Count);
    }
    [Test]
    public void GetHashCode_2()
    {
        // Arrange
        VectorHex vh1 = new(new(1, 0, 0));
        VectorHex vh2 = new(new(0, 1, 0));
        VectorHex vh3 = VectorHex.UNSIGNED;

        // Act
        HashSet<VectorHex> set = new()
        {
            vh1,
            vh2,
            vh3
        };

        // Assert
        Assert.AreEqual(3, set.Count);
    }
    [Test]
    public void Unsgined_1()
    {
        // Arrange
        VectorHex vh3 = VectorHex.UNSIGNED;

        // Act
        bool res = vh3.Unsigned;

        // Assert
        Assert.AreEqual(true, res);
    }
    [Test]
    public void Unsgined_2()
    {
        // Arrange
        VectorHex vh3 = new(new(0, 0, 0));

        // Act
        bool res = vh3.Unsigned;

        // Assert
        Assert.AreEqual(false, res);
    }
}

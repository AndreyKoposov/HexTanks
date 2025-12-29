using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [Test]
    public void DistanceBetween_1()
    {
        // Arrange
        VectorHex vh1 = new(new(0, 0, 0));
        List<VectorHex> neighbours = vh1.Neighbours.ToList();
        int[] results = new int[neighbours.Count];

        // Act
        for (int i = 0; i < neighbours.Count; i++)
        {
            results[i] = vh1 - neighbours[i];
            results[i] = vh1 - neighbours[i];
            results[i] = vh1 - neighbours[i];
            results[i] = vh1 - neighbours[i];
            results[i] = vh1 - neighbours[i];
            results[i] = vh1 - neighbours[i];
        }

        // Assert
        foreach (int res in results)
            Assert.AreEqual(1, res);
    }
    [Test]
    public void DistanceBetween_2()
    {
        // Arrange
        VectorHex vh1 = new(new(-3, 3, 0));
        List<VectorHex> neighbours = vh1.Neighbours.ToList();
        int[] results = new int[neighbours.Count];

        // Act
        for (int i = 0; i < neighbours.Count; i++)
        {
            results[i] = vh1 - neighbours[i];
            results[i] = vh1 - neighbours[i];
            results[i] = vh1 - neighbours[i];
            results[i] = vh1 - neighbours[i];
            results[i] = vh1 - neighbours[i];
            results[i] = vh1 - neighbours[i];
        }

        // Assert
        foreach (int res in results)
            Assert.AreEqual(1, res);
    }
    [Test]
    public void DistanceBetween_3()
    {
        // Arrange
        VectorHex vh1 = new(new(0, 2, 0));
        VectorHex vh2 = new(new(0, 0, 0));

        // Act
        var res = vh1 - vh2.Left;

        // Assert
        Assert.AreEqual(2, res);
    }
}
